using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq;
using EtherMVC;
using EtherMVC.Asset;
using EtherMVC.Data;
using EtherMVC.View;
using EtherMVC.Control;

namespace EtherMVC
{
    /// <summary>
    /// Program.cs - EtherMVC Main Executor
    /// Entry point for the EtherMVC framework
    /// Initializes all components and starts the system
    /// </summary>
    class Program
    {
        private static Backbone _backbone;
        private static ASetting _assetSettings;
        private static DSetting _dataSettings;
        private static VSetting _viewSettings;
        private static ESetting _controlSettings;
        private static VenSetting _vendorSettings;
        private static ErrorConsole _errorConsole;
        private static WebServer _webServer;
        private static GenerateApp _generateApp;
        private static Homunculus _homunculus;
        private static BindingVow _bindingVow;
        private static bool _webServerRunning = false;
        private static Task _webServerTask = null;

        static void Main(string[] args)
        {
            try
            {
                PrintBanner();
                
                string projectRoot = GetProjectRoot();
                Console.WriteLine($"[Program] Project Root: {projectRoot}\n");

                // Initialize all components
                InitializeSystem(projectRoot);

                // Display system information
                DisplaySystemInfo();

                // Verify system integrity
                if (Backbone.VerifySystemIntegrity())
                {
                    Console.WriteLine("[Program] ✓ System integrity verified\n");
                }
                else
                {
                    Console.WriteLine("[Program] ✗ System integrity check failed\n");
                }

                // Show menu
                ShowMainMenu(projectRoot);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[Program] ❌ STARTUP ERROR: {ex.GetType().Name}");
                Console.WriteLine($"[Program] Message: {ex.Message}");
                Console.WriteLine($"[Program] Stack Trace:\n{ex.StackTrace}");
                Console.ResetColor();

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("📝 ERROR DETAILS:");
                Console.WriteLine("  - Check docs/logs for detailed error log");
                Console.WriteLine("  - Ensure all required files are present");
                Console.WriteLine("  - Try running as Administrator");
                Console.WriteLine("  - Contact support if problem persists");
                Console.WriteLine(new string('=', 60));
                
                Console.WriteLine("\n[Program] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Print banner
        /// </summary>
        private static void PrintBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║               ███████╗████████╗██╗  ██╗███████╗██████╗    ║
║               ██╔════╝╚══██╔══╝██║  ██║██╔════╝██╔══██╗   ║
║               █████╗     ██║   ███████║█████╗  ██████╔╝   ║
║               ██╔══╝     ██║   ██╔══██║██╔══╝  ██╔══██╗   ║
║               ███████╗   ██║   ██║  ██║███████╗██║  ██║   ║
║               ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ║
║                                                            ║
║                   EtherMVC Framework v1.0                 ║
║            Secure Web & Application Development           ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝
");
            Console.ResetColor();
        }

        /// <summary>
        /// Get project root directory
        /// </summary>
        private static string GetProjectRoot()
        {
            string currentDir = Directory.GetCurrentDirectory();
            
            // Check if running from project root
            if (File.Exists(Path.Combine(currentDir, "Backbone.cs")))
            {
                return currentDir;
            }

            // Try to find project root
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            while (dir.Parent != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Backbone.cs")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }

            return currentDir;
        }

        /// <summary>
        /// Initialize all system components
        /// </summary>
        private static void InitializeSystem(string projectRoot)
        {
            Console.WriteLine("[Program] Initializing EtherMVC System...\n");

            try
            {
                // Initialize Backbone
                _backbone = new Backbone(projectRoot);

                // Initialize Asset Settings
                string assetPath = Path.Combine(projectRoot, "Asset");
                _assetSettings = new ASetting(assetPath);

                // Initialize Data Settings
                string dataPath = Path.Combine(projectRoot, "Data");
                _dataSettings = new DSetting(dataPath);

                // Initialize View Settings
                string viewPath = Path.Combine(projectRoot, "View");
                _viewSettings = new VSetting(viewPath);

                // Initialize Vendor Settings
                string vendorPath = Path.Combine(projectRoot, "vendor");
                _vendorSettings = new VenSetting(vendorPath);

                // Initialize Error Console
                string logPath = Path.Combine(projectRoot, "docs", "logs");
                _errorConsole = new ErrorConsole(logPath);

                // Initialize Control Settings
                string controlPath = Path.Combine(projectRoot, "EtherControl");
                _controlSettings = new ESetting(controlPath);

                // Initialize Generate App (Desktop App Generator)
                _generateApp = new GenerateApp(projectRoot);

                // Initialize Homunculus (SDK Replacement)
                _homunculus = new Homunculus(projectRoot);

                // Initialize BindingVow (App Copying & Encryption)
                _bindingVow = new BindingVow();

                Console.WriteLine("[Program] ✓ All components initialized successfully\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Program] Initialization error: {ex.Message}\n");
                Console.ResetColor();
                throw;
            }
        }

        /// <summary>
        /// Display system information
        /// </summary>
        private static void DisplaySystemInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║       SYSTEM INFORMATION                  ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine($"System Type:        {Backbone.GetSystemType()}");
            Console.WriteLine($"Database Type:      {_dataSettings.GetDatabaseType()}");
            Console.WriteLine($"Control Version:    {_controlSettings.GetVersion()}");
            Console.WriteLine($"Error Console:      {(_errorConsole.IsEnabled() ? "Enabled" : "Disabled")}");
            Console.WriteLine($"Assets:             {(_assetSettings.GetSetting("icon") != null ? "Ready" : "Pending")}");
            Console.WriteLine();
        }

        /// <summary>
        /// Show main menu
        /// </summary>
        private static void ShowMainMenu(string projectRoot)
        {
            bool running = true;

            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("╔═══════════════════════════════════════════╗");
                Console.WriteLine("║           MAIN MENU                       ║");
                if (_webServerRunning)
                {
                    Console.WriteLine("║      🌐 WEB SERVER ACTIVE 🌐              ║");
                }
                Console.WriteLine("╚═══════════════════════════════════════════╝");
                Console.ResetColor();

                if (!_webServerRunning)
                {
                    Console.WriteLine("1. 🌐 Start Web Server (Recommended)");
                    Console.WriteLine("2. 🚀 Generate Desktop App");
                    Console.WriteLine("3. View System Information");
                    Console.WriteLine("4. View Database Configuration");
                    Console.WriteLine("5. View Asset Settings");
                    Console.WriteLine("6. View View Settings");
                    Console.WriteLine("7. View Routes & Controllers");
                    Console.WriteLine("8. View Error Logs");
                    Console.WriteLine("9. Test API Request");
                    Console.WriteLine("10. 🧬 Homunculus - SDK Replacement");
                    Console.WriteLine("11. 🔐 BindingVow - App Copying");
                    Console.WriteLine("0. Exit");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("⚙️  WEB SERVER CONTROL:");
                    Console.ResetColor();
                    Console.WriteLine("1. 🔴 Stop Web Server");
                    Console.WriteLine("2. 🚀 Generate Desktop App");
                    Console.WriteLine("3. 📋 View Server Status");
                    Console.WriteLine("4. 📜 View Web Server Logs");
                    Console.WriteLine("5. 🌐 Open Website in Browser");
                    Console.WriteLine("6. 📊 View System Information");
                    Console.WriteLine("0. Return to Main Menu (Web Server will stop)");
                }
                
                Console.WriteLine();

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                Console.WriteLine();

                if (!_webServerRunning)
                {
                    switch (choice)
                    {
                        case "1":
                            StartWebServer(projectRoot);
                            break;
                        case "2":
                            _generateApp.StartGenerationWizard();
                            break;
                        case "3":
                            DisplaySystemInfo();
                            break;
                        case "4":
                            DisplayDatabaseConfig();
                            break;
                        case "5":
                            DisplayAssetSettings();
                            break;
                        case "6":
                            DisplayViewSettings();
                            break;
                        case "7":
                            DisplayRoutes();
                            break;
                        case "8":
                            DisplayErrorLogs();
                            break;
                        case "9":
                            TestApiRequest();
                            break;
                        case "10":
                            ShowHomunculusMenu();
                            break;
                        case "11":
                            ShowBindingVowMenu();
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("[Program] Shutting down EtherMVC...");
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1":
                            StopWebServer();
                            break;
                        case "2":
                            _generateApp.StartGenerationWizard();
                            break;
                        case "3":
                            ViewServerStatus();
                            break;
                        case "4":
                            DisplayServerLogs();
                            break;
                        case "5":
                            OpenWebsiteInBrowser();
                            break;
                        case "6":
                            DisplaySystemInfo();
                            break;
                        case "0":
                            StopWebServer();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ResetColor();
                            break;
                    }
                }

                if (running && choice != "0" && !(_webServerRunning && choice == "1"))
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Start web server - RUNS IN BACKGROUND - Menu remains interactive
        /// </summary>
        private static void StartWebServer(string projectRoot)
        {
            try
            {
                if (_webServerRunning)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Program] Web server is already running!");
                    Console.ResetColor();
                    return;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║               🌐 WEB SERVER MODE                           ║
║                                                            ║
║        EtherMVC Web Server Starting...                     ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝
");
                Console.ResetColor();

                int port = 8080;
                string hostname = "EtherSite";
                
                _webServer = new WebServer(port, projectRoot, hostname);
                
                Console.WriteLine($"[Program] Initializing web server...");
                Console.WriteLine($"[Program] Hostname: {hostname}");
                Console.WriteLine($"[Program] Port: {port}");
                Console.WriteLine($"[Program] ℹ️  Menu will remain accessible while server runs\n");

                // Start web server in background task
                _webServerRunning = true;
                _webServerTask = Task.Run(async () =>
                {
                    try
                    {
                        await _webServer.StartAsync(openBrowser: true);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n[Program] ❌ Web Server Error: {ex.GetType().Name}");
                        Console.WriteLine($"[Program] Message: {ex.Message}");
                        if (!string.IsNullOrEmpty(ex.StackTrace))
                        {
                            Console.WriteLine($"[Program] Stack: {ex.StackTrace.Substring(0, Math.Min(200, ex.StackTrace.Length))}");
                        }
                        Console.ResetColor();
                    }
                    finally
                    {
                        _webServerRunning = false;
                    }
                });

                // Give server time to start
                Task.Delay(1000).Wait();
                
                if (_webServerRunning)
                {
                    Console.WriteLine("[Program] ✅ Web server started successfully!");
                    Console.WriteLine("[Program] 🌐 Open browser: http://localhost:8080");
                    Console.WriteLine("[Program] Use menu to control the server");
                    Console.WriteLine("[Program] Press any key to return to main menu...");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Program] ❌ Web server failed to start");
                    Console.ResetColor();
                    Console.WriteLine("[Program] Press any key to continue...");
                    Console.ReadKey();
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                _webServerRunning = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[Program] ❌ Error: {ex.GetType().Name}");
                Console.WriteLine($"[Program] Message: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("[Program] Press any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Stop web server gracefully
        /// </summary>
        private static void StopWebServer()
        {
            if (!_webServerRunning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Program] Web server is not running.");
                Console.ResetColor();
                return;
            }

            try
            {
                Console.WriteLine("[Program] ⏹️  Stopping web server...");
                _webServer?.Stop();
                _webServerRunning = false;

                // Wait for server task to complete (with timeout)
                if (_webServerTask != null)
                {
                    _webServerTask.Wait(TimeSpan.FromSeconds(5));
                }

                Console.WriteLine("[Program] ✅ Web server stopped successfully");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Program] Error stopping server: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// View web server status
        /// </summary>
        private static void ViewServerStatus()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║         SERVER STATUS                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine($"Server Running:     {(_webServerRunning ? "✅ YES" : "❌ NO")}");
            Console.WriteLine($"Port:               8080");
            Console.WriteLine($"Hostname:           Etherhost");
            Console.WriteLine($"Access:             http://localhost:8080");
            Console.WriteLine($"                    or http://Etherhost:8080");
            
            if (_webServerTask != null)
            {
                Console.WriteLine($"Task Status:        {_webServerTask.Status}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display web server logs
        /// </summary>
        private static void DisplayServerLogs()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║       WEB SERVER LOGS (Last 20)           ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                string logsDir = Path.Combine(Directory.GetCurrentDirectory(), "docs", "logs");
                if (!Directory.Exists(logsDir))
                {
                    Console.WriteLine("No logs directory found.");
                    return;
                }

                var logFiles = Directory.GetFiles(logsDir, "*.log").OrderByDescending(f => f).FirstOrDefault();
                if (logFiles == null)
                {
                    Console.WriteLine("No log files found.");
                    return;
                }

                var lines = File.ReadAllLines(logFiles).TakeLast(20);
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading logs: {ex.Message}");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Open website in browser
        /// </summary>
        private static void OpenWebsiteInBrowser()
        {
            try
            {
                Console.WriteLine("[Program] Opening website in default browser...");
                string url = "http://localhost:8080";
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }

                Console.WriteLine("[Program] ✅ Browser opened successfully");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Program] ❌ Could not open browser: {ex.Message}");
                Console.WriteLine($"[Program] Try opening manually: http://localhost:8080");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Rebuild executable with icon
        /// </summary>
        private static void RebuildExecutable(string projectRoot)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║               🔨 REBUILDING EXECUTABLE                     ║
║                                                            ║
║        Compiling EtherMVC.exe with settings...            ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝
");
                Console.ResetColor();

                Console.WriteLine("[Program] Building release version...\n");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "publish -c Release -r win-x64 -p:PublishSingleFile=true",
                    WorkingDirectory = projectRoot,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[Program] ✅ Build completed successfully!");
                        Console.ResetColor();

                        // Copy to root with file-lock handling
                        string publishPath = Path.Combine(projectRoot, "bin", "Release", "net6.0", "win-x64", "publish", "EtherMVC.exe");
                        string rootExe = Path.Combine(projectRoot, "EtherMVC.exe");

                        if (File.Exists(publishPath))
                        {
                            try
                            {
                                // Try direct copy first
                                File.Copy(publishPath, rootExe, true);
                                Console.WriteLine($"[Program] ✅ EXE copied to root: {rootExe}");
                            }
                            catch (System.IO.IOException ex)
                            {
                                // File is locked, try rename approach
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"[Program] ⚠️  File is in use. Attempting safe replacement...");
                                Console.ResetColor();

                                try
                                {
                                    string backupExe = Path.Combine(projectRoot, "EtherMVC.exe.old");
                                    
                                    // Remove old backup if exists
                                    if (File.Exists(backupExe))
                                    {
                                        File.Delete(backupExe);
                                    }

                                    // Rename current exe to backup
                                    if (File.Exists(rootExe))
                                    {
                                        File.Move(rootExe, backupExe, true);
                                    }

                                    // Copy new exe
                                    File.Copy(publishPath, rootExe, true);

                                    // Delete backup
                                    try
                                    {
                                        File.Delete(backupExe);
                                    }
                                    catch { } // Ignore backup deletion errors

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[Program] ✅ EXE replaced successfully!");
                                    Console.WriteLine($"[Program] 💡 Old version: EtherMVC.exe.old (will be cleaned up next rebuild)");
                                    Console.ResetColor();
                                }
                                catch (Exception retryEx)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[Program] ❌ Could not replace EXE: {retryEx.Message}");
                                    Console.WriteLine($"[Program] 💡 The running instance may still be using the file.");
                                    Console.WriteLine($"[Program] 💡 Try stopping the web server first (menu option 1).");
                                    Console.ResetColor();
                                }
                            }

                            if (File.Exists(rootExe))
                            {
                                var fileInfo = new FileInfo(rootExe);
                                Console.WriteLine($"[Program] 📦 File size: {fileInfo.Length / (1024 * 1024):F2} MB");
                                Console.WriteLine($"[Program] ⚙️  Settings: General_Setting.json");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[Program] ❌ Published EXE not found at: {publishPath}");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n[Program] ❌ Build failed with exit code: {process.ExitCode}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[Program] ❌ Error rebuilding: {ex.Message}");
                Console.WriteLine($"[Program] 💡 Tip: Make sure no other instance is running.");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Display database configuration
        /// </summary>
        private static void DisplayDatabaseConfig()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║      DATABASE CONFIGURATION               ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            var settings = _dataSettings.GetAllSettings();
            foreach (var kvp in settings)
            {
                Console.WriteLine($"{kvp.Key.PadRight(20)}: {kvp.Value}");
            }
        }

        /// <summary>
        /// Display asset settings
        /// </summary>
        private static void DisplayAssetSettings()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║         ASSET SETTINGS                    ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            var settings = _assetSettings.GetAllSettings();
            foreach (var kvp in settings)
            {
                Console.WriteLine($"{kvp.Key.PadRight(20)}: {kvp.Value}");
            }
        }

        /// <summary>
        /// Display view settings
        /// </summary>
        private static void DisplayViewSettings()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║         VIEW SETTINGS                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            var settings = _viewSettings.GetAllSettings();
            foreach (var kvp in settings)
            {
                Console.WriteLine($"{kvp.Key.PadRight(20)}: {kvp.Value}");
            }
        }

        /// <summary>
        /// Display routes and controllers
        /// </summary>
        private static void DisplayRoutes()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║      ROUTES & CONTROLLERS                 ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            var routes = _controlSettings.GetRegisteredRoutes();
            var controllers = _controlSettings.GetRegisteredControllers();

            Console.WriteLine($"\nTotal Routes: {routes.Count}");
            foreach (var route in routes)
            {
                Console.WriteLine($"  - {Path.GetFileName(route)}");
            }

            Console.WriteLine($"\nTotal Controllers: {controllers.Count}");
            foreach (var controller in controllers)
            {
                Console.WriteLine($"  - {Path.GetFileName(controller)}");
            }
        }

        /// <summary>
        /// Display error logs
        /// </summary>
        private static void DisplayErrorLogs()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║         ERROR LOGS                        ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            var errors = _errorConsole.GetAllErrors();
            
            if (errors.Count == 0)
            {
                Console.WriteLine("No errors recorded.");
            }
            else
            {
                foreach (var error in errors)
                {
                    Console.WriteLine($"[{error.Timestamp}] {error.Level}: {error.Source}");
                    Console.WriteLine($"  {error.Message}\n");
                }
            }
        }

        /// <summary>
        /// Test API request
        /// </summary>
        private static void TestApiRequest()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║         TEST API REQUEST                  ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("Test encryption/decryption:");
            
            string testData = "Sensitive Information";
            Console.WriteLine($"Original: {testData}");
            
            string encrypted = Backbone.EncryptData(testData);
            Console.WriteLine($"Encrypted: {encrypted}");
            
            string decrypted = Backbone.DecryptData(encrypted);
            Console.WriteLine($"Decrypted: {decrypted}");
            
            Console.WriteLine($"Verification: {(decrypted == testData ? "✓ Success" : "✗ Failed")}");
        }

