from functools import lru_cache
from pydantic_settings import BaseSettings, SettingsConfigDict
from typing import List
 
class Settings(BaseSettings):
    """
    Consolidated application settings from the previous config.py and settings.py.
    Loaded from environment variables and .env file.
    """
    # App
    env: str = "dev"
    app_name: str = "Twilio Multi-Tenant Manager"
    app_version: str = "1.0.0"
    debug: bool = False
    log_level: str = "INFO"
 
    # API
    api_base_path: str = "/v1"
    cors_allow_origins: List[str] = ["http://localhost:5173"]
    prometheus_enabled: bool = True
    public_base_url: str = "http://localhost:8000"
 
    # Security
    secret_key: str = "CHANGE_ME"  # Override via env
 
    # Database (sqlite manager) and legacy async URL (kept for compatibility)
    database_path: str = "/data/tenants.db"
    database_backup_path: str = "/data/backups"
    database_url: str = "sqlite+aiosqlite:///./data/app.db"
 
    # File storage
    upload_path: str = "/data/files"
    max_file_size: int = 10485760  # 10MB
    allowed_extensions: List[str] = ["mp3", "wav", "m4a", "xml"]
 
    # Server
    host: str = "0.0.0.0"
    port: int = 8000
    workers: int = 1
 
    model_config = SettingsConfigDict(
        env_prefix="APP_",
        env_file=".env",
        extra="ignore",
    )
 
@lru_cache
def get_settings() -> Settings:
    """
    Returns a cached instance of the Settings class.
    """
    return Settings()
