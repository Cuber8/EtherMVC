using System;
using System.Collections.Generic;
using System.IO;

namespace EtherMVC.View
{
    /// <summary>
    /// VSetting.cs - View Settings Manager
    /// Handles view configuration and security to prevent displaying sensitive information
    /// </summary>
    public class VSetting
    {
        private Dictionary<string, object> _settings;
        private string _viewPath;
        private HashSet<string> _sensitiveFields;
        private bool _errorConsoleEnabled = true;

        public VSetting(string viewPath)
        {
            _viewPath = viewPath;
            _settings = new Dictionary<string, object>();
            _sensitiveFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "password", "apikey", "secret", "token", "ssn", "creditcard",
                "privatekey", "accesstoken", "refreshtoken", "authkey"
            };
            Initialize();
        }

        /// <summary>
        /// Initialize view settings
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("[VSetting] Initializing view settings...");
            
            LoadLayoutConfiguration();
            LoadPageConfiguration();
            LoadSecuritySettings();
            
            Console.WriteLine("[VSetting] View settings initialized successfully");
        }

        /// <summary>
        /// Load layout configuration
        /// </summary>
        private void LoadLayoutConfiguration()
        {
            try
            {
                string layoutPath = Path.Combine(_viewPath, "layout");
                string basicLayoutPath = Path.Combine(layoutPath, "basic.html");

                if (Directory.Exists(layoutPath))
                {
                    _settings["layoutPath"] = layoutPath;
                    
                    if (File.Exists(basicLayoutPath))
                    {
                        _settings["basicLayout"] = basicLayoutPath;
                        Console.WriteLine("[VSetting] Basic layout found");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VSetting] Layout loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load page configuration
        /// </summary>
        private void LoadPageConfiguration()
        {
            try
            {
                string pagesPath = Path.Combine(_viewPath, "pages");

                if (Directory.Exists(pagesPath))
                {
                    _settings["pagesPath"] = pagesPath;
                    string[] pageFiles = Directory.GetFiles(pagesPath, "*.html");
                    _settings["pageCount"] = pageFiles.Length;
                    
                    Console.WriteLine($"[VSetting] Found {pageFiles.Length} pages");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VSetting] Page loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load security settings
        /// </summary>
        private void LoadSecuritySettings()
        {
            try
            {
                _settings["sanitizeHtml"] = true;
                _settings["preventXss"] = true;
                _settings["hideSensitiveData"] = true;
                _settings["enableErrorConsole"] = _errorConsoleEnabled;
                
                Console.WriteLine("[VSetting] Security settings loaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VSetting] Security settings error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sanitize output to prevent XSS and sensitive data exposure
        /// </summary>
        public string SanitizeOutput(string output)
        {
            if (string.IsNullOrEmpty(output))
                return output;

            try
            {
                // Remove script tags
                output = System.Text.RegularExpressions.Regex.Replace(output, "<script[^>]*>.*?</script>", "", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove event handlers
                output = System.Text.RegularExpressions.Regex.Replace(output, @"on\w+\s*=\s*['""]?[^'"">\s]+['""]?", "");

                // HTML encode special characters
                output = System.Web.HttpUtility.HtmlEncode(output);

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VSetting] Sanitization error: {ex.Message}");
                return output;
            }
        }

        /// <summary>
        /// Check if a field should be hidden (sensitive)
        /// </summary>
        public bool IsSensitiveField(string fieldName)
        {
            return _sensitiveFields.Contains(fieldName);
        }

        /// <summary>
        /// Remove sensitive data from output
        /// </summary>
        public Dictionary<string, object> FilterSensitiveData(Dictionary<string, object> data)
        {
            var filtered = new Dictionary<string, object>();

            foreach (var kvp in data)
            {
                if (!IsSensitiveField(kvp.Key))
                {
                    filtered[kvp.Key] = kvp.Value;
                }
                else
                {
                    filtered[kvp.Key] = "***HIDDEN***";
                }
            }

            return filtered;
        }

        /// <summary>
        /// Enable/Disable error console
        /// </summary>
        public void SetErrorConsoleEnabled(bool enabled)
        {
            _errorConsoleEnabled = enabled;
            _settings["enableErrorConsole"] = enabled;
        }

        /// <summary>
        /// Get setting
        /// </summary>
        public object GetSetting(string key)
        {
            return _settings.ContainsKey(key) ? _settings[key] : null;
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        public Dictionary<string, object> GetAllSettings()
        {
            return new Dictionary<string, object>(_settings);
        }
    }
}
