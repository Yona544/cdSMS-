from fastapi import Depends, Header, HTTPException, status
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.future import select
import hashlib

from app.core.db import get_session
from app.models.tenant import Tenant

async def get_tenant(
    x_api_key: str | None = Header(None, alias="X-API-Key"),
    session: AsyncSession = Depends(get_session),
) -> Tenant:
    """
    Dependency to get the current tenant based on the X-API-Key header.
    """
    if not x_api_key:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Missing X-API-Key header"
        )

    # Hash the provided API key to compare with the stored hash
    key_hash = hashlib.sha256(x_api_key.encode()).hexdigest()

    # Query for an active tenant with the matching API key hash
    query = select(Tenant).where(
        Tenant.api_key_hash == key_hash,
        Tenant.active == True
    )
    result = await session.execute(query)
    tenant = result.scalar_one_or_none()

    if not tenant:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Invalid or inactive API key"
        )

    return tenant
