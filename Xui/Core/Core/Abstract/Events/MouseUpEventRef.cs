using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event indicating that a mouse button
/// was released at a given position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It is typically used to complete interactions such as clicks, drags,
/// or other pointer-driven gestures.
/// </remarks>
public ref struct MouseUpEventRef
{
    /// <summary>
    /// The position of the mouse pointer at the time of the event,
    /// in logical window coordinates.
    /// </summary>
    public Point Position;

    /// <summary>
    /// The mouse button that was released.
    /// </summary>
    public MouseButton Button;

    /// <summary>
    /// Optional text measure context for hit-testing text positions.
    /// May be null on platforms that do not provide one.
    /// </summary>
    public ITextMeasureContext? TextMeasure;
}
