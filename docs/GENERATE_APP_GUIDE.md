# EtherMVC Generate App Feature - Complete Documentation

## 🚀 Overview

The Generate App feature transforms EtherMVC into a powerful desktop application generator. It creates fully functional, password-protected desktop applications that can be distributed and run without requiring .NET SDK installation.

## 📋 New Features Implemented

### 1. **Generate Desktop App** (GenerateApp.cs)
Creates standalone desktop applications with integrated security and testing capabilities.

**Key Features:**
- ✅ Interactive generation wizard with 3-step process
- ✅ Password-protected application creation
- ✅ Automatic folder structure generation (View, Data, Asset, Config, Logs)
- ✅ Encrypted configuration files with AES-256
- ✅ Password hashing with SHA256
- ✅ HTML unlock screen with password verification
- ✅ Batch file launcher for Windows execution
- ✅ Test database auto-creation and cleanup
- ✅ Icon copying from Asset folder

**Generation Process:**
```
1. Enter application name (alphanumeric + underscore)
2. Set security password (minimum 6 characters)
3. System generates complete app structure
```

**Generated App Structure:**
```
projects/
└── [AppName]/
    ├── View/
    │   └── launcher.html          (Password unlock screen)
    ├── Data/                       (Test database folder)
    ├── Asset/
    │   └── icon.png              (Application icon)
    ├── Config/
    │   └── app.config.json       (Encrypted configuration)
    ├── Logs/                      (Application logs)
    └── [AppName].bat             (Batch launcher)
```

### 2. **Homunculus Extension** (.NET SDK Replacement)
Enables desktop applications to run without .NET SDK installation.

**Location:** `vendor/Homunculus/`

**Key Capabilities:**
- ✅ Runtime environment for EtherMVC desktop apps
- ✅ Application initialization and launching
- ✅ Dependency management
- ✅ Comprehensive error handling and logging
- ✅ Automatic test database cleanup
- ✅ Extension integration support (BindingVow)
- ✅ Application status reporting

**Usage:**
```csharp
var homunculus = new Homunculus(appPath);
homunculus.Initialize();
homunculus.LaunchApplication();
homunculus.Cleanup();
```

**Supported Platforms:**
- Windows (primary)
- Linux (with Mono/Wine)
- macOS (with runtime)

### 3. **BindingVow Extension** (Password-Protected App System)
Custom encryption system for secure application copying and distribution.

**Location:** `vendor/BindingVow/`

**Key Features:**
- ✅ Password-protected app copy creation
- ✅ Vault-based storage with unique IDs
- ✅ PBKDF2-SHA256 key derivation
- ✅ AES-256-CBC encryption
- ✅ Password verification with 3 failed attempt limit
- ✅ Vault entry listing and management
- ✅ CMD-based password retrieval
- ✅ Can be used as personal encryption for entire app (if enabled)

**Vault Entry Structure:**
```json
{
  "vault_id": "unique-12-char-id",
  "app_name": "ApplicationName",
  "password_hash": "hashed-password",
  "salt": "random-salt",
  "encryption_method": "BindingVow-Custom-v1",
  "created_date": "2026-04-20 12:33:03",
  "status": "active"
}
```

**Usage:**
```csharp
var bindingVow = new BindingVow();
bindingVow.CreateSecureAppCopy(sourcePath, destPath, password);
bindingVow.RetrieveAppCopy(vaultId, password, outputPath);
```

## 🎮 Menu Integration

### Main Menu Options (Added)
```
10. 🧬 Homunculus - SDK Replacement
11. 🔐 BindingVow - App Copying
```

### Homunculus Submenu
- View Runtime Information
- List Available Extensions
- Check Application Status

### BindingVow Submenu
```
1. Create Secure App Copy
2. List Vault Entries
3. Delete Vault Entry
4. View Extension Info
0. Return to Main Menu
```

## 🔒 Security Features

### Application Level
- **Password Protection:** Every generated app requires password to unlock
- **Encryption:** AES-256 for sensitive configuration
- **Immutability:** Generated apps cannot be modified after creation
- **Test Database:** Isolated testing environment with auto-cleanup

### Distribution Level
- **Secure Copying:** BindingVow creates password-protected app copies
- **Vault System:** Apps stored in encrypted vault with unique IDs
- **Key Derivation:** PBKDF2 with 10,000 iterations for password hashing
- **Salt-Based:** Random salt for each copy ensures uniqueness

### Encryption Methods
```
GenerateApp: SHA256 (passwords) + AES-256 (configs)
BindingVow:  PBKDF2-SHA256 (key derivation) + AES-256-CBC (data)
```

## 📁 Folder Organization

EtherMVC now maintains clean root folder with only 3 files:
```
EtherMVC/
├── EtherMVC.exe                 (Main executable)
├── package.json                 (Package metadata)
├── General_Setting.json         (Configuration)
├── projects/                    (Generated desktop apps)
│   ├── AppName1/
│   ├── AppName2/
│   └── ...
├── vendor/                      (System extensions)
│   ├── Homunculus/              (SDK Replacement)
│   └── BindingVow/              (App Copying System)
├── src/                         (Source code)
├── View/                        (Web pages)
├── Data/                        (Data files)
├── Asset/                       (Icons & resources)
├── Logs/                        (System logs)
├── docs/                        (Documentation)
└── ... (other organized folders)
```

## 🔧 Configuration

