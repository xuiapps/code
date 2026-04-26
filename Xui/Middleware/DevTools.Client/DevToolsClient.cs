using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xui.Middleware.DevTools.Client;

/// <summary>Named-pipe JSON-RPC client for the Xui DevTools protocol.</summary>
public sealed class DevToolsClient : IDisposable
{
    public static readonly JsonSerializerOptions JsonOptions = new()
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

    /// <param name="clientName">
    /// Optional display name sent to the app immediately after connecting (e.g. "Claude" or
    /// "E2E Automation"). When provided, the app shows an overlay label on the next frame.
    /// </param>
    public async Task ConnectAsync(string? clientName = null)
    {
        await pipe.ConnectAsync(10_000);
        writer = new StreamWriter(pipe, new UTF8Encoding(false), bufferSize: 1024, leaveOpen: true) { AutoFlush = true };
        reader = new StreamReader(pipe, new UTF8Encoding(false), detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);

        if (clientName != null)
            await SendAsync("app.identify", new { label = clientName });
    }

    /// <summary>Sends a JSON-RPC request and returns the result element, or null on void responses.</summary>
    public async Task<object?> SendAsync(string method, object? @params = null)
    {
        if (writer == null || reader == null)
            throw new InvalidOperationException("Not connected.");

        var id = nextId++;
        var json = JsonSerializer.Serialize(new { method, id, @params }, JsonOptions);
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
