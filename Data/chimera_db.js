// chimera_db.js - Chimera Database Schema Manager with Encryption
// Handles JSON data structure definitions, encryption/decryption, and secure data navigation

class ChimeraDB {
    constructor() {
        this.schemas = {};
        this.encryptionEnabled = true;
        this.schemaVersion = "1.0";
        console.log("[ChimeraDB] Initializing Chimera Database System...");
    }

    /**
     * Create a new schema/table
     * @param {string} tableName - Name of the table
     * @param {object} structure - Field definitions
     */
    createSchema(tableName, structure) {
        try {
            if (this.schemas[tableName]) {
                console.warn(`[ChimeraDB] Schema '${tableName}' already exists`);
                return false;
            }

            this.schemas[tableName] = {
                name: tableName,
                fields: structure,
                created: new Date().toISOString(),
                encrypted: this.encryptionEnabled
            };

            console.log(`[ChimeraDB] Schema '${tableName}' created successfully`);
            return true;
        } catch (error) {
            console.error(`[ChimeraDB] Error creating schema: ${error.message}`);
            return false;
        }
    }

    /**
     * Define field encryption status
     * @param {string} tableName - Table name
     * @param {string} fieldName - Field name
     * @param {boolean} shouldEncrypt - Whether to encrypt this field
     */
    setFieldEncryption(tableName, fieldName, shouldEncrypt = true) {
        try {
            if (!this.schemas[tableName]) {
                throw new Error(`Schema '${tableName}' not found`);
            }

            if (!this.schemas[tableName].fields[fieldName]) {
                throw new Error(`Field '${fieldName}' not found in schema '${tableName}'`);
            }

            this.schemas[tableName].fields[fieldName].encrypted = shouldEncrypt;
            console.log(`[ChimeraDB] Field '${fieldName}' encryption set to ${shouldEncrypt}`);
        } catch (error) {
            console.error(`[ChimeraDB] Error setting field encryption: ${error.message}`);
        }
    }

    /**
     * Get schema definition
     * @param {string} tableName - Table name
     */
    getSchema(tableName) {
        return this.schemas[tableName] || null;
    }

    /**
     * Validate data against schema
     * @param {string} tableName - Table name
     * @param {object} data - Data to validate
     */
    validateData(tableName, data) {
        try {
            const schema = this.schemas[tableName];
            if (!schema) {
                throw new Error(`Schema '${tableName}' not found`);
            }

            for (const [fieldName, fieldDef] of Object.entries(schema.fields)) {
                if (fieldDef.required && !(fieldName in data)) {
                    throw new Error(`Required field '${fieldName}' is missing`);
                }

                if (fieldName in data) {
                    const fieldType = typeof data[fieldName];
                    const expectedType = fieldDef.type.toLowerCase();

                    if (expectedType === 'string' && fieldType !== 'string') {
                        throw new Error(`Field '${fieldName}' must be a string`);
                    }
                    if (expectedType === 'number' && fieldType !== 'number') {
                        throw new Error(`Field '${fieldName}' must be a number`);
                    }
                }
            }

            return true;
        } catch (error) {
            console.error(`[ChimeraDB] Validation error: ${error.message}`);
            return false;
        }
    }

    /**
     * Simple encryption function (should use proper crypto library in production)
     * @param {string} data - Data to encrypt
     */
    encrypt(data) {
        if (!this.encryptionEnabled) return data;
        
        try {
            // Base64 encoding as simple encryption (replace with proper encryption in production)
            return Buffer.from(JSON.stringify(data)).toString('base64');
        } catch (error) {
            console.error(`[ChimeraDB] Encryption error: ${error.message}`);
            return data;
        }
    }

    /**
     * Simple decryption function
     * @param {string} encryptedData - Encrypted data
     */
    decrypt(encryptedData) {
        if (!this.encryptionEnabled) return encryptedData;
        
        try {
            // Base64 decoding
            return JSON.parse(Buffer.from(encryptedData, 'base64').toString('utf-8'));
        } catch (error) {
            console.error(`[ChimeraDB] Decryption error: ${error.message}`);
            return encryptedData;
        }
    }

    /**
     * Encrypt field names for secure navigation
     * @param {string} tableName - Table name
     * @param {object} data - Data object
     */
    encryptFieldNames(tableName, data) {
        try {
            const schema = this.schemas[tableName];
            if (!schema) throw new Error(`Schema '${tableName}' not found`);

            const encrypted = {};
            for (const [fieldName, value] of Object.entries(data)) {
                const encryptedFieldName = this.encryptString(fieldName);
                encrypted[encryptedFieldName] = schema.fields[fieldName]?.encrypted 
                    ? this.encrypt(value) 
                    : value;
            }
            return encrypted;
        } catch (error) {
            console.error(`[ChimeraDB] Field name encryption error: ${error.message}`);
            return data;
        }
    }

    /**
     * Encrypt a string value
     * @param {string} str - String to encrypt
     */
    encryptString(str) {
        return Buffer.from(str).toString('hex');
    }

    /**
     * Decrypt field names
     * @param {string} tableName - Table name
     * @param {object} encryptedData - Encrypted data object
     */
    decryptFieldNames(tableName, encryptedData) {
        try {
            const schema = this.schemas[tableName];
            if (!schema) throw new Error(`Schema '${tableName}' not found`);

            const decrypted = {};
            const fieldMappings = {};

            // Create hex to field name mapping
            for (const fieldName of Object.keys(schema.fields)) {
                const hex = this.encryptString(fieldName);
                fieldMappings[hex] = fieldName;
            }

            for (const [encryptedFieldName, value] of Object.entries(encryptedData)) {
                const originalFieldName = fieldMappings[encryptedFieldName] || encryptedFieldName;
                decrypted[originalFieldName] = schema.fields[originalFieldName]?.encrypted
                    ? this.decrypt(value)
                    : value;
            }
            return decrypted;
        } catch (error) {
            console.error(`[ChimeraDB] Field name decryption error: ${error.message}`);
            return encryptedData;
        }
    }

    /**
     * Get all schemas
     */
    getAllSchemas() {
        return { ...this.schemas };
    }

    /**
     * Export schema as example
     * @param {string} tableName - Table name
     */
    exportSchemaExample(tableName) {
        const schema = this.schemas[tableName];
        if (!schema) return null;

        const example = {};
        for (const [fieldName, fieldDef] of Object.entries(schema.fields)) {
            example[fieldName] = `[${fieldDef.type}${fieldDef.required ? ', required' : ''}]`;
        }
        return example;
    }

    /**
     * Create database sample for users
     */
    static createUserDatabaseSample() {
        const db = new ChimeraDB();

        // Users table example
        db.createSchema('users', {
            id: { type: 'string', required: true, encrypted: false },
            username: { type: 'string', required: true, encrypted: false },
            email: { type: 'string', required: true, encrypted: true },
            password: { type: 'string', required: true, encrypted: true },
            createdAt: { type: 'string', required: true, encrypted: false }
        });

        // Products table example
        db.createSchema('products', {
            id: { type: 'string', required: true, encrypted: false },
            name: { type: 'string', required: true, encrypted: false },
            price: { type: 'number', required: true, encrypted: false },
            description: { type: 'string', required: false, encrypted: false }
        });

        return db;
    }
}

// Export for Node.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ChimeraDB;
}
