@echo off
setlocal EnableExtensions EnableDelayedExpansion
title cdSMS Dev Launcher

REM ==========================================================
REM Modes: start-dev.bat [both|backend|frontend|stop|clean|help]
REM Features: modes, smart ports, DB prep (safe), health checks,
REM           logs, Windows Terminal panes (if available),
REM           crash assist + optional retry.
REM ==========================================================

REM ----- repo root -----
pushd "%~dp0"
set "REPO_ROOT=%CD%"

REM ----- config -----
set "BACKEND_DIR=modern\backend"
set "FRONTEND_DIR=modern\frontend"
set "VENV_DIR=%BACKEND_DIR%\.venv"
set "UVICORN_APP=app.main:app"
set "BACKEND_PORT_START=8001"
set "FRONTEND_PORT_START=5173"
set "STATE_DIR=.dev"
set "LOG_DIR=%STATE_DIR%\logs"
set "STATE_FILE=%STATE_DIR%\state.env"
set "RETRY_ON_FAIL=1"

REM ----- args / mode -----
set "MODE=%~1"
if "%MODE%"=="" set "MODE=both"
if /I "%MODE%"=="help" goto :usage

REM ----- dirs -----
if not exist "%STATE_DIR%"  mkdir "%STATE_DIR%"
if not exist "%LOG_DIR%"    mkdir "%LOG_DIR%"

REM ----- tools -----
where powershell >nul 2>nul || (echo [ERROR] PowerShell not found. & goto :fail_pause)

REM Gate Node/npm checks to modes that involve the frontend (both|frontend)
if /I not "%MODE%"=="backend" (
  where npm        >nul 2>nul || (echo [ERROR] npm not found. Install Node.js LTS. & goto :fail_pause)
  where node       >nul 2>nul || (echo [ERROR] Node.js not found. Install Node.js LTS. & goto :fail_pause)

  REM Find npm.cmd explicitly (bypasses npm.ps1 execution policy issues)
  set "NPM_CMD_EXE="
  for /f "delims=" %%P in ('where npm.cmd 2^>nul') do (
    if not defined NPM_CMD_EXE set "NPM_CMD_EXE=%%~fP"
  )
  if not defined NPM_CMD_EXE if exist "%ProgramFiles%\nodejs\npm.cmd" set "NPM_CMD_EXE=%ProgramFiles%\nodejs\npm.cmd"
  if not defined NPM_CMD_EXE (
    echo [ERROR] Unable to locate npm.cmd. Ensure Node.js is installed.
    goto :fail_pause
  )
)

set "PYTHON_EXE=python"
where %PYTHON_EXE% >nul 2>nul || (
  set "PYTHON_EXE=py"
  where %PYTHON_EXE% >nul 2>nul || (echo [ERROR] Neither 'python' nor 'py' found on PATH. & goto :fail_pause)
)

REM ----- versions (info only; NO PIPES) -----
for /f %%V in ('%PYTHON_EXE% -c "import sys; print(str(sys.version_info[0])+'.'+str(sys.version_info[1]))"') do set "PY_SHORT=%%V"
for /f %%V in ('node -p "process.version" 2^>nul') do set "NODE_VER=%%V"
if not defined NODE_VER set "NODE_VER=(unknown)"
echo [INFO] Python %PY_SHORT%  -  Node %NODE_VER%

REM ----- layout checks -----
if not exist "%BACKEND_DIR%\app\main.py" (
  echo [ERROR] Backend entry not found: %BACKEND_DIR%\app\main.py
  goto :fail_pause
)
if /I not "%MODE%"=="backend" if not exist "%FRONTEND_DIR%\package.json" (
  echo [ERROR] Frontend package.json not found: %FRONTEND_DIR%\package.json
  goto :fail_pause
)

REM ----- route modes -----
if /I "%MODE%"=="stop"     goto :stop_servers
if /I "%MODE%"=="clean"    goto :clean_all
if /I "%MODE%"=="backend"  set "RUN_BACKEND_ONLY=1"
if /I "%MODE%"=="frontend" set "RUN_FRONTEND_ONLY=1"

