using Xui.Core.UI;

namespace Xui.Core.Debug;

/// <summary>Forwards a single render surface's events to its owning run-loop sink.</summary>
internal sealed class RenderSurfaceInstrumentsSink(IInstrumentsSink owner, string name) : IRenderSurfaceInstrumentsSink
{
    public string Name { get; } = name;

    public void BeginFrame() { }
    public void EndFrame() { }
    public bool IsEnabled(Scope scope, LevelOfDetail lod) => owner.IsEnabled(this, scope, lod);
    public void TrackView(Scope scope, View view) => owner.TrackView(this, scope, view);
    public void TrackTextMeasure(long elapsedTicks) => owner.TrackTextMeasure(this, elapsedTicks);
    public void Log(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message) => owner.Log(this, scope, lod, message);
    public void BeginTrace(Scope scope, LevelOfDetail lod, ReadOnlySpan<char> message) => owner.BeginTrace(this, scope, lod, message);
    public void EndTrace() => owner.EndTrace(this);
    public void Dispose() { }
}
