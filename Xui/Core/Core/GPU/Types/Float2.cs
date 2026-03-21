namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 2-component vector of 32-bit floats.
/// </summary>
public struct Float2
{
    /// <summary>
    /// The X component.
    /// </summary>
    public F32 X;

    /// <summary>
    /// The Y component.
    /// </summary>
    public F32 Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Float2"/> struct.
    /// </summary>
    public Float2(F32 x, F32 y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance with both components set to the same value.
    /// </summary>
    public Float2(F32 value)
    {
        X = value;
        Y = value;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Float2 Zero => new(F32.Zero, F32.Zero);

    /// <summary>
    /// Gets the one vector.
    /// </summary>
    public static Float2 One => new(F32.One, F32.One);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    public static Float2 operator +(Float2 left, Float2 right) =>
        new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    public static Float2 operator -(Float2 left, Float2 right) =>
        new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    public static Float2 operator *(Float2 left, Float2 right) =>
        new(left.X * right.X, left.Y * right.Y);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    public static Float2 operator *(Float2 left, F32 right) =>
        new(left.X * right, left.Y * right);

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    public static Float2 operator *(F32 left, Float2 right) =>
        new(left * right.X, left * right.Y);

    /// <summary>
    /// Divides two vectors component-wise.
    /// </summary>
    public static Float2 operator /(Float2 left, Float2 right) =>
        new(left.X / right.X, left.Y / right.Y);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    public static Float2 operator /(Float2 left, F32 right) =>
        new(left.X / right, left.Y / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    public static Float2 operator -(Float2 value) =>
        new(-value.X, -value.Y);

    // ===== Swizzle Properties =====

    /// <summary>
    /// Gets the XX swizzle (X, X).
    /// </summary>
    public readonly Float2 XX => new(X, X);

    /// <summary>
    /// Gets the XY swizzle (X, Y).
    /// </summary>
    public readonly Float2 XY => new(X, Y);

    /// <summary>
    /// Gets the YX swizzle (Y, X).
    /// </summary>
    public readonly Float2 YX => new(Y, X);

    /// <summary>
    /// Gets the YY swizzle (Y, Y).
    /// </summary>
    public readonly Float2 YY => new(Y, Y);

    /// <summary>
    /// Implicitly converts from <see cref="Xui.Core.Math2D.Point"/> to <see cref="Float2"/>.
    /// </summary>
    public static implicit operator Float2(Xui.Core.Math2D.Point point) =>
        new((float)point.X, (float)point.Y);

    /// <summary>
    /// Implicitly converts from <see cref="Xui.Core.Math2D.Vector"/> to <see cref="Float2"/>.
    /// </summary>
    public static implicit operator Float2(Xui.Core.Math2D.Vector vector) =>
        new((float)vector.X, (float)vector.Y);

    /// <summary>
    /// Implicitly converts from <see cref="Xui.Core.Math2D.Size"/> to <see cref="Float2"/>.
    /// </summary>
    public static implicit operator Float2(Xui.Core.Math2D.Size size) =>
        new((float)size.Width, (float)size.Height);

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y})";
}