REM ===== envs / deps =====
if not defined RUN_FRONTEND_ONLY (
  call :ensure_python_env
  if errorlevel 1 goto :fail_pause
)
if not defined RUN_BACKEND_ONLY (
  call :ensure_frontend_deps
  if errorlevel 1 goto :fail_pause
)

REM ===== ports =====
call :choose_ports

REM ===== start windows =====
call :start_windows
if errorlevel 1 goto :fail_pause

REM ===== health + open browser (and optional one-shot retry) =====
call :post_launch_checks

echo.
echo [OK] Launch complete. This window stays open; press Ctrl+C to exit.
echo.
timeout /t -1 >nul
goto :end

:: ---------------- subroutines ----------------

:usage
echo.
echo Usage:
echo   start-dev.bat ^<mode^>
echo     both      - start backend and frontend  ^(default^)
echo     backend   - start backend only
echo     frontend  - start frontend only
echo     stop      - stop servers by recorded ports
echo     clean     - stop and remove venv, node_modules, caches
echo     help      - show this help
echo.
goto :end

:ensure_python_env
if not exist "%VENV_DIR%\Scripts\python.exe" (
  echo [SETUP] Creating venv at %VENV_DIR% ...
  %PYTHON_EXE% -m venv "%VENV_DIR%"
  if errorlevel 1 ( echo [ERROR] venv creation failed. & exit /b 1 )
)
set "VENV_PY=%VENV_DIR%\Scripts\python.exe"

echo [SETUP] pip install/upgrade...
"%VENV_PY%" -m pip install --upgrade pip
if exist "%BACKEND_DIR%\requirements.txt" (
  "%VENV_PY%" -m pip install -r "%BACKEND_DIR%\requirements.txt"
  if errorlevel 1 ( echo [ERROR] pip install failed. & exit /b 1 )
) else (
  echo [WARN] %BACKEND_DIR%\requirements.txt not found. Skipping pip install.
)

REM ensure uvicorn present
"%VENV_PY%" -c "import importlib.util,sys; sys.exit(0 if importlib.util.find_spec('uvicorn') else 1)"
if errorlevel 1 (
  echo [SETUP] Installing uvicorn...
  "%VENV_PY%" -m pip install "uvicorn[standard]"
  if errorlevel 1 ( echo [ERROR] uvicorn install failed. & exit /b 1 )
)

REM Alembic (safe) — only run if ini AND a migrations dir likely exists
if exist "%BACKEND_DIR%\alembic.ini" (
  set "ALEMBIC_DIR="
  for %%D in ("migrations" "alembic" "app\alembic") do (
    if exist "%BACKEND_DIR%\%%~D" set "ALEMBIC_DIR=%BACKEND_DIR%\%%~D"
  )
  if defined ALEMBIC_DIR (
    echo [DB] Running Alembic migrations...
    pushd "%BACKEND_DIR%"
    "%VENV_PY%" -m alembic upgrade head
    if errorlevel 1 ( echo [WARN] Alembic failed; continuing. )
    popd
  ) else (
    echo [DB] alembic.ini found but no migrations folder detected; skipping.
  )
)
exit /b 0

:ensure_frontend_deps
pushd "%FRONTEND_DIR%"
set "NPM_CMD=install"
if exist "package-lock.json" set "NPM_CMD=ci"

echo [SETUP] npm %NPM_CMD% in %CD% ...
call "%NPM_CMD_EXE%" %NPM_CMD%
set "ERR=%ERRORLEVEL%"
if /I "%NPM_CMD%"=="ci" if not "%ERR%"=="0" (
  echo [WARN] npm ci failed. Falling back to npm install...
  call "%NPM_CMD_EXE%" install
  set "ERR=%ERRORLEVEL%"
)
popd
if not "%ERR%"=="0" ( echo [ERROR] npm dependencies failed. & exit /b 1 )
exit /b 0

