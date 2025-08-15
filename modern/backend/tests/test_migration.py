import pytest
import tempfile
import os
from app.utils.migration import AccessToSQLiteMigrator
from app.core.database import DatabaseManager

def test_sample_tenant_creation():
    """Test that sample tenants are created correctly"""
    temp_dir = tempfile.mkdtemp()
    db_path = os.path.join(temp_dir, "test_migration.db")

    # Initialize database
    db_manager = DatabaseManager(db_path)

    # Run migration
    migrator = AccessToSQLiteMigrator(db_path)
    success = migrator.migrate_from_legacy_data("dummy_path")

    assert success

    # Verify tenants were created
    with db_manager.get_connection() as conn:
        cursor = conn.cursor()
        cursor.execute("SELECT COUNT(*) FROM tenants")
        tenant_count = cursor.fetchone()[0]
        assert tenant_count >= 2

        # Verify voice messages were created
        cursor.execute("SELECT COUNT(*) FROM voice_messages")
        message_count = cursor.fetchone()[0]
        assert message_count >= 6  # 3 messages per tenant

    # Cleanup
    os.unlink(db_path)
    os.rmdir(temp_dir)
