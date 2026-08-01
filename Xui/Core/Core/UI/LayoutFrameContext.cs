using Xui.Core.Canvas;
using Xui.Core.Debug;

namespace Xui.Core.UI;

/// <summary>
/// Immutable services and timing shared by every view visited during one render-surface update.
/// </summary>
public readonly struct LayoutFrameContext
{
    /// <summary>Initializes a frame context.</summary>
    public LayoutFrameContext(
        TimeSpan previousTime,
        TimeSpan currentTime,
        IMeasureContext? measureContext,
        IContext? renderContext,
        InstrumentsAccessor instruments)
    {
        PreviousTime = previousTime;
        CurrentTime = currentTime;
        MeasureContext = measureContext;
        RenderContext = renderContext;
        Instruments = instruments;
    }

    /// <summary>Time of the preceding frame.</summary>
    public TimeSpan PreviousTime { get; }
    /// <summary>Time of this frame.</summary>
    public TimeSpan CurrentTime { get; }
    /// <summary>Measurement services for this render surface.</summary>
    public IMeasureContext? MeasureContext { get; }
    /// <summary>Rendering context for this render surface.</summary>
    public IContext? RenderContext { get; }
    /// <summary>Instrumentation associated with this render surface.</summary>
    public InstrumentsAccessor Instruments { get; }
}