:choose_ports
set "BACKEND_PORT=%BACKEND_PORT_START%"
call :next_free_port %BACKEND_PORT% BACKEND_PORT

if not defined RUN_BACKEND_ONLY set "FRONTEND_PORT=%FRONTEND_PORT_START%"
if not defined RUN_BACKEND_ONLY call :next_free_port %FRONTEND_PORT% FRONTEND_PORT

> "%STATE_FILE%" echo BACKEND_PORT=%BACKEND_PORT%
if not defined RUN_BACKEND_ONLY >> "%STATE_FILE%" echo FRONTEND_PORT=%FRONTEND_PORT%
>> "%STATE_FILE%" echo STARTED_AT=%DATE% %TIME%
exit /b 0

:next_free_port
set "PORT=%~1"
set "OUTVAR=%~2"
:loop_free
powershell -NoProfile -Command "$p=%PORT%; try{$l=[Net.Sockets.TcpListener]::new([Net.IPAddress]::Parse('127.0.0.1'),$p); $l.Start(); $l.Stop(); exit 0}catch{exit 1}"
if errorlevel 1 (
  set /a PORT+=1
  goto :loop_free
)
set "%OUTVAR%=%PORT%"
exit /b 0

:start_windows
for /f %%D in ('powershell -NoProfile -Command "(Get-Date -Format yyyyMMdd_HHmmss)"') do set "STAMP=%%D"
set "BACKEND_LOG=%LOG_DIR%\backend-%STAMP%.log"
set "FRONTEND_LOG=%LOG_DIR%\frontend-%STAMP%.log"

where wt >nul 2>nul
if not errorlevel 1 (
  REM ---- Windows Terminal panes (no pipes) ----
  if defined RUN_BACKEND_ONLY (
    wt -w 0 new-tab PowerShell -NoProfile -NoExit -Command ^
      "Set-Location '%REPO_ROOT%\%BACKEND_DIR%'; $env:WATCHFILES_FORCE_POLLING='1'; $env:PYTHONUTF8='1'; & '%REPO_ROOT%\%VENV_DIR%\Scripts\python.exe' -m uvicorn %UVICORN_APP% --reload --port %BACKEND_PORT% *>> '%REPO_ROOT%\%BACKEND_LOG%'" 
    goto :record_logs
  )
  if defined RUN_FRONTEND_ONLY (
    wt -w 0 new-tab PowerShell -NoProfile -NoExit -Command ^
      "Set-Location '%REPO_ROOT%\%FRONTEND_DIR%'; $env:CHOKIDAR_USEPOLLING='1'; & '%NPM_CMD_EXE%' run dev -- --port %FRONTEND_PORT% *>> '%REPO_ROOT%\%FRONTEND_LOG%'" 
    goto :record_logs
  )
  wt -w 0 new-tab PowerShell -NoProfile -NoExit -Command ^
    "Set-Location '%REPO_ROOT%\%BACKEND_DIR%'; $env:WATCHFILES_FORCE_POLLING='1'; $env:PYTHONUTF8='1'; & '%REPO_ROOT%\%VENV_DIR%\Scripts\python.exe' -m uvicorn %UVICORN_APP% --reload --port %BACKEND_PORT% *>> '%REPO_ROOT%\%BACKEND_LOG%'" ^
    ; split-pane -H PowerShell -NoProfile -NoExit -Command ^
    "Set-Location '%REPO_ROOT%\%FRONTEND_DIR%'; $env:CHOKIDAR_USEPOLLING='1'; & '%NPM_CMD_EXE%' run dev -- --port %FRONTEND_PORT% *>> '%REPO_ROOT%\%FRONTEND_LOG%'" 
  goto :record_logs
)

