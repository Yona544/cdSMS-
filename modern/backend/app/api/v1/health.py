from fastapi import APIRouter, Depends, status
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import text

from app.core.db import get_session

router = APIRouter(prefix="/health", tags=["Health"])

@router.get(
    "/healthz",
    status_code=status.HTTP_200_OK,
    summary="Liveness probe",
    description="A simple endpoint to verify that the API server is running and responsive."
)
async def healthz():
    """
    Liveness probe endpoint.
    """
    return {"status": "ok"}

@router.get(
    "/readyz",
    status_code=status.HTTP_200_OK,
    summary="Readiness probe",
    description="An endpoint to verify that the API is ready to serve requests, including database connectivity."
)
async def readyz(session: AsyncSession = Depends(get_session)):
    """
    Readiness probe endpoint. Checks database connectivity.
    """
    # This is a lightweight query to check DB connection.
    await session.execute(text("SELECT 1"))
    return {"status": "ready"}
