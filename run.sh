#!/bin/bash
# EtherMVC Startup Script for Linux/Mac
# This script starts the EtherMVC framework

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                                                            ║"
echo "║                   ETHERMVC FRAMEWORK v1.0                 ║"
echo "║                                                            ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Check if EtherMVC.exe exists (or build it)
if [ ! -f "EtherMVC.exe" ]; then
    echo "[INFO] EtherMVC.exe not found. Building..."
    
    if command -v dotnet &> /dev/null; then
        dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
        echo "[INFO] Build complete. Copy the EXE to this directory."
    else
        echo "[ERROR] .NET SDK not found. Please install .NET 6.0 or later."
        echo "Visit: https://dotnet.microsoft.com/download"
        exit 1
    fi
fi

echo "[INFO] Starting EtherMVC..."
echo ""

# Run the executable with any passed arguments
./EtherMVC.exe "$@"

# Check exit code
EXIT_CODE=$?
if [ $EXIT_CODE -ne 0 ]; then
    echo ""
    echo "[ERROR] EtherMVC encountered an error (exit code: $EXIT_CODE)"
    exit $EXIT_CODE
else
    echo ""
    echo "[SUCCESS] EtherMVC terminated successfully"
fi
