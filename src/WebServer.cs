using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// WebServer.cs - Built-in HTTP server for EtherMVC
/// Serves static files and handles API routes without external dependencies
/// </summary>
public class WebServer
{
    private HttpListener _listener;
    private int _port;
    private string _rootPath;
    private string _hostname;
    private bool _isRunning = false;
    private StreamWriter _logWriter;

    public WebServer(int port = 8080, string rootPath = ".", string hostname = "localhost")
    {
        _port = port;
        _rootPath = rootPath;
        _hostname = hostname;
        _listener = new HttpListener();
        InitializeLogging();
    }

    /// <summary>
    /// Initialize logging
    /// </summary>
    private void InitializeLogging()
    {
        try
        {
            string logsPath = Path.Combine(_rootPath, "docs", "logs");
            if (!Directory.Exists(logsPath))
            {
                try { Directory.CreateDirectory(logsPath); }
                catch { }
            }

            try
            {
                string logFile = Path.Combine(logsPath, $"webserver_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
                _logWriter = new StreamWriter(logFile, true) { AutoFlush = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WebServer] Warning: Could not open log file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WebServer] Failed to initialize logging: {ex.Message}");
        }
    }

    /// <summary>
    /// Write to log file and console
    /// </summary>
    private void Log(string message)
    {
        try
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Console.WriteLine(logMessage);
            try
            {
                if (_logWriter != null && _logWriter.BaseStream != null && _logWriter.BaseStream.CanWrite)
                {
                    _logWriter.WriteLine(logMessage);
                    _logWriter.Flush();
                }
            }
            catch (ObjectDisposedException)
            {
                // Log writer is closed, continue without file logging
            }
        }
        catch { }
    }

    /// <summary>
    /// Starts the web server and optionally opens browser
    /// </summary>
    public async Task StartAsync(bool openBrowser = true)
    {
        try
        {
            Log($"[WebServer] Initializing server on {_hostname}:{_port}");
            Log($"[WebServer] Root path: {_rootPath}");
            Log($"[WebServer] View path: {Path.Combine(_rootPath, "View")}");

            string bindHostname = _hostname;
            if (_hostname.ToLower() == "etherhost" || _hostname.ToLower() == "localhost" || _hostname.ToLower() == "ethersite")
            {
                bindHostname = "localhost";
            }

            string prefix = $"http://{bindHostname}:{_port}/";
            Log($"[WebServer] Adding listener prefix: {prefix}");
            
            // Clear any existing prefixes first
            _listener.Prefixes.Clear();
            _listener.Prefixes.Add(prefix);
            
            try
            {
                Log($"[WebServer] Attempting to start HttpListener...");
                _listener.Start();
                Log($"[WebServer] HttpListener started successfully");
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 5)
            {
                Log($"⚠️  [WebServer] Access Denied - trying with + (all interfaces)");
                _listener.Prefixes.Clear();
                _listener.Prefixes.Add($"http://+:{_port}/");
                _listener.Start();
                Log($"[WebServer] Bound to all interfaces on port {_port}");
            }
            
            _isRunning = true;

            Log($"✅ [WebServer] Server started successfully!");
            Log($"🌐 [WebServer] Access EtherSite at: EtherSite:{_port}");
            Log($"📝 [WebServer] Waiting for requests...");
            Console.WriteLine();

            if (openBrowser)
            {
                await Task.Delay(500);
                OpenBrowser($"http://{bindHostname}:{_port}/");
            }

            await HandleRequestsAsync();
        }
        catch (HttpListenerException ex)
        {
            Log($"❌ [WebServer] HttpListener error: {ex.Message}");
            Log($"❌ [WebServer] Error Code: {ex.ErrorCode}");
            
            if (ex.Message.Contains("Address already in use"))
            {
                Log($"⚠️  [WebServer] Port {_port} is already in use!");
                Log($"💡 [WebServer] Try changing port in config.json");
            }
            if (ex.ErrorCode == 5)
            {
                Log($"❌ [WebServer] Access Denied! Run as Administrator!");
            }
        }
        catch (Exception ex)
        {
            Log($"❌ [WebServer] Unexpected error: {ex.GetType().Name}: {ex.Message}");
            Log($"📍 [WebServer] Stack trace: {ex.StackTrace}");
        }
        finally
        {
            Stop();
        }
    }

