import sqlite3
import os
from typing import Generator, Optional
from contextlib import contextmanager
from app.core.config import settings
import logging

logger = logging.getLogger(__name__)

class DatabaseManager:
    def __init__(self, db_path: str = None):
        self.db_path = db_path or settings.database_path
        self.ensure_database_exists()
        self.init_schema()

    def ensure_database_exists(self):
        """Ensure database directory and file exist"""
        os.makedirs(os.path.dirname(self.db_path), exist_ok=True)
        if not os.path.exists(self.db_path):
            # Create empty database file
            open(self.db_path, 'a').close()

    @contextmanager
    def get_connection(self) -> Generator[sqlite3.Connection, None, None]:
        """Get database connection with automatic cleanup"""
        conn = sqlite3.connect(self.db_path)
        conn.row_factory = sqlite3.Row  # Enable column access by name
        try:
            yield conn
        except Exception as e:
            conn.rollback()
            logger.error(f"Database error: {e}")
            raise
        finally:
            conn.close()

    def init_schema(self):
        """Initialize database schema"""
        with self.get_connection() as conn:
            cursor = conn.cursor()

            # Create all tables
            self._create_tenants_table(cursor)
            self._create_voice_messages_table(cursor)
            self._create_voice_xml_table(cursor)
            self._create_voice_files_table(cursor)
            self._create_communication_log_table(cursor)
            self._create_error_log_table(cursor)
            self._create_users_table(cursor)
            self._create_indexes(cursor)

            conn.commit()
            logger.info("Database schema initialized successfully")

    def _create_tenants_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS tenants (
                id TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                api_key TEXT UNIQUE NOT NULL,
                twilio_sid TEXT,
                twilio_token TEXT,
                from_number TEXT,
                is_active BOOLEAN DEFAULT 1,
                settings JSON,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
            )
        """)

    def _create_voice_messages_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS voice_messages (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT NOT NULL,
                name TEXT NOT NULL,
                voice_text TEXT NOT NULL,
                voice_gender TEXT DEFAULT 'alice',
                voice_age INTEGER DEFAULT 25,
                voice_rate TEXT DEFAULT 'medium',
                tropo_voice TEXT,
                voice_type TEXT DEFAULT 'message',
                voice_settings JSON,
                is_active BOOLEAN DEFAULT 1,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            )
        """)

    def _create_voice_xml_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS voice_xml (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT NOT NULL,
                xml_content TEXT NOT NULL,
                xml_filename TEXT,
                voice_message_id INTEGER,
                is_active BOOLEAN DEFAULT 1,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                FOREIGN KEY (voice_message_id) REFERENCES voice_messages(id) ON DELETE SET NULL
            )
        """)

    def _create_voice_files_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS voice_files (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT NOT NULL,
                filename TEXT NOT NULL,
                file_type TEXT NOT NULL,
                file_path TEXT NOT NULL,
                file_size INTEGER,
                description TEXT,
                tag_list TEXT,
                caller_number TEXT,
                is_active BOOLEAN DEFAULT 1,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            )
        """)

    def _create_communication_log_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS communication_log (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT NOT NULL,
                network_type TEXT NOT NULL,
                to_number TEXT NOT NULL,
                from_number TEXT NOT NULL,
                message_content TEXT,
                twilio_sid TEXT,
                status TEXT,
                error_message TEXT,
                xml_filename TEXT,
                cost_amount DECIMAL(10,4),
                duration_seconds INTEGER,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            )
        """)

    def _create_error_log_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS error_log (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT,
                error_type TEXT NOT NULL,
                error_message TEXT NOT NULL,
                error_details JSON,
                endpoint TEXT,
                user_agent TEXT,
                ip_address TEXT,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE SET NULL
            )
        """)

    def _create_users_table(self, cursor):
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tenant_id TEXT NOT NULL,
                username TEXT NOT NULL,
                password_hash TEXT NOT NULL,
                display_.py
                display_name TEXT NOT NULL,
                email TEXT,
                permissions JSON,
                is_admin BOOLEAN DEFAULT 0,
                is_active BOOLEAN DEFAULT 1,
                last_login_at DATETIME,
                created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                UNIQUE(tenant_id, username)
            )
        """)

    def _create_indexes(self, cursor):
        """Create performance indexes"""
        indexes = [
            "CREATE INDEX IF NOT EXISTS idx_tenants_api_key ON tenants(api_key)",
            "CREATE INDEX IF NOT EXISTS idx_voice_messages_tenant ON voice_messages(tenant_id, is_active)",
            "CREATE INDEX IF NOT EXISTS idx_voice_xml_tenant ON voice_xml(tenant_id, is_active)",
            "CREATE INDEX IF NOT EXISTS idx_voice_files_tenant ON voice_files(tenant_id, is_active)",
            "CREATE INDEX IF NOT EXISTS idx_communication_log_tenant_date ON communication_log(tenant_id, created_at DESC)",
            "CREATE INDEX IF NOT EXISTS idx_error_log_tenant_date ON error_log(tenant_id, created_at DESC)",
            "CREATE INDEX IF NOT EXISTS idx_users_tenant_username ON users(tenant_id, username)",
        ]

        for index_sql in indexes:
            cursor.execute(index_sql)

# Initialize database instance
db_manager = DatabaseManager()
