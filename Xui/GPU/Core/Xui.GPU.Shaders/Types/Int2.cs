namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 2-component vector of 32-bit signed integers.
/// </summary>
public struct Int2
{
    /// <summary>
    /// The X component.
    /// </summary>
    public I32 X;

    /// <summary>
    /// The Y component.
    /// </summary>
    public I32 Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Int2"/> struct.
    /// </summary>
    public Int2(I32 x, I32 y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new instance with both components set to the same value.
    /// </summary>
    public Int2(I32 value)
    {
        X = value;
        Y = value;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Int2 Zero => new(I32.Zero, I32.Zero);

    /// <summary>
    /// Gets the one vector.
    /// </summary>
    public static Int2 One => new(I32.One, I32.One);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    public static Int2 operator +(Int2 left, Int2 right) =>
        new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    public static Int2 operator -(Int2 left, Int2 right) =>
        new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    public static Int2 operator *(Int2 left, Int2 right) =>
        new(left.X * right.X, left.Y * right.Y);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    public static Int2 operator *(Int2 left, I32 right) =>
        new(left.X * right, left.Y * right);

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    public static Int2 operator *(I32 left, Int2 right) =>
        new(left * right.X, left * right.Y);

    /// <summary>
    /// Divides two vectors component-wise.
    /// </summary>
    public static Int2 operator /(Int2 left, Int2 right) =>
        new(left.X / right.X, left.Y / right.Y);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    public static Int2 operator /(Int2 left, I32 right) =>
        new(left.X / right, left.Y / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    public static Int2 operator -(Int2 value) =>
        new(-value.X, -value.Y);

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y})";
}
