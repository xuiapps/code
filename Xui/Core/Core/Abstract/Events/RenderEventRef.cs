using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level render event dispatched after a timing frame,
/// indicating that views should perform layout and rendering operations within a given region.
/// </summary>
/// <remarks>
/// This event follows a <see cref="FrameEventRef"/> and marks the phase during which
/// views are expected to perform their <c>Measure</c>, <c>Arrange</c>, and <c>Render</c> passes.
/// It is emitted by the <c>Actual</c> window and forwarded to the <c>Abstract</c> layer.
/// </remarks>
public ref struct RenderEventRef
{
    /// <summary>
    /// The region of the screen or surface that should be re-rendered.
    /// </summary>
    public Rect Rect;

    /// <summary>
    /// Timing information associated with this frame, typically used for animations.
    /// </summary>
    public FrameEventRef Frame;

    /// <summary>
    /// The frame-bound drawing context for this render pass. It must not be retained
    /// after the event returns to the render surface.
    /// </summary>
    public IContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderEventRef"/> struct
    /// with the given invalidation region and frame timing data.
    /// </summary>
    /// <param name="rect">The region to be rendered.</param>
    /// <param name="frame">The timing information for this frame.</param>
    /// <param name="context">The drawing context for this render pass.</param>
    public RenderEventRef(Rect rect, FrameEventRef frame, IContext context)
    {
        Frame = frame;
        Rect = rect;
        Context = context;
    }
}
