import pytest
import os
from app.core.database import DatabaseManager
from app.utils.migration import AccessToSQLiteMigrator

@pytest.fixture(scope="session")
def seeded_db():
    """
    This fixture creates and seeds a database for the test session.
    It uses the DATABASE_PATH from the environment (set by pytest.ini).
    """
    # This path is set by the pytest.ini file
    db_path = os.environ.get("DATABASE_PATH", "/tmp/test_default.db")

    # Ensure the db file is clean before the session
    if os.path.exists(db_path):
        os.unlink(db_path)

    # Initialize the database schema
    db_manager = DatabaseManager(db_path=db_path)

    # Seed the database with sample data
    migrator = AccessToSQLiteMigrator(sqlite_db_path=db_path)
    migrator.migrate_from_legacy_data("dummy_path")

    yield db_manager

    # Teardown: remove the test database file
    if os.path.exists(db_path):
        os.unlink(db_path)

@pytest.fixture(scope="session")
def client(seeded_db):
    """
    A TestClient fixture that depends on the database being seeded.
    """
    from app.main import app
    from fastapi.testclient import TestClient

    yield TestClient(app)
