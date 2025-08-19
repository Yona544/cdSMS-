# Legacy UI Parity Tracker — Sequential, Executable Playbook

Purpose
- Achieve functional and visual parity with the legacy ASP.NET Admin UI in the modern Vue SPA.
- Provide a single living document with:
  - A status checklist (done / in-progress / blocked)
  - Step-by-step execution instructions
  - Copy-pasteable AI prompts to implement features surgically in known files
  - QA procedures and definition of done
- All changes must preserve modern SPA routing (no .aspx) and keep legacy visual tokens scoped under `.legacy-scope`.

Primary repo anchors (clickable)
- Vite config: [vite.config.js](modern/frontend/vite.config.js), see [defineConfig()](modern/frontend/vite.config.js:5)
- Router: [index.js](modern/frontend/src/router/index.js), see [createRouter()](modern/frontend/src/router/index.js:24)
- Layout shell: [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)
- Entry: [main.js](modern/frontend/src/main.js)
- API client: [api.js](modern/frontend/src/utils/api.js)
- Views:
  - Dashboard: [AdminDashboard.vue](modern/frontend/src/views/AdminDashboard.vue)
  - Voice: [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)
  - SMS: [SMSManager.vue](modern/frontend/src/views/SMSManager.vue)
  - Files: [FileManager.vue](modern/frontend/src/views/FileManager.vue)
  - Logs: [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)
  - Login: [LoginView.vue](modern/frontend/src/views/LoginView.vue)
  - Users: [UsersList.vue](modern/frontend/src/views/UsersList.vue), [UserForm.vue](modern/frontend/src/views/UserForm.vue)
  - Voice XML: [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue), [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)
- Execution log: [LEGACY_UI_EXECUTION_LOG.md](docs/LEGACY_UI_EXECUTION_LOG.md)
- This tracker: [LEGACY_UI_PARITY_TRACKER.md](docs/LEGACY_UI_PARITY_TRACKER.md)

Status snapshot (live checklist)
- Theme/Assets
  - [x] Scoped legacy theme created: [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css)
  - [x] Imported in entry: [main.js](modern/frontend/src/main.js)
  - [x] Legacy images copied to /legacy/admin/images via [copy-legacy-assets.ps1](scripts/copy-legacy-assets.ps1)
- Layout/Nav/Footer
  - [x] `.legacy-scope` wrapper + nav strip + footer: [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)
  - [x] Legacy nav “current” state on active route
  - [x] JSON-driven nav content (single source)
- Router/Auth
  - [x] Routes: /login, /logout, /, /voice, /sms, /files, /users, /voice-xml, /logs
  - [x] Guard based on localStorage apiKey in [createRouter()](modern/frontend/src/router/index.js:24)
  - [x] /logout clears apiKey and redirects to /login
- Reusable building blocks (UI)
  - [x] SearchRow (inputs + submit)
  - [x] PaginationBar (first/prev/next/last using paging_* gifs)
  - [x] Sortable headers (table_icon_* sprites)
  - [x] TagInput (tokenized -> comma-delimited)
  - [x] ActionButtons (New/Edit/Delete/Duplicate)
- Pages (minimum viable parity)
  - Files: [x] search [x] sort [x] paging [x] actions [x] tags [FileManager.vue](modern/frontend/src/views/FileManager.vue)
  - Voice: [x] search [x] paging [x] actions [x] edit modal [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)
  - Voice XML: [x] actions [x] preview modal [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue), [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)
  - Users: [x] search [x] paging [x] actions [x] admin flags [x] tags [UsersList.vue](modern/frontend/src/views/UsersList.vue), [UserForm.vue](modern/frontend/src/views/UserForm.vue)
  - Logs: [x] filters (network/date/text) [x] paging [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)
- API Client
  - [x] Axios with X-API-Key header: [api.js](modern/frontend/src/utils/api.js)
- Windows dev stability
  - [x] Vite watch polling + ignores for legacy assets in [defineConfig()](modern/frontend/vite.config.js:5)
- QA
  - [ ] Page-level visual/functional checks
  - [ ] Final definition-of-done acceptance

