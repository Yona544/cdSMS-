# Twilio Multi-Tenant Voice & SMS Management Platform

A complete modernization of a legacy ASP.NET Web Forms VB.NET application to a modern FastAPI + Vue.js multi-tenant system.

## 🏗️ Repository Structure

This repository contains both the legacy system and the modern replacement, organized for efficient AI-assisted development:

```
twilio-manager/
├── legacy/                    # 🔒 Legacy ASP.NET Web Forms System (Reference Only)
│   ├── Admin/                 # Administrative interface
│   ├── App_Code/              # VB.NET business logic
│   ├── App_data/              # Access database and configurations
│   ├── Bin/                   # .NET assemblies and dependencies
│   ├── css/                   # Legacy stylesheets
│   ├── ErrorLog/              # Application error logs
│   ├── files/                 # File storage (MP3, uploads)
│   ├── js/                    # JavaScript libraries
│   ├── Web.config             # ASP.NET configuration
│   └── *.aspx/.asp files      # Web pages and handlers
│
├── modern/                    # 🚀 New FastAPI + Vue.js System (Active Development)
│   ├── backend/               # Python FastAPI API
│   │   ├── app/
│   │   │   ├── api/           # API route definitions
│   │   │   ├── core/          # Core functionality (config, database)
│   │   │   ├── models/        # Pydantic data models
│   │   │   ├── services/      # Business logic services
│   │   │   └── utils/         # Utility functions
│   │   └── tests/             # Backend tests
│   ├── frontend/              # Vue.js 3 Application
│   │   ├── src/
│   │   │   ├── components/    # Reusable Vue components
│   │   │   ├── views/         # Page-level components
│   │   │   ├── stores/        # Pinia state management
│   │   │   └── utils/         # Frontend utilities
│   │   └── public/            # Static assets
│   └── deployment/            # Docker, Fly.io configurations
│
├── migration/                 # 🔄 Migration Tools & Scripts
│   ├── data-migration/        # Database migration utilities
│   ├── testing/               # Migration validation tests
│   └── validation/            # Data integrity checks
│
├── docs/                      # 📖 Project Documentation
│   ├── PROJECT_GUIDE_FOR_AI_AGENTS.md    # Legacy system analysis
│   ├── PROJECT_SPECIFICATION.md          # Technical specifications
│   └── IMPLEMENTATION_ROADMAP.md         # Development roadmap
│
├── shared/                    # 🔧 Shared Resources
│   ├── data/                  # Sample data, schemas
│   └── assets/                # Common files and configurations
│
├── .gitignore                 # Git ignore patterns
└── README.md                  # This file
```

## 🎯 Project Overview

### **Legacy System (Reference Only)**
- **Technology**: ASP.NET Web Forms 4.5, VB.NET, Microsoft Access
- **Purpose**: Original Twilio voice and SMS management system
- **Status**: Preserved for reference and data migration
- **Location**: `legacy/` directory

### **Modern System (Active Development)**
- **Backend**: Python FastAPI with SQLite database
- **Frontend**: Vue.js 3 with Composition API and Tailwind CSS
- **Purpose**: Multi-tenant, scalable replacement system
- **Status**: Under development following AI-friendly patterns
- **Location**: `modern/` directory

## 🚀 Quick Start

### **For Julius AI Development**

1. **Understanding the Project**:
   - Read `docs/PROJECT_SPECIFICATION.md` for complete technical details
   - Review `docs/IMPLEMENTATION_ROADMAP.md` for step-by-step development plan
   - Reference `docs/PROJECT_GUIDE_FOR_AI_AGENTS.md` for legacy system analysis

2. **Development Focus**:
   - **Active Work**: `modern/` directory
   - **Reference**: `legacy/` directory
   - **Guidance**: `docs/` directory

3. **Key Features to Implement**:
   - Multi-tenant architecture with API key authentication
   - Voice message management with TwiML generation
   - SMS sending with variable substitution
   - File upload and management for audio files
   - Real-time communication logging
   - iframe embedding for Delphi applications

### **For Local Development**

```bash
# Backend setup
cd modern/backend
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
pip install -r requirements.txt
uvicorn app.main:app --reload

# Frontend setup
cd modern/frontend
npm install
npm run dev
```

## 🏢 Architecture Overview

### **Multi-Tenant Design**
- **50 tenants maximum** with isolated data and configurations
- **API key authentication** (header or URL parameter for iframe)
- **Tenant-specific Twilio configurations** (SID, token, phone numbers)

### **Technology Stack**
- **Backend**: Python 3.11+, FastAPI, SQLite, Twilio SDK
- **Frontend**: Vue.js 3.3+, Vite, Tailwind CSS, Pinia
- **Deployment**: Docker, Fly.io cloud hosting
- **Integration**: iframe embedding in Delphi applications

### **Key Capabilities**
- **Voice Calls**: Dynamic TwiML generation with SSML support
- **SMS Messaging**: Template-based with variable substitution
- **File Management**: MP3 upload and storage
- **Activity Logging**: Complete audit trail of communications
- **Error Handling**: Comprehensive logging and user feedback

## 📋 Migration Strategy

### **Phase 1: Legacy Analysis** ✅
- Complete system documentation
- Technology stack identification
- Feature inventory and mapping

### **Phase 2: Modern Implementation** 🚧
- FastAPI backend development
- Vue.js frontend creation
- Multi-tenant architecture implementation

### **Phase 3: Data Migration** 📋
- Access database to SQLite conversion
- File migration and validation
- Feature parity verification

### **Phase 4: Deployment** 📅
- Production environment setup
- Delphi integration testing
- System cutover and monitoring

## 🔧 Development Guidelines

### **For AI Agents**
- Follow the step-by-step roadmap in `docs/IMPLEMENTATION_ROADMAP.md`
- Reference complete specifications in `docs/PROJECT_SPECIFICATION.md`
- Use the legacy system (`legacy/`) for understanding existing functionality
- Focus development efforts in `modern/` directory
- Maintain backward compatibility for Delphi iframe integration

### **Code Organization**
- **Clear separation** between legacy (reference) and modern (active) code
- **Modular architecture** with well-defined service boundaries
- **Comprehensive testing** at both unit and integration levels
- **AI-friendly patterns** with clear documentation and type hints

## 🌐 Deployment

### **Target Platform**: Fly.io
- **Hosting**: Containerized FastAPI + Vue.js application
- **Database**: SQLite with volume persistence
- **Scaling**: Support for 50 tenants, 500 concurrent users
- **Cost**: <$10/month operational overhead

### **Integration Requirements**
- **iframe embedding** in existing Delphi applications
- **API compatibility** for seamless transition
- **Security compliance** with multi-tenant isolation
- **Performance targets** of <200ms API response times

## 📖 Documentation

- **[PROJECT_SPECIFICATION.md](docs/PROJECT_SPECIFICATION.md)**: Complete technical specifications
- **[IMPLEMENTATION_ROADMAP.md](docs/IMPLEMENTATION_ROADMAP.md)**: Step-by-step development plan
- **[PROJECT_GUIDE_FOR_AI_AGENTS.md](docs/PROJECT_GUIDE_FOR_AI_AGENTS.md)**: Legacy system analysis

## 🤝 Contributing

This project is designed for AI agent development. Follow the implementation roadmap and refer to the comprehensive documentation for guidance on contributing to the modern system development.

---

**Status**: Active development in `modern/` directory  
**Legacy**: Preserved in `legacy/` directory for reference  
**Documentation**: Complete specifications available in `docs/`