from fastapi import HTTPException, status, Depends, Request
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from typing import Optional
import logging
from app.core.database import db_manager
from app.models.schemas import TenantResponse

logger = logging.getLogger(__name__)

security = HTTPBearer(auto_error=False)

class TenantAuth:
    """Handle tenant authentication and context"""

    def __init__(self):
        self.db = db_manager

    def get_tenant_by_api_key(self, api_key: str) -> Optional[TenantResponse]:
        """Retrieve tenant by API key"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute("""
                SELECT id, name, api_key, twilio_sid, twilio_token,
                       from_number, is_active, settings, created_at, updated_at
                FROM tenants
                WHERE api_key = ? AND is_active = 1
            """, (api_key,))

            row = cursor.fetchone()
            if not row:
                return None

            # Pydantic models expect datetimes, not strings.
            # We need to convert them if they are not already datetimes.
            # However, sqlite3.Row with detect_types should handle this.
            # For safety, let's ensure conversion if needed, though it might be redundant.
            return TenantResponse.model_validate(dict(row))


    def authenticate_request(self, request: Request) -> TenantResponse:
        """Extract and validate API key from request"""
        api_key = None

        # Try Authorization header first
        auth_header = request.headers.get('Authorization')
        if auth_header and auth_header.startswith('Bearer '):
            api_key = auth_header[7:]  # Remove 'Bearer ' prefix

        # Try X-API-Key header
        if not api_key:
            api_key = request.headers.get('X-API-Key')

        # Try query parameter (for iframe URLs)
        if not api_key:
            api_key = request.query_params.get('apiKey')

        if not api_key:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="API key required. Provide via Authorization header, X-API-Key header, or apiKey query parameter."
            )

        tenant = self.get_tenant_by_api_key(api_key)
        if not tenant:
            logger.warning(f"Invalid API key attempt: {api_key[:10]}...")
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Invalid API key"
            )

        return tenant

# Global auth instance
tenant_auth = TenantAuth()

# FastAPI dependency
async def get_current_tenant(request: Request) -> TenantResponse:
    """FastAPI dependency to get current authenticated tenant"""
    return tenant_auth.authenticate_request(request)

# Optional authentication for public endpoints
async def get_optional_tenant(request: Request) -> Optional[TenantResponse]:
    """FastAPI dependency for optional authentication"""
    try:
        return tenant_auth.authenticate_request(request)
    except HTTPException:
        return None