How to use and update this tracker
- After completing any step, check the box above and commit this file with a short note.
- If a task is blocked, add a “Blocked” note below the checklist with the reason and a proposed fix.
- Keep all prompts and acceptance criteria in sync with actual code anchors.

Sequential execution plan

0) Preconditions (Windows)
- Use Node LTS (LTS recommended; Node v21 works with our config but may need cache clean on optional deps).
- If watch errors occur: the updated [vite.config.js](modern/frontend/vite.config.js) enables polling and ignores large public assets.
- Backend run options:
  - Without reload (stable): `cd modern\backend && .venv\Scripts\python.exe -m uvicorn app.main:app --port 8001`
  - With reload: set `WATCHFILES_FORCE_POLLING=1` and `PYTHONUTF8=1` before running

1) Theme & Assets (Complete)
- Actions:
  - [x] Create [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css) and import it in [main.js](modern/frontend/src/main.js)
  - [x] Copy legacy images via [copy-legacy-assets.ps1](scripts/copy-legacy-assets.ps1)
- Acceptance:
  - Login background and box appear
  - Header/nav/footer texture strips render
  - Table headers show textured background, alternating rows visible

2) Layout / Nav / Footer (Complete)
- Actions:
  - [x] Wrap app with `.legacy-scope` and render nav/footer in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue)
  - [x] Move nav items to a JSON list (single source) and iterate to render links
  - [x] Compute active item and add “current” class
- Acceptance:
  - Top nav shows Dashboard, Voice, SMS, Files, Users, Voice XML, Logs, Logout
  - Active route visibly highlighted
  - Logout icon uses /legacy/admin/images/nav_logout.gif

3) Router / Guard (Complete)
- Actions:
  - [x] Add routes in [index.js](modern/frontend/src/router/index.js) and guard near [createRouter()](modern/frontend/src/router/index.js:24)
- Acceptance:
  - Without apiKey -> redirect to /login?nosession=1
  - With apiKey -> page routes normally

4) Componentize primitives (recommended to implement once)
- Create simple, lightweight components (no extra deps) under a `/components/common/` or reuse page-local:
  - [x] SearchRow: props (fields/placeholder), emits (submit), uses `.inp-form`
  - [x] PaginationBar: props (page, pageCount, canPrev, canNext), emits (first, prev, next, last); uses paging_* sprites
  - [x] SortableHeader: props (key, label, dir), emits (sort), uses table_icon_* sprites
  - [x] TagInput: simple token input saving comma-delimited string
  - [x] ActionButtons: standard New/Edit/Delete/Duplicate buttons with legacy icons
- Acceptance:
  - Pages reuse these primitives; consistent visuals and behaviors

5) Files page parity — [FileManager.vue](modern/frontend/src/views/FileManager.vue)
- Implement:
  - [ ] Search row: text input, file type select, Search button (top_search_btn.gif)
  - [ ] Sortable headers: filename, size, date (table_icon_* toggles)
  - [ ] Pagination: legacy paging_* gif buttons; keep ?page=… in route query
  - [ ] Actions: Upload (existing), Edit, Delete (confirm), show message-* banners
  - [ ] Tag filter; edit form uses TagInput
- Acceptance:
  - Filters and sort change the list; paging works; banners show on operations

6) Voice page parity — [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue)
- Implement:
  - [ ] Search input; pagination
  - [ ] Actions: New/Edit/Delete/Duplicate; duplicate creates “Copy of <Name>”
  - [ ] Edit uses existing modal or a simple form; banner results
- Acceptance:
  - CRUD paths wired; duplicate works; banners show; paging/sort operate

7) Voice XML parity — [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue), [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue)
- Implement:
  - [ ] List: New/Edit/Delete with banners
  - [ ] Form: “Preview TwiML” button that opens a modal
    - Try API call to preview if endpoint exists, else generate locally from form fields
- Acceptance:
  - Preview shows TwiML; errors show message-red; saving navigates back to list

