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

    /// <summary>
    /// Divides two vectors component-wise.
    /// </summary>
    public static Float3 operator /(Float3 left, Float3 right) =>
        new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    public static Float3 operator /(Float3 left, F32 right) =>
        new(left.X / right, left.Y / right, left.Z / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    public static Float3 operator -(Float3 value) =>
        new(-value.X, -value.Y, -value.Z);

    /// <summary>
    /// Computes the cross product of two vectors.
    /// </summary>
    public static Float3 Cross(Float3 a, Float3 b)
    {
        return new Float3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
    }

    /// <summary>
    /// Normalizes the vector to unit length.
    /// </summary>
    public static Float3 Normalize(Float3 v)
    {
        float lengthSquared = (v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        float length = MathF.Sqrt(lengthSquared);
        float invLength = 1.0f / length;
        return new Float3(new F32((float)v.X * invLength), new F32((float)v.Y * invLength), new F32((float)v.Z * invLength));
    }

    // ===== Swizzle Properties =====

    // 2-component swizzles
    /// <summary>
    /// Gets the XY swizzle (X, Y).
    /// </summary>
    public readonly Float2 XY => new(X, Y);

    /// <summary>
    /// Gets the XZ swizzle (X, Z).
    /// </summary>
    public readonly Float2 XZ => new(X, Z);

    /// <summary>
    /// Gets the YX swizzle (Y, X).
    /// </summary>
    public readonly Float2 YX => new(Y, X);

    /// <summary>
    /// Gets the YZ swizzle (Y, Z).
    /// </summary>
    public readonly Float2 YZ => new(Y, Z);

    /// <summary>
    /// Gets the ZX swizzle (Z, X).
    /// </summary>
    public readonly Float2 ZX => new(Z, X);

    /// <summary>
    /// Gets the ZY swizzle (Z, Y).
    /// </summary>
    public readonly Float2 ZY => new(Z, Y);

    // 3-component swizzles
    /// <summary>
    /// Gets the XYZ swizzle (X, Y, Z).
    /// </summary>
    public readonly Float3 XYZ => new(X, Y, Z);

    /// <summary>
    /// Gets the XZY swizzle (X, Z, Y).
    /// </summary>
    public readonly Float3 XZY => new(X, Z, Y);

    /// <summary>
    /// Gets the YXZ swizzle (Y, X, Z).
    /// </summary>
    public readonly Float3 YXZ => new(Y, X, Z);

    /// <summary>
    /// Gets the YZX swizzle (Y, Z, X).
    /// </summary>
    public readonly Float3 YZX => new(Y, Z, X);

    /// <summary>
    /// Gets the ZXY swizzle (Z, X, Y).
    /// </summary>
    public readonly Float3 ZXY => new(Z, X, Y);

    /// <summary>
    /// Gets the ZYX swizzle (Z, Y, X).
    /// </summary>
    public readonly Float3 ZYX => new(Z, Y, X);

    /// <inheritdoc/>
    public override string ToString() => $"({X}, {Y}, {Z})";
}
