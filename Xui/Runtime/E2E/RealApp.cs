using System.Diagnostics;
using System.Globalization;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    private readonly TestLog? testLog;
    private readonly List<string> log = [];
    private readonly object logLock = new();
    private readonly CancellationTokenSource drainCts = new();
    private Task? stdoutDrain;
    private Task? stderrDrain;
    private const int MaxLogLines = 200;

    private RealApp(Process process, DevToolsClient client, TestLog? testLog)
    {
        this.process = process;
        this.client = client;
        this.testLog = testLog;
    }

    /// <summary>
    /// Throws if the app process has crashed or a drain task has faulted.
    /// Called before every DevTools operation so errors surface immediately.
    /// </summary>
    private void ThrowIfFaulted()
    {
        if (stdoutDrain?.IsFaulted == true)
            throw new InvalidOperationException(
                $"stdout drain faulted: {stdoutDrain.Exception!.InnerException!.Message}\n\n{GetLog()}",
                stdoutDrain.Exception.InnerException);
        if (stderrDrain?.IsFaulted == true)
            throw new InvalidOperationException(
                $"stderr drain faulted: {stderrDrain.Exception!.InnerException!.Message}\n\n{GetLog()}",
                stderrDrain.Exception.InnerException);
        if (process.HasExited)
            throw new InvalidOperationException(
                $"App process exited unexpectedly with code {process.ExitCode}.\n\n{GetLog()}");
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
        TimeSpan? timeout = null,
        TestLog? testLog = null)
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

        using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(90));

        // Drain stderr concurrently so build errors and crash output are always captured.
        // We await this task before throwing on failure so the log is fully populated.
        // Uses the startup CTS so we can stop it before handing stderr to DrainAsync.
        var stderrCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
        var stderrTask = Task.Run(async () =>
        {
            try
            {
                while (!stderrCts.Token.IsCancellationRequested)
                {
                    var line = await process.StandardError.ReadLineAsync(stderrCts.Token);
                    if (line == null) break;
                    lock (startupLog) startupLog.Add($"[err] {line}");
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                lock (startupLog) startupLog.Add($"[err][drain] {ex.Message}");
            }
        });
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
            // Give stderr draining a moment to collect build/crash output before reporting.
            await Task.WhenAny(stderrTask, Task.Delay(500));
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

        // Stop the startup stderr drain before handing stderr to DrainAsync,
        // otherwise two concurrent ReadLineAsync calls on the same stream will throw.
        stderrCts.Cancel();
        await stderrTask;
        stderrCts.Dispose();

        var app = new RealApp(process, client, testLog);
        app.SeedLog(startupLog);
        testLog?.LogSection("App Started", $"Project: `{projectPath}`\nPipe: `{pipeName}`");

        // Continue draining stdout and stderr after DEVTOOLS_READY.
        app.stdoutDrain = app.DrainAsync(process.StandardOutput, app.drainCts.Token);
        app.stderrDrain = app.DrainAsync(process.StandardError, app.drainCts.Token);

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

    private async Task DrainAsync(StreamReader reader, CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(ct);
                if (line == null) break;
                lock (logLock)
                {
                    log.Add(line);
                    while (log.Count > MaxLogLines) log.RemoveAt(0);
                }
            }
        }
        catch (OperationCanceledException) { }
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
    /// Polls <see cref="InspectAsync"/> until the root node is a real window (not the
    /// <c>no-window</c> fallback). This accounts for the delay between the DevTools pipe
    /// becoming ready and the first window being fully created.
    /// </summary>
    /// <param name="timeout">Maximum wait time. Defaults to 10 seconds.</param>
    public async Task<ViewNode> WaitForWindowAsync(TimeSpan? timeout = null)
    {
        using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(10));
        while (!cts.Token.IsCancellationRequested)
        {
            ThrowIfFaulted();
            var root = await InspectAsync();
            if (root != null && root.Type != "no-window")
                return root;
            await Task.Delay(100, cts.Token);
        }
        throw new TimeoutException(
            $"Window did not become ready within the timeout period.\n\n{GetLog()}");
    }

    /// <summary>
    /// Polls the UI tree until an element with the given <paramref name="id"/> appears.
    /// Returns the root <see cref="ViewNode"/> of the tree that contains the element.
    /// Throws <see cref="TimeoutException"/> if the element does not appear in time.
    /// </summary>
    /// <param name="id">The element ID to wait for.</param>
    /// <param name="timeout">Maximum wait time. Defaults to 10 seconds.</param>
    public async Task<ViewNode> WaitForElementAsync(string id, TimeSpan? timeout = null)
    {
        using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(10));
        while (!cts.Token.IsCancellationRequested)
        {
            ThrowIfFaulted();
            var root = await InspectAsync();
            if (root != null && root.FindById(id) != null)
                return root;
            await Task.Delay(100, cts.Token);
        }
        throw new TimeoutException(
            $"Element with id '{id}' did not appear within the timeout period.\n\n{GetLog()}");
    }

    /// <summary>
    /// Inspects the visual UI tree of the running app and returns the root <see cref="ViewNode"/>.
    /// Returns <c>null</c> when the result cannot be parsed.
    /// </summary>
    public async Task<ViewNode?> InspectAsync()
    {
        ThrowIfFaulted();
        var result = await client.SendAsync("ui.inspect");
        if (result is JsonElement el)
        {
            var inspectResult = el.Deserialize<InspectResult>(DevToolsClient.JsonOptions);
            var root = inspectResult?.Root;
            testLog?.LogSection("ui.inspect", root != null
                ? $"```xml\n{root.ToXml()}\n```"
                : "_Result was null_");
            return root;
        }
        testLog?.LogSection("ui.inspect", "_No JSON result returned_");
        return null;
    }

    /// <summary>
    /// Takes an SVG screenshot of the running app and returns the SVG markup string.
    /// </summary>
    public async Task<string> ScreenshotAsync()
    {
        ThrowIfFaulted();
        var result = await client.SendAsync("ui.screenshot");
        if (result is JsonElement el && el.TryGetProperty("svg", out var svg))
        {
            var svgStr = svg.GetString() ?? string.Empty;
            testLog?.LogSection("ui.screenshot", svgStr);
            return svgStr;
        }
        testLog?.LogSection("ui.screenshot", "_No SVG returned_");
        return string.Empty;
    }

    /// <summary>Sends a mouse click (down + up) at coordinates (<paramref name="x"/>, <paramref name="y"/>).</summary>
    public Task ClickAsync(NFloat x, NFloat y)
    {
        ThrowIfFaulted();
        testLog?.LogSection("input.click", $"x={x}, y={y}");
        return client.SendAsync("input.click", new { x = (float)x, y = (float)y });
    }

    /// <summary>Sends a tap at coordinates (<paramref name="x"/>, <paramref name="y"/>).</summary>
    public Task TapAsync(NFloat x, NFloat y)
    {
        ThrowIfFaulted();
        testLog?.LogSection("input.tap", $"x={x}, y={y}");
        return client.SendAsync("input.tap", new { x = (float)x, y = (float)y });
    }

    /// <summary>
    /// Sends a pointer event. <paramref name="phase"/> must be one of
    /// <c>"start"</c>, <c>"move"</c>, <c>"end"</c>, or <c>"cancel"</c>.
    /// </summary>
    public Task PointerAsync(string phase, NFloat x, NFloat y, int index = 0)
    {
        ThrowIfFaulted();
        return client.SendAsync("input.pointer", new { phase, x = (float)x, y = (float)y, index });
    }

    /// <summary>Forces an immediate redraw of the app window.</summary>
    public Task InvalidateAsync()
    {
        ThrowIfFaulted();
        return client.SendAsync("app.invalidate");
    }

    /// <summary>
    /// Sets the identity label shown next to the AI pointer overlay
    /// (e.g. <c>"Claude, VSCode"</c>). Pass an empty string to clear.
    /// </summary>
    public Task IdentifyAsync(string label)
    {
        ThrowIfFaulted();
        return client.SendAsync("app.identify", new { label });
    }

    /// <summary>
    /// Stops the running app process and disposes all resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        drainCts.Cancel();
        client.Dispose();
        if (!process.HasExited)
        {
            try { process.Kill(entireProcessTree: true); } catch { }
        }
        await process.WaitForExitAsync().ConfigureAwait(false);

        var appLog = GetLog();
        testLog?.LogSection("App Log", string.IsNullOrWhiteSpace(appLog)
            ? "_empty_"
            : $"```\n{appLog}\n```");

        process.Dispose();
        drainCts.Dispose();
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
/// Coordinates (<see cref="X"/>, <see cref="Y"/>, <see cref="CenterX"/>, <see cref="CenterY"/>,
/// etc.) are <see cref="float"/> because they are deserialized directly from the JSON-RPC
/// response. Pass them to <see cref="RealApp.ClickAsync"/> and related methods, which accept
/// <see cref="NFloat"/> and handle the widening conversion automatically.
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

    /// <summary>
    /// Returns an XML representation of this view subtree for diagnostic logging.
    /// </summary>
    public string ToXml(int indent = 0)
    {
        var sb = new StringBuilder();
        WriteXml(sb, indent);
        return sb.ToString();
    }

    private void WriteXml(StringBuilder sb, int indent)
    {
        var pad = new string(' ', indent * 2);
        sb.Append(pad).Append('<').Append(Type);
        if (Id != null) sb.Append($" id=\"{Id}\"");
        if (ClassName != null) sb.Append($" class=\"{ClassName}\"");
        sb.Append($" x=\"{X.ToString(CultureInfo.InvariantCulture)}\"");
        sb.Append($" y=\"{Y.ToString(CultureInfo.InvariantCulture)}\"");
        sb.Append($" w=\"{W.ToString(CultureInfo.InvariantCulture)}\"");
        sb.Append($" h=\"{H.ToString(CultureInfo.InvariantCulture)}\"");
        if (!Visible) sb.Append(" visible=\"false\"");
        if (Children.Length == 0)
        {
            sb.AppendLine(" />");
        }
        else
        {
            sb.AppendLine(">");
            foreach (var child in Children)
                child.WriteXml(sb, indent + 1);
            sb.Append(pad).Append("</").Append(Type).AppendLine(">");
        }
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

/// <summary>
/// Records all E2E test operations and writes a diagnostic markdown file.
/// <para>
/// Usage:
/// <code>
/// await using var log = new TestLog("Can_Navigate_To_GridLayout_And_Back");
/// await using var app = await RealApp.StartAsync(project, testLog: log);
/// // ... test steps ...
/// </code>
/// The <c>.md</c> file is written to <c>Xui/Tests/E2E/TestResults/</c> on dispose,
/// containing all DevTools commands, view tree XML, and the app's stdout/stderr log.
/// </para>
/// </summary>
public sealed class TestLog : IAsyncDisposable
{
    private readonly string testName;
    private readonly string outputDir;
    private readonly StringBuilder content = new();
    private readonly Stopwatch stopwatch = Stopwatch.StartNew();

    public TestLog(string testName, [CallerFilePath] string callerPath = "")
    {
        this.testName = testName;
        outputDir = Path.Combine(
            Path.GetDirectoryName(callerPath) ?? ".",
            "TestResults");
        content.AppendLine($"# {testName}");
        content.AppendLine();
        content.AppendLine($"Run at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        content.AppendLine();
    }

    public void LogSection(string title, string body)
    {
        var elapsed = stopwatch.Elapsed;
        content.AppendLine($"## [{elapsed.TotalSeconds:F2}s] {title}");
        content.AppendLine();
        content.AppendLine(body);
        content.AppendLine();
    }

    public ValueTask DisposeAsync()
    {
        Directory.CreateDirectory(outputDir);
        var path = Path.Combine(outputDir, $"{testName}.md");
        File.WriteAllText(path, content.ToString());
        return default;
    }
}