REM ---- Fallback: two PowerShell windows (no pipes) ----
if defined RUN_BACKEND_ONLY (
  start "cdSMS Backend" powershell -NoProfile -NoExit -Command ^
    "Set-Location '%REPO_ROOT%\%BACKEND_DIR%'; $env:WATCHFILES_FORCE_POLLING='1'; $env:PYTHONUTF8='1'; & '%REPO_ROOT%\%VENV_DIR%\Scripts\python.exe' -m uvicorn %UVICORN_APP% --reload --port %BACKEND_PORT% *>> '%REPO_ROOT%\%BACKEND_LOG%'" 
  goto :record_logs
)
start "cdSMS Backend" powershell -NoProfile -NoExit -Command ^
  "Set-Location '%REPO_ROOT%\%BACKEND_DIR%'; $env:WATCHFILES_FORCE_POLLING='1'; $env:PYTHONUTF8='1'; & '%REPO_ROOT%\%VENV_DIR%\Scripts\python.exe' -m uvicorn %UVICORN_APP% --reload --port %BACKEND_PORT% *>> '%REPO_ROOT%\%BACKEND_LOG%'" 
start "cdSMS Frontend" powershell -NoProfile -NoExit -Command ^
  "Set-Location '%REPO_ROOT%\%FRONTEND_DIR%'; $env:CHOKIDAR_USEPOLLING='1'; & '%NPM_CMD_EXE%' run dev -- --port %FRONTEND_PORT% *>> '%REPO_ROOT%\%FRONTEND_LOG%'" 

:record_logs
echo BACKEND_LOG=%BACKEND_LOG%>> "%STATE_FILE%"
if not defined RUN_BACKEND_ONLY echo FRONTEND_LOG=%FRONTEND_LOG%>> "%STATE_FILE%"
exit /b 0

:post_launch_checks
set "PROBE_BACKEND_URL=http://localhost:%BACKEND_PORT%/v1/health/healthz"
powershell -NoProfile -Command "$ProgressPreference='SilentlyContinue'; try { $null = Invoke-WebRequest '%PROBE_BACKEND_URL%' -UseBasicParsing -TimeoutSec 1; exit 0 } catch { exit 1 }"
if errorlevel 1 set "PROBE_BACKEND_URL=http://localhost:%BACKEND_PORT%/docs"

if not defined RUN_FRONTEND_ONLY call :wait_up "%PROBE_BACKEND_URL%" 60 "BACKEND"
if not defined RUN_BACKEND_ONLY  call :wait_up "http://localhost:%FRONTEND_PORT%/" 60 "FRONTEND"

if "%READY_BACKEND%"=="1"  start "" "http://localhost:%BACKEND_PORT%/docs"
if "%READY_FRONTEND%"=="1" start "" "http://localhost:%FRONTEND_PORT%/"

REM ----- Optional one-shot retry with bumped ports -----
if "%RETRY_ON_FAIL%"=="1" (
  set "DO_RETRY="
  if "%READY_BACKEND%"=="0" set "DO_RETRY=1"
  if "%READY_FRONTEND%"=="0" set "DO_RETRY=1"
  if defined DO_RETRY (
    echo [RETRY] Attempting one restart with bumped ports and fresh logs...
    call :stop_servers_silent

    set /a BACKEND_PORT=%BACKEND_PORT%+1
    if not defined RUN_BACKEND_ONLY set /a FRONTEND_PORT=%FRONTEND_PORT%+1

    > "%STATE_FILE%" echo BACKEND_PORT=%BACKEND_PORT%
    if not defined RUN_BACKEND_ONLY >> "%STATE_FILE%" echo FRONTEND_PORT=%FRONTEND_PORT%
    >> "%STATE_FILE%" echo STARTED_AT=%DATE% %TIME%

    for /f %%D in ('powershell -NoProfile -Command "(Get-Date -Format yyyyMMdd_HHmmss)"') do set "STAMP2=%%D"
    set "BACKEND_LOG=%LOG_DIR%\backend-%STAMP2%-retry.log"
    set "FRONTEND_LOG=%LOG_DIR%\frontend-%STAMP2%-retry.log"

    call :start_windows

    if not defined RUN_FRONTEND_ONLY call :wait_up "%PROBE_BACKEND_URL%" 60 "BACKEND"
    if not defined RUN_BACKEND_ONLY  call :wait_up "http://localhost:%FRONTEND_PORT%/" 60 "FRONTEND"

    if "%READY_BACKEND%"=="0" (
      echo [RETRY] Backend still failing. Last 40 backend log lines:
      powershell -NoProfile -Command "if (Test-Path '%REPO_ROOT%\%BACKEND_LOG%') { Get-Content '%REPO_ROOT%\%BACKEND_LOG%' -Tail 40 }"
    )
    if "%READY_FRONTEND%"=="0" (
      echo [RETRY] Frontend still failing. Last 40 frontend log lines:
      powershell -NoProfile -Command "if (Test-Path '%REPO_ROOT%\%FRONTEND_LOG%') { Get-Content '%REPO_ROOT%\%FRONTEND_LOG%' -Tail 40 }"
    )
  )
)
exit /b 0

