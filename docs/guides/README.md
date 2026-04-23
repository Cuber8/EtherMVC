# EtherMVC Framework - User Guide

## Overview

**EtherMVC** is a secure, lightweight framework designed to simplify the development of websites and applications. It provides built-in security, flexible architecture, and an easy-to-use interface for developers of all levels.

**Version**: 1.0  
**Release Date**: April 20, 2026

---

## Table of Contents

1. [Installation & Setup](#installation--setup)
2. [System Architecture](#system-architecture)
3. [Project Structure](#project-structure)
4. [Getting Started](#getting-started)
5. [Database Setup](#database-setup)
6. [Routes & Controllers](#routes--controllers)
7. [Views & Templates](#views--templates)
8. [Security Features](#security-features)
9. [Examples](#examples)
10. [Troubleshooting](#troubleshooting)

---

## Installation & Setup

### Prerequisites

- Windows OS (with .NET Framework 4.7.2 or higher for .exe execution)
- Basic understanding of web development
- Text editor or IDE (Visual Studio Code recommended)

### Setup Instructions

1. **Extract Files**: Extract the EtherMVC package to your desired location.

2. **Run the Executor**:
   ```bash
   EtherMVC.exe
   ```

3. **Initialize Project**:
   - The system will automatically scan your project structure
   - Verify all components are present
   - Display system information

4. **Configure Settings**:
   - Edit config files in each component folder
   - Customize themes, database, and security settings

---

## System Architecture

EtherMVC follows a modular architecture with clear separation of concerns:

```
EtherMVC
├── Backbone.cs           ← Core security & communication layer
├── Asset/                ← Visual assets & themes
├── Data/                 ← Database management
├── View/                 ← Presentation layer
└── EtherControl/         ← Routing & controllers
```

### Core Components

| Component | Purpose |
|-----------|---------|
| **Backbone** | Central security handler, encryption/decryption, system verification |
| **Asset** | Themes, icons, CSS frameworks, visual configurations |
| **Data** | Database abstraction layer (MySQL or JSON/Chimera) |
| **View** | View security, error handling, layout management |
| **EtherControl** | Routing, controllers, request handling |

---

## Project Structure

```
EtherMVC/
│
├── EtherMVC.exe                    # Main executable
├── README.md                       # This file
├── Backbone.cs                     # Core system
│
├── Asset/                          # Visual Assets
│   ├── ASetting.cs                # Asset configuration
│   ├── bootstrap.css              # CSS framework
│   ├── icon.png                   # App icon
│   └── theme/
│       └── theme1/
│           ├── display.html       # Theme display
│           └── rendering.css      # Theme styles
│
├── Data/                           # Data Management
│   ├── DSetting.cs                # Database configuration
│   ├── chimera_db.js              # JSON encryption handler
│   ├── db_retro.php               # MySQL connection
│   └── db_json/                   # JSON database storage
│       └── [encrypted].json       # Encrypted data files
│
├── View/                           # Presentation Layer
│   ├── VSetting.cs                # View security settings
│   ├── ErrorConsole.cs            # Error display handler
│   ├── Script.js                  # Backend UX logic
│   ├── index.jsx                  # View renderer
│   ├── Index.html                 # Main page
│   ├── layout/
│   │   └── basic.html             # Basic layout template
│   └── pages/
│       └── example.html           # Example page
│
└── EtherControl/                  # Routing & Control
    ├── ESetting.cs                # Control configuration
    ├── EtherChemistery.js         # Modern pattern (recommended)
    ├── Controller_retro/
    │   └── Controller_example.js  # Example controller
    └── route_retro/
        └── route_example.js       # Example route
```

---

## Getting Started

### 1. Start the System

```bash
cd EtherMVC
EtherMVC.exe
```

### 2. View Main Menu

The executable will display a menu with options to:
- View system information
- Configure databases
- Manage assets
- Check routes
- View error logs
- Test encryption
- Generate documentation

### 3. Create Your First Page

1. Add an HTML file to `View/pages/`:
   ```html
   <!-- View/pages/mypage.html -->
   <h1>My Page</h1>
   <p>Welcome to EtherMVC!</p>
   ```

2. Create a route handler in EtherControl (using EtherChemistry pattern):
   ```javascript
   // EtherControl/handlers/my-handler.js
   const handler = EtherChemistry.createHandler({
       name: 'myPage',
       logic: async (request) => {
           return { message: 'Page loaded successfully' };
       }
   });
   ```

---

## Database Setup

### Option 1: JSON with Chimera Encryption (Recommended)

**Best for**: Small to medium projects, rapid development, security-focused

1. **Define Schema**:
   ```javascript
   // Data/chimera_db.js
   const db = new ChimeraDB();
   
   db.createSchema('users', {
       id: { type: 'string', required: true, encrypted: false },
       name: { type: 'string', required: true, encrypted: false },
       email: { type: 'string', required: true, encrypted: true },
       password: { type: 'string', required: true, encrypted: true }
   });
   ```

2. **Encrypt Field Names**:
   ```javascript
   const userData = { id: '1', name: 'John', email: 'john@example.com' };
   const encrypted = db.encryptFieldNames('users', userData);
   ```

3. **Store Data**:
   - Files are automatically saved in `Data/db_json/` as encrypted JSON
   - File naming: `users.json`, `products.json`, etc.

### Option 2: MySQL with Retro Backend

**Best for**: Large projects, complex queries, enterprise applications

1. **Configure Connection**:
   ```php
   // Data/db_retro.php
   $db = new RetroDatabase([
       'host' => 'localhost',
       'user' => 'root',
       'password' => '',
       'database' => 'ethermvc'
   ]);
   ```

2. **Execute Queries**:
   ```php
   $users = $db->select('users', 'active=1', 10);
   $db->insert('users', ['name' => 'John', 'email' => 'john@example.com']);
   ```

---

## Routes & Controllers

### Using EtherChemistry (Modern Pattern - Recommended)

**Simple, secure, and powerful**

```javascript
// EtherControl/EtherChemistery.js
const router = new EtherChemistry();

// Define route
router.post('/users', EtherChemistry.createHandler({
    name: 'createUser',
    description: 'Create a new user',
    requiredParams: ['name', 'email'],
    logic: async (request) => {
        // Your logic here
        return { success: true, id: 1 };
    }
}));

// Add middleware
router.use((request) => {
    console.log(`Request: ${request.method} ${request.path}`);
});

// Error handling
router.onError((error, method, path) => {
    console.error(`Error on ${method} ${path}: ${error.message}`);
    return true;
});
```

### Using Traditional MVC (Retro Pattern)

**For complex applications**

1. **Create Controller**:
   ```javascript
   // EtherControl/Controller_retro/UserController.js
   class UserController {
       async getUsers(request) {
           // Logic here
           return { users: [...] };
       }
   }
   ```

2. **Create Route**:
   ```javascript
   // EtherControl/route_retro/UserRoute.js
   router.get('/users', UserController.getUsers);
   ```

---

## Views & Templates

### Using the View System

```javascript
// View/index.jsx
const view = new EtherView();

// Render a page
view.renderPage('dashboard', { 
    title: 'My Dashboard',
    user: { name: 'John' }
});

// Access rendered state
const state = view.getState();
```

### Using Frontend UX Handler

```javascript
// Automatic API requests
if (window.EtherUX) {
    // GET request
    const data = await EtherUX.get('/api/users');
    
    // POST request
    const result = await EtherUX.post('/api/users', { name: 'John' });
    
    // Listen to events
    EtherUX.on('form-submit-success', (data) => {
        console.log('Form submitted:', data);
    });
}
```

---

## Security Features

### Built-in Encryption

All sensitive data is encrypted using AES-256:

```csharp
// Encrypt data
string encrypted = Backbone.EncryptData("sensitive information");

// Decrypt data
string decrypted = Backbone.DecryptData(encrypted);
```

### View Security

Prevents XSS and sensitive data exposure:

```csharp
// Sanitize output
string safe = viewSettings.SanitizeOutput(userInput);

// Filter sensitive fields
var filtered = viewSettings.FilterSensitiveData(data);
```

### Field-Level Encryption

Encrypt specific database fields:

```javascript
db.setFieldEncryption('users', 'password', true);
db.setFieldEncryption('users', 'email', true);
```

### CSRF Protection

Enabled by default in EtherControl settings.

---

## Examples

### Example 1: Create a Contact Form

1. **HTML** (`View/pages/contact.html`):
```html
<form data-ether-form data-ether-endpoint="/api/contact">
    <input type="text" name="name" required>
    <input type="email" name="email" required>
    <textarea name="message" required></textarea>
    <button type="submit">Send</button>
</form>
```

2. **Handler** (`EtherControl/handlers/contact.js`):
```javascript
router.post('/api/contact', EtherChemistry.createHandler({
    name: 'sendContact',
    requiredParams: ['name', 'email', 'message'],
    logic: async (request) => {
        // Process contact form
        return { success: true, message: 'Message sent!' };
    }
}));
```

### Example 2: Protect Sensitive Data

```csharp
// In View/VSetting.cs
var sensitiveFields = new[] { "password", "apikey", "ssn" };
var filtered = viewSettings.FilterSensitiveData(userData);
// Sensitive fields are masked as "***HIDDEN***"
```

---

## Troubleshooting

### Issue: "System integrity check failed"

**Solution**: Verify all required folders exist:
- Asset/
- Data/
- View/
- EtherControl/

### Issue: Database connection error

**For MySQL (Retro)**:
- Check database credentials in `db_retro.php`
- Ensure MySQL server is running
- Verify database and tables exist

**For JSON (Chimera)**:
- Ensure `db_json/` folder exists
- Check file permissions
- Verify JSON syntax in schema files

### Issue: Routes not found

**Solution**:
- Verify routes are registered in `EtherControl/`
- Check route naming matches request paths
- Review error console for detailed messages

### Issue: Encryption/Decryption failing

**Solution**:
- Ensure encryption key is initialized
- Check data format
- Review error logs in `Logs/` folder

### Issue: Page not displaying correctly

**Solution**:
- Check `rendering.css` is linked correctly
- Verify HTML file exists in `View/pages/`
- Check browser console for errors
- Ensure Script.js and index.jsx are loaded

---

## Support & Resources

### Documentation
- Full API reference available in code comments
- Examples in `View/pages/example.html`
- Template files in `View/layout/`

### Error Console
- Enable via `VSetting.cs`
- View logs in error console
- Export logs as JSON for analysis

### Command Line Options
```bash
EtherMVC.exe --help           # Show help
EtherMVC.exe --config         # Edit configuration
EtherMVC.exe --test           # Run system tests
EtherMVC.exe --export-docs    # Generate documentation
```

---

## Best Practices

1. **Security**
   - Always encrypt sensitive fields
   - Validate user input on both client and server
   - Use HTTPS in production
   - Keep encryption keys secure

2. **Performance**
   - Use JSON database for small projects
   - Cache frequently accessed data
   - Minimize HTTP requests
   - Optimize asset sizes

3. **Maintenance**
   - Regular backups of JSON database
   - Monitor error logs
   - Keep framework updated
   - Document custom controllers

4. **Development**
   - Use EtherChemistry pattern for new code
   - Follow naming conventions
   - Test thoroughly before deployment
   - Use error console during development

---

## License & Terms

EtherMVC v1.0 - Secure Web Development Framework
© 2026 All Rights Reserved

**For support and updates, visit the official documentation.**

---

**Happy Building! 🚀**
