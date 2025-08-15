@echo off
setlocal enabledelayedexpansion

REM =========================
REM cdSMS Dev Starter (Windows)
REM Starts FastAPI backend + Vite frontend in separate CMD windows.
REM Place this file in the REPO ROOT and double‑click.
REM =========================

REM Move to the directory this script lives in (repo root)
pushd "%~dp0"

REM -------- Paths (adjust if your layout differs) --------
set "BACKEND_DIR=modern\backend"
set "FRONTEND_DIR=modern\frontend"
set "VENV_DIR=%BACKEND_DIR%\.venv"

REM -------- Tooling checks --------
where py >nul 2>nul
if errorlevel 1 (
  echo [ERROR] Python Launcher ^('py'^) not found. Install Python 3 from https://www.python.org/downloads/ with "Add python.exe to PATH".
  exit /b 1
)

where npm >nul 2>nul
if errorlevel 1 (
  echo [ERROR] npm not found. Install Node.js LTS from https://nodejs.org/ .
  exit /b 1
)

REM -------- Python venv setup --------
if not exist "%VENV_DIR%\Scripts\python.exe" (
  echo [SETUP] Creating Python venv in %VENV_DIR% ...
  py -3 -m venv "%VENV_DIR%"
  if errorlevel 1 (
    echo [ERROR] Failed to create venv. Aborting.
    exit /b 1
  )
)

echo [SETUP] Upgrading pip and installing backend requirements...
"%VENV_DIR%\Scripts\python.exe" -m pip install --upgrade pip
if exist "%BACKEND_DIR%\requirements.txt" (
  "%VENV_DIR%\Scripts\python.exe" -m pip install -r "%BACKEND_DIR%\requirements.txt"
) else (
  echo [WARN] %BACKEND_DIR%\requirements.txt not found. Skipping pip install.
)

REM -------- Frontend deps --------
set "NPM_INSTALL=install"
if exist "%FRONTEND_DIR%\package-lock.json" set "NPM_INSTALL=ci"

echo [SETUP] Installing frontend packages with npm %NPM_INSTALL% ...
pushd "%FRONTEND_DIR%"
npm %NPM_INSTALL%
if errorlevel 1 (
  echo [ERROR] npm %NPM_INSTALL% failed. Check Node.js install and package.json.
  popd
  exit /b 1
)
popd

REM -------- Runtime config --------
set "UVICORN_APP=app.main:app"
set "UVICORN_PORT=8000"
set "BACKEND_TITLE=cdSMS Backend (Uvicorn)"
set "FRONTEND_TITLE=cdSMS Frontend (Vite)"

echo [RUN] Starting backend at http://localhost:%UVICORN_PORT% ...
REM Use python -m uvicorn to ensure the venv's Uvicorn is used.
start "%BACKEND_TITLE%" cmd /k ""%VENV_DIR%\Scripts\python.exe" -m uvicorn %UVICORN_APP% --reload --port %UVICORN_PORT% --app-dir "%BACKEND_DIR%""

echo [RUN] Starting frontend dev server ...
start "%FRONTEND_TITLE%" cmd /k "cd /d "%FRONTEND_DIR%" && npm run dev"

echo.
echo [OK] Launched two windows:
echo  - Backend: http://localhost:%UVICORN_PORT%
echo  - Frontend (Vite default): http://localhost:5173
echo.
echo Close the opened windows to stop the servers.
echo.

popd
endlocal