:wait_up
REM Args: URL, seconds_to_wait, LABEL
set "WU_URL=%~1"
set /a WU_SECS=%~2
set "WU_LABEL=%~3"
set "WU_FLAG=READY_%WU_LABEL%"
set /a elapsed=0
echo [WAIT] Checking %WU_LABEL% at %WU_URL% for up to %WU_SECS%s...
:wait_loop
powershell -NoProfile -Command "$ProgressPreference='SilentlyContinue'; try { $null = Invoke-WebRequest '%WU_URL%' -UseBasicParsing -TimeoutSec 2; exit 0 } catch { exit 1 }"
if not errorlevel 1 (
  echo [WAIT] %WU_LABEL% is UP.
  set "%WU_FLAG%=1"
  goto :wait_end
)
set /a elapsed+=1
if %elapsed% GEQ %WU_SECS% (
  echo [WARN] %WU_LABEL% did not respond in %WU_SECS%s.
  set "%WU_FLAG%=0"
  goto :wait_end
)
timeout /t 1 >nul
goto :wait_loop
:wait_end
exit /b 0

:stop_servers
echo [STOP] Stopping servers...
call :stop_servers_silent
echo [STOP] Done.
goto :end

:stop_servers_silent
set "BKP=%BACKEND_PORT_START%"
set "FRP=%FRONTEND_PORT_START%"
if exist "%STATE_FILE%" (
  for /f "tokens=1,2 delims==" %%A in (%STATE_FILE%) do (
    if /I "%%A"=="BACKEND_PORT"  set "BKP=%%B"
    if /I "%%A"=="FRONTEND_PORT" set "FRP=%%B"
  )
)
call :kill_by_port %BKP%
call :kill_by_port %FRP%
exit /b 0

:kill_by_port
set "KPORT=%~1"
if "%KPORT%"=="" exit /b 0
for /f "tokens=5" %%P in ('netstat -ano ^| findstr /r /c:":%KPORT% .*LISTENING"') do (
  echo [STOP] Killing PID %%P on port %KPORT% ...
  taskkill /PID %%P /F >nul 2>nul
)
exit /b 0

:clean_all
echo [CLEAN] Stopping servers and removing build artifacts...
call :stop_servers_silent

if exist "%VENV_DIR%" (
  echo [CLEAN] Removing %VENV_DIR% ...
  rmdir /s /q "%VENV_DIR%"
)

if exist "%FRONTEND_DIR%\node_modules" (
  echo [CLEAN] Removing %FRONTEND_DIR%\node_modules ...
  rmdir /s /q "%FRONTEND_DIR%\node_modules"
)
if exist "%FRONTEND_DIR%\package-lock.json" del /q "%FRONTEND_DIR%\package-lock.json"

for /r "%BACKEND_DIR%" %%D in (__pycache__) do if exist "%%D" rmdir /s /q "%%D"
if exist "%BACKEND_DIR%\.pytest_cache" rmdir /s /q "%BACKEND_DIR%\.pytest_cache"

echo [CLEAN] Done.
goto :end

:fail_pause
echo.
echo [ABORTED] Fix the error above and run again. This window will stay open.
echo.
timeout /t -1 >nul
exit /b 1

:end
popd
endlocal
