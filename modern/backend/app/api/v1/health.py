from fastapi import APIRouter, status
from app.core.database import db_manager
 
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
async def readyz():
    """
    Readiness probe endpoint. Checks database connectivity using the sqlite DatabaseManager.
    """
    with db_manager.get_connection() as conn:
        cursor = conn.cursor()
        cursor.execute("SELECT 1")
        cursor.fetchone()
    return {"status": "ready"}
