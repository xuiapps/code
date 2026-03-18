namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 4-component RGBA color value.
/// </summary>
public struct Color4
{
    /// <summary>
    /// The red component.
    /// </summary>
    public F32 R;

    /// <summary>
    /// The green component.
    /// </summary>
    public F32 G;

    /// <summary>
    /// The blue component.
    /// </summary>
    public F32 B;

    /// <summary>
    /// The alpha component.
    /// </summary>
    public F32 A;

    /// <summary>
    /// Initializes a new instance of the <see cref="Color4"/> struct.
    /// </summary>
    public Color4(F32 r, F32 g, F32 b, F32 a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>
    /// Initializes a new instance from RGB values with alpha set to 1.
    /// </summary>
    public Color4(F32 r, F32 g, F32 b) : this(r, g, b, F32.One)
    {
    }

    /// <summary>
    /// Gets a white color (1, 1, 1, 1).
    /// </summary>
    public static Color4 White => new(F32.One, F32.One, F32.One, F32.One);

    /// <summary>
    /// Gets a black color (0, 0, 0, 1).
    /// </summary>
    public static Color4 Black => new(F32.Zero, F32.Zero, F32.Zero, F32.One);

    /// <summary>
    /// Gets a transparent color (0, 0, 0, 0).
    /// </summary>
    public static Color4 Transparent => new(F32.Zero, F32.Zero, F32.Zero, F32.Zero);

    /// <summary>
    /// Multiplies two colors component-wise.
    /// </summary>
    public static Color4 operator *(Color4 left, Color4 right) =>
        new(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);

    /// <summary>
    /// Multiplies a color by a scalar.
    /// </summary>
    public static Color4 operator *(Color4 left, F32 right) =>
        new(left.R * right, left.G * right, left.B * right, left.A * right);

    /// <inheritdoc/>
    public override string ToString() => $"({R}, {G}, {B}, {A})";
}
