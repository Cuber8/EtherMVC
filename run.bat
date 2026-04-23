@echo off
REM EtherMVC Startup Script
REM This script starts the EtherMVC framework

echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║                                                            ║
echo ║                   ETHERMVC FRAMEWORK v1.0                 ║
echo ║                                                            ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

REM Check if EtherMVC.exe exists
if not exist "EtherMVC.exe" (
    echo [ERROR] EtherMVC.exe not found!
    echo Please ensure you are running this script from the EtherMVC root directory.
    echo.
    echo To build the EXE, run:
    echo   dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
    echo.
    pause
    exit /b 1
)

echo [INFO] Starting EtherMVC...
echo.

REM Run the executable with any passed arguments
EtherMVC.exe %*

REM Check exit code
if errorlevel 1 (
    echo.
    echo [ERROR] EtherMVC encountered an error (exit code: %errorlevel%)
    pause
) else (
    echo.
    echo [SUCCESS] EtherMVC terminated successfully
)
