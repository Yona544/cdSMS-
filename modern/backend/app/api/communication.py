from fastapi import APIRouter, Depends, HTTPException, status, Query
from typing import List

from app.models.schemas import (
    SMSCreate, VoiceCallCreate, TenantResponse, APIResponseWithData,
    PaginatedResponse, PaginationMeta, CommunicationLogResponse
)
from app.core.security import get_current_tenant
from app.services.twilio_service import TwilioService
from app.services.communication_service import communication_service

router = APIRouter(prefix="/communication", tags=["Communication"])

@router.post("/voice/send", response_model=APIResponseWithData)
async def send_voice_call(
    call_data: VoiceCallCreate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Initiate a new voice call."""
    try:
        twilio_service = TwilioService(tenant)
        result = await twilio_service.create_voice_call(
            to_number=call_data.to_number,
            voice_message_id=call_data.voice_message_id
        )
        return APIResponseWithData(data=result, message="Voice call initiated successfully")
    except Exception as e:
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=str(e))

@router.post("/sms/send", response_model=APIResponseWithData)
async def send_sms_message(
    sms_data: SMSCreate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Send a new SMS message."""
    try:
        twilio_service = TwilioService(tenant)
        result = await twilio_service.send_sms(
            to_number=sms_data.to_number,
            message_content=sms_data.message
        )
        return APIResponseWithData(data=result, message="SMS sent successfully")
    except Exception as e:
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=str(e))

@router.get("/history", response_model=PaginatedResponse)
async def get_communication_history(
    page: int = Query(1, ge=1),
    per_page: int = Query(20, ge=1, le=100),
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Get the communication history for the tenant."""
    logs, total_count = communication_service.list_communication_history(
        tenant.id, limit=per_page, offset=(page - 1) * per_page
    )
    total_pages = (total_count + per_page - 1) // per_page

    return PaginatedResponse(
        data=logs,
        message=f"Retrieved {len(logs)} communication logs",
        pagination=PaginationMeta(
            page=page,
            per_page=per_page,
            total_items=total_count,
            total_pages=total_pages,
            has_next=page < total_pages,
            has_prev=page > 1,
        ),
    )

@router.get("/status/{twilio_sid}", response_model=APIResponseWithData)
async def get_communication_status(
    twilio_sid: str,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Get the status of a specific communication by its Twilio SID."""
    log = communication_service.get_communication_status(tenant.id, twilio_sid)
    if not log:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Communication log not found for the given SID")
    return APIResponseWithData(data=log)
