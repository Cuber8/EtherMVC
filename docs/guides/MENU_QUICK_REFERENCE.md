# 🎯 EtherMVC Interactive Menu - Quick Reference

## Main Menu (Web Server NOT Running)

```
1. 🌐 Start Web Server (Recommended)    ← Starts server in background
2. View System Information              ← Shows system config
3. View Database Configuration          ← Shows DB settings
4. View Asset Settings                  ← Shows asset config
5. View View Settings                   ← Shows view config
6. View Routes & Controllers            ← Lists all routes
7. View Error Logs                      ← Shows error log
8. Test API Request                     ← Test encryption
9. Rebuild EXE with Icon                ← Full clean rebuild
0. Exit                                 ← Quit application
```

## Server Control Menu (Web Server IS Running)

```
⚙️  WEB SERVER CONTROL:
1. 🔴 Stop Web Server                   ← Stop gracefully
2. 📋 View Server Status                ← See port, hostname, status
3. 📜 View Web Server Logs              ← Last 20 log lines
4. 🌐 Open Website in Browser           ← Reopen in default browser
5. 📊 View System Information           ← General system info
6. 🔄 Rebuild EXE (Server will stop)    ← Clean rebuild
0. Return to Main Menu (Server will stop) ← Exit server mode
```

## Key Features

### 🌐 Web Server
- Runs in **background** (menu stays responsive)
- Auto-opens browser on startup
- Serves files from `View/` folder
- Accessible at `http://localhost:8080`
- Can be stopped/restarted without restarting app

### 🔄 Live Server Control
- **View Status**: See server port, hostname, task state
- **View Logs**: Last 20 lines of web server log
- **Reopen Browser**: Opens website without restarting
- **Stop Server**: Graceful shutdown (doesn't exit app)
- **Rebuild**: Stops server → Rebuilds EXE → Returns to menu

### 🔨 Rebuild EXE
- **From Menu**: Option 9 (or 6 if server running)
- **From Command Line**: `.\REBUILD.ps1` (PowerShell)
- **Process**:
  1. Cleans old build
  2. Restores dependencies
  3. Compiles release version
  4. Creates win-x64 single-file EXE
  5. Copies to root as `EtherMVC.exe`
- **Time**: ~30-40 seconds

### 📁 Icon Support
- Icon stored in: `Asset/icon.png`
- Referenced in project for branding
- Can be converted to `.ico` for EXE embedding (optional)

## Quick Start Scenarios

### Scenario 1: Just Run the Website
```
1. Double-click EtherMVC.exe
2. Select 1 (Start Web Server)
3. Website opens automatically
4. Select 1 again to stop server
5. Select 0 to exit
```

### Scenario 2: Develop & Rebuild
```
1. Start EtherMVC.exe
2. Select 1 (Start Web Server)
3. Edit View/ or Data/ files in external editor
4. Select 6 (Rebuild EXE with icon)
5. Server stops → builds → menu returns
6. Select 1 again to test changes
```

### Scenario 3: Monitor Server While Running
```
1. Start web server (option 1)
2. Select 2 (View Status) - see if running
3. Select 3 (View Logs) - check for errors
4. Select 4 (Open Browser) - if website closed
5. Select 1 (Stop) - when done
```

### Scenario 4: Use Command Line Build
```
# PowerShell in project root
.\REBUILD.ps1

# Output shows:
# ✅ Icon found
# ✅ Icon configured
# [1/3] Cleaning...
# [2/3] Restoring...
# [3/3] Building...
# ✅ BUILD SUCCESS!
```

## Menu Flow Diagram

```
START EtherMVC.exe
        ↓
   MAIN MENU
   (Server OFF)
        ↓
  ┌─────┴─────┬─────────────┬──────────┐
  ↓           ↓             ↓          ↓
[1] Web    [2-8] View    [9] Rebuild  [0] Exit
Server      Info         EXE
  ↓           ↓             ↓          ↓
Start      Display       Build      Shutdown
Server      Info         & Copy
  ↓           ↓        to Root
RUNNING   Return to    ↓
  ↓        Menu      New EXE
SERVER    ↑          Ready
 MENU     └──────┘
  ↓
┌─┴─────────────┐
↓               ↓
[1] Stop     [2-5] View
Server       Info
  ↓           ↓
Stops      Display
  ↓           ↓
MAIN       MAIN
MENU ←─────MENU
(Server OFF)
  ↓
[6] Rebuild
  ↓
Stops Server → Builds → MAIN MENU
  ↓
Back to MAIN MENU
```

## Command Reference

### From Menu

| Action | Location | Key Press |
|--------|----------|-----------|
| Start Web Server | Main Menu | `1` |
| Stop Web Server | Server Menu | `1` |
| View Server Status | Server Menu | `2` |
| View Server Logs | Server Menu | `3` |
| Open Website | Server Menu | `4` |
| Rebuild EXE | Main Menu | `9` or Server Menu | `6` |
| Exit App | Any Menu | `0` |

### From Command Line

```powershell
# In project root, PowerShell

# Rebuild EXE with icon
.\REBUILD.ps1

# Run application
.\EtherMVC.exe

# Manually build (if REBUILD.ps1 not available)
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

## Important Notes

⚠️ **Web Server in Background**
- Server continues running while menu is active
- Consumes minimal resources
- Can have menu open for hours with server running

⚠️ **Graceful Shutdown**
- `Stop Web Server` (option 1 in server menu) stops gracefully
- Server log continues until stopped
- No data loss

⚠️ **Rebuild Behavior**
- Requires full compile (~30-40 seconds)
- Server automatically stops before rebuild
- New EXE replaces old one
- Next run uses new version

⚠️ **Icon Format**
- Current: PNG format (Asset/icon.png)
- For EXE embedding: Need ICO format
- Optional: Convert PNG to ICO for full embedding

## File Locations

```
EtherMVC/
├── EtherMVC.exe                ← Main executable
├── REBUILD.ps1                 ← PowerShell build script
├── Asset/icon.png              ← Application icon (PNG)
├── View/                       ← Website files
├── Data/                       ← Database files
├── docs/
│   ├── guides/INTERACTIVE_MENU.md
│   └── logs/webserver_*.log    ← Server logs
└── docs/logs/                  ← Log files
```

## Troubleshooting Quick Tips

| Problem | Solution |
|---------|----------|
| Server won't stop | Press `Ctrl+C` to force shutdown |
| Can't rebuild | Run as Administrator |
| Build fails | Check disk space, `dotnet --version` |
| Website won't load | Try option 4 (Reopen in Browser) |
| Port in use | Edit config.json, change port, rebuild |
| No logs | Check `docs/logs/` folder exists |

---

**For detailed info, see** [INTERACTIVE_MENU.md](INTERACTIVE_MENU.md)

**Ready? Start EtherMVC.exe and select option 1! 🚀**
