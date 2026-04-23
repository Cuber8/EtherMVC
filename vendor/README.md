# EtherMVC Vendor Directory

This directory contains third-party libraries, utilities, and dependencies for the EtherMVC framework.

## Structure

```
vendor/
├── composer.json          # PHP Composer dependencies
├── package.json           # NPM/Node.js dependencies
├── php/                   # PHP utilities & libraries
│   ├── CryptoUtil.php    # AES-256 encryption utilities
│   └── DatabaseUtil.php  # Database connection pooling & helpers
├── js/                    # JavaScript utilities & libraries
│   └── crypto.js         # Cryptographic utilities for frontend
├── bin/                   # Executable scripts
└── lib/                   # Third-party libraries
```

## Installing Dependencies

### PHP Dependencies (Composer)

To install PHP dependencies from `composer.json`:

```bash
# If you have Composer installed globally
composer install

# Or use the included Composer PHAR
php composer.phar install
```

This installs:
- PHPUnit for testing
- PHP_CodeSniffer for code quality

### JavaScript Dependencies (NPM)

To install JavaScript dependencies from `package.json`:

```bash
# Make sure Node.js and npm are installed
npm install
```

This installs:
- Webpack for bundling
- Babel for transpiling
- ESLint for linting
- Jest for testing
- Crypto-JS for cryptography
- Axios for HTTP requests

## Using Vendor Utilities

### PHP Utilities

#### CryptoUtil.php

Symmetric encryption compatible with C# Backbone:

```php
require_once 'vendor/php/CryptoUtil.php';

// Encrypt
$encrypted = CryptoUtil::encryptAES256('secret data', 'my-secret-key');

// Decrypt
$decrypted = CryptoUtil::decryptAES256($encrypted, 'my-secret-key');

// Hash
$hash = CryptoUtil::sha256('data');

// Tokens
$token = CryptoUtil::generateToken();
$csrfToken = CryptoUtil::generateCSRFToken();
```

#### DatabaseUtil.php

Connection pooling and query helpers:

```php
require_once 'vendor/php/DatabaseUtil.php';

// Initialize configuration
DatabaseUtil::initialize([
    'default' => [
        'host' => 'localhost',
        'port' => 3306,
        'database' => 'ethermvc',
        'username' => 'root',
        'password' => 'secret'
    ]
]);

// Fetch one
$user = DatabaseUtil::fetchOne(
    'SELECT * FROM users WHERE id = ?',
    [1]
);

// Fetch all
$users = DatabaseUtil::fetchAll(
    'SELECT * FROM users WHERE active = ?',
    [1]
);

// Insert
$id = DatabaseUtil::insert('users', [
    'email' => 'user@example.com',
    'password' => 'hashed_password',
    'name' => 'John Doe'
]);

// Update
DatabaseUtil::update('users', 
    ['name' => 'Jane Doe'],
    ['id' => 1]
);

// Delete
DatabaseUtil::delete('users', ['id' => 1]);
```

### JavaScript Utilities

#### crypto.js

Symmetric encryption compatible with C# Backbone:

```javascript
// Include the library
<script src="vendor/js/crypto.js"></script>

// Encrypt (async)
const encrypted = await CryptoUtil.encryptAES256('secret data', 'my-secret-key');

// Decrypt (async)
const decrypted = await CryptoUtil.decryptAES256(encrypted, 'my-secret-key');

// Hash
const hash = await CryptoUtil.sha256('data');

// Generate random bytes
const bytes = CryptoUtil.generateRandomBytes(32);
```

## Composer Dependencies

The following PHP packages are available in composer.json:

- **phpunit/phpunit** - Unit testing framework
- **squizlabs/php_codesniffer** - Code quality checker

Install them with:
```bash
composer install --dev
```

Run tests:
```bash
composer test
```

Run linting:
```bash
composer lint
```

## NPM Scripts

Available npm commands from package.json:

```bash
# Build for production
npm run build

# Watch for changes and rebuild
npm run dev

# Run tests
npm test

# Run ESLint
npm run lint
```

## Adding More Dependencies

### Add PHP Package

```bash
composer require vendor/package
```

### Add NPM Package

```bash
npm install package-name
```

For development dependencies:
```bash
npm install --save-dev package-name
```

## Security Notes

- Always keep dependencies updated: `composer update` and `npm update`
- Review security advisories: `npm audit` and check Composer warnings
- Use dependency pinning in production
- Regular security audits of vendor code
- Don't commit node_modules or vendor directories to version control (use .gitignore)

## File Gitignore Example

```gitignore
vendor/
node_modules/
composer.lock
package-lock.json
*.phar
```

## Support & Documentation

For more information:
- PHP: See individual utility files for docstrings and examples
- JavaScript: See crypto.js for implementation details
- Composer: https://getcomposer.org
- NPM: https://www.npmjs.com

---

**EtherMVC Framework v1.0**
