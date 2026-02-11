namespace Xui.Core.Debug;

/// <summary>
/// Per-run-loop instrumentation sink that receives fully-formatted messages.
/// Thread-aligned — must not be shared across run loops or threads.
/// </summary>
public interface IInstrumentsSink : IDisposable
{
    bool IsEnabled(Scope scope, LevelOfDetail lod);
    void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    void EndTrace();
}
