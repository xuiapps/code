namespace Xui.Core.Canvas;

/// <summary>
/// Web-like font style:
/// https://developer.mozilla.org/en-US/docs/Web/CSS/font-style
/// 
/// Without the global styles:
/// font-style: inherit;
/// font-style: initial;
/// font-style: revert;
/// font-style: revert-layer;
/// font-style: unset;
/// </summary>
public struct FontStyle
{
    private readonly bool italic;
    private readonly bool oblique;
    private readonly nfloat obliqueAngle;

    [DebuggerStepThrough]
    public FontStyle(bool italic, bool oblique, nfloat obliqueAngle)
    {
        this.italic = italic;
        this.oblique = oblique;
        this.obliqueAngle = obliqueAngle;
    }

    public bool IsItalic => this.italic;

    public bool IsOblique => this.oblique;

    /// <summary>
    /// Oblique angle in degrees from -90 to 90.
    /// </summary>
    public nfloat ObliqueAngle => this.obliqueAngle;

    public static FontStyle Normal => new FontStyle(false, false, 14f);

    public static FontStyle Italic => new FontStyle(true, false, 14f);

    public static FontStyle Oblique => new FontStyle(false, true, 14f);

    /// <summary>
    /// Create oblique with angle from -90 to 90.
    /// </summary>
    /// <param name="obliqueAngle">In degrees.</param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static FontStyle CustomOblique(nfloat obliqueAngle) => new FontStyle(false, true, obliqueAngle);
}