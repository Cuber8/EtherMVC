using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace EtherMVC.Control
{
    /// <summary>
    /// Homunculus - .NET SDK Replacement for Desktop Applications
    /// Enables standalone desktop applications to run without requiring .NET SDK installation
    /// Acts as a runtime environment for generated EtherMVC desktop applications
    /// </summary>
    public class Homunculus
    {
        private string _appPath;
        private string _configPath;
        private string _logPath;
        private string _appName;
        private Dictionary<string, string> _config;

        public string Version => "1.0.0";
        public string SDKVersion => ".NET 6.0 Compatible";

        public Homunculus(string appPath)
        {
            _appPath = appPath;
            _appName = Path.GetFileName(appPath);
            _logPath = Path.Combine(appPath, "Logs");
            _configPath = Path.Combine(appPath, "Config", "app.config.json");
            
            // Create Logs directory if not exists
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            _config = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initialize the Homunculus runtime environment
        /// </summary>
        public bool Initialize()
        {
            try
            {
                LogMessage("Initializing Homunculus SDK Replacement...");
                LogMessage($"Application: {_appName}");
                LogMessage($"Path: {_appPath}");
                LogMessage($"SDK Version: {SDKVersion}");

                // Verify app structure
                if (!VerifyAppStructure())
                {
                    LogMessage("ERROR: App structure verification failed");
                    return false;
                }

                LogMessage("✓ App structure verified");

                // Load configuration
                if (!LoadConfiguration())
                {
                    LogMessage("WARNING: Could not load configuration, using defaults");
                }

                LogMessage("✓ Initialization complete");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: Initialization failed - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verify app folder structure
        /// </summary>
        private bool VerifyAppStructure()
        {
            string[] requiredFolders = { "View", "Data", "Asset", "Config", "Logs" };
            foreach (string folder in requiredFolders)
            {
                string folderPath = Path.Combine(_appPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    LogMessage($"WARNING: Missing folder - {folder}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Load app configuration
        /// </summary>
        private bool LoadConfiguration()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    string configContent = File.ReadAllText(_configPath);
                    _config["loaded"] = "true";
                    _config["config_path"] = _configPath;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: Configuration loading failed - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Launch the desktop application
        /// </summary>
        public bool LaunchApplication()
        {
            try
            {
                LogMessage("Launching application...");

                // Check for launcher script
                string launcherScript = Path.Combine(_appPath, $"{_appName}.bat");
                if (!File.Exists(launcherScript))
                {
                    LogMessage("ERROR: Launcher script not found");
                    return false;
                }

                // Execute launcher
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = launcherScript,
                    WorkingDirectory = _appPath,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                using (var process = Process.Start(psi))
                {
                    LogMessage($"Application process started (PID: {process.Id})");
                    process.WaitForExit();
                    LogMessage($"Application exited with code: {process.ExitCode}");
                }

                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: Application launch failed - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get application status
        /// </summary>
        public Dictionary<string, string> GetApplicationStatus()
        {
            var status = new Dictionary<string, string>
            {
                { "app_name", _appName },
                { "app_path", _appPath },
                { "sdk_version", SDKVersion },
                { "runtime_version", Version },
                { "structure_valid", VerifyAppStructure() ? "true" : "false" },
                { "config_loaded", _config.ContainsKey("loaded") ? "true" : "false" }
            };

            // Check for required files
            status["launcher_exists"] = File.Exists(Path.Combine(_appPath, $"{_appName}.bat")) ? "true" : "false";
            status["config_exists"] = File.Exists(_configPath) ? "true" : "false";

            return status;
        }

        /// <summary>
        /// Get runtime information
        /// </summary>
        public Dictionary<string, string> GetRuntimeInfo()
        {
            var info = new Dictionary<string, string>
            {
                { "runtime_version", Version },
                { "sdk_replacement", "Homunculus v1.0.0" },
                { "target_framework", ".NET 6.0" },
                { "platform", System.Runtime.InteropServices.RuntimeInformation.OSDescription },
                { "process_architecture", System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString() }
            };

            return info;
        }

        /// <summary>
        /// Log a message to file
        /// </summary>
        private void LogMessage(string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"[{timestamp}] {message}";
                string logFile = Path.Combine(_logPath, "homunculus.log");

                File.AppendAllText(logFile, logEntry + Environment.NewLine);
                Console.WriteLine(logEntry);
            }
            catch { }
        }

        /// <summary>
        /// Cleanup application resources
        /// </summary>
        public bool Cleanup()
        {
            try
            {
                LogMessage("Cleaning up application resources...");

                // Clean test database if configured
                string testDbPath = Path.Combine(_appPath, "Data", "test_db.json");
                if (File.Exists(testDbPath))
                {
                    File.Delete(testDbPath);
                    LogMessage("✓ Test database cleaned up");
                }

                LogMessage("✓ Cleanup complete");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"WARNING: Cleanup issue - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get list of available extensions
        /// </summary>
        public List<string> GetAvailableExtensions()
        {
            var extensions = new List<string>
            {
                "BindingVow - Password-Protected App Copying",
                "Encryption - AES-256 Data Protection",
                "Logging - Comprehensive Error Tracking",
                "Validation - Input Verification System"
            };

            return extensions;
        }
    }
}
