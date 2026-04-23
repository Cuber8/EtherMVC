using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EtherMVC.Control
{
    /// <summary>
    /// VenSetting.cs - Vendor/Extension Configuration Manager
    /// Manages third-party dependencies and custom extensions
    /// </summary>
    public class VenSetting
    {
        private string _vendorPath;
        private Dictionary<string, object> _vendorConfig;

        public VenSetting(string vendorPath)
        {
            _vendorPath = vendorPath;
            _vendorConfig = new Dictionary<string, object>();
            
            Console.WriteLine($"[VenSetting] Initializing vendor manager at: {_vendorPath}");
            
            if (!Directory.Exists(_vendorPath))
            {
                Directory.CreateDirectory(_vendorPath);
                Console.WriteLine($"[VenSetting] ✓ Created vendor directory");
            }
            
            LoadVendorConfig();
            DiscoverExtensions();
        }

        /// <summary>
        /// Load vendor configuration
        /// </summary>
        private void LoadVendorConfig()
        {
            try
            {
                string configPath = Path.Combine(_vendorPath, "vendor.config.json");
                
                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    _vendorConfig = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent, options) ?? new Dictionary<string, object>();
                    Console.WriteLine($"[VenSetting] ✓ Loaded vendor configuration");
                }
                else
                {
                    Console.WriteLine($"[VenSetting] No vendor config found, creating default");
                    CreateDefaultVendorConfig(configPath);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[VenSetting] Warning: Could not load vendor config: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Create default vendor configuration
        /// </summary>
        private void CreateDefaultVendorConfig(string configPath)
        {
            var defaultConfig = new
            {
                enabled = true,
                extensions = new List<object>(),
                packages = new
                {
                    php = new
                    {
                        composer = true,
                        installed = new List<string>()
                    },
                    javascript = new
                    {
                        npm = true,
                        installed = new List<string>()
                    }
                },
                autoloader = "vendor/autoload.php"
            };

            try
            {
                string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);
                Console.WriteLine($"[VenSetting] ✓ Created default vendor.config.json");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[VenSetting] Error creating default config: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Discover available extensions
        /// </summary>
        private void DiscoverExtensions()
        {
            try
            {
                string phpPath = Path.Combine(_vendorPath, "php");
                string jsPath = Path.Combine(_vendorPath, "js");

                int phpCount = 0;
                int jsCount = 0;

                if (Directory.Exists(phpPath))
                {
                    var phpFiles = Directory.GetFiles(phpPath, "*.php");
                    phpCount = phpFiles.Length;
                    Console.WriteLine($"[VenSetting] ✓ Found {phpCount} PHP extensions");
                }

                if (Directory.Exists(jsPath))
                {
                    var jsFiles = Directory.GetFiles(jsPath, "*.js");
                    jsCount = jsFiles.Length;
                    Console.WriteLine($"[VenSetting] ✓ Found {jsCount} JavaScript extensions");
                }

                Console.WriteLine($"[VenSetting] ✓ Total extensions: {phpCount + jsCount}\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[VenSetting] Warning: Could not discover extensions: {ex.Message}\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Register a new extension
        /// </summary>
        public void RegisterExtension(string name, string type, string path)
        {
            try
            {
                var extension = new
                {
                    name = name,
                    type = type,
                    path = path,
                    enabled = true,
                    registered = DateTime.Now
                };

                // Store in config
                if (!_vendorConfig.ContainsKey("registeredExtensions"))
                {
                    _vendorConfig["registeredExtensions"] = new List<object>();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[VenSetting] ✓ Registered extension: {name} ({type})");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[VenSetting] Error registering extension: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Add PHP package
        /// </summary>
        public void AddPhpPackage(string packageName)
        {
            RegisterExtension(packageName, "php", $"php/{packageName}.php");
        }

        /// <summary>
        /// Add JavaScript package
        /// </summary>
        public void AddJsPackage(string packageName)
        {
            RegisterExtension(packageName, "js", $"js/{packageName}.js");
        }

        /// <summary>
        /// Get all registered extensions
        /// </summary>
        public Dictionary<string, object> GetAllExtensions()
        {
            return _vendorConfig;
        }

        /// <summary>
        /// Get setting value
        /// </summary>
        public object GetSetting(string key)
        {
            return _vendorConfig.ContainsKey(key) ? _vendorConfig[key] : null;
        }

        /// <summary>
        /// Set setting value
        /// </summary>
        public void SetSetting(string key, object value)
        {
            _vendorConfig[key] = value;
        }

        /// <summary>
        /// Get vendor path
        /// </summary>
        public string GetVendorPath()
        {
            return _vendorPath;
        }

        /// <summary>
        /// Get PHP vendor path
        /// </summary>
        public string GetPhpVendorPath()
        {
            return Path.Combine(_vendorPath, "php");
        }

        /// <summary>
        /// Get JavaScript vendor path
        /// </summary>
        public string GetJsVendorPath()
        {
            return Path.Combine(_vendorPath, "js");
        }

        /// <summary>
        /// Check if extension is installed
        /// </summary>
        public bool IsExtensionInstalled(string name)
        {
            try
            {
                string phpPath = Path.Combine(_vendorPath, "php", $"{name}.php");
                string jsPath = Path.Combine(_vendorPath, "js", $"{name}.js");
                
                return File.Exists(phpPath) || File.Exists(jsPath);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get installed extensions count
        /// </summary>
        public int GetInstalledCount()
        {
            int count = 0;
            
            try
            {
                string phpPath = Path.Combine(_vendorPath, "php");
                if (Directory.Exists(phpPath))
                    count += Directory.GetFiles(phpPath, "*.php").Length;

                string jsPath = Path.Combine(_vendorPath, "js");
                if (Directory.Exists(jsPath))
                    count += Directory.GetFiles(jsPath, "*.js").Length;
            }
            catch { }

            return count;
        }
    }
}
