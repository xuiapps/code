namespace Xui.Core.Canvas;

using System.Diagnostics;

/// <summary>
/// Represents a web-like font style, supporting <c>normal</c>, <c>italic</c>, and <c>oblique</c> variations.
/// Global CSS styles like <c>inherit</c>, <c>unset</c>, and <c>initial</c> are not supported.
///
/// Reference: https://developer.mozilla.org/en-US/docs/Web/CSS/font-style
/// </summary>
public struct FontStyle
{
    private readonly bool italic;
    private readonly bool oblique;
    private readonly nfloat obliqueAngle;

    /// <summary>
    /// Initializes a new <see cref="FontStyle"/> with explicit flags for italic or oblique styling.
    /// </summary>
    /// <param name="italic">Whether the font is italic.</param>
    /// <param name="oblique">Whether the font is oblique (slanted).</param>
    /// <param name="obliqueAngle">Angle for oblique style, in degrees.</param>
    [DebuggerStepThrough]
    public FontStyle(bool italic, bool oblique, nfloat obliqueAngle)
    {
        this.italic = italic;
        this.oblique = oblique;
        this.obliqueAngle = obliqueAngle;
    }

    /// <summary>
    /// Gets a value indicating whether the font style is italic.
    /// </summary>
    public bool IsItalic => this.italic;

    /// <summary>
    /// Gets a value indicating whether the font style is oblique (slanted).
    /// </summary>
    public bool IsOblique => this.oblique;

    /// <summary>
    /// Gets the oblique slant angle, in degrees. Only meaningful if <see cref="IsOblique"/> is true.
    /// Range is typically from -90° to 90°.
    /// </summary>
    public nfloat ObliqueAngle => this.obliqueAngle;

    /// <summary>
    /// Normal (upright) font style.
    /// </summary>
    public static FontStyle Normal => new FontStyle(false, false, 14f);

    /// <summary>
    /// Italic font style.
    /// </summary>
    public static FontStyle Italic => new FontStyle(true, false, 14f);

    /// <summary>
    /// Oblique font style using the default slant angle (14°).
    /// </summary>
    public static FontStyle Oblique => new FontStyle(false, true, 14f);

    /// <summary>
    /// Creates a custom oblique font style with a specific slant angle.
    /// </summary>
    /// <param name="obliqueAngle">The angle of the slant, in degrees. Should be between -90 and 90.</param>
    /// <returns>A <see cref="FontStyle"/> representing the custom oblique angle.</returns>
    [DebuggerStepThrough]
    public static FontStyle CustomOblique(nfloat obliqueAngle) => new FontStyle(false, true, obliqueAngle);
}
