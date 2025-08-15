from pydantic_settings import BaseSettings
from typing import List, Optional
import os

class Settings(BaseSettings):
    # Application
    app_name: str = "Twilio Multi-Tenant Manager"
    app_version: str = "1.0.0"
    debug: bool = False
    log_level: str = "INFO"

    # Database
    database_path: str = "/data/tenants.db"
    database_backup_path: str = "/data/backups"

    # Security
    secret_key: str
    allowed_origins: List[str] = ["http://localhost:3000"]

    # File Storage
    upload_path: str = "/data/files"
    max_file_size: int = 10485760  # 10MB
    allowed_extensions: List[str] = ["mp3", "wav", "m4a", "xml"]

    # Server
    host: str = "0.0.0.0"
    port: int = 8000
    workers: int = 1

    class Config:
        env_file = ".env"
        case_sensitive = False

settings = Settings()
