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

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y})";
}
