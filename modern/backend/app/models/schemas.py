from pydantic import BaseModel, Field, validator
from typing import Optional, List, Dict, Any
from datetime import datetime
from enum import Enum

# Enums
class NetworkType(str, Enum):
    CALL = "call"
    SMS = "sms"

class VoiceGender(str, Enum):
    ALICE = "alice"
    BOB = "bob"
    WOMAN = "woman"
    MAN = "man"

class VoiceRate(str, Enum):
    SLOW = "slow"
    MEDIUM = "medium"
    FAST = "fast"

class FileType(str, Enum):
    MP3 = "mp3"
    WAV = "wav"
    M4A = "m4a"
    XML = "xml"

# Base Models
class BaseSchema(BaseModel):
    class Config:
        from_attributes = True
        json_encoders = {
            datetime: lambda v: v.isoformat()
        }

# Tenant Models
class TenantBase(BaseSchema):
    name: str = Field(..., min_length=1, max_length=100)
    twilio_sid: Optional[str] = None
    twilio_token: Optional[str] = None
    from_number: Optional[str] = Field(None, pattern=r'^\+[1-9]\d{1,14}$')
    settings: Optional[Dict[str, Any]] = None

class TenantCreate(TenantBase):
    id: str = Field(..., min_length=1, max_length=50, pattern=r'^[a-z0-9-]+$')
    api_key: str = Field(..., min_length=20)

class TenantUpdate(BaseSchema):
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    twilio_sid: Optional[str] = None
    twilio_token: Optional[str] = None
    from_number: Optional[str] = Field(None, pattern=r'^\+[1-9]\d{1,14}$')
    settings: Optional[Dict[str, Any]] = None

class TenantResponse(TenantBase):
    id: str
    api_key: str
    is_active: bool
    created_at: datetime
    updated_at: datetime

# Voice Message Models
class VoiceMessageBase(BaseSchema):
    name: str = Field(..., min_length=1, max_length=100)
    voice_text: str = Field(..., min_length=1)
    voice_gender: VoiceGender = VoiceGender.ALICE
    voice_age: int = Field(25, ge=1, le=100)
    voice_rate: VoiceRate = VoiceRate.MEDIUM
    tropo_voice: Optional[str] = None
    voice_type: str = Field("message", max_length=50)
    voice_settings: Optional[Dict[str, Any]] = None

class VoiceMessageCreate(VoiceMessageBase):
    pass

class VoiceMessageUpdate(BaseSchema):
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    voice_text: Optional[str] = Field(None, min_length=1)
    voice_gender: Optional[VoiceGender] = None
    voice_age: Optional[int] = Field(None, ge=1, le=100)
    voice_rate: Optional[VoiceRate] = None
    tropo_voice: Optional[str] = None
    voice_type: Optional[str] = Field(None, max_length=50)
    voice_settings: Optional[Dict[str, Any]] = None

class VoiceMessageResponse(VoiceMessageBase):
    id: int
    tenant_id: str
    is_active: bool
    created_at: datetime
    updated_at: datetime

# File Models
class VoiceFileBase(BaseSchema):
    filename: str = Field(..., min_length=1, max_length=255)
    file_type: FileType
    description: Optional[str] = Field(None, max_length=500)
    tag_list: Optional[str] = Field(None, max_length=500)
    caller_number: Optional[str] = Field(None, pattern=r'^\+[1-9]\d{1,14}$')

class VoiceFileCreate(VoiceFileBase):
    file_path: str
    file_size: int = Field(..., ge=0)

class VoiceFileUpdate(BaseSchema):
    description: Optional[str] = Field(None, max_length=500)
    tag_list: Optional[str] = Field(None, max_length=500)
    caller_number: Optional[str] = Field(None, pattern=r'^\+[1-9]\d{1,14}$')

class VoiceFileResponse(VoiceFileBase):
    id: int
    tenant_id: str
    file_path: str
    file_size: int
    is_active: bool
    created_at: datetime
    updated_at: datetime

# Communication Models
class SMSCreate(BaseSchema):
    to_number: str = Field(..., pattern=r'^\+[1-9]\d{1,14}$')
    message: str = Field(..., min_length=1, max_length=1600)

class VoiceCallCreate(BaseSchema):
    to_number: str = Field(..., pattern=r'^\+[1-9]\d{1,14}$')
    voice_message_id: int = Field(..., ge=1)

class CommunicationLogResponse(BaseSchema):
    id: int
    tenant_id: str
    network_type: NetworkType
    to_number: str
    from_number: str
    message_content: Optional[str]
    twilio_sid: Optional[str]
    status: Optional[str]
    error_message: Optional[str]
    xml_filename: Optional[str]
    cost_amount: Optional[float]
    duration_seconds: Optional[int]
    created_at: datetime

# API Response Models
class APIResponse(BaseSchema):
    success: bool = True
    message: str = "Operation completed successfully"
    timestamp: datetime = Field(default_factory=datetime.utcnow)

class APIResponseWithData(APIResponse):
    data: Any

class APIError(BaseSchema):
    success: bool = False
    error: Dict[str, Any]
    timestamp: datetime = Field(default_factory=datetime.utcnow)

class PaginationMeta(BaseSchema):
    page: int = Field(..., ge=1)
    per_page: int = Field(..., ge=1, le=100)
    total_items: int = Field(..., ge=0)
    total_pages: int = Field(..., ge=0)
    has_next: bool
    has_prev: bool

class PaginatedResponse(APIResponse):
    data: List[Any]
    pagination: PaginationMeta

# File Upload Models
class FileUploadResponse(APIResponse):
    data: Dict[str, Any] = Field(..., description="File information")
