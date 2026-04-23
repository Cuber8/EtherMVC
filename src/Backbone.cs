using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EtherMVC
{
    /// <summary>
    /// Backbone.cs - Core system handler for EtherMVC
    /// Manages security, inter-component communication, and system type verification
    /// </summary>
    public class Backbone
    {
        private static string _systemType = "unknown"; // "website" or "app"
        private static string _encryptionKey = string.Empty;
        private static string _projectRoot = string.Empty;

        // Security settings
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int Iterations = 10000;

        public Backbone(string projectRoot)
        {
            _projectRoot = projectRoot;
            Initialize();
        }

        /// <summary>
        /// Initialize the system and determine if it's a website or app
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("[Backbone] Initializing EtherMVC System...");
            
            // Load configuration
            LoadSystemConfiguration();
            
            // Initialize encryption
            InitializeEncryption();
            
            Console.WriteLine($"[Backbone] System Type: {_systemType}");
            Console.WriteLine("[Backbone] System initialized successfully");
        }

        /// <summary>
        /// Load system configuration and determine app type
        /// </summary>
        private void LoadSystemConfiguration()
        {
            try
            {
                string configPath = Path.Combine(_projectRoot, "config.json");
                
                if (File.Exists(configPath))
                {
                    string configContent = File.ReadAllText(configPath);
                    // Parse config to determine system type
                    _systemType = configContent.Contains("\"type\":\"website\"") ? "website" : "app";
                }
                else
                {
                    // Default to website
                    _systemType = "website";
                    SaveDefaultConfiguration();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Configuration Error: {ex.Message}");
                _systemType = "website"; // fallback
            }
        }

        /// <summary>
        /// Save default configuration
        /// </summary>
        private void SaveDefaultConfiguration()
        {
            try
            {
                string configPath = Path.Combine(_projectRoot, "config.json");
                string defaultConfig = "{\"type\":\"website\",\"security\":\"enabled\",\"version\":\"1.0\"}";
                File.WriteAllText(configPath, defaultConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Failed to save config: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialize encryption for sensitive data
        /// </summary>
        private void InitializeEncryption()
        {
            try
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] keyBytes = new byte[32];
                    rng.GetBytes(keyBytes);
                    _encryptionKey = Convert.ToBase64String(keyBytes);
                }
                Console.WriteLine("[Backbone] Encryption initialized");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Encryption Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Encrypt sensitive data
        /// </summary>
        public static string EncryptData(string plainText)
        {
            if (string.IsNullOrEmpty(_encryptionKey))
                throw new InvalidOperationException("Encryption key not initialized");

            try
            {
                byte[] key = Convert.FromBase64String(_encryptionKey);
                
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (var ms = new MemoryStream())
                        {
                            ms.Write(aes.IV, 0, aes.IV.Length);
                            
                            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (var sw = new StreamWriter(cs))
                                {
                                    sw.Write(plainText);
                                }
                            }
                            
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Encryption failed: {ex.Message}");
                return plainText;
            }
        }

        /// <summary>
        /// Decrypt sensitive data
        /// </summary>
        public static string DecryptData(string cipherText)
        {
            if (string.IsNullOrEmpty(_encryptionKey))
                throw new InvalidOperationException("Encryption key not initialized");

            try
            {
                byte[] key = Convert.FromBase64String(_encryptionKey);
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    byte[] iv = new byte[aes.IV.Length];
                    Array.Copy(buffer, 0, iv, 0, iv.Length);

                    using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                    {
                        using (var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                        {
                            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                            {
                                using (var sr = new StreamReader(cs))
                                {
                                    return sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Decryption failed: {ex.Message}");
                return cipherText;
            }
        }

        /// <summary>
        /// Get system type
        /// </summary>
        public static string GetSystemType()
        {
            return _systemType;
        }

        /// <summary>
        /// Verify system integrity
        /// </summary>
        public static bool VerifySystemIntegrity()
        {
            try
            {
                string[] requiredFolders = { "Asset", "Data", "View", "EtherControl" };
                
                foreach (var folder in requiredFolders)
                {
                    if (!Directory.Exists(Path.Combine(_projectRoot, folder)))
                    {
                        Console.WriteLine($"[Backbone] Missing folder: {folder}");
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Backbone] Integrity check failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Log system events
        /// </summary>
        public static void LogEvent(string source, string message, string level = "INFO")
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[{timestamp}] [{level}] [{source}] {message}");
        }
    }
}
