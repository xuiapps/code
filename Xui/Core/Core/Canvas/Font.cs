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
public ref struct Font
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
    public nfloat FontWeight;

    /// <summary>
    /// The line height in user-space units. Controls vertical spacing between lines.
    /// </summary>
    public nfloat LineHeight;
}
