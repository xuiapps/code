namespace Xui.Core.UI;

/// <summary>
/// Indicates the semantic inline direction of content, such as left-to-right or right-to-left.
/// Used to resolve text alignment, layout flow, and mirroring behavior.
/// </summary>
public enum Direction : byte
{
    /// <summary>
    /// Inherit direction from the parent view.
    /// </summary>
    Inherit = 0,

    /// <summary>
    /// Left-to-right flow (default for Western languages).
    /// </summary>
    LeftToRight = 1,

    /// <summary>
    /// Right-to-left flow (used in Arabic, Hebrew, etc).
    /// </summary>
    RightToLeft = 2
}