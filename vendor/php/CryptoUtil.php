<?php
/**
 * vendor/php/CryptoUtil.php - Cryptographic Utilities for PHP
 * AES-256 encryption compatible with C# Backbone
 */

class CryptoUtil {
    const ALGORITHM = 'aes-256-gcm';
    const KEY_LENGTH = 32;
    const IV_LENGTH = 12;

    /**
     * Encrypt data using AES-256-GCM
     */
    public static function encryptAES256($plaintext, $key) {
        // Ensure key is correct length
        $key = substr(hash('sha256', $key, true), 0, self::KEY_LENGTH);
        
        // Generate random IV
        $iv = openssl_random_pseudo_bytes(self::IV_LENGTH);
        
        // Encrypt
        $ciphertext = openssl_encrypt(
            $plaintext,
            self::ALGORITHM,
            $key,
            OPENSSL_RAW_DATA,
            $iv
        );
        
        // Get tag
        $tag = '';
        openssl_encrypt(
            $plaintext,
            self::ALGORITHM,
            $key,
            OPENSSL_RAW_DATA,
            $iv,
            $tag
        );
        
        // Combine IV + ciphertext + tag
        $encrypted = $iv . $ciphertext . $tag;
        
        return base64_encode($encrypted);
    }

    /**
     * Decrypt data using AES-256-GCM
     */
    public static function decryptAES256($ciphertext, $key) {
        // Ensure key is correct length
        $key = substr(hash('sha256', $key, true), 0, self::KEY_LENGTH);
        
        // Decode
        $encrypted = base64_decode($ciphertext);
        
        // Extract components
        $iv = substr($encrypted, 0, self::IV_LENGTH);
        $tag = substr($encrypted, -16);
        $cipher = substr($encrypted, self::IV_LENGTH, -16);
        
        // Decrypt
        $plaintext = openssl_decrypt(
            $cipher,
            self::ALGORITHM,
            $key,
            OPENSSL_RAW_DATA,
            $iv,
            $tag
        );
        
        return $plaintext;
    }

    /**
     * Hash using SHA-256
     */
    public static function sha256($text) {
        return hash('sha256', $text);
    }

    /**
     * Generate random token
     */
    public static function generateToken($length = 32) {
        return bin2hex(openssl_random_pseudo_bytes($length / 2));
    }

    /**
     * Validate CSRF token
     */
    public static function validateCSRFToken($token) {
        return isset($_SESSION['csrf_token']) && $_SESSION['csrf_token'] === $token;
    }

    /**
     * Generate CSRF token
     */
    public static function generateCSRFToken() {
        if (!isset($_SESSION['csrf_token'])) {
            $_SESSION['csrf_token'] = self::generateToken();
        }
        return $_SESSION['csrf_token'];
    }
}
?>
