namespace Xui.Core.UI;

/// <summary>
/// Specifies how a view should be aligned vertically within its layout bounds.
/// Used by parent containers to control vertical positioning along the block axis.
/// </summary>
public enum VerticalAlignment : byte
{
    /// <summary>
    /// Stretch to fill the full available vertical space.
    /// </summary>
    Stretch = 0,

    /// <summary>
    /// Align to the top edge (or start edge in top-down layouts).
    /// </summary>
    Top = 1,

    /// <summary>
    /// Center vertically within the available space.
    /// </summary>
    Middle = 2,

    /// <summary>
    /// Align to the bottom edge (or start edge in bottom-up layouts).
    /// </summary>
    Bottom = 3
}
