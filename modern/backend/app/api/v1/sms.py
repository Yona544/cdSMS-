from fastapi import APIRouter, Depends, status, HTTPException
 
from app.schemas.sms import SendSMSRequest, SendSMSResponse
from app.core.security import get_current_tenant
from app.models.schemas import TenantResponse
from app.services.twilio_service import TwilioService
from app.core.database import db_manager
 
router = APIRouter(prefix="/sms", tags=["SMS"])
 
@router.post(
    "/send",
    response_model=SendSMSResponse,
    status_code=status.HTTP_202_ACCEPTED,
    summary="Send an SMS using a stored template",
    description="Renders a template with variables and sends the SMS via Twilio.",
    responses={
        status.HTTP_202_ACCEPTED: {
            "description": "SMS has been accepted for processing.",
            "content": {
                "application/json": {
                    "example": {"message_id": "SMxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "status": "queued"}
                }
            },
        },
        status.HTTP_400_BAD_REQUEST: {"description": "Missing Twilio credentials or template."},
        status.HTTP_401_UNAUTHORIZED: {"description": "Invalid or missing API key."},
        status.HTTP_404_NOT_FOUND: {"description": "Template not found."},
        status.HTTP_500_INTERNAL_SERVER_ERROR: {"description": "Failed to send via Twilio."},
    },
)
async def send_sms(
    payload: SendSMSRequest,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """
    Sends an SMS using a stored template.
 
    - Validates the request against `SendSMSRequest` schema.
    - Authenticates the tenant via Authorization bearer token or X-API-Key header.
    - Looks up the template by `template_id` for the tenant.
    - Performs variable substitution and sends the SMS via Twilio.
    """
    # Ensure Twilio is configured for this tenant
    if not tenant.twilio_sid or not tenant.twilio_token or not tenant.from_number:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="Twilio credentials are not configured for this tenant."
        )
 
    # Lookup the template for this tenant
    with db_manager.get_connection() as conn:
        cursor = conn.cursor()
        cursor.execute(
            """
            SELECT content FROM sms_templates
            WHERE tenant_id = ? AND template_id = ? AND is_active = 1
            """,
            (tenant.id, payload.template_id),
        )
        row = cursor.fetchone()
 
    if not row:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Template not found or inactive."
        )
 
    template_content = row["content"]
 
    try:
        twilio = TwilioService(tenant)
        result = await twilio.send_sms(
            to_number=payload.to,
            message_content=template_content,
            variables=payload.variables,
        )
        # Map Twilio result to response schema
        return SendSMSResponse(message_id=result.get("message_sid", ""), status=result.get("status", "queued"))
    except HTTPException:
        raise
    except Exception as e:
        # Surface Twilio errors as 500
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=str(e))
