# Implementation Roadmap: FastAPI + Vue.js Multi-Tenant Application
## Step-by-Step Development Guide for AI Agents

### 📋 Overview

This roadmap provides a systematic, phase-based approach to building the complete multi-tenant Twilio management system. Each phase contains specific tasks with dependencies, tools, and deliverable milestones designed for AI agent execution.

---

## 🚀 Phase 1: Project Setup & Environment Configuration
**Goal**: Establish development environment and project structure

### Task 1.1: Initialize Project Structure
**Dependencies**: None  
**Tools**: Git, Python 3.11+, Node.js 18+

```bash
# Create project structure
mkdir twilio-multi-tenant
cd twilio-multi-tenant

# Initialize git repository
git init
git remote add origin <your-repo-url>

# Create main directories
mkdir -p backend/app/{api,core,models,services,utils}
mkdir -p backend/tests/{unit,integration}
mkdir -p frontend/{src,public,dist}
mkdir -p docs
mkdir -p deployment/{docker,fly}
mkdir -p data/{sqlite,files,logs}

# Create configuration files
touch backend/requirements.txt
touch backend/.env.example
touch frontend/package.json
touch frontend/vite.config.js
touch README.md
touch .gitignore
```

**Deliverables**:
- ✅ Complete project directory structure
- ✅ Git repository initialized
- ✅ Basic configuration files created

### Task 1.2: Backend Environment Setup
**Dependencies**: Task 1.1  
**Tools**: Python, pip, virtual environment

```python
# backend/requirements.txt
fastapi==0.104.1
uvicorn[standard]==0.24.0
python-multipart==0.0.6
pydantic==2.5.0
pydantic-settings==2.1.0
python-jose[cryptography]==3.3.0
passlib[bcrypt]==1.7.4
python-decouple==3.8
twilio==8.12.0
aiofiles==23.2.1
jinja2==3.1.2
pytest==7.4.3
pytest-asyncio==0.21.1
httpx==0.25.2
black==23.11.0
isort==5.12.0
flake8==6.1.0

# For database operations
sqlite3  # Built into Python

# For file handling
pillow==10.1.0

# For development
python-dotenv==1.0.0
watchdog==3.0.0
```

```python
# backend/.env.example
# Application Configuration
APP_NAME="Twilio Multi-Tenant Manager"
APP_VERSION="1.0.0"
DEBUG=True
LOG_LEVEL="INFO"

# Database Configuration
DATABASE_PATH="/data/tenants.db"
DATABASE_BACKUP_PATH="/data/backups"

# Security Configuration
SECRET_KEY="your-secret-key-here"
ALLOWED_ORIGINS="http://localhost:3000,https://your-delphi-app.com"

# Twilio Configuration (default/testing)
DEFAULT_TWILIO_SID="your-default-twilio-sid"
DEFAULT_TWILIO_TOKEN="your-default-twilio-token"
DEFAULT_FROM_NUMBER="+1234567890"

# File Storage Configuration
UPLOAD_PATH="/data/files"
MAX_FILE_SIZE=10485760  # 10MB
ALLOWED_EXTENSIONS="mp3,wav,m4a,xml"

# Server Configuration
HOST="0.0.0.0"
PORT=8000
WORKERS=1

# Fly.io Configuration
FLY_APP_NAME="twilio-multi-tenant"
FLY_REGION="ord"
```

```python
# backend/app/core/config.py
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
```

**Testing Strategy**:
```python
# backend/tests/test_config.py
import pytest
from app.core.config import settings

def test_settings_loaded():
    assert settings.app_name is not None
    assert settings.database_path is not None
    assert len(settings.secret_key) > 10
```

**Deliverables**:
- ✅ Python virtual environment configured
- ✅ All dependencies installed and tested
- ✅ Configuration management implemented
- ✅ Basic tests passing

### Task 1.3: Frontend Environment Setup
**Dependencies**: Task 1.1  
**Tools**: Node.js, npm, Vite, Vue 3

```json
// frontend/package.json
{
  "name": "twilio-manager-frontend",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "test": "vitest",
    "test:ui": "vitest --ui",
    "lint": "eslint . --ext .vue,.js,.jsx,.cjs,.mjs,.ts,.tsx,.cts,.mts --fix --ignore-path .gitignore",
    "format": "prettier --write src/"
  },
  "dependencies": {
    "vue": "^3.3.8",
    "vue-router": "^4.2.5",
    "pinia": "^2.1.7",
    "axios": "^1.6.2",
    "@headlessui/vue": "^1.7.16",
    "@heroicons/vue": "^2.0.18",
    "date-fns": "^2.30.0",
    "lodash-es": "^4.17.21"
  },
  "devDependencies": {
    "@vitejs/plugin-vue": "^4.5.0",
    "vite": "^5.0.0",
    "vitest": "^0.34.6",
    "@vue/test-utils": "^2.4.0",
    "jsdom": "^22.1.0",
    "tailwindcss": "^3.3.6",
    "autoprefixer": "^10.4.16",
    "postcss": "^8.4.32",
    "eslint": "^8.54.0",
    "eslint-plugin-vue": "^9.18.1",
    "prettier": "^3.1.0",
    "@types/lodash-es": "^4.17.11"
  }
}
```

