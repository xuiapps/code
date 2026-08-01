using Xui.Core.UI;

namespace Xui.Core.Debug;

/// <summary>
/// Per-run-loop instrumentation sink that receives fully-formatted messages.
/// Thread-aligned — must not be shared across run loops or threads.
/// </summary>
public interface IInstrumentsSink : IViewInstrumentsSink, IDisposable
{
    /// <summary>Creates a dedicated sink for one window, popup, or other render surface.</summary>
    IRenderSurfaceInstrumentsSink CreateRenderSurface(string name) => new RenderSurfaceInstrumentsSink(this, name);

    /// <summary>Writes a surface event while retaining its source identity for aggregation.</summary>
    bool IsEnabled(IRenderSurfaceInstrumentsSink surface, Scope scope, LevelOfDetail lod) => IsEnabled(scope, lod);
    /// <inheritdoc cref="IViewInstrumentsSink.TrackView"/>
    void TrackView(IRenderSurfaceInstrumentsSink surface, Scope scope, View view) => TrackView(scope, view);
    /// <inheritdoc cref="IViewInstrumentsSink.TrackTextMeasure"/>
    void TrackTextMeasure(IRenderSurfaceInstrumentsSink surface, long elapsedTicks) => TrackTextMeasure(elapsedTicks);
    /// <inheritdoc cref="IViewInstrumentsSink.Log"/>
    void Log(IRenderSurfaceInstrumentsSink surface, Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message) => Log(scope, lod, message);
    /// <inheritdoc cref="IViewInstrumentsSink.BeginTrace"/>
    void BeginTrace(IRenderSurfaceInstrumentsSink surface, Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message) => BeginTrace(scope, lod, message);
    /// <inheritdoc cref="IViewInstrumentsSink.EndTrace"/>
    void EndTrace(IRenderSurfaceInstrumentsSink surface) => EndTrace();
}