8) Users parity — [UsersList.vue](modern/frontend/src/views/UsersList.vue), [UserForm.vue](modern/frontend/src/views/UserForm.vue)
- Implement:
  - [ ] List: search, pagination, actions
  - [ ] Form: admin flag (checkbox), tags input (TagInput), save/cancel flows
- Acceptance:
  - Saving navigates back; tags persisted as comma-delimited; banners show save result

9) Logs parity — [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue)
- Implement:
  - [ ] Filters: network type (call/sms/both), date range (simple inputs), text search
  - [ ] Pagination and sort
- Acceptance:
  - Filters affect list; paging/sort operate; banners on error

10) API Client & endpoints — [api.js](modern/frontend/src/utils/api.js) (Complete)
- Actions:
  - [x] Ensure X-API-Key header is attached; baseURL ‘/api’; rely on proxy rewrite to ‘/v1’ in [defineConfig()](modern/frontend/vite.config.js:5)
- Acceptance:
  - Network tab shows X-API-Key on requests

11) QA — Visual & Functional
- Routes to verify:
  - /login, /, /voice, /sms, /files, /users, /voice-xml, /logs
- Checks:
  - No console errors; assets load under /legacy/admin/images
  - Nav active state correct
  - Pages provide search/sort/paging and actions per acceptance above
  - API errors render visible message-red banners
  - X-API-Key header present on all requests

12) Definition of Done (project-level)
- Visual: Header/nav/footer textures and legacy look present on all pages
- Functional: Search/sort/paging/actions implemented on Files/Voice/Users/Voice XML/Logs
- Stability: No unhandled exceptions; watch/dev stable on Windows (documented in [LEGACY_UI_EXECUTION_LOG.md](docs/LEGACY_UI_EXECUTION_LOG.md))
- Documentation: This tracker updated; examples and prompts current
- Parity: Team confirms features and visuals match legacy at minimum expectations

AI Implementation Prompts (copy/paste)

A) Master “Parity Uplift” prompt
- “Implement functional parity across navigation and pages in the Vue SPA using the following anchors and acceptance criteria. Use precise diffs to only these files: [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue), [index.js](modern/frontend/src/router/index.js), [FileManager.vue](modern/frontend/src/views/FileManager.vue), [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue), [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue), [UsersList.vue](modern/frontend/src/views/UsersList.vue), [UserForm.vue](modern/frontend/src/views/UserForm.vue), [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue), [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue), [api.js](modern/frontend/src/utils/api.js). Ensure styles remain scoped under `.legacy-scope`. Reference [createRouter()](modern/frontend/src/router/index.js:24) and [defineConfig()](modern/frontend/vite.config.js:5) as needed. Add SearchRow, PaginationBar, SortableHeader, TagInput, and ActionButtons (page-local or common components). Acceptances: see steps 5–9 in this tracker.”

B) Page-scoped prompts
1) Files
- “Open [FileManager.vue](modern/frontend/src/views/FileManager.vue) and add legacy SearchRow (text + fileType + Search btn using top_search_btn.gif), SortableHeader (filename/size/date with table_icon_*), and PaginationBar with paging_* gifs; reflect URL query ?page, ?q; add actions: Edit/Delete with confirm, and banners (message-*) for results.”

2) Voice
- “Open [VoiceManager.vue](modern/frontend/src/views/VoiceManager.vue); add search + pagination; actions: New/Edit/Delete/Duplicate; duplicate creates ‘Copy of …’; apply gridtable/tblheader; banners on results.”

3) Voice XML
- “Open [VoiceXmlList.vue](modern/frontend/src/views/VoiceXmlList.vue) and [VoiceXmlForm.vue](modern/frontend/src/views/VoiceXmlForm.vue); add New/Edit/Delete and a Preview TwiML modal; call an API if available else generate from form. Show message-red on errors.”

4) Users
- “Open [UsersList.vue](modern/frontend/src/views/UsersList.vue) and [UserForm.vue](modern/frontend/src/views/UserForm.vue); add search + pagination + actions; in form add admin flag checkbox and TagInput; banners for save.”

