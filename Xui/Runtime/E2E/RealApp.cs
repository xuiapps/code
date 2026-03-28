using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xui.Runtime.E2E;

/// <summary>
/// An end-to-end test harness that launches a Xui app as a real process in Debug mode
/// with DevTools enabled, then connects to the DevTools named-pipe to drive input and
/// inspect the running UI.
/// <para>
/// Usage:
/// <code>
/// await using var app = await RealApp.StartAsync("Xui/Apps/TestApp/TestApp.Desktop.csproj");
/// var root = await app.InspectAsync();
/// await app.ClickAsync(root!.FindById("GridLayout")!.CenterX, root.FindById("GridLayout")!.CenterY);
/// var svg = await app.ScreenshotAsync();
/// </code>
/// </para>
/// </summary>
public sealed class RealApp : IAsyncDisposable
{
    private readonly Process process;
    private readonly DevToolsClient client;
    private readonly List<string> log = [];
    private readonly object logLock = new();
    private const int MaxLogLines = 200;

    private RealApp(Process process, DevToolsClient client)
    {
        this.process = process;
        this.client = client;
    }

    /// <summary>
    /// Launches a Xui app in Debug mode and waits for DevTools to be ready.
    /// </summary>
    /// <param name="projectPath">
    /// Path to the app's .csproj file. Can be absolute or relative to
    /// <paramref name="workingDirectory"/> (or the current directory when not specified).
    /// Example: <c>"Xui/Apps/TestApp/TestApp.Desktop.csproj"</c>
    /// </param>
    /// <param name="workingDirectory">
    /// Working directory for the process. Defaults to <see cref="Directory.GetCurrentDirectory"/>.
    /// </param>
    /// <param name="timeout">
    /// How long to wait for the DevTools pipe to become ready. Defaults to 90 seconds.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the process exits before printing <c>DEVTOOLS_READY:</c> or when
    /// connecting to the DevTools pipe fails.
    /// </exception>
    public static async Task<RealApp> StartAsync(
        string projectPath,
        string? workingDirectory = null,
        TimeSpan? timeout = null)
    {
        workingDirectory ??= Directory.GetCurrentDirectory();

        var fullPath = Path.IsPathRooted(projectPath)
            ? projectPath
            : Path.Combine(workingDirectory, projectPath);

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{fullPath}\" -c Debug",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory,
        };

        var process = new Process { StartInfo = psi };
        process.Start();

        var startupLog = new List<string>();

        // Drain stderr concurrently so build errors and crash output are always captured.
        var stderrTask = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    var line = await process.StandardError.ReadLineAsync();
                    if (line == null) break;
                    lock (startupLog) startupLog.Add($"[err] {line}");
                }
            }
            catch { }
        });

        using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(90));
        string? pipeName = null;
        try
        {
            while (true)
            {
                var line = await process.StandardOutput.ReadLineAsync(cts.Token);
                if (line == null) break;
                lock (startupLog) startupLog.Add(line);
                if (line.StartsWith("DEVTOOLS_READY:"))
                {
                    pipeName = line["DEVTOOLS_READY:".Length..];
                    break;
                }
            }
        }
        catch (OperationCanceledException) { }

        if (pipeName == null)
        {
            var exitInfo = process.HasExited ? $" Process exited with code {process.ExitCode}." : "";
            try { process.Kill(entireProcessTree: true); } catch { }
            process.Dispose();
            var logText = string.Join("\n", startupLog);
            throw new InvalidOperationException(
                $"Timed out waiting for DEVTOOLS_READY.{exitInfo}\n\n{logText}");
        }

        var client = new DevToolsClient(pipeName);
        try
        {
            await client.ConnectAsync();
        }
        catch (Exception ex)
        {
            try { process.Kill(entireProcessTree: true); } catch { }
            process.Dispose();
            throw new InvalidOperationException(
                $"App started but could not connect to DevTools pipe '{pipeName}': {ex.Message}");
        }

        var app = new RealApp(process, client);
        app.SeedLog(startupLog);

        // Continue draining stdout after DEVTOOLS_READY.
        _ = app.DrainAsync(process.StandardOutput);

        return app;
    }

    private void SeedLog(List<string> lines)
    {
        lock (logLock)
        {
            log.AddRange(lines);
            while (log.Count > MaxLogLines) log.RemoveAt(0);
        }
    }

    private async Task DrainAsync(StreamReader reader)
    {
        try
        {
            while (true)
            {
                var line = await reader.ReadLineAsync();
                if (line == null) break;
                lock (logLock)
                {
                    log.Add(line);
                    while (log.Count > MaxLogLines) log.RemoveAt(0);
                }
            }
        }
        catch { }
    }

    /// <summary>Returns the recent stdout/stderr output captured from the running app.</summary>
    public string GetLog()
    {
        lock (logLock) return string.Join("\n", log);
    }

    /// <summary>
    /// Returns true if the app process has exited unexpectedly.
    /// </summary>
    public bool HasCrashed => process.HasExited;

    /// <summary>
    /// Inspects the visual UI tree of the running app and returns the root <see cref="ViewNode"/>.
    /// Returns <c>null</c> when the result cannot be parsed.
    /// </summary>
    public async Task<ViewNode?> InspectAsync()
    {
        var result = await client.SendAsync("ui.inspect");
        if (result is JsonElement el)
        {
            var inspectResult = el.Deserialize<InspectResult>(DevToolsClient.JsonOptions);
            return inspectResult?.Root;
        }
        return null;
    }

    /// <summary>
    /// Takes an SVG screenshot of the running app and returns the SVG markup string.
    /// </summary>
    public async Task<string> ScreenshotAsync()
    {
        var result = await client.SendAsync("ui.screenshot");
        if (result is JsonElement el && el.TryGetProperty("svg", out var svg))
            return svg.GetString() ?? string.Empty;
        return string.Empty;
    }

    /// <summary>Sends a mouse click (down + up) at coordinates (<paramref name="x"/>, <paramref name="y"/>).</summary>
    public Task ClickAsync(float x, float y)
        => client.SendAsync("input.click", new { x, y });

    /// <summary>Sends a tap at coordinates (<paramref name="x"/>, <paramref name="y"/>).</summary>
    public Task TapAsync(float x, float y)
        => client.SendAsync("input.tap", new { x, y });

    /// <summary>
    /// Sends a pointer event. <paramref name="phase"/> must be one of
    /// <c>"start"</c>, <c>"move"</c>, <c>"end"</c>, or <c>"cancel"</c>.
    /// </summary>
    public Task PointerAsync(string phase, float x, float y, int index = 0)
        => client.SendAsync("input.pointer", new { phase, x, y, index });

    /// <summary>Forces an immediate redraw of the app window.</summary>
    public Task InvalidateAsync()
        => client.SendAsync("app.invalidate");

    /// <summary>
    /// Sets the identity label shown next to the AI pointer overlay
    /// (e.g. <c>"Claude, VSCode"</c>). Pass an empty string to clear.
    /// </summary>
    public Task IdentifyAsync(string label)
        => client.SendAsync("app.identify", new { label });

    /// <summary>
    /// Stops the running app process and disposes all resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        if (!process.HasExited)
        {
            try { process.Kill(entireProcessTree: true); } catch { }
        }
        await process.WaitForExitAsync().ConfigureAwait(false);
        process.Dispose();
    }

    /// <summary>
    /// Walks up from <paramref name="startPath"/> (typically the calling test file's directory)
    /// until it finds a directory that contains a <c>.sln</c> file, and returns that directory.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no solution directory is found before reaching the filesystem root.
    /// </exception>
    public static string FindSolutionRoot(string startPath)
    {
        var dir = Directory.Exists(startPath) ? startPath : Path.GetDirectoryName(startPath)!;
        while (true)
        {
            if (Directory.GetFiles(dir, "*.sln").Length > 0)
                return dir;
            var parent = Path.GetDirectoryName(dir);
            if (parent == null || parent == dir)
                throw new InvalidOperationException(
                    $"Could not find a solution directory by walking up from '{startPath}'.");
            dir = parent;
        }
    }
}

