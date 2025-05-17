namespace Xui.Core.Canvas;

/// <summary>
/// Represents the numeric weight of a font, corresponding to CSS font weights (100–900).
/// </summary>
/// <remarks>
/// Font weights are numeric and allow comparison using operators like <c>&lt;</c>, <c>&gt;=</c>, etc.
/// </remarks>
public readonly struct FontWeight : IComparable<FontWeight>, IEquatable<FontWeight>
{
    /// <summary>
    /// The numeric value of the font weight.
    /// </summary>
    public readonly nfloat Value;

    /// <summary>
    /// Creates a new <see cref="FontWeight"/> with the specified numeric value.
    /// </summary>
    /// <param name="value">The numeric font weight value.</param>
    public FontWeight(nfloat value) => Value = value;

    /// <summary>Implicitly converts a <see cref="FontWeight"/> to <see cref="nfloat"/>.</summary>
    public static implicit operator nfloat(FontWeight weight) => weight.Value;

    /// <summary>Implicitly converts a <see cref="nfloat"/> to <see cref="FontWeight"/>.</summary>
    public static implicit operator FontWeight(nfloat value) => new(value);

    /// <summary>Implicitly converts a <see cref="float"/> to <see cref="FontWeight"/>.</summary>
    public static implicit operator FontWeight(float value) => new(value);

    /// <summary>Implicitly converts an <see cref="int"/> to <see cref="FontWeight"/>.</summary>
    public static implicit operator FontWeight(int value) => new(value);

    /// <summary>Returns <c>true</c> if two font weights are equal.</summary>
    public static bool operator ==(FontWeight a, FontWeight b) => a.Value == b.Value;

    /// <summary>Returns <c>true</c> if two font weights are not equal.</summary>
    public static bool operator !=(FontWeight a, FontWeight b) => a.Value != b.Value;

    /// <summary>Returns <c>true</c> if the left font weight is less than the right.</summary>
    public static bool operator <(FontWeight a, FontWeight b) => a.Value < b.Value;

    /// <summary>Returns <c>true</c> if the left font weight is less than or equal to the right.</summary>
    public static bool operator <=(FontWeight a, FontWeight b) => a.Value <= b.Value;

    /// <summary>Returns <c>true</c> if the left font weight is greater than the right.</summary>
    public static bool operator >(FontWeight a, FontWeight b) => a.Value > b.Value;

    /// <summary>Returns <c>true</c> if the left font weight is greater than or equal to the right.</summary>
    public static bool operator >=(FontWeight a, FontWeight b) => a.Value >= b.Value;

    /// <inheritdoc/>
    public int CompareTo(FontWeight other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public bool Equals(FontWeight other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FontWeight other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    // --- Predefined Weights ---

    /// <summary>Font weight 100 – Thin. The lightest possible weight.</summary>
    public static readonly FontWeight Thin = 100;

    /// <summary>Font weight 200 – Extra Light. Slightly heavier than Thin.</summary>
    public static readonly FontWeight ExtraLight = 200;

    /// <summary>Font weight 300 – Light. Common for minimalist design.</summary>
    public static readonly FontWeight Light = 300;

    /// <summary>Font weight 400 – Normal. Standard weight for body text.</summary>
    public static readonly FontWeight Normal = 400;

    /// <summary>Font weight 500 – Medium. Slightly heavier than normal.</summary>
    public static readonly FontWeight Medium = 500;

    /// <summary>Font weight 600 – Semi Bold. Between medium and bold.</summary>
    public static readonly FontWeight SemiBold = 600;

    /// <summary>Font weight 700 – Bold. Common for emphasis and headings.</summary>
    public static readonly FontWeight Bold = 700;

    /// <summary>Font weight 800 – Extra Bold. Heavier than Bold.</summary>
    public static readonly FontWeight ExtraBold = 800;

    /// <summary>Font weight 900 – Black. The heaviest available weight.</summary>
    public static readonly FontWeight Black = 900;
}
