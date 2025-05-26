using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Composite structure combining string-specific and font-specific text metrics.
/// Used for precise layout, alignment, and rendering across all platforms.
/// </summary>
public readonly struct TextMetrics
{
    /// <summary>
    /// Metrics specific to the given text string (shaping, kerning, bounding box).
    /// </summary>
    public readonly LineMetrics Line;

    /// <summary>
    /// Metrics derived from the current font and size, independent of input string.
    /// </summary>
    public readonly FontMetrics Font;

    /// <summary>
    /// Creates a new composite text metrics structure.
    /// </summary>
    public TextMetrics(LineMetrics line, FontMetrics font)
    {
        Line = line;
        Font = font;
    }

    /// <summary>
    /// Returns the visual bounding box of the text string, relative to the origin.
    /// X may be negative for glyphs that extend left of the starting point.
    /// </summary>
    public Rect BoundingBox => new Rect(
        x: Line.ActualBoundingBoxLeft,
        y: -Font.FontBoundingBoxAscent,
        width: Line.ActualBoundingBoxRight - Line.ActualBoundingBoxLeft,
        height: Font.FontBoundingBoxAscent + Font.FontBoundingBoxDescent
    );

    public Size Size => new Size(this.Line.Width, this.Font.Height);

    /// <summary>
    /// Implicitly converts the text metrics to a <see cref="Size"/>, using the
    /// advance width and total height of the string. The width corresponds to
    /// how far the cursor should advance, not the visual width.
    /// </summary>
    public static implicit operator Size(TextMetrics metrics) =>
        new Size(metrics.Line.Width, metrics.Font.Height);
}
