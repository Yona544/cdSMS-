import pytest
from httpx import AsyncClient
from fastapi import status

from app.models.tenant import Tenant

pytestmark = pytest.mark.asyncio

async def test_send_sms_requires_auth(async_client: AsyncClient):
    """
    Tests that the /v1/sms/send endpoint returns 401 without an API key.
    """
    payload = {"to": "+15551234567", "template_id": "test", "variables": {}}
    response = await async_client.post("/v1/sms/send", json=payload)
    assert response.status_code == status.HTTP_401_UNAUTHORIZED

async def test_send_sms_with_invalid_auth(async_client: AsyncClient):
    """
    Tests that the /v1/sms/send endpoint returns 401 with an invalid API key.
    """
    payload = {"to": "+15551234567", "template_id": "test", "variables": {}}
    headers = {"X-API-Key": "invalid-key"}
    response = await async_client.post("/v1/sms/send", json=payload, headers=headers)
    assert response.status_code == status.HTTP_401_UNAUTHORIZED

async def test_send_sms_happy_path(async_client: AsyncClient, test_tenant: Tenant):
    """
    Tests the happy path for sending an SMS with a valid API key and payload.
    """
    payload = {
        "to": "+15551234567",
        "template_id": "welcome_email",
        "variables": {"name": "Test User"},
    }
    headers = {"X-API-Key": test_tenant.raw_api_key}

    response = await async_client.post("/v1/sms/send", json=payload, headers=headers)

    assert response.status_code == status.HTTP_202_ACCEPTED
    data = response.json()
    assert data["status"] == "queued"
    assert "message_id" in data
