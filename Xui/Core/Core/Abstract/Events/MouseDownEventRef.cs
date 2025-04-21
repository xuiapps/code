using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event indicating that a mouse button
/// was pressed at a given position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It may be used to initiate focus, dragging, selection, or other pointer interactions.
/// </remarks>
public ref struct MouseDownEventRef
{
    /// <summary>
    /// The position of the mouse pointer at the time of the event,
    /// in logical window coordinates.
    /// </summary>
    public Point Position;

    /// <summary>
    /// The mouse button that was pressed.
    /// </summary>
    public MouseButton Button;
}
