namespace Xui.Core.Canvas;

/// <summary>
/// Describes a font used for text layout and rendering, similar to the CSS font model.
/// </summary>
/// <remarks>
/// This type mirrors a simplified subset of CSS font properties as documented at:
/// https://developer.mozilla.org/en-US/docs/Web/CSS/font
///
/// <para><b>Key differences:</b></para>
/// <list type="bullet">
/// <item><description><see cref="FontFamily"/> refers to a single font name only. No fallback list or character substitution is provided.</description></item>
/// <item><description><see cref="FontSize"/> and <see cref="LineHeight"/> are specified in user-space units (e.g., pixels or points).</description></item>
/// <item><description>Font weight is numeric and roughly corresponds to CSS values between 100–900.</description></item>
/// </list>
/// </remarks>
public ref partial struct Font
{
    /// <summary>
    /// The primary font family name. No fallback fonts are supported.
    /// </summary>
    public ReadOnlySpan<string> FontFamily;

    /// <summary>
    /// The font size in user-space units.
    /// </summary>
    public nfloat FontSize;

    /// <summary>
    /// The font style (normal, italic, oblique).
    /// </summary>
    public FontStyle FontStyle;

    // Not implemented: FontVariant

    /// <summary>
    /// The numeric weight of the font. Matches common web font weights:
    /// <code>
    /// 100 - Thin
    /// 200 - Extra Light
    /// 300 - Light
    /// 400 - Normal
    /// 500 - Medium
    /// 600 - Semi Bold
    /// 700 - Bold
    /// 800 - Extra Bold
    /// 900 - Black
    /// </code>
    /// </summary>
    public FontWeight FontWeight;

    /// <summary>
    /// The stretch or width of the font relative to its normal width (100%).
    /// Values correspond to common CSS/OpenType stretch percentages:
    /// <code>
    ///  50 - Ultra Condensed
    ///  62.5 - Extra Condensed
    ///  75 - Condensed
    ///  87.5 - Semi Condensed
    /// 100 - Normal
    /// 112.5 - Semi Expanded
    /// 125 - Expanded
    /// 150 - Extra Expanded
    /// 200 - Ultra Expanded
    /// </code>
    /// </summary>
    /// <remarks>
    /// Font stretch controls the horizontal expansion or compression of glyphs.
    /// A value of 100 indicates normal width. Values less than 100 indicate
    /// condensed fonts; values greater than 100 indicate expanded fonts.
    /// </remarks>
    public FontStretch FontStretch;

    /// <summary>
    /// The line height in user-space units. Controls vertical spacing between lines.
    /// </summary>
    public nfloat LineHeight;

    /// <summary>
    /// Initializes a <see cref="Font"/> with specified size and optional styling attributes.
    /// </summary>
    /// <param name="fontSize">The font size in user-space units (e.g., pixels).</param>
    /// <param name="fontFamily">The font family (optional; no fallback list).</param>
    /// <param name="fontWeight">The font weight (default: <see cref="FontWeight.Normal"/>).</param>
    /// <param name="fontStyle">The font style (default: <see cref="FontStyle.Normal"/>).</param>
    /// <param name="fontStretch">The font stretch (default: <see cref="FontStretch.Normal"/>).</param>
    /// <param name="lineHeight">The line height (default: 1.2 × font size).</param>
    public Font(
        nfloat fontSize,
        ReadOnlySpan<string> fontFamily = default,
        FontWeight? fontWeight = null,
        FontStyle? fontStyle = null,
        FontStretch? fontStretch = null,
        nfloat? lineHeight = null)
    {
        FontSize = fontSize;
        FontFamily = fontFamily;
        FontWeight = fontWeight ?? FontWeight.Normal;
        FontStyle = fontStyle ?? FontStyle.Normal;
        FontStretch = fontStretch ?? FontStretch.Normal;
        LineHeight = lineHeight ?? fontSize * 1.2f;
    }
}