```javascript
// frontend/vite.config.js
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src'),
    },
  },
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:8000',
        changeOrigin: true,
      },
    },
  },
  build: {
    outDir: 'dist',
    assetsDir: 'assets',
    sourcemap: true,
  },
  define: {
    __VUE_OPTIONS_API__: false,
    __VUE_PROD_DEVTOOLS__: false,
  },
})
```

```javascript
// frontend/tailwind.config.js
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#eff6ff',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
        },
        gray: {
          50: '#f9fafb',
          100: '#f3f4f6',
          500: '#6b7280',
          900: '#111827',
        }
      },
    },
  },
  plugins: [],
}
```

**Testing Strategy**:
```javascript
// frontend/src/components/__tests__/HelloWorld.test.js
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import HelloWorld from '../HelloWorld.vue'

describe('HelloWorld', () => {
  it('renders properly', () => {
    const wrapper = mount(HelloWorld, { props: { msg: 'Hello Vitest' } })
    expect(wrapper.text()).toContain('Hello Vitest')
  })
})
```

**Deliverables**:
- ✅ Vue.js 3 application initialized
- ✅ Vite build system configured
- ✅ Tailwind CSS integrated
- ✅ Development server running
- ✅ Basic component tests passing

---

## 🗄️ Phase 2: Database Design & Implementation
**Goal**: Create robust multi-tenant database architecture

### Task 2.1: Database Schema Implementation
**Dependencies**: Phase 1 complete  
**Tools**: SQLite, Python sqlite3 module

```python
# backend/app/core/database.py
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
```

**Testing Strategy**:
```python
# backend/tests/test_database.py
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
```

**Deliverables**:
- ✅ Complete SQLite schema implemented
- ✅ Database connection management
- ✅ Performance indexes created
- ✅ Database tests passing

### Task 2.2: Data Migration Tools
**Dependencies**: Task 2.1  
**Tools**: Python, pandas (for data processing)

```python
# backend/app/utils/migration.py
import sqlite3
import json
import os
from typing import Dict, List, Optional
from datetime import datetime
import logging
from pathlib import Path

logger = logging.getLogger(__name__)

class AccessToSQLiteMigrator:
    """Migrate data from legacy Access database to SQLite"""
    
    def __init__(self, sqlite_db_path: str):
        self.sqlite_db_path = sqlite_db_path
        self.migration_log = []
    
    def migrate_from_legacy_data(self, legacy_data_path: str):
        """
        Migrate data from legacy system
        
        For demonstration, this shows how to handle the migration process.
        In practice, you would need to extract data from the Access database
        and transform it to the new schema.
        """
        try:
            # Step 1: Create sample tenant data
            self._create_sample_tenants()
            
            # Step 2: Migrate voice messages (if legacy data exists)
            # self._migrate_voice_messages(legacy_data_path)
            
            # Step 3: Migrate files (if legacy files exist)
            # self._migrate_files(legacy_data_path)
            
            logger.info("Migration completed successfully")
            return True
            
        except Exception as e:
            logger.error(f"Migration failed: {e}")
            return False
    
    def _create_sample_tenants(self):
        """Create sample tenant configurations"""
        sample_tenants = [
            {
                'id': 'acme-corp',
                'name': 'Acme Corporation',
                'api_key': 'acme_api_key_12345',
                'twilio_sid': 'AC_sample_acme_sid',
                'twilio_token': 'sample_acme_token',
                'from_number': '+1234567890',
                'settings': json.dumps({
                    'max_calls_per_day': 100,
                    'allowed_countries': ['US', 'CA'],
                    'timezone': 'America/New_York'
                })
            },
            {
                'id': 'globex-inc',
                'name': 'Globex Inc',
                'api_key': 'globex_api_key_67890',
                'twilio_sid': 'AC_sample_globex_sid',
                'twilio_token': 'sample_globex_token',
                'from_number': '+0987654321',
                'settings': json.dumps({
                    'max_calls_per_day': 50,
                    'allowed_countries': ['US'],
                    'timezone': 'America/Los_Angeles'
                })
            }
        ]
        
        conn = sqlite3.connect(self.sqlite_db_path)
        cursor = conn.cursor()
        
        for tenant in sample_tenants:
            cursor.execute("""
                INSERT OR REPLACE INTO tenants 
                (id, name, api_key, twilio_sid, twilio_token, from_number, settings)
                VALUES (?, ?, ?, ?, ?, ?, ?)
            """, (
                tenant['id'], tenant['name'], tenant['api_key'],
                tenant['twilio_sid'], tenant['twilio_token'],
                tenant['from_number'], tenant['settings']
            ))
            
            # Create sample voice messages for each tenant
            self._create_sample_voice_messages(cursor, tenant['id'])
        
        conn.commit()
        conn.close()
        
        logger.info(f"Created {len(sample_tenants)} sample tenants")
    
    def _create_sample_voice_messages(self, cursor, tenant_id: str):
        """Create sample voice messages for a tenant"""
        sample_messages = [
            {
                'name': 'Welcome Message',
                'voice_text': 'Welcome to {company_name}. Please hold while we connect you.',
                'voice_gender': 'alice',
                'voice_rate': 'medium'
            },
            {
                'name': 'Appointment Reminder',
                'voice_text': 'Hello {customer_name}, this is a reminder for your appointment on {appointment_date} at {appointment_time}.',
                'voice_gender': 'alice',
                'voice_rate': 'slow'
            },
            {
                'name': 'Payment Due Notice',
                'voice_text': 'This is {company_name}. Your payment of {amount}~SSML~currency is due on {due_date}. Please call us at {phone_number}~SSML~phone.',
                'voice_gender': 'bob',
                'voice_rate': 'medium'
            }
        ]
        
        for message in sample_messages:
            cursor.execute("""
                INSERT INTO voice_messages 
                (tenant_id, name, voice_text, voice_gender, voice_rate)
                VALUES (?, ?, ?, ?, ?)
            """, (
                tenant_id, message['name'], message['voice_text'],
                message['voice_gender'], message['voice_rate']
            ))

# Migration script
def run_migration():
    """Run the complete migration process"""
    from app.core.config import settings
    
    migrator = AccessToSQLiteMigrator(settings.database_path)
    
    # For now, we'll create sample data
    # In production, you would provide the path to legacy data
    success = migrator.migrate_from_legacy_data("legacy_data_path")
    
    if success:
        print("Migration completed successfully!")
    else:
        print("Migration failed. Check logs for details.")

if __name__ == "__main__":
    run_migration()
```

