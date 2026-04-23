# EtherMVC - Quick Start

## 🚀 Run the Application

**Simply double-click:** `EtherMVC.exe`

## 📋 Folder Structure

```
EtherMVC/
├── 🎯 EtherMVC.exe              ← MAIN EXECUTABLE (Click to run!)
├── 📖 README.md                  ← Full documentation
├── ⚡ QUICKSTART.md              ← Getting started guide
├── ⚙️  config.json               ← Configuration (copy at root)
│
├── 📁 docs/                      ← All documentation
│   ├── guides/                   ← Complete guides
│   │   ├── ARCHITECTURE.md
│   │   ├── FEATURES.md
│   │   ├── BUILD.md
│   │   ├── VENDOR_GUIDE.md
│   │   ├── ETHERHOST_SETUP.md
│   │   ├── TROUBLESHOOTING.md
│   │   └── ...
│   └── logs/                     ← Server logs
│
├── 📁 config/                    ← Configuration files
│   ├── config.json
│   └── config.ini
│
├── 📁 vendor/                    ← Third-party dependencies
│   ├── php/                      ← PHP extensions
│   ├── js/                       ← JavaScript utilities
│   ├── composer.json
│   └── package.json
│
├── 📁 View/                      ← Frontend
│   ├── Index.html
│   ├── Script.js
│   └── pages/
│
├── 📁 Data/                      ← Database layer
│   ├── chimera_db.js
│   └── db_retro.php
│
├── 📁 EtherControl/              ← Routes
│   └── EtherChemistery.js
│
├── 📁 Asset/                     ← Themes
│   └── theme1/
│
└── 📁 src/                       ← Source files
```

## ⚡ First Run

1. **Double-click** `EtherMVC.exe`
2. **Select option 1** - Start Web Server
3. **Wait for browser** - Should open automatically
4. **See your app** at http://localhost:8080 or http://Etherhost:8080

## 📝 Important Files

| File | Purpose | Location |
|------|---------|----------|
| `EtherMVC.exe` | Main application | Root |
| `README.md` | Full documentation | Root |
| `QUICKSTART.md` | Getting started | Root |
| `config.json` | Configuration | Root (also in config/) |
| Error logs | Server logs | `docs/logs/` |
| Guides | All guides | `docs/guides/` |

## 🔧 Troubleshooting

**Console closes immediately?**
- Check `docs/logs/` for error log
- Try running as Administrator
- See `docs/guides/TROUBLESHOOTING.md`

**Port 8080 already in use?**
- Edit `config.json` - change port to 8081
- Rebuild EXE

**View not loading?**
- Check `View/Index.html` exists
- Check logs for errors

## 📚 More Information

- **Full Guide:** [README.md](README.md)
- **5-Min Start:** [QUICKSTART.md](QUICKSTART.md)  
- **Architecture:** [docs/guides/ARCHITECTURE.md](docs/guides/ARCHITECTURE.md)
- **Features:** [docs/guides/FEATURES.md](docs/guides/FEATURES.md)
- **Troubleshooting:** [docs/guides/TROUBLESHOOTING.md](docs/guides/TROUBLESHOOTING.md)

---

**Ready to go!** 🎉

Click `EtherMVC.exe` and select option **1** to start!
