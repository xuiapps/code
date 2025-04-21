using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a single touch point within a multi-touch input event.
/// </summary>
/// <remarks>
/// This structure contains identifying and positional data for a specific finger
/// or contact point on a touch surface. Multiple <see cref="Touch"/> instances may be
/// reported in a single event when handling gestures or complex touch interactions.
/// </remarks>
public struct Touch
{
    /// <summary>
    /// A unique index identifying this touch point during its lifetime.
    /// Typically assigned by the platform and reused after release.
    /// </summary>
    public long Index;

    /// <summary>
    /// The current position of the touch in logical window coordinates.
    /// </summary>
    public Point Position;

    /// <summary>
    /// The estimated contact radius of the touch, in logical units.
    /// Used for gesture recognition or pressure emulation.
    /// </summary>
    public nfloat Radius;

    /// <summary>
    /// The current phase of the touch (e.g., began, moved, ended).
    /// </summary>
    public TouchPhase Phase;
}
