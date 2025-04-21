namespace Xui.Core.UI;

/// <summary>
/// Specifies the orientation and flow direction of text and block layout.
/// Affects which axis is considered "block" and "inline".
/// </summary>
public enum WritingMode : byte
{
    /// <summary>
    /// Inherit direction from the parent view.
    /// </summary>
    Inherit,

    /// <summary>
    /// Horizontal writing mode. Text flows left-to-right, lines stack top-to-bottom.
    /// </summary>
    HorizontalTB,

    /// <summary>
    /// Vertical writing mode. Lines stack right-to-left, text flows top-to-bottom.
    /// </summary>
    VerticalRL,

    /// <summary>
    /// Vertical writing mode. Lines stack left-to-right, text flows top-to-bottom.
    /// </summary>
    VerticalLR,

    /// <summary>
    /// Sideways vertical mode. Lines stack right-to-left, but glyphs are rotated to remain horizontal.
    /// </summary>
    SidewaysRL,

    /// <summary>
    /// Sideways vertical mode. Lines stack left-to-right, with horizontal glyph orientation.
    /// </summary>
    SidewaysLR
}
