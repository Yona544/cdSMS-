from pydantic import BaseModel, Field

class SendSMSRequest(BaseModel):
    """
    Request model for sending an SMS.
    """
    to: str = Field(..., description="The recipient's phone number in E.164 format.", example="+15551234567")
    template_id: str = Field(..., description="The ID of the message template to use.", example="template-123")
    variables: dict[str, str] = Field(default_factory=dict, description="Key-value pairs for template variable substitution.", example={"name": "Jules"})

class SendSMSResponse(BaseModel):
    """
    Response model after successfully queuing an SMS for sending.
    """
    message_id: str = Field(..., description="A unique identifier for the message.", example="SMxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
    status: str = Field(..., description="The initial status of the message.", example="queued")
