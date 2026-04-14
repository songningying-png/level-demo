@echo off
setlocal

set "PYTHON_EXE=%LocalAppData%\Programs\Python\Python312\python.exe"
set "LIVERELOAD_EXE=%LocalAppData%\Programs\Python\Python312\Scripts\livereload.exe"
set "PROJECT_DIR=%~dp0"
if "%PROJECT_DIR:~-1%"=="\" set "PROJECT_DIR=%PROJECT_DIR:~0,-1%"
set "PORT=5501"

if not exist "%PYTHON_EXE%" (
  echo [ERROR] Python not found at:
  echo %PYTHON_EXE%
  echo.
  pause
  exit /b 1
)

if not exist "%LIVERELOAD_EXE%" (
  echo [INFO] livereload not found, installing...
  "%PYTHON_EXE%" -m pip install livereload
  if errorlevel 1 (
    echo [ERROR] Failed to install livereload.
    echo.
    pause
    exit /b 1
  )
)

echo [INFO] Starting LiveReload at http://127.0.0.1:%PORT%/index.html
start "" "http://127.0.0.1:%PORT%/index.html"

"%LIVERELOAD_EXE%" "%PROJECT_DIR%" -p %PORT% -t "%PROJECT_DIR%"

endlocal
