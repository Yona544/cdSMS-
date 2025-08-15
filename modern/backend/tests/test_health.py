import pytest
from httpx import AsyncClient
from fastapi import status

pytestmark = pytest.mark.asyncio

async def test_healthz(async_client: AsyncClient):
    """
    Tests the /v1/health/healthz endpoint for a simple 200 OK response.
    """
    response = await async_client.get("/v1/health/healthz")
    assert response.status_code == status.HTTP_200_OK
    assert response.json() == {"status": "ok"}

async def test_readyz(async_client: AsyncClient):
    """
    Tests the /v1/health/readyz endpoint for a successful DB connection.
    """
    response = await async_client.get("/v1/health/readyz")
    assert response.status_code == status.HTTP_200_OK
    assert response.json() == {"status": "ready"}