**Testing Strategy**:
```python
# backend/tests/test_migration.py
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
```

**Deliverables**:
- ✅ Migration utilities implemented
- ✅ Sample data generation
- ✅ Data validation checks
- ✅ Migration tests passing

---

## 🔧 Phase 3: Backend API Development
**Goal**: Build complete FastAPI backend with all endpoints

### Task 3.1: Core Models & Schemas
**Dependencies**: Phase 2 complete  
**Tools**: Pydantic, FastAPI

```python
# backend/app/models/schemas.py
from pydantic import BaseModel, Field, validator
from typing import Optional, List, Dict, Any
from datetime import datetime
from enum import Enum

# Enums
class NetworkType(str, Enum):
    CALL = "call"
    SMS = "sms"

class VoiceGender(str, Enum):
    ALICE = "alice"
    BOB = "bob"
    WOMAN = "woman"
    MAN = "man"

class VoiceRate(str, Enum):
    SLOW = "slow"
    MEDIUM = "medium"
    FAST = "fast"

class FileType(str, Enum):
    MP3 = "mp3"
    WAV = "wav"
    M4A = "m4a"
    XML = "xml"

# Base Models
class BaseSchema(BaseModel):
    class Config:
        from_attributes = True
        json_encoders = {
            datetime: lambda v: v.isoformat()
        }

# Tenant Models
class TenantBase(BaseSchema):
    name: str = Field(..., min_length=1, max_length=100)
    twilio_sid: Optional[str] = None
    twilio_token: Optional[str] = None
    from_number: Optional[str] = Field(None, regex=r'^\+[1-9]\d{1,14}$')
    settings: Optional[Dict[str, Any]] = None

class TenantCreate(TenantBase):
    id: str = Field(..., min_length=1, max_length=50, regex=r'^[a-z0-9-]+$')
    api_key: str = Field(..., min_length=20)

class TenantUpdate(BaseSchema):
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    twilio_sid: Optional[str] = None
    twilio_token: Optional[str] = None
    from_number: Optional[str] = Field(None, regex=r'^\+[1-9]\d{1,14}$')
    settings: Optional[Dict[str, Any]] = None

class TenantResponse(TenantBase):
    id: str
    api_key: str
    is_active: bool
    created_at: datetime
    updated_at: datetime

# Voice Message Models
class VoiceMessageBase(BaseSchema):
    name: str = Field(..., min_length=1, max_length=100)
    voice_text: str = Field(..., min_length=1)
    voice_gender: VoiceGender = VoiceGender.ALICE
    voice_age: int = Field(25, ge=1, le=100)
    voice_rate: VoiceRate = VoiceRate.MEDIUM
    tropo_voice: Optional[str] = None
    voice_type: str = Field("message", max_length=50)
    voice_settings: Optional[Dict[str, Any]] = None

class VoiceMessageCreate(VoiceMessageBase):
    pass

class VoiceMessageUpdate(BaseSchema):
    name: Optional[str] = Field(None, min_length=1, max_length=100)
    voice_text: Optional[str] = Field(None, min_length=1)
    voice_gender: Optional[VoiceGender] = None
    voice_age: Optional[int] = Field(None, ge=1, le=100)
    voice_rate: Optional[VoiceRate] = None
    tropo_voice: Optional[str] = None
    voice_type: Optional[str] = Field(None, max_length=50)
    voice_settings: Optional[Dict[str, Any]] = None

class VoiceMessageResponse(VoiceMessageBase):
    id: int
    tenant_id: str
    is_active: bool
    created_at: datetime
    updated_at: datetime

# File Models
class VoiceFileBase(BaseSchema):
    filename: str = Field(..., min_length=1, max_length=255)
    file_type: FileType
    description: Optional[str] = Field(None, max_length=500)
    tag_list: Optional[str] = Field(None, max_length=500)
    caller_number: Optional[str] = Field(None, regex=r'^\+[1-9]\d{1,14}$')

class VoiceFileCreate(VoiceFileBase):
    file_path: str
    file_size: int = Field(..., ge=0)

class VoiceFileUpdate(BaseSchema):
    description: Optional[str] = Field(None, max_length=500)
    tag_list: Optional[str] = Field(None, max_length=500)
    caller_number: Optional[str] = Field(None, regex=r'^\+[1-9]\d{1,14}$')

class VoiceFileResponse(VoiceFileBase):
    id: int
    tenant_id: str
    file_path: str
    file_size: int
    is_active: bool
    created_at: datetime
    updated_at: datetime

# Communication Models
class SMSCreate(BaseSchema):
    to_number: str = Field(..., regex=r'^\+[1-9]\d{1,14}$')
    message: str = Field(..., min_length=1, max_length=1600)
    
class VoiceCallCreate(BaseSchema):
    to_number: str = Field(..., regex=r'^\+[1-9]\d{1,14}$')
    voice_message_id: int = Field(..., ge=1)

class CommunicationLogResponse(BaseSchema):
    id: int
    tenant_id: str
    network_type: NetworkType
    to_number: str
    from_number: str
    message_content: Optional[str]
    twilio_sid: Optional[str]
    status: Optional[str]
    error_message: Optional[str]
    xml_filename: Optional[str]
    cost_amount: Optional[float]
    duration_seconds: Optional[int]
    created_at: datetime

# API Response Models
class APIResponse(BaseSchema):
    success: bool = True
    message: str = "Operation completed successfully"
    timestamp: datetime = Field(default_factory=datetime.utcnow)

class APIResponseWithData(APIResponse):
    data: Any

class APIError(BaseSchema):
    success: bool = False
    error: Dict[str, Any]
    timestamp: datetime = Field(default_factory=datetime.utcnow)

class PaginationMeta(BaseSchema):
    page: int = Field(..., ge=1)
    per_page: int = Field(..., ge=1, le=100)
    total_items: int = Field(..., ge=0)
    total_pages: int = Field(..., ge=0)
    has_next: bool
    has_prev: bool

class PaginatedResponse(APIResponse):
    data: List[Any]
    pagination: PaginationMeta

# File Upload Models
class FileUploadResponse(APIResponse):
    data: Dict[str, Any] = Field(..., description="File information")
```

