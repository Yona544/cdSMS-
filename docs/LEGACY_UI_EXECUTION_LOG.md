# Legacy UI Replication — Execution Log

Purpose
- Implement the legacy ASP.NET admin look within the modern Vue SPA using scoped CSS and modern routes only (no .aspx).
- Add minimal auth flow with an API key, router guard, and stub views for users and voice-xml.
- Provide deterministic steps, references, and troubleshooting for Windows.

Primary references
- Plan: [LEGACY_UI_REPLICATION_PLAN.md](docs/LEGACY_UI_REPLICATION_PLAN.md)
- Router creation: [createRouter()](modern/frontend/src/router/index.js:24)
- Vite config: [defineConfig()](modern/frontend/vite.config.js:5)
- Layout shell: [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)

What was implemented

1) Scoped legacy theme
- Added scoped legacy theme styled under .legacy-scope: [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css)
- Imported once in app entry: [import](modern/frontend/src/main.js:5)

2) Legacy images availability
- Asset copy script (Windows PowerShell): [copy-legacy-assets.ps1](scripts/copy-legacy-assets.ps1)
- Copies legacy/Admin/images/* → modern/frontend/public/legacy/admin/images/*
- Absolute URLs used in CSS: /legacy/admin/images/...

3) Layout updated with legacy look
- Wrapped the app in .legacy-scope and added legacy-style top nav strip and footer shell while preserving modern shell (Header/Sidebar).
- Implemented in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)
  - Logout icon path: /legacy/admin/images/nav_logout.gif
  - Nav backgrounds: repeat.jpg, pro_line_1.gif (pro_line_0.gif not present, CSS uses pro_line_0x.gif)

4) Router and auth guard
- Added top-level /login and /logout (logout clears key then redirects).
- Added Users and Voice XML route groups beneath the AppLayout.
- Simple guard enforced via localStorage apiKey in [createRouter()](modern/frontend/src/router/index.js:24).

5) Views added or modified
- Login (API-key-first): [LoginView.vue](modern/frontend/src/views/LoginView.vue)
- Logout (clears key → /login): [LogoutView.vue](modern/frontend/src/views/LogoutView.vue)
- Users: [UsersList.vue](modern/frontend/src/views/UsersList.vue), [UserForm.vue](modern/frontend/src/views/UserForm.vue)
- Voice XML: [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue), [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)
- Applied legacy classes to existing pages:
  - Files: [FileManager.vue](modern/frontend/src/views/FileManager.vue)
  - Voice: [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)
  - Logs: [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)

6) API client header injection
- Axios client with X-API-Key for all requests: [api.js](modern/frontend/src/utils/api.js)
- Works with proxy baseURL '/api' configured by [defineConfig()](modern/frontend/vite.config.js:15)

Run instructions (Windows)

A) Copy legacy assets (one-time or when refreshed)
- PowerShell:
  - scripts\copy-legacy-assets.ps1 -CleanDest

B) Start backend (FastAPI) — option 1: via launcher (recommended)
- start-dev.bat both
- The launcher handles venv, ports, and health checks out of the box.

C) Start backend (FastAPI) — option 2: manual
- Create venv and install:
  - python -m venv modern\backend\.venv
  - modern\backend\.venv\Scripts\python.exe -m pip install -r modern\backend\requirements.txt
- Launch:
  - Without reload (more stable on Windows):
    - cd modern\backend
    - ..\.venv\Scripts\python.exe -m uvicorn app.main:app --port 8001
  - With reload (if you need it):
    - set WATCHFILES_FORCE_POLLING=1
    - set PYTHONUTF8=1
    - ..\.venv\Scripts\python.exe -m uvicorn app.main:app --reload --port 8001
- Health: http://localhost:8001/v1/health/healthz

D) Start frontend (Vite) — recommended clean routine
- Due to npm optional dependency bugs on Windows (rollup/esbuild), perform a fully clean install if you see errors like:
  - "Cannot find module @rollup/rollup-win32-x64-msvc"
  - EPERM unlink for esbuild.exe
- Clean and reinstall:
  - Close any Vite/Node processes that may hold locks (kill node, close editors if needed)
  - PowerShell:
    - Remove-Item -Recurse -Force modern/frontend/node_modules
    - Remove-Item -Force modern/frontend/package-lock.json -ErrorAction SilentlyContinue
  - Reinstall:
    - cd modern\frontend
    - npm install
  - Run:
    - npm run dev -- --port 5174

E) Vite proxy
- Already configured to proxy '/api' → 'http://localhost:8001' with '/v1' rewrite by [defineConfig()](modern/frontend/vite.config.js:15)
- Frontend requests go to /api/... and are rewritten to backend /v1/... endpoints.

Troubleshooting notes (Windows)

Frontend: npm optional dependencies (Rollup / esbuild)
- Symptom: "Cannot find module @rollup/rollup-win32-x64-msvc" or locked esbuild.exe on Windows
- Fix:
  1) Close any running node/vite processes
  2) Remove node_modules and package-lock.json
  3) npm install
  4) npm run dev

- If antivirus is locking node binaries:
  - Pause realtime scanning temporarily during install, or retry the clean steps.

Backend: uvicorn early crash or reloader issues
- Symptom: Exit codes 3221225477/3221225478, broken pipe, or reloader crash on Windows
- Fix:
  - Prefer running without --reload first:
    - modern\backend\.venv\Scripts\python.exe -m uvicorn app.main:app --port 8001
  - If you need reload, set:
    - set WATCHFILES_FORCE_POLLING=1
    - set PYTHONUTF8=1
    - then run with --reload
  - Confirm the app imports: [app.main](modern/backend/app/main.py)

Visual QA checklist
- Pages:
  - /login → background + login box use legacy assets
  - / → dashboard under legacy nav strip
  - /voice, /sms → navigation and header visible
  - /files → list + header uses gridtable/tblheader
  - /users → list page renders; add/edit routes exist
  - /voice-xml → list + form stubs render
  - /logs → list styled with legacy table wrappers
- Assets resolve from /legacy/admin/images
- Guard redirects to /login without apiKey set
- X-API-Key sent on requests via [api.js](modern/frontend/src/utils/api.js)

Source of truth — updated files
- Theme: [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css)
- Theme import: [main.js](modern/frontend/src/main.js)
- Layout shell: [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)
- Router + guard + route additions: [index.js](modern/frontend/src/router/index.js)
- API client: [api.js](modern/frontend/src/utils/api.js)
- Views:
  - [LoginView.vue](modern/frontend/src/views/LoginView.vue)
  - [LogoutView.vue](modern/frontend/src/views/LogoutView.vue)
  - [UsersList.vue](modern/frontend/src/views/UsersList.vue)
  - [UserForm.vue](modern/frontend/src/views/UserForm.vue)
  - [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue)
  - [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)
  - [FileManager.vue](modern/frontend/src/views/FileManager.vue)
  - [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)
  - [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)
- Dev helper stylesheet: [index.css](modern/frontend/src/assets/styles/index.css)
- Asset copy script: [copy-legacy-assets.ps1](scripts/copy-legacy-assets.ps1)

Acceptance status vs plan
- Implemented:
  - Scoped theme with legacy visuals and public asset paths
  - App layout wrapper with legacy nav/footer
  - Router updates, /login, /logout, users and voice-xml routes
  - Guard and header injection
  - View stubs for users and voice-xml
  - Legacy classes applied to key pages
- Outstanding (environment-specific):
  - Local run issues on Windows for npm optional dependencies and uvicorn reloader
  - Provided deterministic fix steps

Next steps
- Perform the frontend clean install sequence, then run:
  - npm run dev -- --port 5174
- Start backend without reload:
  - modern\backend\.venv\Scripts\python.exe -m uvicorn app.main:app --port 8001
- Proceed with visual QA and adjust [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css) if spacing or alignment requires tuning.

Update — 2025-08-19 22:02 ET
- Navigation:
 - JSON-driven top nav with active “current” state in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue); logout icon uses /legacy/admin/images/nav_logout.gif.
- Router guard:
 - /logout now clears apiKey and redirects to /login via beforeEach guard in [router/index.js](modern/frontend/src/router/index.js).
- Files ([FileManager.vue](modern/frontend/src/views/FileManager.vue)):
 - Added legacy-style search row (q, type, tags), sortable headers (Filename/Size/Updated), and pagination using paging_* gifs.
 - State syncs to route query (?q, ?type, ?tag, ?sort, ?dir, ?page). Success/error banners render. Uses existing FileListItem rows.
- Voice ([VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)):
 - Added search and pagination with route query sync; actions (create/edit/delete/duplicate) wired to store; banners on results.
- Voice XML:
 - List ([VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue)): search, pagination, Edit/Delete actions with banners.
 - Form ([VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)): “Preview TwiML” modal; tries backend /voice-xml/preview then falls back to local generation.
- Users:
 - List ([UsersList.vue](modern/frontend/src/views/UsersList.vue)): search, sort, pagination, Edit/Delete actions; banners for outcomes.
 - Form ([UserForm.vue](modern/frontend/src/views/UserForm.vue)): admin flag and tags (comma-delimited) added; save returns to list.
- Activity Logs ([ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)):
 - Filters for network (call/sms/both), date range, and free text; sortable headers; pagination with paging_* gifs; query sync.

Tracker
- Updated [LEGACY_UI_PARITY_TRACKER.md](docs/LEGACY_UI_PARITY_TRACKER.md) to reflect completed navigation, router/logout handling, and page-level parity progress.