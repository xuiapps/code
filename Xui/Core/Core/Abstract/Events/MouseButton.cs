namespace Xui.Core.Abstract.Events;

/// <summary>
/// Identifies a specific mouse button involved in a pointer event.
/// </summary>
public enum MouseButton
{
    /// <summary>
    /// The left mouse button, typically used for primary actions like selection or dragging.
    /// </summary>
    Left,

    /// <summary>
    /// The right mouse button, typically used for context menus or alternate actions.
    /// </summary>
    Right,

    /// <summary>
    /// Any other mouse button, such as middle-click or additional buttons on advanced mice.
    /// </summary>
    Other
}
