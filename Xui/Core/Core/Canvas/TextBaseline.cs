namespace Xui.Core.Canvas;

/// <summary>
/// Specifies the vertical alignment of text relative to the drawing position.
/// Mirrors the <c>textBaseline</c> property in the HTML5 Canvas 2D API.
/// </summary>
public enum TextBaseline
{
    /// <summary>
    /// The top of the em box is aligned with the drawing position.
    /// </summary>
    Top,

    /// <summary>
    /// The hanging baseline is aligned with the drawing position (used in some scripts like Devanagari).
    /// </summary>
    Hanging,

    /// <summary>
    /// The middle of the text (roughly half the em height) is aligned with the drawing position.
    /// </summary>
    Middle,

    /// <summary>
    /// The alphabetic baseline (default for Latin scripts) is aligned with the drawing position.
    /// </summary>
    Alphabetic,

    /// <summary>
    /// The ideographic baseline is aligned with the drawing position (used in scripts like Chinese or Japanese).
    /// </summary>
    Ideographic,

    /// <summary>
    /// The bottom of the em box is aligned with the drawing position.
    /// </summary>
    Bottom
}
