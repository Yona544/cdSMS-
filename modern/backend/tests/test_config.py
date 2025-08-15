import pytest
from app.core.config import settings

def test_settings_loaded():
    assert settings.app_name is not None
    assert settings.database_path is not None
    assert len(settings.secret_key) > 10
