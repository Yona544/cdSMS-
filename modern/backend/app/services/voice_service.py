import sqlite3
from typing import List, Tuple, Optional
from app.core.database import DatabaseManager
from app.models.schemas import VoiceMessageCreate, VoiceMessageUpdate, VoiceMessageResponse

class VoiceService:
    def __init__(self):
        # This will instantiate a new manager, which is not ideal for connection pooling
        # but necessary given the issues with a global db_manager instance.
        self.db_manager = DatabaseManager()

    def _row_to_response(self, row: sqlite3.Row) -> VoiceMessageResponse:
        return VoiceMessageResponse.model_validate(dict(row))

    def create_voice_message(self, tenant_id: str, message_data: VoiceMessageCreate) -> VoiceMessageResponse:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                """
                INSERT INTO voice_messages (tenant_id, name, voice_text, voice_gender, voice_age, voice_rate, tropo_voice, voice_type, voice_settings)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
                """,
                (
                    tenant_id,
                    message_data.name,
                    message_data.voice_text,
                    message_data.voice_gender.value,
                    message_data.voice_age,
                    message_data.voice_rate.value,
                    message_data.tropo_voice,
                    message_data.voice_type,
                    None # message_data.voice_settings (json)
                ),
            )
            new_id = cursor.lastrowid
            conn.commit()

        return self.get_voice_message(tenant_id, new_id)

    def list_voice_messages(self, tenant_id: str, limit: int, offset: int) -> Tuple[List[VoiceMessageResponse], int]:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()

            # Get total count
            cursor.execute("SELECT COUNT(*) FROM voice_messages WHERE tenant_id = ?", (tenant_id,))
            total_count = cursor.fetchone()[0]

            # Get paginated results
            cursor.execute(
                "SELECT * FROM voice_messages WHERE tenant_id = ? ORDER BY created_at DESC LIMIT ? OFFSET ?",
                (tenant_id, limit, offset),
            )
            messages = [self._row_to_response(row) for row in cursor.fetchall()]
            return messages, total_count

    def get_voice_message(self, tenant_id: str, message_id: int) -> Optional[VoiceMessageResponse]:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "SELECT * FROM voice_messages WHERE id = ? AND tenant_id = ?",
                (message_id, tenant_id),
            )
            row = cursor.fetchone()
            return self._row_to_response(row) if row else None

    def update_voice_message(self, tenant_id: str, message_id: int, update_data: VoiceMessageUpdate) -> Optional[VoiceMessageResponse]:
        update_fields = {k: v for k, v in update_data.model_dump(exclude_unset=True).items()}
        if not update_fields:
            raise ValueError("No update data provided")

        # Convert enums to their string values for the query
        if 'voice_gender' in update_fields:
            update_fields['voice_gender'] = update_fields['voice_gender'].value
        if 'voice_rate' in update_fields:
            update_fields['voice_rate'] = update_fields['voice_rate'].value

        set_clause = ", ".join([f"{field} = ?" for field in update_fields.keys()])

        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                f"UPDATE voice_messages SET {set_clause} WHERE id = ? AND tenant_id = ?",
                (*update_fields.values(), message_id, tenant_id),
            )
            conn.commit()

        return self.get_voice_message(tenant_id, message_id)

    def delete_voice_message(self, tenant_id: str, message_id: int) -> bool:
        with self.db_manager.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute(
                "DELETE FROM voice_messages WHERE id = ? AND tenant_id = ?",
                (message_id, tenant_id),
            )
            conn.commit()
            return cursor.rowcount > 0

    def duplicate_voice_message(self, tenant_id: str, message_id: int) -> VoiceMessageResponse:
        original = self.get_voice_message(tenant_id, message_id)
        if not original:
            raise ValueError("Voice message not found")

        # Create a new message object from the original, but change the name
        new_name = f"Copy of {original.name}"

        # Create a VoiceMessageCreate object from the original message
        duplicate_data = VoiceMessageCreate(
            name=new_name,
            voice_text=original.voice_text,
            voice_gender=original.voice_gender,
            voice_age=original.voice_age,
            voice_rate=original.voice_rate,
            tropo_voice=original.tropo_voice,
            voice_type=original.voice_type,
            voice_settings=original.voice_settings
        )

        return self.create_voice_message(tenant_id, duplicate_data)

# Global service instance
voice_service = VoiceService()