**Testing Strategy**:
```python
# backend/tests/test_schemas.py
import pytest
from pydantic import ValidationError
from app.models.schemas import (
    VoiceMessageCreate, TenantCreate, SMSCreate, VoiceCallCreate
)

def test_voice_message_create_valid():
    """Test valid voice message creation"""
    data = {
        "name": "Test Message",
        "voice_text": "Hello world",
        "voice_gender": "alice",
        "voice_rate": "medium"
    }
    
    message = VoiceMessageCreate(**data)
    assert message.name == "Test Message"
    assert message.voice_gender == "alice"

def test_voice_message_create_invalid():
    """Test invalid voice message creation"""
    with pytest.raises(ValidationError):
        VoiceMessageCreate(name="", voice_text="")

def test_tenant_create_valid():
    """Test valid tenant creation"""
    data = {
        "id": "test-tenant",
        "name": "Test Tenant",
        "api_key": "test_api_key_123456789",
        "from_number": "+1234567890"
    }
    
    tenant = TenantCreate(**data)
    assert tenant.id == "test-tenant"
    assert tenant.from_number == "+1234567890"

def test_sms_create_valid():
    """Test valid SMS creation"""
    data = {
        "to_number": "+1234567890",
        "message": "Test SMS message"
    }
    
    sms = SMSCreate(**data)
    assert sms.to_number == "+1234567890"
```

**Deliverables**:
- ✅ Complete Pydantic models for all entities
- ✅ Request/response schemas
- ✅ Data validation rules
- ✅ Schema tests passing

### Task 3.2: Authentication & Tenant Management
**Dependencies**: Task 3.1  
**Tools**: FastAPI dependencies, security utilities

```python
# backend/app/core/security.py
from fastapi import HTTPException, status, Depends, Request
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from typing import Optional
import logging
from app.core.database import db_manager
from app.models.schemas import TenantResponse

logger = logging.getLogger(__name__)

security = HTTPBearer(auto_error=False)

class TenantAuth:
    """Handle tenant authentication and context"""
    
    def __init__(self):
        self.db = db_manager
    
    def get_tenant_by_api_key(self, api_key: str) -> Optional[TenantResponse]:
        """Retrieve tenant by API key"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            cursor.execute("""
                SELECT id, name, api_key, twilio_sid, twilio_token, 
                       from_number, is_active, settings, created_at, updated_at
                FROM tenants 
                WHERE api_key = ? AND is_active = 1
            """, (api_key,))
            
            row = cursor.fetchone()
            if not row:
                return None
            
            return TenantResponse(
                id=row['id'],
                name=row['name'],
                api_key=row['api_key'],
                twilio_sid=row['twilio_sid'],
                twilio_token=row['twilio_token'],
                from_number=row['from_number'],
                is_active=bool(row['is_active']),
                settings=row['settings'],
                created_at=row['created_at'],
                updated_at=row['updated_at']
            )
    
    def authenticate_request(self, request: Request) -> TenantResponse:
        """Extract and validate API key from request"""
        api_key = None
        
        # Try Authorization header first
        auth_header = request.headers.get('Authorization')
        if auth_header and auth_header.startswith('Bearer '):
            api_key = auth_header[7:]  # Remove 'Bearer ' prefix
        
        # Try X-API-Key header
        if not api_key:
            api_key = request.headers.get('X-API-Key')
        
        # Try query parameter (for iframe URLs)
        if not api_key:
            api_key = request.query_params.get('apiKey')
        
        if not api_key:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="API key required. Provide via Authorization header, X-API-Key header, or apiKey query parameter."
            )
        
        tenant = self.get_tenant_by_api_key(api_key)
        if not tenant:
            logger.warning(f"Invalid API key attempt: {api_key[:10]}...")
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail="Invalid API key"
            )
        
        return tenant

# Global auth instance
tenant_auth = TenantAuth()

# FastAPI dependency
async def get_current_tenant(request: Request) -> TenantResponse:
    """FastAPI dependency to get current authenticated tenant"""
    return tenant_auth.authenticate_request(request)

# Optional authentication for public endpoints
async def get_optional_tenant(request: Request) -> Optional[TenantResponse]:
    """FastAPI dependency for optional authentication"""
    try:
        return tenant_auth.authenticate_request(request)
    except HTTPException:
        return None
```

