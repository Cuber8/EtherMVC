/**
 * vendor/js/crypto.js - Cryptographic Utilities for JavaScript
 * AES-256 encryption compatible with C# Backbone
 */

class CryptoUtil {
    /**
     * Generate random bytes
     */
    static generateRandomBytes(length = 16) {
        return crypto.getRandomValues(new Uint8Array(length));
    }

    /**
     * Encrypt data using AES-256-GCM
     */
    static async encryptAES256(plaintext, key) {
        const encoder = new TextEncoder();
        const data = encoder.encode(plaintext);
        const keyData = await crypto.subtle.importKey(
            'raw',
            encoder.encode(key.padEnd(32, '\0')).slice(0, 32),
            { name: 'AES-GCM' },
            false,
            ['encrypt']
        );
        
        const iv = this.generateRandomBytes(12);
        const encrypted = await crypto.subtle.encrypt(
            { name: 'AES-GCM', iv: iv },
            keyData,
            data
        );
        
        // Combine IV + encrypted data
        const combined = new Uint8Array(iv.length + encrypted.byteLength);
        combined.set(iv);
        combined.set(new Uint8Array(encrypted), iv.length);
        
        return btoa(String.fromCharCode.apply(null, combined));
    }

    /**
     * Decrypt data using AES-256-GCM
     */
    static async decryptAES256(ciphertext, key) {
        const combined = Uint8Array.from(atob(ciphertext), c => c.charCodeAt(0));
        const iv = combined.slice(0, 12);
        const encrypted = combined.slice(12);
        
        const encoder = new TextEncoder();
        const keyData = await crypto.subtle.importKey(
            'raw',
            encoder.encode(key.padEnd(32, '\0')).slice(0, 32),
            { name: 'AES-GCM' },
            false,
            ['decrypt']
        );
        
        const decrypted = await crypto.subtle.decrypt(
            { name: 'AES-GCM', iv: iv },
            keyData,
            encrypted
        );
        
        const decoder = new TextDecoder();
        return decoder.decode(decrypted);
    }

    /**
     * Hash using SHA-256
     */
    static async sha256(text) {
        const encoder = new TextEncoder();
        const data = encoder.encode(text);
        const hashBuffer = await crypto.subtle.digest('SHA-256', data);
        const hashArray = Array.from(new Uint8Array(hashBuffer));
        return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
    }
}

// Export for use in modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = CryptoUtil;
}
