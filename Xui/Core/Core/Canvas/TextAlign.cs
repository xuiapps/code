namespace Xui.Core.Canvas;

/// <summary>
/// Specifies the horizontal alignment of text relative to the drawing position.
/// Mirrors the <c>textAlign</c> property in the HTML5 Canvas 2D API.
/// </summary>
public enum TextAlign
{
    /// <summary>
    /// Aligns text to the start of the writing direction (left for LTR, right for RTL).
    /// </summary>
    Start,

    /// <summary>
    /// Aligns text to the end of the writing direction (right for LTR, left for RTL).
    /// </summary>
    End,

    /// <summary>
    /// Aligns text to the left, regardless of writing direction.
    /// </summary>
    Left,

    /// <summary>
    /// Aligns text to the right, regardless of writing direction.
    /// </summary>
    Right,

    /// <summary>
    /// Centers the text horizontally around the drawing position.
    /// </summary>
    Center
}