5) Logs
- “Open [ActivityLogs.vue](modern/frontend/src/views/ActivityLogs.vue); add network filter (call/sms/both), date range inputs, text search, pagination, SortableHeader. Banners on errors.”

C) Nav
- “Open [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue); replace hard-coded nav with a JSON array; render router-links and apply legacy ‘select/current’ classes; compute current by startsWith($route.path, item.path). Keep /logout image and .legacy-scope.”

D) Router guard
- “Open [index.js](modern/frontend/src/router/index.js) near [createRouter()](modern/frontend/src/router/index.js:24); ensure /login is public, everything else protected; redirect unauthenticated to /login?nosession=1; /logout clears apiKey.”

Dev and troubleshooting notes

- Frontend
  - If Vite watcher errors on Windows: we already set `watch.usePolling = true` and ignore legacy/public directories in [defineConfig()](modern/frontend/vite.config.js:5)
  - If npm optional dependency errors appear, delete `node_modules` and `package-lock.json`, then run `npm install`
- Backend
  - If reloader crashes, run without `--reload` as documented; otherwise set `WATCHFILES_FORCE_POLLING=1` and `PYTHONUTF8=1`

Appendix A — Legacy sprites used
- Menu: pro_line_*.gif
- Table icons: table_icon_*.gif
- Pagination: paging_*.gif
- Search: top_search_btn.gif
- Buttons: submit_login.gif
- Backgrounds: repeat.jpg, content_repeat.jpg, table_header_repeat.jpg
(All in /legacy/admin/images; ensure present via [copy-legacy-assets.ps1](scripts/copy-legacy-assets.ps1))

Appendix B — Link anchors for constructs
- Router creation: [createRouter()](modern/frontend/src/router/index.js:24)
- Vite config: [defineConfig()](modern/frontend/vite.config.js:5)

Changelog (maintain as you go)
- 2025-08-19: Initial tracker created; theme/assets/layout/routes/guard complete; page stubs added; Windows dev stability improved via Vite config; QA and parity tasks enumerated.
## Architecture Decision: Top Navigation Source-of-Truth (ADR-001)

Date: 2025-08-19

Context
- We must reproduce the legacy UI look, but all legacy visuals must stay scoped to `.legacy-scope`. Theme import is already handled by [main.js](modern/frontend/src/main.js) and styles live in [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css).
- SPA routing is created by [createRouter()](modern/frontend/src/router/index.js:24). We cannot introduce any `.aspx` routes.
- Deliverable 1 explicitly requires replacing hard-coded nav links with a JSON-driven menu in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue), with an active “current” state and legacy classes/textures.

Decision
- Use a single JSON array constant defined in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue) as the source of truth for top-level nav items. Each item includes:
  - key: stable identifier (e.g., "dashboard", "voice", "files", "users", "voice-xml", "logs", "logout")
  - label: display text (Logout renders an image instead of text)
  - path: exact SPA path matching [index.js](modern/frontend/src/router/index.js) routes
- Render the nav under a `.legacy-scope` ancestor using a UL with classes "nav select". Apply the "current" class on the LI when active.
- Active-state logic:
  - Dashboard (path "/"): exact match via `route.path === "/"` (or exact match if using "/dashboard")
  - All other items: prefix match via `route.path.startsWith(item.path)` and ensure `item.path !== "/"` to avoid false positives.
- SPA links via &lt;router-link&gt; exclusively; no plain anchors.
- Logout renders the image `/legacy/admin/images/nav_logout.gif` and links to `/logout`.
- SMS is omitted unless a real route exists in [index.js](modern/frontend/src/router/index.js).

Rationale
- Keeps the nav config localized to the layout, aligning with Deliverable 1 while minimizing cross-file coupling.
- Reduces risk of drift because updates for new sections are performed in one obvious place (the JSON array).
- Avoids introducing router metadata as a config surface, preserving the existing routing guard and structure.

Maintainability safeguards
- Dev-only assertion (optional): Compare `navItems[].path` against `router.getRoutes().map(r =&gt; r.path)` and `console.warn` on mismatches.
- Clear contributor rule: When adding/removing a top-level feature, update both [index.js](modern/frontend/src/router/index.js) and [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue), then verify active-state behavior and legacy textures.
- Omit SMS until a real route is introduced to prevent dead links.

