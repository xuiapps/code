namespace Xui.Core.Debug;

/// <summary>
/// Per-run-loop instrumentation sink that receives fully-formatted messages.
/// Thread-aligned — must not be shared across run loops or threads.
/// </summary>
public interface IInstrumentsSink : IDisposable
{
    /// <summary>Returns <c>true</c> if this sink should receive events at the given scope and level of detail.</summary>
    bool IsEnabled(Scope scope, LevelOfDetail lod);
    /// <summary>Writes a single log message to the sink.</summary>
    void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    /// <summary>Begins a named trace scope. Must be paired with <see cref="EndTrace"/>.</summary>
    void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    /// <summary>Ends the most recently opened trace scope.</summary>
    void EndTrace();
}
