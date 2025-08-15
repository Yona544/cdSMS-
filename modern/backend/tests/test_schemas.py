import pytest
from pydantic import ValidationError
from app.models.schemas import (
    VoiceMessageCreate, TenantCreate, SMSCreate, VoiceCallCreate
)

def test_voice_message_create_valid():
    """Test valid voice message creation"""
    data = {
        "name": "Test Message",
        "voice_text": "Hello world",
        "voice_gender": "alice",
        "voice_rate": "medium"
    }

    message = VoiceMessageCreate(**data)
    assert message.name == "Test Message"
    assert message.voice_gender == "alice"

def test_voice_message_create_invalid():
    """Test invalid voice message creation"""
    with pytest.raises(ValidationError):
        VoiceMessageCreate(name="", voice_text="")

def test_tenant_create_valid():
    """Test valid tenant creation"""
    data = {
        "id": "test-tenant",
        "name": "Test Tenant",
        "api_key": "test_api_key_12345678901234567890",
        "from_number": "+1234567890"
    }

    tenant = TenantCreate(**data)
    assert tenant.id == "test-tenant"
    assert tenant.from_number == "+1234567890"

def test_sms_create_valid():
    """Test valid SMS creation"""
    data = {
        "to_number": "+1234567890",
        "message": "Test SMS message"
    }

    sms = SMSCreate(**data)
    assert sms.to_number == "+1234567890"
