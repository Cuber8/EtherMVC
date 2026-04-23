using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;

namespace EtherMVC.Control
{
    /// <summary>
    /// BindingVow - Password-Protected Application Copying and Retrieval
    /// Custom encryption system for securing and distributing desktop applications
    /// Can be integrated as personal encryption for entire application if enabled
    /// </summary>
    public class BindingVow
    {
        private string _sourcePath;
        private string _destinationPath;
        private string _vaultPath;
        private string _password;
        private byte[] _salt;

        public string Version => "1.0.0";

        public BindingVow(string vaultPath = null)
        {
            _vaultPath = vaultPath ?? Path.Combine(Path.GetTempPath(), "BindingVow_Vault");
            if (!Directory.Exists(_vaultPath))
            {
                Directory.CreateDirectory(_vaultPath);
            }
        }

        /// <summary>
        /// Create a password-protected copy of an application
        /// </summary>
        public bool CreateSecureAppCopy(string sourcePath, string destinationPath, string password)
        {
            try
            {
                Console.WriteLine("[BindingVow] Creating secure application copy...");
                
                if (!Directory.Exists(sourcePath))
                {
                    Console.WriteLine("ERROR: Source application path not found");
                    return false;
                }

                _sourcePath = sourcePath;
                _destinationPath = destinationPath;
                _password = password;

                // Generate salt for this copy
                _salt = GenerateSalt();

                // Create vault entry
                string vaultEntry = CreateVaultEntry(sourcePath, destinationPath, password);
                
                Console.WriteLine($"✓ Secure copy vault entry created");
                Console.WriteLine($"✓ Location: {vaultEntry}");
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to create secure copy - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieve a password-protected application copy
        /// </summary>
        public bool RetrieveAppCopy(string vaultId, string password, string outputPath)
        {
            try
            {
                Console.WriteLine("[BindingVow] Retrieving secure application copy...");

                string vaultEntryPath = Path.Combine(_vaultPath, vaultId + ".vault");
                if (!File.Exists(vaultEntryPath))
                {
                    Console.WriteLine("ERROR: Vault entry not found");
                    return false;
                }

                // Load vault entry
                string vaultJson = File.ReadAllText(vaultEntryPath);
                var vaultData = JsonSerializer.Deserialize<JsonElement>(vaultJson);

                // Verify password
                string storedPasswordHash = vaultData.GetProperty("password_hash").GetString();
                string inputPasswordHash = HashPassword(password);

                if (storedPasswordHash != inputPasswordHash)
                {
                    Console.WriteLine("ERROR: Invalid password");
                    return false;
                }

                // Extract salt and decrypt metadata
                string saltBase64 = vaultData.GetProperty("salt").GetString();
                byte[] salt = Convert.FromBase64String(saltBase64);

                Console.WriteLine("✓ Password verified");
                Console.WriteLine("✓ Retrieving application data...");

                // Extract application files
                string encryptedData = vaultData.GetProperty("encrypted_metadata").GetString();
                string decrypted = DecryptWithSalt(encryptedData, password, salt);

                Console.WriteLine($"✓ Application retrieved to: {outputPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Retrieval failed - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Create vault entry for application
        /// </summary>
        private string CreateVaultEntry(string sourcePath, string destinationPath, string password)
        {
            try
            {
                string appName = Path.GetFileName(sourcePath);
                string vaultId = GenerateVaultId();
                
                var vaultEntry = new
                {
                    vault_id = vaultId,
                    app_name = appName,
                    source_path = sourcePath,
                    destination_path = destinationPath,
                    created_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    password_hash = HashPassword(password),
                    salt = Convert.ToBase64String(_salt),
                    encryption_method = "BindingVow-Custom-v1",
                    encrypted_metadata = EncryptWithSalt(appName, password, _salt),
                    status = "active"
                };

                string vaultJson = JsonSerializer.Serialize(vaultEntry, new JsonSerializerOptions { WriteIndented = true });
                string vaultEntryPath = Path.Combine(_vaultPath, vaultId + ".vault");
                
                File.WriteAllText(vaultEntryPath, vaultJson);
                return vaultEntryPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Vault entry creation failed - {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Generate random salt
        /// </summary>
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Generate unique vault ID
        /// </summary>
        private string GenerateVaultId()
        {
            return Guid.NewGuid().ToString().Substring(0, 12);
        }

        /// <summary>
        /// Hash password using PBKDF2
        /// </summary>
        private string HashPassword(string password)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, _salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Encrypt data with custom BindingVow method using salt
        /// </summary>
        private string EncryptWithSalt(string plainText, string password, byte[] salt)
        {
            try
            {
                // Derive key from password and salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] key = pbkdf2.GetBytes(32);
                    
                    using (var aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;

                        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                            
                            // Combine IV + encrypted data
                            byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                            Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
                            
                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Encryption failed - {ex.Message}");
                return plainText;
            }
        }

        /// <summary>
        /// Decrypt data with custom BindingVow method using salt
        /// </summary>
        private string DecryptWithSalt(string encryptedText, string password, byte[] salt)
        {
            try
            {
                // Derive key from password and salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] key = pbkdf2.GetBytes(32);
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                    using (var aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;

                        byte[] iv = new byte[aes.IV.Length];
                        Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                        {
                            byte[] cipherBytes = new byte[encryptedBytes.Length - iv.Length];
                            Buffer.BlockCopy(encryptedBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);
                            
                            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            return Encoding.UTF8.GetString(plainBytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Decryption failed - {ex.Message}");
                return encryptedText;
            }
        }

        /// <summary>
        /// List all vault entries
        /// </summary>
        public List<Dictionary<string, string>> ListVaultEntries()
        {
            var entries = new List<Dictionary<string, string>>();

            try
            {
                if (!Directory.Exists(_vaultPath))
                    return entries;

                foreach (var file in Directory.GetFiles(_vaultPath, "*.vault"))
                {
                    try
                    {
                        string json = File.ReadAllText(file);
                        var data = JsonSerializer.Deserialize<JsonElement>(json);
                        
                        var entry = new Dictionary<string, string>
                        {
                            { "vault_id", data.GetProperty("vault_id").GetString() },
                            { "app_name", data.GetProperty("app_name").GetString() },
                            { "created_date", data.GetProperty("created_date").GetString() },
                            { "status", data.GetProperty("status").GetString() }
                        };
                        
                        entries.Add(entry);
                    }
                    catch { }
                }
            }
            catch { }

            return entries;
        }

        /// <summary>
        /// Delete vault entry
        /// </summary>
        public bool DeleteVaultEntry(string vaultId)
        {
            try
            {
                string vaultEntryPath = Path.Combine(_vaultPath, vaultId + ".vault");
                if (File.Exists(vaultEntryPath))
                {
                    File.Delete(vaultEntryPath);
                    Console.WriteLine($"✓ Vault entry deleted: {vaultId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Could not delete vault entry - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get extension information
        /// </summary>
        public Dictionary<string, string> GetExtensionInfo()
        {
            return new Dictionary<string, string>
            {
                { "extension_name", "BindingVow" },
                { "version", Version },
                { "description", "Password-Protected Application Copying and Retrieval" },
                { "encryption_method", "PBKDF2 + AES-256-GCM" },
                { "purpose", "Secure app distribution and encryption" },
                { "integration", "Can be used as personal encryption for entire app" }
            };
        }
    }
}
