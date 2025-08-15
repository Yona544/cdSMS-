```
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
