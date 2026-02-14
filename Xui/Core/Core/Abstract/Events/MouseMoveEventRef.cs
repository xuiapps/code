using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event indicating that the mouse pointer
/// has moved to a new position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It is typically used to trigger hover effects, cursor updates,
/// or to track dragging or gesture movement.
/// </remarks>
public ref struct MouseMoveEventRef
{
    /// <summary>
    /// The current position of the mouse pointer in logical window coordinates.
    /// </summary>
    public Point Position;

    /// <summary>
    /// Optional text measure context for hit-testing text positions.
    /// May be null on platforms that do not provide one.
    /// </summary>
    public ITextMeasureContext? TextMeasure;
}
