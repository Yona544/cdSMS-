from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from prometheus_fastapi_instrumentator import Instrumentator

from app.core.settings import get_settings
from app.api.v1 import health as health_router
from app.api.v1 import sms as sms_router

# Get settings instance
settings = get_settings()

# Create FastAPI app instance
app = FastAPI(
    title="cdSMS API",
    version="1.0.0",
    description="A modern, multi-tenant replacement for a legacy ASP.NET application."
)

# Set up CORS middleware
if settings.cors_allow_origins:
    app.add_middleware(
        CORSMiddleware,
        allow_origins=[str(origin) for origin in settings.cors_allow_origins],
        allow_credentials=True,
        allow_methods=["*"],
        allow_headers=["*"],
    )

# Set up Prometheus metrics
if settings.prometheus_enabled:
    Instrumentator().instrument(app).expose(app)

# Placeholder for root endpoint
@app.get("/", tags=["Root"])
async def read_root():
    return {"message": "Welcome to the cdSMS API"}

# Include v1 routers
app.include_router(health_router.router, prefix=settings.api_base_path)
app.include_router(sms_router.router, prefix=settings.api_base_path)
