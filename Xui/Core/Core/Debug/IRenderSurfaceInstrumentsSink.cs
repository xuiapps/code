namespace Xui.Core.Debug;

/// <summary>
/// Per-surface instrumentation sink. A window or native popup owns one instance;
/// its view tree receives it through <see cref="IViewInstrumentsSink"/>.
/// </summary>
public interface IRenderSurfaceInstrumentsSink : IViewInstrumentsSink, IDisposable
{
    /// <summary>Stable diagnostic name of the surface, such as its owning window type.</summary>
    string Name { get; }
    /// <summary>Begins one update frame for this render surface.</summary>
    void BeginFrame();
    /// <summary>Completes one update frame for this render surface.</summary>
    void EndFrame();
}