    /// <summary>
    /// Handle incoming requests continuously
    /// </summary>
    private async Task HandleRequestsAsync()
    {
        try
        {
            while (_isRunning)
            {
                try
                {
                    HttpListenerContext context = await _listener.GetContextAsync();
                    await ProcessRequestAsync(context);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (_isRunning)
                    {
                        Log($"⚠️  [WebServer] Context error: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log($"❌ [WebServer] Request handler error: {ex.Message}");
        }
    }

    /// <summary>
    /// Process individual HTTP requests
    /// </summary>
    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            var response = context.Response;

            string path = request.Url.LocalPath;
            if (path == "/") path = "/Index.html";

            Log($"[Request] {request.HttpMethod} {path}");

            // Try to serve from View first, then Asset
            string filePath = Path.Combine(_rootPath, "View", path.TrimStart('/'));
            filePath = Path.GetFullPath(filePath);
            
            // Handle case-insensitive file lookup in View
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
            {
                filePath = FindFileCaseInsensitive(filePath);
            }

            // If not found in View, try Asset directory
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
            {
                string assetPath = Path.Combine(_rootPath, "Asset", path.TrimStart('/'));
                assetPath = Path.GetFullPath(assetPath);
                
                if (File.Exists(assetPath) || Directory.Exists(assetPath))
                {
                    filePath = assetPath;
                }
            }

            string viewPath = Path.GetFullPath(Path.Combine(_rootPath, "View"));
            string assetBasePath = Path.GetFullPath(Path.Combine(_rootPath, "Asset"));
            
            // Security check - ensure path is within View or Asset
            if (!filePath.StartsWith(viewPath) && !filePath.StartsWith(assetBasePath))
            {
                Log($"⚠️  [Security] Blocked access attempt: {path}");
                SendResponse(response, 403, "text/plain", "Access denied");
                return;
            }

            if (path.StartsWith("/api/"))
            {
                HandleApiRequest(request, response, path);
                return;
            }

            if (File.Exists(filePath))
            {
                string contentType = GetContentType(filePath);
                byte[] fileContent = File.ReadAllBytes(filePath);

                response.ContentType = contentType;
                response.ContentLength64 = fileContent.Length;
                response.StatusCode = 200;
                response.OutputStream.Write(fileContent, 0, fileContent.Length);
                Log($"✅ [Response] 200 OK - {path}");
            }
            else if (Directory.Exists(filePath))
            {
                string indexPath = Path.Combine(filePath, "Index.html");
                if (!File.Exists(indexPath))
                {
                    indexPath = FindFileCaseInsensitive(indexPath);
                }
                if (File.Exists(indexPath))
                {
                    byte[] fileContent = File.ReadAllBytes(indexPath);
                    response.ContentType = "text/html";
                    response.ContentLength64 = fileContent.Length;
                    response.StatusCode = 200;
                    response.OutputStream.Write(fileContent, 0, fileContent.Length);
                    Log($"✅ [Response] 200 OK - {path}/Index.html");
                }
                else
                {
                    SendResponse(response, 404, "text/html", GenerateErrorPage(404, "Index not found"));
                    Log($"❌ [Response] 404 Not Found - {path}/Index.html");
                }
            }
            else
            {
                SendResponse(response, 404, "text/html", GenerateErrorPage(404, "File not found"));
                Log($"❌ [Response] 404 Not Found - {path}");
            }
        }
        catch (Exception ex)
        {
            Log($"⚠️  [Request Error] {ex.GetType().Name}: {ex.Message}");
            try
            {
                SendResponse(context.Response, 500, "text/plain", "Internal error");
            }
            catch { }
        }
        finally
        {
            context.Response.Close();
        }
    }

    /// <summary>
    /// Handle API requests
    /// </summary>
    private void HandleApiRequest(HttpListenerRequest request, HttpListenerResponse response, string path)
    {
        Log($"[API] {request.HttpMethod} {path}");

        if (path == "/api/info")
        {
            string json = $@"{{""system"": ""EtherMVC"", ""version"": ""1.0"", ""hostname"": ""{_hostname}"", ""port"": {_port}}}";
            SendResponse(response, 200, "application/json", json);
            Log($"✅ [API] 200 OK - /api/info");
        }
        else if (path == "/api/status")
        {
            SendResponse(response, 200, "application/json", @"{""status"": ""running""}");
            Log($"✅ [API] 200 OK - /api/status");
        }
        else
        {
            SendResponse(response, 404, "application/json", @"{""error"": ""Not found""}");
            Log($"❌ [API] 404 - {path}");
        }
    }

    /// <summary>
    /// Send HTTP response
    /// </summary>
    private void SendResponse(HttpListenerResponse response, int statusCode, string contentType, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.StatusCode = statusCode;
        response.ContentType = contentType;
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Determine MIME type from file extension
    /// </summary>
    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".html" or ".htm" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".svg" => "image/svg+xml",
            ".ico" => "image/x-icon",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    /// Generate error page HTML
    /// </summary>
    private string GenerateErrorPage(int statusCode, string message)
    {
        return $@"<!DOCTYPE html><html><head><title>Error {statusCode}</title><style>body {{font-family: Arial; margin: 50px; text-align: center;}} h1 {{color: #e74c3c;}}</style></head><body><h1>Error {statusCode}</h1><p>{message}</p><p><a href=""/"">Home</a></p></body></html>";
    }

    /// <summary>
    /// Open browser to URL
    /// </summary>
    /// <summary>
    /// Find file with case-insensitive lookup
    /// </summary>
    private string FindFileCaseInsensitive(string filePath)
    {
        try
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            
            if (!Directory.Exists(directory))
                return filePath;
            
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return file;
                }
            }
        }
        catch { }
        
        return filePath;
    }

    private void OpenBrowser(string url)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true,
                CreateNoWindow = true
            };
            Process.Start(psi);
            Log($"🌐 [Browser] Opening: EtherSite:{_port}");
        }
        catch (Exception ex)
        {
            Log($"⚠️  [Browser] Could not auto-open: {ex.Message}");
            Log($"ℹ️  Open manually: {url}");
        }
    }

    /// <summary>
    /// Stop the server
    /// </summary>
    public void Stop()
    {
        _isRunning = false;
        try
        {
            // Log before closing writer
            if (_logWriter != null)
            {
                Log($"🛑 [WebServer] Server stopping...");
            }
            else
            {
                Console.WriteLine($"🛑 [WebServer] Server stopping...");
            }
            
            _listener?.Stop();
            _listener?.Close();
        }
        catch { }
        
        // Close log writer last
        try
        {
            if (_logWriter != null)
            {
                _logWriter.Flush();
                _logWriter.Dispose();
                _logWriter = null;
            }
        }
        catch { }
        
        Console.WriteLine($"✅ [WebServer] Server stopped successfully");
    }
}
