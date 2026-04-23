using System;
using System.IO;
using System.Collections.Generic;

namespace EtherMVC.Asset
{
    /// <summary>
    /// ASetting.cs - Asset Settings Manager
    /// Handles app icons, themes, and visual configurations
    /// </summary>
    public class ASetting
    {
        private string _assetPath;
        private Dictionary<string, object> _settings;
        private string _currentTheme = "theme1";
        private string _iconPath = string.Empty;

        public ASetting(string assetPath)
        {
            _assetPath = assetPath;
            _settings = new Dictionary<string, object>();
            LoadSettings();
        }

        /// <summary>
        /// Load asset settings
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                Console.WriteLine("[ASetting] Loading asset settings...");
                
                // Load theme
                LoadTheme();
                
                // Load icon
                LoadIcon();
                
                // Load CSS frameworks
                LoadCSSFrameworks();
                
                Console.WriteLine("[ASetting] Asset settings loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ASetting] Error loading settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Load theme configuration
        /// </summary>
        private void LoadTheme()
        {
            try
            {
                string themePath = Path.Combine(_assetPath, "theme", _currentTheme);
                
                if (Directory.Exists(themePath))
                {
                    _settings["theme"] = _currentTheme;
                    _settings["themeDisplayPath"] = Path.Combine(themePath, "display.html");
                    _settings["themeStylePath"] = Path.Combine(themePath, "rendering.css");
                    
                    Console.WriteLine($"[ASetting] Theme '{_currentTheme}' loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ASetting] Theme loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load application icon
        /// </summary>
        private void LoadIcon()
        {
            try
            {
                string iconPath = Path.Combine(_assetPath, "icon.png");
                
                if (File.Exists(iconPath))
                {
                    _iconPath = iconPath;
                    _settings["icon"] = iconPath;
                    Console.WriteLine("[ASetting] Icon loaded");
                }
                else
                {
                    Console.WriteLine("[ASetting] Warning: icon.png not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ASetting] Icon loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load CSS frameworks
        /// </summary>
        private void LoadCSSFrameworks()
        {
            try
            {
                string bootstrapPath = Path.Combine(_assetPath, "bootstrap.css");
                
                if (File.Exists(bootstrapPath))
                {
                    _settings["bootstrap"] = bootstrapPath;
                    Console.WriteLine("[ASetting] Bootstrap CSS loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ASetting] CSS Framework loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get setting value
        /// </summary>
        public object GetSetting(string key)
        {
            return _settings.ContainsKey(key) ? _settings[key] : null;
        }

        /// <summary>
        /// Set theme
        /// </summary>
        public void SetTheme(string themeName)
        {
            _currentTheme = themeName;
            LoadTheme();
        }

        /// <summary>
        /// Get all settings
        /// </summary>
        public Dictionary<string, object> GetAllSettings()
        {
            return new Dictionary<string, object>(_settings);
        }

        /// <summary>
        /// Get icon path
        /// </summary>
        public string GetIconPath()
        {
            return _iconPath;
        }
    }
}
