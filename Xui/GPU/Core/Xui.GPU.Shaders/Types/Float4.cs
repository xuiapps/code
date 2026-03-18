namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 4-component vector of 32-bit floats.
/// </summary>
public struct Float4
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
    /// The W component.
    /// </summary>
    public F32 W;

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4"/> struct.
    /// </summary>
    public Float4(F32 x, F32 y, F32 z, F32 w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance from a Float2 and z, w components.
    /// </summary>
    public Float4(Float2 xy, F32 z, F32 w)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance from a Float3 and w component.
    /// </summary>
    public Float4(Float3 xyz, F32 w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    /// <summary>
    /// Initializes a new instance with all components set to the same value.
    /// </summary>
    public Float4(F32 value)
    {
        X = value;
        Y = value;
        Z = value;
        W = value;
    }

    /// <summary>
    /// Gets the zero vector.
    /// </summary>
    public static Float4 Zero => new(F32.Zero, F32.Zero, F32.Zero, F32.Zero);

    /// <summary>
    /// Gets the one vector.
    /// </summary>
    public static Float4 One => new(F32.One, F32.One, F32.One, F32.One);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    public static Float4 operator +(Float4 left, Float4 right) =>
        new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    public static Float4 operator -(Float4 left, Float4 right) =>
        new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    public static Float4 operator *(Float4 left, Float4 right) =>
        new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    public static Float4 operator *(Float4 left, F32 right) =>
        new(left.X * right, left.Y * right, left.Z * right, left.W * right);

    /// <summary>
    /// Multiplies a scalar by a vector.
    /// </summary>
    public static Float4 operator *(F32 left, Float4 right) =>
        new(left * right.X, left * right.Y, left * right.Z, left * right.W);

    /// <summary>
    /// Divides two vectors component-wise.
    /// </summary>
    public static Float4 operator /(Float4 left, Float4 right) =>
        new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    public static Float4 operator /(Float4 left, F32 right) =>
        new(left.X / right, left.Y / right, left.Z / right, left.W / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    public static Float4 operator -(Float4 value) =>
        new(-value.X, -value.Y, -value.Z, -value.W);

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y}, {Z}, {W})";
}
