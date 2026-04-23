# EtherMVC Build Instructions

## Prerequisites
- .NET 6.0 SDK or later
- Visual Studio Code or Visual Studio

## Building the EXE

### Method 1: Using .NET CLI

1. **Navigate to project directory**:
   ```bash
   cd C:\Users\Kazuma\Desktop\EtherMVC
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build -c Release
   ```

4. **Publish as standalone executable**:
   ```bash
   dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true
   ```

5. **Find the executable**:
   ```
   bin/Release/net6.0/win-x64/publish/EtherMVC.exe
   ```

### Method 2: Using Visual Studio

1. Open the project in Visual Studio
2. Build > Build Solution
3. Build > Publish EtherMVC
4. Follow the publish wizard
5. Select "Self-contained .exe" option

### Method 3: Batch Build Script

Create a file named `build.bat`:

```batch
@echo off
echo Building EtherMVC...
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true
echo Build complete! Check bin/Release/net6.0/win-x64/publish/ for the EXE
pause
```

Run: `build.bat`

## Running the EXE

### After Building

1. **Standalone mode**:
   ```bash
   bin/Release/net6.0/win-x64/publish/EtherMVC.exe
   ```

2. **From any directory**:
   - Copy EtherMVC.exe to the project root
   - Run: `EtherMVC.exe`

### Command Line Options

```bash
EtherMVC.exe                    # Standard execution
EtherMVC.exe --help            # Show help
EtherMVC.exe --config          # Edit configuration
EtherMVC.exe --test            # Run system tests
EtherMVC.exe --export-docs     # Generate documentation
```

## Output Files

After successful build:
- **Executable**: `EtherMVC.exe`
- **Size**: ~30-50 MB (self-contained)
- **Runtime**: No additional dependencies needed

## Troubleshooting

### "dotnet: command not found"
- Install .NET 6.0 SDK from https://dotnet.microsoft.com/download

### "CSC0006: Metadata file not found"
- Run: `dotnet clean`
- Then: `dotnet build`

### Build times out
- Increase timeout: `dotnet build -c Release --verbosity minimal`

### EXE won't run
- Ensure Windows 7 SP1 or later
- Check if antivirus is blocking execution
- Try right-click > Properties > Unblock

## Deployment

### Distribute the EXE
1. Build using Method 1
2. Copy `bin/Release/net6.0/win-x64/publish/` folder
3. Distribute as a single folder to end users
4. Users can run the EXE directly without .NET installation

### Version the Build
Update in `EtherMVC.csproj`:
```xml
<Version>1.0.1</Version>
```

Then rebuild.

## Development Build

For faster development iterations:
```bash
dotnet build -c Debug
dotnet run --project EtherMVC.csproj
```

---

**For more information, see README.md**
