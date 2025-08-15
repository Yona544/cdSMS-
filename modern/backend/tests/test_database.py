import pytest
import tempfile
import os
from app.core.database import DatabaseManager

@pytest.fixture
def temp_db():
    """Create temporary database for testing"""
    temp_dir = tempfile.mkdtemp()
    db_path = os.path.join(temp_dir, "test.db")
    db = DatabaseManager(db_path)
    yield db
    os.unlink(db_path)
    os.rmdir(temp_dir)

def test_database_creation(temp_db):
    """Test database and tables are created"""
    with temp_db.get_connection() as conn:
        cursor = conn.cursor()

        # Check that all tables exist
        cursor.execute("SELECT name FROM sqlite_master WHERE type='table'")
        tables = [row[0] for row in cursor.fetchall()]

        expected_tables = [
            'tenants', 'voice_messages', 'voice_xml',
            'voice_files', 'communication_log', 'error_log', 'users'
        ]

        for table in expected_tables:
            assert table in tables

def test_database_indexes(temp_db):
    """Test that performance indexes are created"""
    with temp_db.get_connection() as conn:
        cursor = conn.cursor()
        cursor.execute("SELECT name FROM sqlite_master WHERE type='index'")
        indexes = [row[0] for row in cursor.fetchall()]

        assert 'idx_tenants_api_key' in indexes
        assert 'idx_voice_messages_tenant' in indexes
