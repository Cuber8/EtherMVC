# EtherMVC - Vendor & Dependencies Guide

## What's New

Your EtherMVC now includes:

✅ **Fixed Web Server** - Browser auto-opens correctly on Windows/Mac/Linux  
✅ **Vendor Folder** - Professional dependency management  
✅ **Composer.json** - PHP package management  
✅ **Package.json** - NPM/JavaScript package management  
✅ **Utility Libraries** - Ready-to-use crypto and database helpers  

---

## Quick Start

### Run EtherMVC Web Server

**Double-click:** `EtherMVC.exe`  
**Select:** Option `1` - Start Web Server  
**Result:** Browser automatically opens at `http://localhost:8080`

---

## Vendor Structure

```
vendor/
├── composer.json          # PHP dependencies
├── package.json           # JavaScript dependencies
├── autoload.php          # PHP autoloader
├── README.md             # Full documentation
├── php/
│   ├── CryptoUtil.php    # AES-256 encryption
│   └── DatabaseUtil.php  # MySQL connection pooling
├── js/
│   └── crypto.js         # JavaScript crypto utilities
└── bin/                  # Scripts & executables
```

---

## Setting Up Dependencies

### PHP (Optional - for MySQL/Retro database)

If you plan to use the MySQL database option:

```bash
# Install PHP (if not already installed)
# Download from: https://www.php.net/downloads

# Navigate to EtherMVC folder
cd C:\Users\Kazuma\Desktop\EtherMVC

# Install Composer dependencies
composer install
```

### Node.js/NPM (Optional - for JavaScript features)

If you want to use NPM packages:

```bash
# Install Node.js (if not already installed)
# Download from: https://nodejs.org/

# Navigate to EtherMVC folder
cd C:\Users\Kazuma\Desktop\EtherMVC

# Install NPM dependencies
npm install

# Build JavaScript
npm run build
```

---

## Using Vendor Utilities

### In PHP (db_retro.php)

```php
<?php
// Include vendor autoloader
require_once __DIR__ . '/../vendor/autoload.php';

// Initialize database
DatabaseUtil::initialize([
    'default' => [
        'host' => 'localhost',
        'database' => 'ethermvc',
        'username' => 'root',
        'password' => 'secret'
    ]
]);

// Encrypt sensitive data
$encrypted = CryptoUtil::encryptAES256('secret', 'my-key');

// Fetch data
$users = DatabaseUtil::fetchAll('SELECT * FROM users');

// Insert data
$id = DatabaseUtil::insert('users', [
    'email' => 'user@example.com',
    'name' => 'John Doe'
]);
?>
```

### In JavaScript (View/Script.js)

```javascript
// Include crypto utility
<script src="vendor/js/crypto.js"></script>

<script>
// Encrypt/decrypt
const encrypted = await CryptoUtil.encryptAES256('secret', 'my-key');
const decrypted = await CryptoUtil.decryptAES256(encrypted, 'my-key');

// Hash
const hash = await CryptoUtil.sha256('data');
</script>
```

---

## Available Composer Packages

```bash
# Already configured in composer.json:
- phpunit/phpunit          # Testing framework
- squizlabs/php_codesniffer # Code quality

# Install with:
composer install --dev

# Run tests:
composer test

# Run linting:
composer lint
```

## Available NPM Packages

```bash
# Already configured in package.json:
- webpack                  # Module bundler
- babel                    # JS transpiler
- eslint                   # Code linter
- jest                     # Test framework
- axios                    # HTTP client
- crypto-js                # Cryptography
- lodash                   # Utility library

# Install with:
npm install

# Available commands:
npm run build              # Build for production
npm run dev                # Watch and rebuild
npm test                   # Run tests
npm run lint               # Run linter
```

---

## File Structure

```
EtherMVC/
├── EtherMVC.exe               # 🎯 Main executable (double-click to run)
├── Backbone.cs                # Core security layer
├── Program.cs                 # Application entry point
├── config.json                # Configuration (includes web server settings)
├── composer.json              # PHP dependencies manifest
├── package.json               # NPM dependencies manifest
├── vendor/                    # 📦 All dependencies & utilities
│   ├── autoload.php          # PHP auto-loader
│   ├── composer.json         # Composer manifest
│   ├── package.json          # NPM manifest
│   ├── README.md             # Vendor documentation
│   ├── php/
│   │   ├── CryptoUtil.php
│   │   └── DatabaseUtil.php
│   └── js/
│       └── crypto.js
├── View/                      # Frontend HTML/CSS/JS
│   ├── Index.html
│   ├── Script.js
│   ├── pages/
│   └── ...
├── Data/                      # Database layer
│   ├── chimera_db.js         # JSON encryption
│   └── db_retro.php          # MySQL handler
├── EtherControl/              # Routes & controllers
│   └── EtherChemistery.js    # Routing system
└── Asset/                     # Themes & assets
```

---

## Web Server Features

When you click **Option 1** to start the web server:

✅ Serves all files from `View/` folder  
✅ Auto-opens browser to `http://localhost:8080`  
✅ Supports all file types (HTML, CSS, JS, Images, etc.)  
✅ API endpoints ready (`/api/info`, `/api/status`)  
✅ Security features enabled by default  
✅ Press Ctrl+C to stop server  

---

## Configuration

The web server settings are in `config.json`:

```json
{
  "webserver": {
    "enabled": true,
    "port": 8080,
    "host": "localhost",
    "autoOpenBrowser": true,
    "enableHttp": true,
    "enableHttps": false
  }
}
```

To change settings:
1. Edit `config.json`
2. Change `"port"` to use a different port
3. Restart the EXE

---

## Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Browser doesn't auto-open | Check Windows Firewall, or manually navigate to `http://localhost:8080` |
| Port 8080 already in use | Change `"port"` in config.json to another number (e.g., 8081) |
| View files not loading | Ensure View/ folder exists and has Index.html |
| Composer/NPM not found | Install PHP and Node.js on your system |
| Permission denied | Run the EXE as Administrator |

---

## Next Steps

1. ✅ Run the EXE and view your app in browser
2. ✅ Explore the View/ folder and edit pages
3. ✅ Create routes in EtherControl/
4. ✅ (Optional) Install PHP to use MySQL database
5. ✅ (Optional) Install Node.js for advanced features

---

**Happy Coding! 🚀**

For more information, see:
- [README.md](README.md) - Full documentation
- [QUICKSTART.md](QUICKSTART.md) - Getting started
- [vendor/README.md](vendor/README.md) - Dependency details
