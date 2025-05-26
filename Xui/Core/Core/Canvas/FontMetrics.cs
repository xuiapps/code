namespace Xui.Core.Canvas;

/// <summary>
/// Represents font-defined metrics used for vertical alignment and em box sizing.
/// These values are constant for a given font and size.
/// </summary>
public readonly struct FontMetrics
{
    /// <summary>
    /// Maximum ascent defined by the font (not necessarily used glyphs).
    /// </summary>
    public readonly nfloat FontBoundingBoxAscent;

    /// <summary>
    /// Maximum descent defined by the font.
    /// </summary>
    public readonly nfloat FontBoundingBoxDescent;

    /// <summary>
    /// Height from baseline to top of the em square.
    /// Recommended for app layout.
    /// </summary>
    public readonly nfloat EmHeightAscent;

    /// <summary>
    /// Height from baseline to bottom of the em square.
    /// Recommended for app layout.
    /// </summary>
    public readonly nfloat EmHeightDescent;

    /// <summary>
    /// Position of the alphabetic baseline (typically 0, used for alignment).
    /// </summary>
    public readonly nfloat AlphabeticBaseline;

    /// <summary>
    /// Vertical position of the hanging baseline (used in scripts like Devanagari).
    /// </summary>
    public readonly nfloat HangingBaseline;

    /// <summary>
    /// Vertical position of the ideographic baseline (used in CJK layout).
    /// </summary>
    public readonly nfloat IdeographicBaseline;

    /// <summary>
    /// Total height of the font bounding box.
    /// </summary>
    public nfloat Height => this.EmHeightAscent + this.EmHeightDescent;

    /// <summary>
    /// Initializes a new <see cref="FontMetrics"/> instance.
    /// </summary>
    /// <param name="fontAscent">Maximum vertical extent above the baseline from the font’s bounding box.</param>
    /// <param name="fontDescent">Maximum vertical extent below the baseline from the font’s bounding box.</param>
    /// <param name="emAscent">Ascent to the top of the em square.</param>
    /// <param name="emDescent">Descent to the bottom of the em square.</param>
    /// <param name="alphabeticBaseline">Position of the alphabetic baseline (typically 0).</param>
    /// <param name="hangingBaseline">Position of the hanging baseline for scripts that use it.</param>
    /// <param name="ideographicBaseline">Position of the ideographic baseline for CJK layout.</param>
    public FontMetrics(
        nfloat fontAscent,
        nfloat fontDescent,
        nfloat emAscent,
        nfloat emDescent,
        nfloat alphabeticBaseline,
        nfloat hangingBaseline,
        nfloat ideographicBaseline)
    {
        FontBoundingBoxAscent = fontAscent;
        FontBoundingBoxDescent = fontDescent;
        EmHeightAscent = emAscent;
        EmHeightDescent = emDescent;
        AlphabeticBaseline = alphabeticBaseline;
        HangingBaseline = hangingBaseline;
        IdeographicBaseline = ideographicBaseline;
    }
}
