# EtherMVC Quick Start Guide

## 🚀 Get Started in 5 Minutes

### 1. Build the Executable

```bash
cd C:\Users\Kazuma\Desktop\EtherMVC
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true
```

The executable will be created at:
```
bin/Release/net6.0/win-x64/publish/EtherMVC.exe
```

### 2. Run the Application

#### Windows:
```bash
run.bat
# or
EtherMVC.exe
```

#### Linux/Mac:
```bash
bash run.sh
```

### 3. Select Your Type

When the app starts, it will ask:
- **Website**: For web applications, multi-user platforms
- **App**: For desktop or single-user applications

### 4. Choose Your Database

- **Modern (Recommended)**: JSON + Chimera encryption
  - ✓ Fast to set up
  - ✓ Built-in encryption
  - ✓ No database server needed
  
- **Retro**: MySQL/MariaDB
  - ✓ Complex queries
  - ✓ Better for large datasets
  - ✓ Requires MySQL server

### 5. Create Your First Page

**Create file**: `View/pages/home.html`
```html
<h1>Welcome to EtherMVC!</h1>
<p>Your secure web framework is ready.</p>
```

**Create route**: In `EtherControl/EtherChemistery.js`
```javascript
router.get('/home', EtherChemistry.createHandler({
    name: 'homePage',
    logic: async (request) => {
        return { message: 'Home page loaded' };
    }
}));
```

### 6. View Your Application

Navigate to:
- **Local**: `http://localhost/home`
- **View Page**: Open `View/Index.html` in browser

---

## 📁 Project Structure Overview

```
EtherMVC/
├── 🎯 Backbone.cs           # Core security & logic
├── 🎨 Asset/                # Themes & icons
│   └── theme1/              # Modern theme
├── 💾 Data/                 # Database layer
│   ├── db_retro.php         # MySQL connection
│   └── chimera_db.js        # JSON encryption
├── 👁️  View/                 # Frontend
│   ├── Index.html           # Main page
│   ├── pages/               # Page templates
│   └── Script.js            # Frontend logic
└── 🛣️  EtherControl/         # Routes & controllers
    └── EtherChemistery.js   # Modern routing
```

---

## 🔧 Configuration

### config.json - Main Configuration
```json
{
  "system": {
    "type": "website",      // or "app"
    "environment": "development"
  },
  "database": {
    "type": "modern"        // or "retro"
  },
  "security": {
    "enabled": true,
    "encryptionMethod": "AES-256"
  }
}
```

### Modify Settings:
1. Edit `config.json` for system settings
2. Edit `config.ini` for detailed configuration
3. Restart application to apply changes

---

## 💡 Common Tasks

### Create a New Page

1. **Add HTML file**:
   ```html
   <!-- View/pages/about.html -->
   <h1>About Us</h1>
   <p>Learn more about our company...</p>
   ```

2. **Register route**:
   ```javascript
   router.get('/about', EtherChemistry.createHandler({
       name: 'aboutPage',
       logic: async (req) => {
           return { success: true };
       }
   }));
   ```

### Encrypt Sensitive Data

```csharp
// C# - In your controllers
string encrypted = Backbone.EncryptData("sensitive data");
```

```javascript
// JavaScript - In database handlers
const encrypted = db.encrypt(userData);
```

### Add a Form Handler

```html
<!-- Form in HTML -->
<form data-ether-form data-ether-endpoint="/api/contact">
    <input name="email" type="email" required>
    <textarea name="message" required></textarea>
    <button type="submit">Send</button>
</form>
```

```javascript
// Route handler
router.post('/api/contact', EtherChemistry.createHandler({
    name: 'sendContact',
    requiredParams: ['email', 'message'],
    logic: async (request) => {
        // Process form submission
        return { success: true, message: 'Sent!' };
    }
}));
```

---

## 🔐 Security Checklist

- [ ] Enable encryption in config.json
- [ ] Set secure database passwords
- [ ] Use HTTPS in production
- [ ] Enable CSRF protection
- [ ] Validate all user input
- [ ] Encrypt sensitive fields
- [ ] Regular security audits
- [ ] Monitor error logs

---

## 📊 Database Examples

### JSON Schema (Chimera)
```javascript
const db = new ChimeraDB();

db.createSchema('users', {
    id: { type: 'string', required: true },
    email: { type: 'string', required: true, encrypted: true },
    password: { type: 'string', required: true, encrypted: true },
    name: { type: 'string', required: true }
});
```

### MySQL (Retro)
```php
$db = new RetroDatabase([
    'host' => 'localhost',
    'user' => 'root',
    'password' => 'secret',
    'database' => 'ethermvc'
]);

$users = $db->select('users', 'active=1');
```

---

## 🐛 Troubleshooting

| Issue | Solution |
|-------|----------|
| EXE not found | Run: `dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true` |
| Port already in use | Change port in config.json |
| Database connection error | Verify credentials in config.json |
| Page not loading | Check View/pages/ for HTML file |
| Routes not found | Verify registration in EtherControl/ |

---

## 📚 Next Steps

1. ✅ Build and run the application
2. ✅ Create your first page
3. ✅ Add a database schema
4. ✅ Create routes and handlers
5. ✅ Add security features
6. ✅ Test thoroughly
7. ✅ Deploy to production

---

## 🎓 Learn More

- **Full Documentation**: See `README.md`
- **Build Instructions**: See `BUILD.md`
- **Code Examples**: See `View/pages/example.html`
- **Configuration Reference**: See `config.ini`

---

## 🎉 You're Ready!

Your EtherMVC application is now ready for development. Happy coding! 🚀

**For support and questions**, refer to the comprehensive documentation.
