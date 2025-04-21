namespace Xui.Core.UI;

/// <summary>
/// Specifies how a view should be aligned horizontally within its layout bounds.
/// Used by parent containers during layout to position the view along the inline axis.
/// </summary>
public enum HorizontalAlignment : byte
{
    /// <summary>
    /// Stretch to fill the full available horizontal space.
    /// </summary>
    Stretch = 0,

    /// <summary>
    /// Align to the left edge (or start edge in LTR layouts).
    /// </summary>
    Left = 1,

    /// <summary>
    /// Center horizontally within the available space.
    /// </summary>
    Center = 2,

    /// <summary>
    /// Align to the right edge (or start edge in RTL layouts).
    /// </summary>
    Right = 3
}
