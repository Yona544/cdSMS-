from fastapi import APIRouter, Depends, status

from app.api.deps import get_tenant
from app.schemas.sms import SendSMSRequest, SendSMSResponse
from app.models.tenant import Tenant

router = APIRouter(prefix="/sms", tags=["SMS"])

@router.post(
    "/send",
    response_model=SendSMSResponse,
    status_code=status.HTTP_202_ACCEPTED,
    summary="Send an SMS using a stored template",
    description="Renders a template with variables and enqueues the SMS for delivery via a background task.",
    responses={
        status.HTTP_202_ACCEPTED: {
            "description": "SMS has been accepted for processing.",
            "content": {
                "application/json": {
                    "example": {"message_id": "SMxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "status": "queued"}
                }
            },
        },
        status.HTTP_401_UNAUTHORIZED: {"description": "Invalid or missing API key."},
    },
)
async def send_sms(
    payload: SendSMSRequest,
    tenant: Tenant = Depends(get_tenant)
):
    """
    This endpoint accepts a request to send an SMS.

    - It validates the request against the `SendSMSRequest` schema.
    - It authenticates the tenant via the `X-API-Key` header.
    - **TODO**: In a real implementation, this would trigger a background task.
    """
    # TODO: call service layer; enqueue work; return message id
    # For now, we return a stubbed response.
    return SendSMSResponse(message_id="stub-message-id", status="queued")
