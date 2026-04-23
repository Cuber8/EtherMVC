using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EtherMVC.View
{
    /// <summary>
    /// ErrorConsole.cs - Error Display Console
    /// Handles displaying app errors during testing. Can be disabled from VSetting.
    /// Ensures app continues with error display even if crash occurs.
    /// </summary>
    public class ErrorConsole
    {
        private List<ErrorEntry> _errorLog;
        private bool _isEnabled = true;
        private string _logPath;
        private const int MaxErrorsInMemory = 100;

        public struct ErrorEntry
        {
            public DateTime Timestamp { get; set; }
            public string Source { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
            public ErrorLevel Level { get; set; }
        }

        public enum ErrorLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        public ErrorConsole(string logPath)
        {
            _logPath = logPath;
            _errorLog = new List<ErrorEntry>();
            Initialize();
        }

        /// <summary>
        /// Initialize error console
        /// </summary>
        private void Initialize()
        {
            Console.WriteLine("[ErrorConsole] Initializing error console...");
            
            try
            {
                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }
                
                Console.WriteLine("[ErrorConsole] Error console ready - Errors will be logged and displayed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ErrorConsole] Initialization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Log an error
        /// </summary>
        public void LogError(string source, string message, string stackTrace = "", ErrorLevel level = ErrorLevel.Error)
        {
            if (!_isEnabled)
                return;

            try
            {
                var entry = new ErrorEntry
                {
                    Timestamp = DateTime.Now,
                    Source = source,
                    Message = message,
                    StackTrace = stackTrace,
                    Level = level
                };

                _errorLog.Add(entry);

                // Keep memory clean
                if (_errorLog.Count > MaxErrorsInMemory)
                {
                    _errorLog.RemoveAt(0);
                }

                // Display in console
                DisplayError(entry);

                // Write to file
                WriteToLogFile(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ErrorConsole] Failed to log error: {ex.Message}");
            }
        }

        /// <summary>
        /// Display error in console
        /// </summary>
        private void DisplayError(ErrorEntry entry)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            try
            {
                switch (entry.Level)
                {
                    case ErrorLevel.Debug:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ErrorLevel.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case ErrorLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case ErrorLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case ErrorLevel.Critical:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                }

                string timestamp = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                Console.WriteLine($"\n[{timestamp}] [{entry.Level}] {entry.Source}: {entry.Message}");
                
                if (!string.IsNullOrEmpty(entry.StackTrace))
                {
                    Console.WriteLine($"Stack Trace:\n{entry.StackTrace}");
                }
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// Write error to log file
        /// </summary>
        private void WriteToLogFile(ErrorEntry entry)
        {
            try
            {
                string logFileName = $"error_log_{DateTime.Now:yyyy-MM-dd}.txt";
                string logFilePath = Path.Combine(_logPath, logFileName);

                string logEntry = $"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] [{entry.Level}] [{entry.Source}] {entry.Message}";
                
                if (!string.IsNullOrEmpty(entry.StackTrace))
                {
                    logEntry += $"\nStack Trace:\n{entry.StackTrace}";
                }

                logEntry += "\n" + new string('-', 80) + "\n";

                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ErrorConsole] Failed to write to log file: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all errors
        /// </summary>
        public List<ErrorEntry> GetAllErrors()
        {
            return new List<ErrorEntry>(_errorLog);
        }

        /// <summary>
        /// Get errors by level
        /// </summary>
        public List<ErrorEntry> GetErrorsByLevel(ErrorLevel level)
        {
            var filtered = new List<ErrorEntry>();
            foreach (var entry in _errorLog)
            {
                if (entry.Level == level)
                {
                    filtered.Add(entry);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Clear error log
        /// </summary>
        public void ClearErrorLog()
        {
            _errorLog.Clear();
            Console.WriteLine("[ErrorConsole] Error log cleared");
        }

        /// <summary>
        /// Enable/Disable error console
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
            Console.WriteLine($"[ErrorConsole] Error console {(enabled ? "enabled" : "disabled")}");
        }

        /// <summary>
        /// Get console status
        /// </summary>
        public bool IsEnabled()
        {
            return _isEnabled;
        }

        /// <summary>
        /// Export errors to JSON
        /// </summary>
        public string ExportToJson()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("[");

                for (int i = 0; i < _errorLog.Count; i++)
                {
                    var entry = _errorLog[i];
                    sb.AppendLine($"  {{");
                    sb.AppendLine($"    \"timestamp\": \"{entry.Timestamp:yyyy-MM-dd HH:mm:ss}\",");
                    sb.AppendLine($"    \"source\": \"{entry.Source}\",");
                    sb.AppendLine($"    \"message\": \"{entry.Message.Replace("\"", "\\\"")}\",");
                    sb.AppendLine($"    \"level\": \"{entry.Level}\"");
                    sb.AppendLine(i < _errorLog.Count - 1 ? "  }," : "  }");
                }

                sb.AppendLine("]");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ErrorConsole] JSON export error: {ex.Message}");
                return "[]";
            }
        }
    }
}
