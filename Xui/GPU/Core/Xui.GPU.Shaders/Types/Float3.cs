namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 3-component vector of 32-bit floats.
/// </summary>
public struct Float3
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
    /// The Z component.
    /// </summary>
    public F32 Z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Float3"/> struct.
    /// </summary>
    public Float3(F32 x, F32 y, F32 z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Initializes a new instance with all components set to the same value.
    /// </summary>
    public Float3(F32 value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Float3 Zero => new(F32.Zero, F32.Zero, F32.Zero);

    /// <summary>
    /// Gets the one vector.
    /// </summary>
    public static Float3 One => new(F32.One, F32.One, F32.One);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    public static Float3 operator +(Float3 left, Float3 right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    public static Float3 operator -(Float3 left, Float3 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    public static Float3 operator *(Float3 left, Float3 right) =>
        new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    public static Float3 operator *(Float3 left, F32 right) =>
        new(left.X * right, left.Y * right, left.Z * right);

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    public static Float3 operator *(F32 left, Float3 right) =>
        new(left * right.X, left * right.Y, left * right.Z);

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y}, {Z})";
}