QA checklist (manual)
- UL renders with "nav select" under `.legacy-scope` and shows legacy hover/background textures.
- The active route highlights via "current" exactly as specified (exact for "/", prefix for others).
- Logout shows `/legacy/admin/images/nav_logout.gif` and routes to `/logout`.
- No console errors; all assets load from `/legacy/admin/images`.
- Guard behavior remains: unauthenticated users get redirected to `/login?nosession=1`.

Status
- Adopted for Deliverable 1. Future additions must follow the same pattern and be documented here.
## Architecture Decision: Top Navigation Source-of-Truth (ADR-001)

Date: 2025-08-19

Context
- Legacy visuals must remain scoped to `.legacy-scope`. Theme is imported by [main.js](modern/frontend/src/main.js) and styles reside in [legacy-theme.css](modern/frontend/src/assets/styles/legacy-theme.css).
- SPA routing is created by [createRouter()](modern/frontend/src/router/index.js:24); no `.aspx` routes are allowed.
- Deliverable 1 requires replacing hard-coded nav links with a JSON-driven menu in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue), with an active “current” state and legacy classes/textures.

Decision
- Use a single JSON array constant defined in [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue) as the source of truth for top-level nav items. Each item includes:
  - key: stable identifier (e.g., "dashboard", "voice", "files", "users", "voice-xml", "logs", "logout")
  - label: display text (Logout renders an image instead of text)
  - path: exact SPA path matching routes in [index.js](modern/frontend/src/router/index.js)
- Render the nav under a `.legacy-scope` ancestor using a UL with classes `"nav select"`. Apply the `"current"` class on the LI when active.
- Active-state logic:
  - Dashboard (path "/"): exact match via `route.path === "/"` (or exact match if Dashboard uses a non-root path such as "/dashboard").
  - All other items: prefix match via `route.path.startsWith(item.path)` and ensure `item.path !== "/"` to avoid false positives.
- Use &lt;router-link&gt; elements exclusively; no plain anchors.
- Logout renders the image `/legacy/admin/images/nav_logout.gif` (asset present at [nav_logout.gif](modern/frontend/public/legacy/admin/images/nav_logout.gif)) and links to `/logout`.
- Omit “SMS” unless a real route exists in [index.js](modern/frontend/src/router/index.js); do not add dead links.

Rationale
- Centralizes navigation configuration in one obvious place (the layout), minimizing cross-file coupling and risk of drift.
- Keeps the router simple and preserves existing guards created by [createRouter()](modern/frontend/src/router/index.js:24).
- Aligns with legacy visual constraints by keeping all styling under `.legacy-scope` and using existing textures.

Maintainability safeguards
- Optional dev-time check: Compare `navItems[].path` against `router.getRoutes().map(r =&gt; r.path)` and `console.warn` on mismatches to surface drift during development.
- Contributor rule: When adding/removing a top-level feature, update both [index.js](modern/frontend/src/router/index.js) and [AppLayout.vue](modern/frontend/src/components/layout/AppLayout.vue), then manually verify active-state behavior and legacy textures.
- Keep asset paths absolute under `/legacy/admin/images/*`. Do not relocate assets; reference the copies under [public/legacy/admin/images](modern/frontend/public/legacy/admin/images/) for build-time availability.

QA checklist (manual)
- UL renders with `"nav select"` under `.legacy-scope` and shows legacy hover/background textures.
- The active route highlights via `"current"` exactly as specified (exact for "/", prefix for others).
- Logout shows `/legacy/admin/images/nav_logout.gif` and routes to `/logout`.
- No console errors; all assets load from `/legacy/admin/images`.
- Guard behavior remains: unauthenticated users are redirected to `/login?nosession=1` by the existing router guard created with [createRouter()](modern/frontend/src/router/index.js:24).

Status
- Adopted for Deliverable 1. Future additions must follow the same pattern and be documented here.