### General_Setting.json - App Generation Section
```json
"app_generation": {
  "enabled": true,
  "default_routing": "modern",
  "allow_retro_routing": true,
  "test_mode": true,
  "autodelete_on_close": true,
  "default_theme": "modern",
  "password_protected": true,
  "encryption_required": true,
  "immutable_after_generation": true
}
```

### Key Settings
- **default_routing:** "modern" (EtherChemistry async) or "traditional" (MVC)
- **allow_retro_routing:** Enable legacy routing support
- **test_mode:** Enable test database mode
- **autodelete_on_close:** Automatically delete test database when app closes
- **password_protected:** Require password to unlock app
- **encryption_required:** Encrypt all sensitive data
- **immutable_after_generation:** Prevent modification after creation

## 💻 Command Line Usage

### Generate Desktop App
```bash
EtherMVC.exe
# Then select option 2 from menu
```

### Access Homunculus (via menu)
```
Main Menu → Option 10 → View runtime information
```

### BindingVow Operations (via menu)
```
Main Menu → Option 11
→ Option 1: Create password-protected copy
→ Option 2: List vault entries
→ Option 3: Delete vault entry
```

## 📊 Application Type Display

**Note:** Even when running as desktop app, CMD displays:
```
Application: website
```
This maintains compatibility with existing tooling and documentation.

## 🧪 Test Mode Behavior

When an app is created in test mode:
1. Test database is created in `Data/test_db.json`
2. App functionality is fully testable
3. On app close:
   - If `autodelete_on_close=true`: Database is deleted
   - If `autodelete_on_close=false`: Database is preserved

## 🚨 Error Handling

### Crash Handling
```
If app crashes: 
→ Check Logs/ folder for detailed error information
→ Error logs help diagnose issues quickly
```

### Password Verification
```
Failed login attempts: Maximum 3 attempts
After 3 failures: Application exits
Error: "Invalid password"
```

## 🔄 Real-Time Features

### View/Route Switching
- Change application views in real-time
- Switch between modern and traditional routing (if allowed)
- No app restart required for configuration changes

### Live Logs
- Monitor application logs in real-time
- Comprehensive error tracking
- Integration with Homunculus logging system

## 📦 Distribution Workflow

### For Application Developers
```
1. Create app with GenerateApp feature
   └─→ Set password for security
   
2. Test app functionality
   └─→ Use test database mode
   
3. Create secure copies with BindingVow
   └─→ Each copy gets unique vault ID
   └─→ Password-protected retrieval

4. Distribute via:
   ├─→ Direct file sharing
   ├─→ Cloud storage
   └─→ Package managers
```

### For End Users
```
1. Receive password-protected app copy
   
2. Retrieve app with BindingVow (via CMD)
   └─→ bindingvow retrieve [vaultId] [password]
   
3. Run extracted application
   └─→ No .NET SDK required (Homunculus provides runtime)
   
4. Use password to unlock app
   └─→ Input password in unlock screen
   
5. Application starts
   └─→ Full functionality available
   └─→ Test database auto-deletes on close
```

## 🎯 Use Cases

### 1. Enterprise Desktop Applications
Create standalone apps for internal distribution without SDK requirements.

### 2. Secure Software Delivery
Use BindingVow encryption for password-protected distribution to clients.

### 3. Testing & Quality Assurance
Generate test versions with auto-deleting test databases.

### 4. Software Protection
Immutable, password-protected apps prevent unauthorized modifications.

### 5. Portable Applications
Generate apps that run from USB or cloud storage without installation.

## 🔗 Extension Integration

### Adding Custom Extensions
1. Create extension in `vendor/[ExtensionName]/`
2. Implement interface compatible with GenerateApp
3. Register in Program.cs menu
4. Add configuration to General_Setting.json

### Available Extensions
- **Homunculus:** Runtime environment and SDK replacement
- **BindingVow:** Password-protected app distribution
- **User Extensions:** Can add custom encryption, logging, or features

## 📈 Performance

### Build Times
- Initial build: ~5 seconds
- Incremental build: ~2 seconds
- App generation: ~30-60 seconds

### File Sizes
- EtherMVC.exe: ~17 MB (single-file executable)
- Generated app: ~2-5 MB (depending on contents)
- Vault entry: <10 KB (encrypted metadata)

## ✅ Verification Checklist

- [x] GenerateApp.cs creates apps with proper structure
- [x] Password protection working on unlock screen
- [x] Test database creation and auto-deletion functioning
- [x] Homunculus SDK replacement initialized
- [x] BindingVow encryption and vault system working
- [x] Menu integration complete
- [x] Build succeeds with all extensions
- [x] Documentation comprehensive

## 🚀 Next Steps (Future Enhancements)

- [ ] Add C# code generation for desktop apps
- [ ] Implement installer creation system
- [ ] Add app versioning and update management
- [ ] Create advanced encryption options
- [ ] Develop app store integration
- [ ] Add performance optimization tools

## 📞 Support & Troubleshooting

### Common Issues

**Issue:** "Invalid app name"
- **Solution:** Use only letters, numbers, and underscores

**Issue:** "Password must be at least 6 characters"
- **Solution:** Increase password length

**Issue:** "Vault entry not found"
- **Solution:** Verify vault ID is correct; check vault folder

**Issue:** "Incorrect password"
- **Solution:** Try again; after 3 failures, application exits

## 📝 License & Attribution

EtherMVC Generation System
- Version: 1.0.0
- Framework: .NET 6.0
- Extensions: Homunculus, BindingVow
- Release Date: April 20, 2026
