from fastapi import APIRouter, Depends, HTTPException, status, Request, Form
from fastapi.responses import PlainTextResponse
from typing import Optional

from app.services.twilio_service import webhook_service

router = APIRouter(prefix="/webhooks", tags=["Webhooks"])

@router.post("/twilio/{tenant_id}/voice")
async def twilio_voice_webhook(
    request: Request,
    tenant_id: str,
    CallSid: str = Form(...),
    CallStatus: str = Form(...),
    CallDuration: Optional[str] = Form(None)
):
    """
    Handles voice call status updates from Twilio.
    This endpoint is called by Twilio, so it does not use our standard auth.
    Twilio signature validation should be added in a real application.
    """
    await webhook_service.handle_call_status(
        tenant_id=tenant_id,
        call_sid=CallSid,
        call_status=CallStatus,
        call_duration=int(CallDuration) if CallDuration else None
    )
    return PlainTextResponse("OK")

@router.post("/twilio/{tenant_id}/sms")
async def twilio_sms_webhook(
    request: Request,
    tenant_id: str,
    SmsSid: str = Form(...),
    SmsStatus: str = Form(...)
):
    """
    Handles SMS status updates from Twilio.
    """
    await webhook_service.handle_sms_status(
        tenant_id=tenant_id,
        message_sid=SmsSid,
        message_status=SmsStatus
    )
    return PlainTextResponse("OK")

@router.get("/twilio/voice-xml/{xml_id}", response_class=PlainTextResponse)
async def get_twilio_voice_xml(xml_id: int):
    """
    Serves the pre-generated TwiML to Twilio.
    This is the webhook URL provided to Twilio when making a call.
    """
    xml_content = webhook_service.get_voice_xml(xml_id)
    if not xml_content:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="TwiML not found")

    # Return as XML content type
    return PlainTextResponse(content=xml_content, media_type="application/xml")
