using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level input event generated by a scroll wheel or trackpad gesture,
/// indicating a change in scroll position.
/// </summary>
/// <remarks>
/// This event is dispatched by the <c>Actual</c> window and forwarded
/// to the <c>Abstract</c> layer for routing through the view hierarchy.
/// It may be used to scroll content, zoom views, or trigger kinetic effects,
/// depending on platform and modifier keys.
/// </remarks>
public ref struct ScrollWheelEventRef
{
    /// <summary>
    /// The scroll delta, typically measured in logical units per axis.
    /// Positive Y values usually indicate upward scrolling.
    /// </summary>
    public Vector Delta;
}