        /// <summary>
        /// Generate project documentation
        /// </summary>
        private static void GenerateDocumentation()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║      GENERATING DOCUMENTATION             ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("Documentation would be generated here...");
            Console.WriteLine("Features:");
            Console.WriteLine("  - System Architecture");
            Console.WriteLine("  - API Reference");
            Console.WriteLine("  - Security Guidelines");
            Console.WriteLine("  - Database Schema");
            Console.WriteLine("  - Code Examples");
        }

        /// <summary>
        /// Show Homunculus Menu
        /// </summary>
        private static void ShowHomunculusMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║    🧬 HOMUNCULUS - SDK REPLACEMENT         ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\nHomunculus provides a runtime environment for desktop applications");
            Console.WriteLine("without requiring .NET SDK installation.\n");

            var runtimeInfo = _homunculus.GetRuntimeInfo();
            Console.WriteLine("Runtime Information:");
            foreach (var kvp in runtimeInfo)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nAvailable Extensions:");
            var extensions = _homunculus.GetAvailableExtensions();
            foreach (var ext in extensions)
            {
                Console.WriteLine($"  ✓ {ext}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Show BindingVow Menu
        /// </summary>
        private static void ShowBindingVowMenu()
        {
            bool bindingVowRunning = true;

            while (bindingVowRunning)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔═══════════════════════════════════════════╗");
                Console.WriteLine("║  🔐 BINDINGVOW - PASSWORD APP SYSTEM       ║");
                Console.WriteLine("╚═══════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n1. Create Secure App Copy");
                Console.WriteLine("2. List Vault Entries");
                Console.WriteLine("3. Delete Vault Entry");
                Console.WriteLine("4. View Extension Info");
                Console.WriteLine("0. Return to Main Menu");

                Console.Write("\nSelect option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateSecureAppCopy();
                        break;
                    case "2":
                        ListVaultEntries();
                        break;
                    case "3":
                        DeleteVaultEntry();
                        break;
                    case "4":
                        DisplayBindingVowInfo();
                        break;
                    case "0":
                        bindingVowRunning = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option.");
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void CreateSecureAppCopy()
        {
            Console.WriteLine("\n--- Create Secure App Copy ---");
            Console.Write("Source app path: ");
            string sourcePath = Console.ReadLine();
            
            Console.Write("Destination path: ");
            string destPath = Console.ReadLine();
            
            Console.Write("Password: ");
            string password = Console.ReadLine();

            if (_bindingVow.CreateSecureAppCopy(sourcePath, destPath, password))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Secure copy created successfully");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ Failed to create secure copy");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void ListVaultEntries()
        {
            Console.WriteLine("\n--- Vault Entries ---");
            var entries = _bindingVow.ListVaultEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("No vault entries found.");
            }
            else
            {
                Console.WriteLine($"\nTotal entries: {entries.Count}\n");
                foreach (var entry in entries)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Vault ID: {entry["vault_id"]}");
                    Console.ResetColor();
                    Console.WriteLine($"  App Name: {entry["app_name"]}");
                    Console.WriteLine($"  Created: {entry["created_date"]}");
                    Console.WriteLine($"  Status: {entry["status"]}\n");
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void DeleteVaultEntry()
        {
            Console.WriteLine("\n--- Delete Vault Entry ---");
            Console.Write("Enter vault ID to delete: ");
            string vaultId = Console.ReadLine();

            if (_bindingVow.DeleteVaultEntry(vaultId))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Vault entry deleted");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ Could not delete vault entry");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void DisplayBindingVowInfo()
        {
            Console.WriteLine("\n--- BindingVow Extension Info ---");
            var info = _bindingVow.GetExtensionInfo();
            
            foreach (var kvp in info)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
