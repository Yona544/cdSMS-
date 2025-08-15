from fastapi import FastAPI, Depends
from app.core.security import get_current_tenant
from app.models.schemas import TenantResponse
from app.api import voice as voice_router
from app.api import files as files_router
from app.api import communication as communication_router
from app.api import webhooks as webhooks_router

app = FastAPI()

# API routers that require authentication
app.include_router(voice_router.router, prefix="/api")
app.include_router(files_router.router, prefix="/api")
app.include_router(communication_router.router, prefix="/api")

# Webhook router does not have a prefix and has its own security (or lack thereof)
app.include_router(webhooks_router.router)

@app.get("/health")
async def health_check():
    return {"status": "ok"}

@app.get("/api/tenant/profile", response_model=TenantResponse)
async def get_tenant_profile(tenant: TenantResponse = Depends(get_current_tenant)):
    """
    An endpoint to test authentication.
    Returns the profile of the currently authenticated tenant.
    """
    return tenant
