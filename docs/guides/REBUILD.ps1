# EtherMVC Build Script with Icon Embedding
# This script rebuilds the EXE with the icon from Asset/icon.png

param(
    [switch]$Release = $true
)

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════╗"
Write-Host "║                                                            ║"
Write-Host "║               🔨 ETHERMVC BUILD SCRIPT                     ║"
Write-Host "║                                                            ║"
Write-Host "║        Building EtherMVC.exe with icon.png                ║"
Write-Host "║                                                            ║"
Write-Host "╚════════════════════════════════════════════════════════════╝"
Write-Host ""

# Check if icon exists
$iconPath = ".\Asset\icon.png"
if (!(Test-Path $iconPath)) {
    Write-Host "❌ ERROR: Icon not found at $iconPath" -ForegroundColor Red
    Write-Host "Please add an icon.png file to the Asset folder"
    exit 1
}

Write-Host "✅ Icon found: $iconPath" -ForegroundColor Green

# Check if .csproj has icon configured
$csprojPath = ".\EtherMVC.csproj"
$csprojContent = Get-Content $csprojPath -Raw

if ($csprojContent -like "*<ApplicationIcon>Asset/icon.png</ApplicationIcon>*") {
    Write-Host "✅ Icon configured in EtherMVC.csproj" -ForegroundColor Green
} else {
    Write-Host "⚠️  Icon not configured in .csproj" -ForegroundColor Yellow
    Write-Host "Adding icon configuration..." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "📦 Building release executable..." -ForegroundColor Cyan
Write-Host ""

# Clean previous build
Write-Host "[1/3] Cleaning previous build..."
Remove-Item -Path ".\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path ".\obj" -Recurse -Force -ErrorAction SilentlyContinue

# Restore dependencies
Write-Host "[2/3] Restoring dependencies..."
& dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Restore failed!" -ForegroundColor Red
    exit 1
}

# Build
Write-Host "[3/3] Building release package..."
& dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Copy to root
$publishPath = ".\bin\Release\net6.0\win-x64\publish\EtherMVC.exe"
$rootExe = ".\EtherMVC.exe"

if (Test-Path $publishPath) {
    Write-Host ""
    Write-Host "📋 Copying executable to root..." -ForegroundColor Cyan
    Copy-Item $publishPath $rootExe -Force
    
    $fileInfo = Get-Item $rootExe
    $sizeMB = [math]::Round($fileInfo.Length / 1MB, 2)
    
    Write-Host ""
    Write-Host "╔════════════════════════════════════════════════════════════╗"
    Write-Host "║                   ✅ BUILD SUCCESS!                        ║"
    Write-Host "╚════════════════════════════════════════════════════════════╝"
    Write-Host ""
    Write-Host "📦 Executable:     $rootExe"
    Write-Host "📊 File Size:      $sizeMB MB"
    Write-Host "🎨 Icon:           Asset/icon.png (embedded)"
    Write-Host "⚙️  Framework:      net6.0"
    Write-Host "🖥️  Platform:       win-x64"
    Write-Host ""
    Write-Host "✨ Ready to run! Double-click EtherMVC.exe to start." -ForegroundColor Green
} else {
    Write-Host "❌ Build executable not found at $publishPath" -ForegroundColor Red
    exit 1
}

Write-Host ""
