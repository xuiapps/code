namespace Xui.Core.Canvas;

/// <summary>
/// Represents the stretch or width percentage of a font, relative to its normal width (100%).
/// </summary>
/// <remarks>
/// Values less than 100 represent condensed fonts; values greater than 100 are expanded.
/// Font stretch supports comparison operators for ordering and matching.
/// </remarks>
public readonly struct FontStretch : IComparable<FontStretch>, IEquatable<FontStretch>
{
    /// <summary>
    /// The numeric percentage value of the font stretch.
    /// </summary>
    public readonly nfloat Value;

    /// <summary>
    /// Initializes a new <see cref="FontStretch"/> with the specified numeric value.
    /// </summary>
    /// <param name="value">The numeric width percentage (e.g., 100 = normal).</param>
    public FontStretch(nfloat value) => Value = value;

    /// <summary>Implicitly converts a <see cref="FontStretch"/> to <see cref="nfloat"/>.</summary>
    public static implicit operator nfloat(FontStretch stretch) => stretch.Value;

    /// <summary>Implicitly converts a <see cref="nfloat"/> to <see cref="FontStretch"/>.</summary>
    public static implicit operator FontStretch(nfloat value) => new(value);

    /// <summary>Implicitly converts a <see cref="float"/> to <see cref="FontStretch"/>.</summary>
    public static implicit operator FontStretch(float value) => new(value);

    /// <summary>Implicitly converts an <see cref="int"/> to <see cref="FontStretch"/>.</summary>
    public static implicit operator FontStretch(int value) => new(value);

    /// <summary>Returns <c>true</c> if two stretch values are equal.</summary>
    public static bool operator ==(FontStretch a, FontStretch b) => a.Value == b.Value;

    /// <summary>Returns <c>true</c> if two stretch values are not equal.</summary>
    public static bool operator !=(FontStretch a, FontStretch b) => a.Value != b.Value;

    /// <summary>Returns <c>true</c> if the left stretch is less than the right.</summary>
    public static bool operator <(FontStretch a, FontStretch b) => a.Value < b.Value;

    /// <summary>Returns <c>true</c> if the left stretch is less than or equal to the right.</summary>
    public static bool operator <=(FontStretch a, FontStretch b) => a.Value <= b.Value;

    /// <summary>Returns <c>true</c> if the left stretch is greater than the right.</summary>
    public static bool operator >(FontStretch a, FontStretch b) => a.Value > b.Value;

    /// <summary>Returns <c>true</c> if the left stretch is greater than or equal to the right.</summary>
    public static bool operator >=(FontStretch a, FontStretch b) => a.Value >= b.Value;

    /// <inheritdoc/>
    public int CompareTo(FontStretch other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public bool Equals(FontStretch other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FontStretch other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    // --- Predefined Stretch Values ---

    /// <summary>Font stretch 50% – Ultra Condensed. Very narrow glyph width.</summary>
    public static readonly FontStretch UltraCondensed = 50;

    /// <summary>Font stretch 62.5% – Extra Condensed. Narrower than Condensed.</summary>
    public static readonly FontStretch ExtraCondensed = 62.5f;

    /// <summary>Font stretch 75% – Condensed. Common for space-constrained text.</summary>
    public static readonly FontStretch Condensed = 75;

    /// <summary>Font stretch 87.5% – Semi Condensed. Slightly narrower than normal.</summary>
    public static readonly FontStretch SemiCondensed = 87.5f;

    /// <summary>Font stretch 100% – Normal. Default glyph width.</summary>
    public static readonly FontStretch Normal = 100;

    /// <summary>Font stretch 112.5% – Semi Expanded. Slightly wider than normal.</summary>
    public static readonly FontStretch SemiExpanded = 112.5f;

    /// <summary>Font stretch 125% – Expanded. Wider glyph appearance.</summary>
    public static readonly FontStretch Expanded = 125;

    /// <summary>Font stretch 150% – Extra Expanded. Significantly wider.</summary>
    public static readonly FontStretch ExtraExpanded = 150;

    /// <summary>Font stretch 200% – Ultra Expanded. Very wide glyphs.</summary>
    public static readonly FontStretch UltraExpanded = 200;
}