/// <summary>
/// Represents a node in the Xui view hierarchy as returned by <c>ui.inspect</c>.
/// </summary>
public record ViewNode(
    string Type,
    float X, float Y, float W, float H,
    float CenterX, float CenterY,
    bool Visible,
    string? Id,
    string? ClassName,
    ViewNode[] Children)
{
    /// <summary>
    /// Finds the first descendant (including self) whose <see cref="Id"/> matches
    /// <paramref name="id"/>. Returns <c>null</c> when not found.
    /// </summary>
    public ViewNode? FindById(string id)
    {
        if (Id == id) return this;
        foreach (var child in Children)
        {
            var found = child.FindById(id);
            if (found != null) return found;
        }
        return null;
    }

    /// <summary>
    /// Finds all descendants (including self) whose <see cref="Type"/> matches
    /// <paramref name="typeName"/>.
    /// </summary>
    public IEnumerable<ViewNode> FindAllByType(string typeName)
    {
        if (Type == typeName) yield return this;
        foreach (var child in Children)
            foreach (var node in child.FindAllByType(typeName))
                yield return node;
    }
}

file record InspectResult(ViewNode Root);

/// <summary>Named-pipe JSON-RPC client for the Xui DevTools protocol.</summary>
internal sealed class DevToolsClient : IDisposable
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private static readonly JsonSerializerOptions sendOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly NamedPipeClientStream pipe;
    private StreamWriter? writer;
    private StreamReader? reader;
    private int nextId = 1;

    public DevToolsClient(string pipeName)
        => this.pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

    public async Task ConnectAsync()
    {
        await pipe.ConnectAsync(10_000);
        writer = new StreamWriter(pipe, new UTF8Encoding(false), bufferSize: 1024, leaveOpen: true) { AutoFlush = true };
        reader = new StreamReader(pipe, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
    }

    /// <summary>Sends a JSON-RPC request and returns the result element, or null on void responses.</summary>
    public async Task<object?> SendAsync(string method, object? @params = null)
    {
        if (writer == null || reader == null)
            throw new InvalidOperationException("Not connected.");

        var id = nextId++;
        var json = JsonSerializer.Serialize(new { method, id, @params }, sendOptions);
        await writer.WriteLineAsync(json);

        var line = await reader.ReadLineAsync();
        if (line == null) throw new IOException("DevTools connection closed.");

        using var doc = JsonDocument.Parse(line);
        var root = doc.RootElement;

        if (root.TryGetProperty("error", out var err))
            throw new Exception($"RPC error {err.GetProperty("code").GetInt32()}: {err.GetProperty("message").GetString()}");

        if (root.TryGetProperty("result", out var result) && result.ValueKind != JsonValueKind.Null)
            return result.Clone();

        return null;
    }

    public void Dispose() => pipe.Dispose();
}
