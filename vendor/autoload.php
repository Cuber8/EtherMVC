<?php
/**
 * vendor/autoload.php - EtherMVC Vendor Autoloader
 * Automatically includes all vendor utilities
 */

// Define vendor root
define('VENDOR_ROOT', __DIR__);

// Autoloader function
spl_autoload_register(function ($class) {
    // Map class names to files
    $classMap = [
        'CryptoUtil' => VENDOR_ROOT . '/php/CryptoUtil.php',
        'DatabaseUtil' => VENDOR_ROOT . '/php/DatabaseUtil.php'
    ];
    
    if (isset($classMap[$class])) {
        require_once $classMap[$class];
        return true;
    }
    
    return false;
});

// Include key utilities
require_once VENDOR_ROOT . '/php/CryptoUtil.php';
require_once VENDOR_ROOT . '/php/DatabaseUtil.php';

// Log vendor initialization
if (defined('ETHERMVC_DEBUG') && ETHERMVC_DEBUG) {
    error_log('[Vendor] EtherMVC vendor utilities loaded');
}

return true;
?>
