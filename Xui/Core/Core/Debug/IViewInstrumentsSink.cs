using Xui.Core.UI;

namespace Xui.Core.Debug;

/// <summary>
/// Instrumentation API exposed to a visual tree. A view receives one instance for
/// the render surface that owns its tree.
/// </summary>
public interface IViewInstrumentsSink
{
    /// <summary>Returns <c>true</c> if this sink should receive events at the given scope and level of detail.</summary>
    bool IsEnabled(Scope scope, LevelOfDetail lod);
    /// <summary>Reports structured traversal of a view in the specified scope.</summary>
    void TrackView(Scope scope, View view);
    /// <summary>Reports one text measurement and the elapsed underlying-context ticks.</summary>
    void TrackTextMeasure(long elapsedTicks);
    /// <summary>Writes a single log message to the sink.</summary>
    void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    /// <summary>Begins a named trace scope. Must be paired with <see cref="EndTrace"/>.</summary>
    void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message);
    /// <summary>Ends the most recently opened trace scope.</summary>
    void EndTrace();
}
