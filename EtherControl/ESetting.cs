using System;
using System.Collections.Generic;
using System.IO;

namespace EtherMVC.Control
{
    /// <summary>
    /// ESetting.cs - EtherControl Settings Manager
    /// Handles routing setup for either Retro (PHP) or Modern (JavaScript) architecture
    /// Also provides security configuration
    /// </summary>
    public class ESetting
    {
        public enum ControllerVersion
        {
            Retro,      // Traditional MVC with PHP controllers
            Modern      // Simplified EtherChemistry pattern
        }

        private ControllerVersion _version;
        private Dictionary<string, object> _settings;
        private string _etherControlPath;
        private List<string> _registeredRoutes;
        private List<string> _registeredControllers;

        public ESetting(string etherControlPath)
        {
            _etherControlPath = etherControlPath;
            _settings = new Dictionary<string, object>();
            _registeredRoutes = new List<string>();
            _registeredControllers = new List<string>();
            Initialize();
        }

        /// <summary>
        /// Initialize settings and detect version
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("[ESetting] Initializing EtherControl settings...");
            
            DetectVersion();
            LoadControllerConfiguration();
            LoadSecuritySettings();
            
            Console.WriteLine("[ESetting] EtherControl settings initialized");
        }

        /// <summary>
        /// Detect which version is configured
        /// </summary>
        private void DetectVersion()
        {
            try
            {
                bool hasRetroSetup = Directory.Exists(Path.Combine(_etherControlPath, "Controller_retro")) &&
                                     Directory.Exists(Path.Combine(_etherControlPath, "route_retro"));
                
                bool hasModernSetup = File.Exists(Path.Combine(_etherControlPath, "EtherChemistery.js"));

                if (hasRetroSetup && !hasModernSetup)
                {
                    _version = ControllerVersion.Retro;
                    Console.WriteLine("[ESetting] Version: Retro (Traditional MVC)");
                }
                else if (hasModernSetup)
                {
                    _version = ControllerVersion.Modern;
                    Console.WriteLine("[ESetting] Version: Modern (EtherChemistry)");
                }
                else
                {
                    _version = ControllerVersion.Modern; // Default
                    Console.WriteLine("[ESetting] No version configured, defaulting to Modern");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ESetting] Error detecting version: {ex.Message}");
                _version = ControllerVersion.Modern;
            }
        }

        /// <summary>
        /// Load controller configuration
        /// </summary>
        private void LoadControllerConfiguration()
        {
            try
            {
                if (_version == ControllerVersion.Retro)
                {
                    LoadRetroConfiguration();
                }
                else
                {
                    LoadModernConfiguration();
                }

                Console.WriteLine("[ESetting] Controller configuration loaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ESetting] Controller loading error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load Retro configuration
        /// </summary>
        private void LoadRetroConfiguration()
        {
            try
            {
                string controllerPath = Path.Combine(_etherControlPath, "Controller_retro");
                string routePath = Path.Combine(_etherControlPath, "route_retro");

                _settings["version"] = "retro";
                _settings["pattern"] = "MVC";
                _settings["controllerPath"] = controllerPath;
                _settings["routePath"] = routePath;

                if (Directory.Exists(controllerPath))
                {
                    string[] controllers = Directory.GetFiles(controllerPath, "*.js");
                    _registeredControllers.AddRange(controllers);
                }

                if (Directory.Exists(routePath))
                {
                    string[] routes = Directory.GetFiles(routePath, "*.js");
                    _registeredRoutes.AddRange(routes);
                }

                Console.WriteLine($"[ESetting] Retro: {_registeredControllers.Count} controllers, {_registeredRoutes.Count} routes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ESetting] Retro configuration error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load Modern configuration
        /// </summary>
        private void LoadModernConfiguration()
        {
            try
            {
                string chemistryPath = Path.Combine(_etherControlPath, "EtherChemistery.js");

                _settings["version"] = "modern";
                _settings["pattern"] = "EtherChemistry";
                _settings["chemistryPath"] = chemistryPath;

                Console.WriteLine("[ESetting] Modern: EtherChemistry pattern loaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ESetting] Modern configuration error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load security settings
        /// </summary>
        private void LoadSecuritySettings()
        {
            try
            {
                _settings["enableCors"] = true;
                _settings["corsOrigin"] = "localhost";
                _settings["enableRateLimit"] = true;
                _settings["rateLimitRequests"] = 100;
                _settings["rateLimitWindow"] = 60000; // 1 minute in ms
                _settings["enableInputValidation"] = true;
                _settings["enableCsrfProtection"] = true;

                Console.WriteLine("[ESetting] Security settings configured");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ESetting] Security settings error: {ex.Message}");
            }
        }

        /// <summary>
        /// Register a route
        /// </summary>
        public void RegisterRoute(string route)
        {
            if (!_registeredRoutes.Contains(route))
            {
                _registeredRoutes.Add(route);
                Console.WriteLine($"[ESetting] Route registered: {route}");
            }
        }

        /// <summary>
        /// Register a controller
        /// </summary>
        public void RegisterController(string controller)
        {
            if (!_registeredControllers.Contains(controller))
            {
                _registeredControllers.Add(controller);
                Console.WriteLine($"[ESetting] Controller registered: {controller}");
            }
        }

        /// <summary>
        /// Get current version
        /// </summary>
        public ControllerVersion GetVersion()
        {
            return _version;
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

        /// <summary>
        /// Get registered routes
        /// </summary>
        public List<string> GetRegisteredRoutes()
        {
            return new List<string>(_registeredRoutes);
        }

        /// <summary>
        /// Get registered controllers
        /// </summary>
        public List<string> GetRegisteredControllers()
        {
            return new List<string>(_registeredControllers);
        }
    }
}