**Testing Strategy**:
```python
# backend/tests/test_auth.py
import pytest
from fastapi.testclient import TestClient
from app.main import app
from app.core.security import tenant_auth

client = TestClient(app)

def test_authentication_with_header():
    """Test authentication with X-API-Key header"""
    response = client.get(
        "/api/tenant/profile",
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    assert response.status_code == 200

def test_authentication_with_bearer_token():
    """Test authentication with Bearer token"""
    response = client.get(
        "/api/tenant/profile",
        headers={"Authorization": "Bearer acme_api_key_12345"}
    )
    assert response.status_code == 200

def test_authentication_with_query_param():
    """Test authentication with query parameter"""
    response = client.get("/api/tenant/profile?apiKey=acme_api_key_12345")
    assert response.status_code == 200

def test_invalid_api_key():
    """Test invalid API key returns 401"""
    response = client.get(
        "/api/tenant/profile",
        headers={"X-API-Key": "invalid_key"}
    )
    assert response.status_code == 401

def test_missing_api_key():
    """Test missing API key returns 401"""
    response = client.get("/api/tenant/profile")
    assert response.status_code == 401
```

**Deliverables**:
- ✅ API key authentication system
- ✅ Tenant context management
- ✅ Security middleware implementation
- ✅ Authentication tests passing

### Task 3.3: Voice Message Management APIs
**Dependencies**: Task 3.2  
**Tools**: FastAPI, SQLite operations

```python
# backend/app/api/voice.py
from fastapi import APIRouter, Depends, HTTPException, status, Query
from typing import List
from app.models.schemas import (
    VoiceMessageCreate, VoiceMessageUpdate, VoiceMessageResponse,
    TenantResponse, APIResponseWithData, PaginatedResponse, PaginationMeta
)
from app.core.security import get_current_tenant
from app.services.voice_service import voice_service
import math

router = APIRouter(prefix="/voice-messages", tags=["Voice Messages"])

@router.post("/", response_model=APIResponseWithData)
async def create_voice_message(
    message_data: VoiceMessageCreate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Create a new voice message"""
    try:
        message = voice_service.create_voice_message(tenant.id, message_data)
        return APIResponseWithData(
            data=message,
            message="Voice message created successfully"
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )

@router.get("/", response_model=PaginatedResponse)
async def list_voice_messages(
    page: int = Query(1, ge=1),
    per_page: int = Query(20, ge=1, le=100),
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """List voice messages with pagination"""
    offset = (page - 1) * per_page
    messages, total_count = voice_service.list_voice_messages(
        tenant.id, limit=per_page, offset=offset
    )
    
    total_pages = math.ceil(total_count / per_page)
    
    return PaginatedResponse(
        data=messages,
        message=f"Retrieved {len(messages)} voice messages",
        pagination=PaginationMeta(
            page=page,
            per_page=per_page,
            total_items=total_count,
            total_pages=total_pages,
            has_next=page < total_pages,
            has_prev=page > 1
        )
    )

@router.get("/{message_id}", response_model=APIResponseWithData)
async def get_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Get a specific voice message"""
    message = voice_service.get_voice_message(tenant.id, message_id)
    if not message:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Voice message not found"
        )
    
    return APIResponseWithData(
        data=message,
        message="Voice message retrieved successfully"
    )

@router.put("/{message_id}", response_model=APIResponseWithData)
async def update_voice_message(
    message_id: int,
    update_data: VoiceMessageUpdate,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Update a voice message"""
    try:
        message = voice_service.update_voice_message(tenant.id, message_id, update_data)
        return APIResponseWithData(
            data=message,
            message="Voice message updated successfully"
        )
    except ValueError as e:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=str(e)
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )

@router.delete("/{message_id}", response_model=APIResponseWithData)
async def delete_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Delete a voice message"""
    success = voice_service.delete_voice_message(tenant.id, message_id)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Voice message not found"
        )
    
    return APIResponseWithData(
        data={"deleted": True},
        message="Voice message deleted successfully"
    )

@router.post("/{message_id}/duplicate", response_model=APIResponseWithData)
async def duplicate_voice_message(
    message_id: int,
    tenant: TenantResponse = Depends(get_current_tenant)
):
    """Create a duplicate of an existing voice message"""
    try:
        duplicate = voice_service.duplicate_voice_message(tenant.id, message_id)
        return APIResponseWithData(
            data=duplicate,
            message="Voice message duplicated successfully"
        )
    except ValueError as e:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=str(e)
        )
    except Exception as e:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail=str(e)
        )
```

