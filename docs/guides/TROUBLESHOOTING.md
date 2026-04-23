# EtherMVC - Fixed Version - Setup & Troubleshooting

## What Was Fixed

✅ **Enhanced Logging System**
- All errors are now logged to `Logs/webserver_*.log` 
- Console shows detailed information about what's happening
- Timestamp on every log entry

✅ **Console Window Stays Open**
- No more auto-closing on errors
- You can read error messages
- Press any key to exit

✅ **Better Error Messages**
- Specific error codes and descriptions
- Helpful suggestions for each error type
- Access Denied handled properly

✅ **Custom Hostname Support**
- Uses "Etherhost" instead of "localhost" (no collisions)
- Still resolves to 127.0.0.1 (your machine)
- Fallback to localhost if needed

---

## How to Run

### Step 1: Add Etherhost to Your System

This is OPTIONAL but recommended to avoid conflicts.

**Windows (Easy Method):**
```cmd
REM Open Command Prompt as Administrator and run:
echo 127.0.0.1    Etherhost >> C:\Windows\System32\drivers\etc\hosts
ipconfig /flushdns
```

Or see [ETHERHOST_SETUP.md](ETHERHOST_SETUP.md) for manual setup.

### Step 2: Run EtherMVC

**Double-click:** `EtherMVC.exe`

### Step 3: Start Web Server

Select option: **1** - Start Web Server

**Expected Output:**
```
[WebServer] Initializing server on Etherhost:8080
[WebServer] Root path: C:\Users\Kazuma\Desktop\EtherMVC
[WebServer] View path: C:\Users\Kazuma\Desktop\EtherMVC\View
[WebServer] Adding listener prefix: http://localhost:8080/
✅ [WebServer] Server started successfully!
🌐 [WebServer] Access URL: http://Etherhost:8080/
📝 [WebServer] Press Ctrl+C in this window to stop the server

🌐 [Browser] Opening: http://localhost:8080/
```

### Step 4: Browser Opens

Your browser should automatically open with your EtherMVC application!

---

## Troubleshooting

### Console Closes Immediately

**Old behavior (now fixed)** - If the console closed without showing errors:

1. **Check logs:** Open `Logs/webserver_*.log`
2. **Read the error** in the log file
3. **Follow the solution** below

### Common Errors & Solutions

#### Error: "Access Denied! Run as Administrator!"

**Cause:** HttpListener requires admin privileges on Windows

**Solution:**
1. Right-click `EtherMVC.exe`
2. Click "Run as Administrator"
3. Select option 1 again

#### Error: "Port 8080 is already in use"

**Cause:** Another application is using port 8080 (maybe Node.js, IIS, etc.)

**Solution:** Change the port in `config.json`:

```json
{
  "webserver": {
    "port": 8081
  }
}
```

Then rebuild:
```bash
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

#### Error: "Cannot bind to this address"

**Cause:** Hostname not recognized in hosts file

**Solutions:**

**Option A:** Use localhost instead of Etherhost
- Edit `config.json`: Change `"host"` to `"localhost"`
- Rebuild the EXE

**Option B:** Add Etherhost to hosts file (recommended)
- See [ETHERHOST_SETUP.md](ETHERHOST_SETUP.md)

#### Browser Doesn't Auto-Open

**Cause:** Browser auto-opening failed (can happen if default browser is misconfigured)

**Solution:** Manually open your browser and navigate to:
- `http://localhost:8080` (if not using Etherhost)
- `http://Etherhost:8080` (if you added Etherhost to hosts)

#### View Files Not Loading (404 Not Found)

**Cause:** View folder missing or Index.html not found

**Solutions:**

1. Check folder structure:
   ```
   EtherMVC/
   ├── View/
   │   ├── Index.html ← REQUIRED
   │   ├── Script.js
   │   ├── pages/
   │   └── ...
   ```

2. Verify file exists:
   - Is `View/Index.html` present?
   - Is it spelled correctly?
   - Is it in the right location?

3. Check the log for the exact path:
   ```
   [WebServer] View path: C:\Users\Kazuma\Desktop\EtherMVC\View
   [Request] GET /index.html
   [Response] 200 OK - /index.html
   ```

---

## Checking Logs

### View Server Logs

1. Open the `Logs/` folder in EtherMVC
2. Find the latest `webserver_*.log` file
3. Open it with Notepad

### Log Format

Each line shows:
- **Timestamp** - When it happened
- **Component** - `[WebServer]`, `[Request]`, `[API]`, etc.
- **Message** - What happened

**Example Log:**
```
[2026-04-20 12:09:34.123] [WebServer] Log file initialized: C:\Users\Kazuma\Desktop\EtherMVC\Logs\webserver_2026-04-20_12-09-34.log
[2026-04-20 12:09:34.456] [WebServer] Initializing server on Etherhost:8080
[2026-04-20 12:09:34.789] ✅ [WebServer] Server started successfully!
[2026-04-20 12:09:35.012] 🌐 [Browser] Opening: http://localhost:8080/
[2026-04-20 12:09:36.234] [Request] GET /index.html
[2026-04-20 12:09:36.345] ✅ [Response] 200 OK - /index.html
[2026-04-20 12:09:36.456] [Request] GET /Script.js
[2026-04-20 12:09:36.567] ✅ [Response] 200 OK - /Script.js
```

---

## Console Commands

### While Server is Running

**Stop Server:** Press `Ctrl+C`

The console will show:
```
[WebServer] Server stopped
[WebServer] Check Logs/ folder for complete session log
```

---

## File Structure

```
EtherMVC/
├── EtherMVC.exe                ← 🎯 Run this!
├── Logs/
│   ├── webserver_*.log        ← Check these for errors
│   └── ...
├── View/
│   ├── Index.html             ← Main page (required!)
│   ├── Script.js
│   └── pages/
├── config.json                ← Configure port/host here
├── ETHERHOST_SETUP.md         ← Hostname setup guide
├── VENDOR_GUIDE.md            ← Dependencies guide
└── ...
```

---

## Quick Reference

| Issue | Check | Fix |
|-------|-------|-----|
| Closes immediately | `Logs/webserver_*.log` | See log error |
| "Access Denied" | You're admin? | Run as Administrator |
| Port in use | Other apps running | Change port in config.json |
| Browser doesn't open | Antivirus? | Manually navigate to URL |
| 404 Not Found | View/Index.html exists? | Create View folder with Index.html |
| "Cannot bind" | Etherhost in hosts? | Add to hosts file or use localhost |

---

## Support

1. **Check the log file** - Most errors are documented there
2. **Read the error message** - It tells you what went wrong
3. **See Troubleshooting section** above
4. **Check documentation:**
   - [README.md](README.md) - Full documentation
   - [ETHERHOST_SETUP.md](ETHERHOST_SETUP.md) - Hostname setup
   - [VENDOR_GUIDE.md](VENDOR_GUIDE.md) - Dependencies

---

## Summary

Your EtherMVC is now:
- ✅ More stable (better error handling)
- ✅ More visible (console stays open, logs everything)
- ✅ More flexible (custom hostname support)
- ✅ Easier to debug (detailed logging)

**Ready to go!** 🚀

Try it now:
1. Double-click `EtherMVC.exe`
2. Select option `1`
3. Browser opens automatically
4. View your app!

---

**EtherMVC Framework v1.0 - Fixed Edition**
