namespace Xui.Core.Canvas;

/// <summary>
/// Represents text layout metrics that depend on the specific string being measured.
/// Includes actual bounding box and visual advance width.
/// </summary>
public readonly struct LineMetrics
{
    /// <summary>
    /// Advance width of the string, including kerning and shaping.
    /// This is how far the cursor should advance after drawing the text.
    /// </summary>
    public readonly nfloat Width;

    /// <summary>
    /// Distance from the text origin to the furthest left pixel of any glyph.
    /// May be negative for italic or overhanging glyphs.
    /// </summary>
    public readonly nfloat ActualBoundingBoxLeft;

    /// <summary>
    /// Distance from the text origin to the furthest right pixel of any glyph.
    /// </summary>
    public readonly nfloat ActualBoundingBoxRight;

    /// <summary>
    /// Distance from the baseline to the topmost pixel of any glyph.
    /// </summary>
    public readonly nfloat ActualBoundingBoxAscent;

    /// <summary>
    /// Distance from the baseline to the bottommost pixel of any glyph.
    /// </summary>
    public readonly nfloat ActualBoundingBoxDescent;

    /// <summary>
    /// Full height of the visual bounding box (ascent + descent).
    /// </summary>
    public nfloat ActualHeight => ActualBoundingBoxAscent + ActualBoundingBoxDescent;

    /// <summary>
    /// Initializes a new <see cref="LineMetrics"/> instance.
    /// </summary>
    /// <param name="width">Advance width of the rendered text.</param>
    /// <param name="left">Furthest left pixel offset relative to the origin.</param>
    /// <param name="right">Furthest right pixel offset relative to the origin.</param>
    /// <param name="ascent">Maximum pixel height above the baseline.</param>
    /// <param name="descent">Maximum pixel depth below the baseline.</param>
    public LineMetrics(nfloat width, nfloat left, nfloat right, nfloat ascent, nfloat descent)
    {
        Width = width;
        ActualBoundingBoxLeft = left;
        ActualBoundingBoxRight = right;
        ActualBoundingBoxAscent = ascent;
        ActualBoundingBoxDescent = descent;
    }
}