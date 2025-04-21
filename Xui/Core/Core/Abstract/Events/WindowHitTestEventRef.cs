using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

/// <summary>
/// Represents a platform-level hit test event that allows the application
/// to define how a point within a custom window frame should be interpreted
/// (e.g., as a draggable title bar, resize border, or transparent region).
/// </summary>
/// <remarks>
/// This event is dispatched when the user interacts with the non-client area
/// of a custom-framed window (such as the emulator window or a styled installer).
/// It is primarily used on platforms like Windows that support fine-grained
/// window frame interaction via hit testing.
///
/// On macOS and other platforms where native support is unavailable,
/// this event may be ignored or implemented through custom code.
/// </remarks>
public ref struct WindowHitTestEventRef
{
    /// <summary>
    /// The location of the hit test, in logical window coordinates.
    /// </summary>
    public Point Point;

    /// <summary>
    /// The bounds of the entire window, in logical coordinates.
    /// This may be used to determine edge proximity for resizing logic.
    /// </summary>
    public Rect Window;

    /// <summary>
    /// The result of the hit test, set by the application to control
    /// how the system should interpret the hit location.
    /// </summary>
    public WindowArea Area;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowHitTestEventRef"/> struct.
    /// </summary>
    /// <param name="point">The location of the pointer input.</param>
    /// <param name="window">The bounds of the target window.</param>
    public WindowHitTestEventRef(Point point, Rect window)
    {
        Point = point;
        Window = window;
        Area = WindowArea.Default;
    }

    /// <summary>
    /// Describes the purpose or function of a region within a window.
    /// </summary>
    public enum WindowArea : uint
    {
        /// <summary>
        /// The area is unspecified; the platform should handle it normally.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The area is transparent to hit testing and should not trigger drag or resize.
        /// </summary>
        Transparent,

        /// <summary>
        /// The area represents the client region of the window (normal content).
        /// </summary>
        Client,

        /// <summary>
        /// The area represents a draggable title bar region.
        /// </summary>
        Title,

        /// <summary>Top-left resize corner.</summary>
        BorderTopLeft,

        /// <summary>Top edge resize border.</summary>
        BorderTop,

        /// <summary>Top-right resize corner.</summary>
        BorderTopRight,

        /// <summary>Right edge resize border.</summary>
        BorderRight,

        /// <summary>Bottom-right resize corner.</summary>
        BorderBottomRight,

        /// <summary>Bottom edge resize border.</summary>
        BorderBottom,

        /// <summary>Bottom-left resize corner.</summary>
        BorderBottomLeft,

        /// <summary>Left edge resize border.</summary>
        BorderLeft
    }
}