**Testing Strategy**:
```python
# backend/tests/test_voice_api.py
import pytest
from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)

def test_create_voice_message():
    """Test creating a voice message"""
    data = {
        "name": "Test Message",
        "voice_text": "Hello {name}, this is a test message.",
        "voice_gender": "alice",
        "voice_rate": "medium"
    }
    
    response = client.post(
        "/api/voice-messages/",
        json=data,
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    assert response.status_code == 200
    result = response.json()
    assert result["success"] == True
    assert result["data"]["name"] == "Test Message"

def test_list_voice_messages():
    """Test listing voice messages"""
    response = client.get(
        "/api/voice-messages/",
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    assert response.status_code == 200
    result = response.json()
    assert result["success"] == True
    assert "data" in result
    assert "pagination" in result

def test_get_voice_message():
    """Test getting a specific voice message"""
    # First create a message
    create_response = client.post(
        "/api/voice-messages/",
        json={
            "name": "Test Get Message",
            "voice_text": "Test content",
            "voice_gender": "alice"
        },
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    message_id = create_response.json()["data"]["id"]
    
    # Then get it
    response = client.get(
        f"/api/voice-messages/{message_id}",
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    assert response.status_code == 200
    result = response.json()
    assert result["data"]["name"] == "Test Get Message"

def test_update_voice_message():
    """Test updating a voice message"""
    # Create message
    create_response = client.post(
        "/api/voice-messages/",
        json={
            "name": "Original Name",
            "voice_text": "Original text",
            "voice_gender": "alice"
        },
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    message_id = create_response.json()["data"]["id"]
    
    # Update message
    update_data = {"name": "Updated Name"}
    response = client.put(
        f"/api/voice-messages/{message_id}",
        json=update_data,
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    assert response.status_code == 200
    result = response.json()
    assert result["data"]["name"] == "Updated Name"

def test_duplicate_voice_message():
    """Test duplicating a voice message"""
    # Create original message
    create_response = client.post(
        "/api/voice-messages/",
        json={
            "name": "Original Message",
            "voice_text": "Original content",
            "voice_gender": "alice"
        },
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    message_id = create_response.json()["data"]["id"]
    
    # Duplicate message
    response = client.post(
        f"/api/voice-messages/{message_id}/duplicate",
        headers={"X-API-Key": "acme_api_key_12345"}
    )
    
    assert response.status_code == 200
    result = response.json()
    assert result["data"]["name"] == "Copy of Original Message"
    assert result["data"]["voice_text"] == "Original content"
```

**Deliverables**:
- ✅ Complete voice message CRUD operations
- ✅ Pagination support
- ✅ Voice message duplication feature
- ✅ Comprehensive API tests

### Task 3.4: Twilio Integration Service
**Dependencies**: Task 3.3  
**Tools**: Twilio SDK, TwiML generation

