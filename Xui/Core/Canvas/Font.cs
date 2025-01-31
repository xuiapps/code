namespace Xui.Core.Canvas;

/// <summary>
/// Subset of web font.
/// https://developer.mozilla.org/en-US/docs/Web/CSS/font
/// 
/// FontFamily is a single font instead of collection, with no fall-back on character miss.
/// FontSize, LineHeight is defined in user space, no other options.
/// FontWeight
/// </summary>
public ref struct Font
{
    public ReadOnlySpan<string> FontFamily;
    public nfloat FontSize;
    public FontStyle FontStyle;

    /// <summary>
    /// Web-like font-weights:
    /// <code>
    /// font-weight: 100;
    /// font-weight: 200;
    /// font-weight: 300;
    /// font-weight: 400; /* normal */
    /// font-weight: 500;
    /// font-weight: 600;
    /// font-weight: 700; /* bold */
    /// font-weight: 800;
    /// font-weight: 900;
    /// </code>
    /// </summary>
    public nfloat FontWeight;

    public nfloat LineHeight;
}
