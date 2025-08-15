from functools import lru_cache
from pydantic_settings import BaseSettings, SettingsConfigDict
from typing import List

class Settings(BaseSettings):
    """
    Application settings, loaded from environment variables and .env file.
    """
    env: str = "dev"
    api_base_path: str = "/v1"
    database_url: str = "sqlite+aiosqlite:///./data/app.db"
    cors_allow_origins: List[str] = ["http://localhost:5173"]
    prometheus_enabled: bool = True
    secret_key: str = "CHANGE_ME"  # Example only; override via env

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