```python
# backend/app/services/twilio_service.py
from twilio.rest import Client
from twilio.twiml import TwiML, VoiceResponse
from typing import Optional, Dict, Any, List
import logging
import re
from app.models.schemas import TenantResponse, VoiceMessageResponse
from app.services.voice_service import voice_service
from app.core.config import settings
from app.core.database import db_manager
from datetime import datetime
import json

logger = logging.getLogger(__name__)

class TwilioService:
    """Service for Twilio voice and SMS operations"""
    
    def __init__(self, tenant: TenantResponse):
        self.tenant = tenant
        self.client = Client(tenant.twilio_sid, tenant.twilio_token)
        self.from_number = tenant.from_number
        self.db = db_manager
    
    async def create_voice_call(self, to_number: str, voice_message_id: int, 
                              variables: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """Create a voice call with dynamic TwiML generation"""
        try:
            # Get voice message
            voice_message = voice_service.get_voice_message(self.tenant.id, voice_message_id)
            if not voice_message:
                raise ValueError("Voice message not found")
            
            # Generate and store TwiML
            twiml_content = self._generate_twiml(voice_message, variables or {})
            xml_id = await self._store_voice_xml(voice_message_id, twiml_content)
            
            # Create webhook URL
            webhook_url = f"{settings.base_url}/webhooks/twilio/voice-xml/{xml_id}"
            
            # Create Twilio call
            call = self.client.calls.create(
                to=to_number,
                from_=self.from_number,
                url=webhook_url,
                method='GET'
            )
            
            # Log the call
            await self._log_communication(
                network_type="call",
                to_number=to_number,
                from_number=self.from_number,
                message_content=voice_message.voice_text,
                twilio_sid=call.sid,
                status="initiated"
            )
            
            return {
                "call_sid": call.sid,
                "status": call.status,
                "to": to_number,
                "from": self.from_number,
                "webhook_url": webhook_url
            }
            
        except Exception as e:
            logger.error(f"Error creating voice call: {e}")
            await self._log_error("twilio_call", str(e), {
                "to_number": to_number,
                "voice_message_id": voice_message_id
            })
            raise
    
    async def send_sms(self, to_number: str, message_content: str, 
                      variables: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:
        """Send SMS message with variable substitution"""
        try:
            # Process message with variables
            processed_message = self._process_message_variables(message_content, variables or {})
            
            # Send SMS via Twilio
            message = self.client.messages.create(
                body=processed_message,
                from_=self.from_number,
                to=to_number
            )
            
            # Log the SMS
            await self._log_communication(
                network_type="sms",
                to_number=to_number,
                from_number=self.from_number,
                message_content=processed_message,
                twilio_sid=message.sid,
                status="sent"
            )
            
            return {
                "message_sid": message.sid,
                "status": message.status,
                "to": to_number,
                "from": self.from_number,
                "body": processed_message
            }
            
        except Exception as e:
            logger.error(f"Error sending SMS: {e}")
            await self._log_error("twilio_sms", str(e), {
                "to_number": to_number,
                "message_content": message_content
            })
            raise
    
    def _generate_twiml(self, voice_message: VoiceMessageResponse, 
                       variables: Dict[str, Any]) -> str:
        """Generate TwiML for voice message"""
        # Process voice text with variables and SSML
        processed_text = self._process_voice_text(voice_message.voice_text, variables)
        
        # Create TwiML response
        response = VoiceResponse()
        
        # Split text and audio files
        text_parts, audio_parts = self._parse_mixed_content(processed_text)
        
        for part in text_parts + audio_parts:
            if part.startswith('http://') or part.startswith('https://'):
                # Audio file
                response.play(part)
            else:
                # Text to speech
                say = response.say(
                    part,
                    voice=voice_message.voice_gender,
                    rate=voice_message.voice_rate
                )
        
        return str(response)
    
    def _process_voice_text(self, voice_text: str, variables: Dict[str, Any]) -> str:
        """Process voice text with variable substitution and SSML"""
        processed_text = voice_text
        
        # Replace variables with SSML formatting
        for var_name, var_value in variables.items():
            str_value = str(var_value)
            
            # SSML formatting patterns
            ssml_patterns = {
                f"{{{var_name}}}~SSML~number": self._create_ssml_say_as(str_value, "number"),
                f"{{{var_name}}}~SSML~currency": self._create_ssml_say_as(str_value, "currency"),
                f"{{{var_name}}}~SSML~digits": self._create_ssml_say_as(str_value, "digits"),
                f"{{{var_name}}}~SSML~phone": self._create_ssml_say_as(str_value, "telephone"),
                f"{{{var_name}}}~SSML~date": self._create_ssml_say_as(str_value, "date"),
                f"{{{var_name}}}~SSML~time": self._create_ssml_say_as(str_value, "time"),
                f"{{{var_name}}}": str_value
            }
            
            for pattern, replacement in ssml_patterns.items():
                processed_text = processed_text.replace(pattern, replacement)
        
        # Handle MP3 file references
        processed_text = self._process_mp3_references(processed_text)
        
        # Clean up any remaining variable placeholders
        processed_text = re.sub(r'\{[^}]*\}(?:~SSML~\w+)?', '', processed_text)
        
        return processed_text
    
    def _process_message_variables(self, message: str, variables: Dict[str, Any]) -> str:
        """Process SMS message with simple variable substitution"""
        processed_message = message
        
        for var_name, var_value in variables.items():
            processed_message = processed_message.replace(f"{{{var_name}}}", str(var_value))
        
        # Clean up any remaining placeholders
        processed_message = re.sub(r'\{[^}]*\}', '', processed_message)
        
        return processed_message
    
    def _create_ssml_say_as(self, value: str, interpret_as: str) -> str:
        """Create SSML say-as markup"""
        return f'<say-as interpret-as="{interpret_as}">{value}</say-as>'
    
    def _process_mp3_references(self, text: str) -> str:
        """Convert MP3 references to full URLs"""
        def replace_mp3(match):
            filename = match.group(1)
            return f"{settings.base_url}/files/mp3/{filename}.mp3"
        
        return re.sub(r'\{MP3~([^}]+)\}', replace_mp3, text)
    
    def _parse_mixed_content(self, content: str) -> tuple[List[str], List[str]]:
        """Parse content into text and audio parts"""
        # Split content by HTTP URLs (audio files)
        parts = re.split(r'(https?://[^\s]+\.(?:mp3|wav|m4a))', content)
        
        text_parts = []
        audio_parts = []
        
        for part in parts:
            part = part.strip()
            if not part:
                continue
                
            if part.startswith(('http://', 'https://')):
                audio_parts.append(part)
            else:
                text_parts.append(part)
        
        return text_parts, audio_parts
    
    async def _store_voice_xml(self, voice_message_id: int, xml_content: str) -> int:
        """Store generated TwiML in database"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            cursor.execute("""
                INSERT INTO voice_xml (tenant_id, xml_content, voice_message_id)
                VALUES (?, ?, ?)
            """, (self.tenant.id, xml_content, voice_message_id))
            
            xml_id = cursor.lastrowid
            conn.commit()
            
            return xml_id
    
    async def _log_communication(self, network_type: str, to_number: str, 
                                from_number: str, message_content: str,
                                twilio_sid: str, status: str):
        """Log communication activity"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            cursor.execute("""
                INSERT INTO communication_log 
                (tenant_id, network_type, to_number, from_number, 
                 message_content, twilio_sid, status)
                VALUES (?, ?, ?, ?, ?, ?, ?)
            """, (
                self.tenant.id, network_type, to_number, from_number,
                message_content, twilio_sid, status
            ))
            
            conn.commit()
    
    async def _log_error(self, error_type: str, error_message: str, 
                        error_details: Dict[str, Any]):
        """Log error to database"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            cursor.execute("""
                INSERT INTO error_log (tenant_id, error_type, error_message, error_details)
                VALUES (?, ?, ?, ?)
            """, (
                self.tenant.id, error_type, error_message, 
                json.dumps(error_details)
            ))
            
            conn.commit()

class TwilioWebhookService:
    """Service for handling Twilio webhooks"""
    
    def __init__(self):
        self.db = db_manager
    
    def get_voice_xml(self, xml_id: int) -> Optional[str]:
        """Get stored TwiML content for webhook"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            cursor.execute("""
                SELECT xml_content FROM voice_xml 
                WHERE id = ? AND is_active = 1
            """, (xml_id,))
            
            row = cursor.fetchone()
            return row['xml_content'] if row else None
    
    async def handle_call_status(self, tenant_id: str, call_sid: str, 
                                call_status: str, call_duration: Optional[int] = None):
        """Handle call status updates from Twilio"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            # Update communication log
            update_fields = ["status = ?"]
            values = [call_status]
            
            if call_duration is not None:
                update_fields.append("duration_seconds = ?")
                values.append(call_duration)
            
            values.extend([tenant_id, call_sid])
            
            cursor.execute(f"""
                UPDATE communication_log 
                SET {', '.join(update_fields)}
                WHERE tenant_id = ? AND twilio_sid = ?
            """, values)
            
            conn.commit()
            
            logger.info(f"Updated call status: {call_sid} -> {call_status}")
    
    async def handle_sms_status(self, tenant_id: str, message_sid: str, 
                               message_status: str):
        """Handle SMS status updates from Twilio"""
        with self.db.get_connection() as conn:
            cursor = conn.cursor()
            
            cursor.execute("""
                UPDATE communication_log 
                SET status = ?
                WHERE tenant_id = ? AND twilio_sid = ?
            """, (message_status, tenant_id, message_sid))
            
            conn.commit()
            
            logger.info(f"Updated SMS status: {message_sid} -> {message_status}")

# Global webhook service instance
webhook_service = TwilioWebhookService()
```

