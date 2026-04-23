# EtherMVC Etherhost Setup

## About Etherhost

"Etherhost" is a custom hostname used by EtherMVC to avoid port collisions with other localhost services. It resolves to 127.0.0.1 (your local machine).

## Setup Instructions

### Windows

#### Option 1: Manual Hosts File Edit (Recommended)

1. **Open Notepad as Administrator**
   - Right-click Notepad
   - Select "Run as Administrator"

2. **Open the Hosts File**
   - File → Open
   - Navigate to: `C:\Windows\System32\drivers\etc\`
   - Set file type to "All Files"
   - Open `hosts` file

3. **Add Etherhost Entry**
   Add this line at the end of the file:
   ```
   127.0.0.1    Etherhost
   ```

4. **Save and Close**

5. **Flush DNS Cache** (Optional but recommended)
   - Open Command Prompt as Administrator
   - Run: `ipconfig /flushdns`

#### Option 2: Using Command Prompt (Quick)

```cmd
REM Open Command Prompt as Administrator and run:
echo 127.0.0.1    Etherhost >> C:\Windows\System32\drivers\etc\hosts
ipconfig /flushdns
```

### macOS

```bash
# Edit hosts file
sudo nano /etc/hosts

# Add this line:
127.0.0.1    Etherhost

# Save: Ctrl+X, then Y, then Enter
```

Flush DNS:
```bash
sudo dscacheutil -flushcache
```

### Linux

```bash
# Edit hosts file (as root or with sudo)
sudo nano /etc/hosts

# Add this line:
127.0.0.1    Etherhost

# Save: Ctrl+X, then Y, then Enter
```

Flush DNS (if using systemd-resolved):
```bash
sudo systemctl restart systemd-resolved
```

---

## After Setup

### Run EtherMVC

```bash
# Double-click EtherMVC.exe
# Or from command line:
.\EtherMVC.exe
```

### Start Web Server

1. Select option `1` - Start Web Server
2. Browser automatically opens to: **http://Etherhost:8080**

---

## Troubleshooting

### "Cannot access http://Etherhost:8080"

**Solution:** Hosts file not updated or DNS cache not flushed

- On Windows: Run `ipconfig /flushdns` again
- On macOS: Run `sudo dscacheutil -flushcache`
- On Linux: Restart systemd-resolved

### "Port 8080 already in use"

**Solution:** Another service is using port 8080

- Change port in `config.json`:
  ```json
  "webserver": {
    "port": 8081
  }
  ```

### "Access Denied" Error

**Solution:** Need administrator privileges

- **Windows**: Right-click EtherMVC.exe → Run as Administrator
- **macOS**: `sudo ./EtherMVC`
- **Linux**: `sudo ./EtherMVC`

---

## Alternative: Use Localhost

If you don't want to edit the hosts file, you can use localhost instead:

1. **Edit config.json:**
   ```json
   "webserver": {
     "host": "localhost"
   }
   ```

2. **Rebuild EXE:**
   ```bash
   dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
   ```

3. **Access at:** `http://localhost:8080`

---

## Verify Setup

### Test Etherhost is Working

```bash
# Windows - Open Command Prompt and run:
ping Etherhost

# Should return: 127.0.0.1
```

### Test Web Server

Once running, check:
- Browser: `http://Etherhost:8080`
- API: `http://Etherhost:8080/api/info`
- Logs: Check `Logs/webserver_*.log` for details

---

## Removing Etherhost (If Needed)

### Windows

1. Open Notepad as Administrator
2. Open `C:\Windows\System32\drivers\etc\hosts`
3. Remove or comment out the line: `127.0.0.1    Etherhost`
4. Save and close

### macOS/Linux

```bash
sudo nano /etc/hosts
# Remove the line: 127.0.0.1    Etherhost
# Save and exit
```

---

## More Information

- **Hosts File Purpose:** Maps hostnames to IP addresses
- **127.0.0.1:** Always refers to localhost (your computer)
- **Etherhost:** Custom alias for 127.0.0.1 to avoid collisions

For more help, check:
- `Logs/` folder for detailed error logs
- `VENDOR_GUIDE.md` for dependency information
- `README.md` for full documentation

---

**EtherMVC Framework v1.0**
