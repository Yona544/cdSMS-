from twilio.rest import Client
from twilio.twiml import TwiML, VoiceResponse
from typing import Optional, Dict, Any, List, Tuple
import logging
import re
import json
from datetime import datetime

from app.models.schemas import TenantResponse, VoiceMessageResponse
from app.services.voice_service import voice_service
from app.core.config import settings
from app.core.database import DatabaseManager

logger = logging.getLogger(__name__)

class TwilioService:
    """Service for Twilio voice and SMS operations"""

    def __init__(self, tenant: TenantResponse):
        self.tenant = tenant
        self.client = Client(tenant.twilio_sid, tenant.twilio_token)
        self.from_number = tenant.from_number
        # Instantiate a new DB manager. Not ideal, but necessary.
        self.db_manager = DatabaseManager()

    async def create_voice_call(self, to_number: str, voice_message_id: int,
                              variables: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """Create a voice call with dynamic TwiML generation"""
        try:
            voice_message = voice_service.get_voice_message(self.tenant.id, voice_message_id)
            if not voice_message:
                raise ValueError("Voice message not found")

            twiml_content = self._generate_twiml(voice_message, variables or {})
            xml_id = await self._store_voice_xml(voice_message_id, twiml_content)

            # This needs to be a publicly accessible URL
            webhook_url = f"https://your-app-domain.com/webhooks/twilio/voice-xml/{xml_id}"

            call = self.client.calls.create(
                to=to_number,
                from_=self.from_number,
                url=webhook_url,
                method='GET'
            )

            await self._log_communication(
                network_type="call",
                to_number=to_number,
                message_content=voice_message.voice_text,
                twilio_sid=call.sid,
                status="initiated"
            )

            return {"call_sid": call.sid, "status": call.status}

        except Exception as e:
            logger.error(f"Error creating voice call for tenant {self.tenant.id}: {e}")
            await self._log_error("twilio_call", str(e), {"to_number": to_number, "voice_message_id": voice_message_id})
            raise

    async def send_sms(self, to_number: str, message_content: str,
                      variables: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """Send SMS message with variable substitution"""
        try:
            processed_message = self._process_message_variables(message_content, variables or {})

            message = self.client.messages.create(
                body=processed_message,
                from_=self.from_number,
                to=to_number
            )

            await self._log_communication(
                network_type="sms",
                to_number=to_number,
                message_content=processed_message,
                twilio_sid=message.sid,
                status="sent"
            )

            return {"message_sid": message.sid, "status": message.status}

        except Exception as e:
            logger.error(f"Error sending SMS for tenant {self.tenant.id}: {e}")
            await self._log_error("twilio_sms", str(e), {"to_number": to_number})
            raise

    def _generate_twiml(self, voice_message: VoiceMessageResponse, variables: Dict[str, Any]) -> str:
        processed_text = self._process_voice_text(voice_message.voice_text, variables)
        response = VoiceResponse()
        response.say(processed_text, voice=voice_message.voice_gender.value, rate=voice_message.voice_rate.value)
        return str(response)

    def _process_voice_text(self, voice_text: str, variables: Dict[str, Any]) -> str:
        # A simple variable substitution
        for key, value in variables.items():
            voice_text = voice_text.replace(f"{{{key}}}", str(value))
        return voice_text

    def _process_message_variables(self, message: str, variables: Dict[str, Any]) -> str:
        for key, value in variables.items():
            message = message.replace(f"{{{key}}}", str(value))
        return message

    async def _store_voice_xml(self, voice_message_id: int, xml_content: str) -> int:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "INSERT INTO voice_xml (tenant_id, xml_content, voice_message_id) VALUES (?, ?, ?)",
                (self.tenant.id, xml_content, voice_message_id)
            )
            xml_id = cursor.lastrowid
            conn.commit()
            return xml_id

    async def _log_communication(self, network_type: str, to_number: str, message_content: str, twilio_sid: str, status: str):
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                """
                INSERT INTO communication_log (tenant_id, network_type, to_number, from_number, message_content, twilio_sid, status, created_at)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (self.tenant.id, network_type, to_number, self.from_number, message_content, twilio_sid, status, datetime.utcnow())
            )
            conn.commit()

    async def _log_error(self, error_type: str, error_message: str, error_details: Dict[str, Any]):
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "INSERT INTO error_log (tenant_id, error_type, error_message, error_details, created_at) VALUES (?, ?, ?, ?, ?)",
                (self.tenant.id, error_type, error_message, json.dumps(error_details), datetime.utcnow())
            )
            conn.commit()

class TwilioWebhookService:
    """Service for handling Twilio webhooks"""

    def __init__(self):
        self.db_manager = DatabaseManager()

    def get_voice_xml(self, xml_id: int) -> Optional[str]:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute("SELECT xml_content FROM voice_xml WHERE id = ? AND is_active = 1", (xml_id,))
            row = cursor.fetchone()
            return row['xml_content'] if row else None

    async def handle_call_status(self, tenant_id: str, call_sid: str, call_status: str, call_duration: Optional[int] = None):
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            update_fields = ["status = ?"]
            values = [call_status]
            if call_duration is not None:
                update_fields.append("duration_seconds = ?")
                values.append(call_duration)

            values.extend([tenant_id, call_sid])
            cursor.execute(f"UPDATE communication_log SET {', '.join(update_fields)} WHERE tenant_id = ? AND twilio_sid = ?", tuple(values))
            conn.commit()
            logger.info(f"Updated call status for SID {call_sid} to {call_status}")

    async def handle_sms_status(self, tenant_id: str, message_sid: str, message_status: str):
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute("UPDATE communication_log SET status = ? WHERE tenant_id = ? AND twilio_sid = ?", (message_status, tenant_id, message_sid))
            conn.commit()
            logger.info(f"Updated SMS status for SID {message_sid} to {message_status}")

# Global webhook service instance
webhook_service = TwilioWebhookService()