**Testing Strategy**:
```python
# backend/tests/test_twilio_service.py
import pytest
from unittest.mock import Mock, patch
from app.services.twilio_service import TwilioService
from app.models.schemas import TenantResponse, VoiceMessageResponse

@pytest.fixture
def sample_tenant():
    return TenantResponse(
        id="test-tenant",
        name="Test Tenant",
        api_key="test_key",
        twilio_sid="test_sid",
        twilio_token="test_token",
        from_number="+1234567890",
        is_active=True,
        settings=None,
        created_at="2024-01-01T00:00:00",
        updated_at="2024-01-01T00:00:00"
    )

@pytest.fixture
def sample_voice_message():
    return VoiceMessageResponse(
        id=1,
        tenant_id="test-tenant",
        name="Test Message",
        voice_text="Hello {name}, your balance is {amount}~SSML~currency",
        voice_gender="alice",
        voice_age=25,
        voice_rate="medium",
        tropo_voice=None,
        voice_type="message",
        voice_settings=None,
        is_active=True,
        created_at="2024-01-01T00:00:00",
        updated_at="2024-01-01T00:00:00"
    )

def test_process_voice_text(sample_tenant, sample_voice_message):
    """Test voice text processing with variables"""
    service = TwilioService(sample_tenant)
    
    variables = {
        "name": "John Doe",
        "amount": "100.50"
    }
    
    result = service._process_voice_text(sample_voice_message.voice_text, variables)
    
    assert "John Doe" in result
    assert '<say-as interpret-as="currency">100.50</say-as>' in result

def test_generate_twiml(sample_tenant, sample_voice_message):
    """Test TwiML generation"""
    service = TwilioService(sample_tenant)
    
    variables = {"name": "Jane", "amount": "50.00"}
    result = service._generate_twiml(sample_voice_message, variables)
    
    assert "<?xml version=" in result
    assert "<Response>" in result
    assert "<Say" in result
    assert "voice=\"alice\"" in result

@patch('app.services.twilio_service.Client')
async def test_send_sms(mock_client, sample_tenant):
    """Test SMS sending"""
    mock_message = Mock()
    mock_message.sid = "test_sms_sid"
    mock_message.status = "sent"
    
    mock_client.return_value.messages.create.return_value = mock_message
    
    service = TwilioService(sample_tenant)
    
    result = await service.send_sms(
        "+1987654321",
        "Hello {name}",
        {"name": "Test User"}
    )
    
    assert result["message_sid"] == "test_sms_sid"
    assert result["status"] == "sent"
    
    # Verify Twilio was called correctly
    mock_client.return_value.messages.create.assert_called_once()
    call_args = mock_client.return_value.messages.create.call_args
    assert call_args[1]["body"] == "Hello Test User"
    assert call_args[1]["to"] == "+1987654321"
```

**Deliverables**:
- ✅ Complete Twilio integration service
- ✅ TwiML generation with SSML support
- ✅ Variable substitution system
- ✅ Webhook handling capability
- ✅ Comprehensive service tests

---

This implementation roadmap provides systematic, step-by-step guidance for AI agents to build the complete FastAPI + Vue.js multi-tenant application without human-oriented time estimates or complexity ratings. Each task includes specific dependencies, tools, complete code implementations, testing strategies, and clear deliverable milestones suitable for automated execution.