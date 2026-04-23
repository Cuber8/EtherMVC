using System;
using System.Collections.Generic;

namespace EtherMVC.Data
{
    /// <summary>
    /// DSetting.cs - Data Settings Manager
    /// Handles modern JSON database configuration with Chimera encryption
    /// (MODERN ROUTING ONLY - No retro/MySQL support)
    /// </summary>
    public class DSetting
    {
        public enum DatabaseType
        {
            Modern      // JSON with Chimera encryption (ONLY SUPPORTED METHOD)
        }

        private DatabaseType _databaseType;
        private Dictionary<string, object> _settings;
        private string _dataPath;

        public DSetting(string dataPath)
        {
            _dataPath = dataPath;
            _settings = new Dictionary<string, object>();
            Initialize();
        }

        /// <summary>
        /// Initialize data settings
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("[DSetting] Initializing data settings (Modern/JSON only)...");
            _databaseType = DatabaseType.Modern;
            LoadDatabaseConfiguration();
        }

        /// <summary>
        /// Load database configuration (Modern/JSON only)
        /// </summary>
        private void LoadDatabaseConfiguration()
        {
            try
            {
                _settings["type"] = "modern";
                _settings["backend"] = "JSON";
                _settings["encryption"] = "Chimera";
                _settings["chimeraPath"] = System.IO.Path.Combine(_dataPath, "chimera_db.js");
                _settings["jsonDbPath"] = System.IO.Path.Combine(_dataPath, "db_json");
                
                Console.WriteLine("[DSetting] Modern configuration loaded - JSON with Chimera encryption");
                Console.WriteLine("[DSetting] Database configuration loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DSetting] Configuration error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get database type
        /// </summary>
        public DatabaseType GetDatabaseType()
        {
            return _databaseType;
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
