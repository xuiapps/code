namespace Xui.GPU.Shaders.Types;

/// <summary>
/// A 4x4 matrix of 32-bit floats.
/// Used for transformations in shader code.
/// </summary>
public struct Float4x4
{
    /// <summary>
    /// First row of the matrix.
    /// </summary>
    public Float4 Row0;

    /// <summary>
    /// Second row of the matrix.
    /// </summary>
    public Float4 Row1;

    /// <summary>
    /// Third row of the matrix.
    /// </summary>
    public Float4 Row2;

    /// <summary>
    /// Fourth row of the matrix.
    /// </summary>
    public Float4 Row3;

    /// <summary>
    /// Initializes a new instance of the <see cref="Float4x4"/> struct.
    /// </summary>
    public Float4x4(Float4 row0, Float4 row1, Float4 row2, Float4 row3)
    {
        Row0 = row0;
        Row1 = row1;
        Row2 = row2;
        Row3 = row3;
    }

    /// <summary>
    /// Gets the identity matrix.
    /// </summary>
    public static Float4x4 Identity => new(
        new Float4(F32.One, F32.Zero, F32.Zero, F32.Zero),
        new Float4(F32.Zero, F32.One, F32.Zero, F32.Zero),
        new Float4(F32.Zero, F32.Zero, F32.One, F32.Zero),
        new Float4(F32.Zero, F32.Zero, F32.Zero, F32.One)
    );

    /// <summary>
    /// Multiplies a matrix by a vector.
    /// </summary>
    public static Float4 operator *(Float4x4 matrix, Float4 vector)
    {
        return new Float4(
            matrix.Row0.X * vector.X + matrix.Row0.Y * vector.Y + matrix.Row0.Z * vector.Z + matrix.Row0.W * vector.W,
            matrix.Row1.X * vector.X + matrix.Row1.Y * vector.Y + matrix.Row1.Z * vector.Z + matrix.Row1.W * vector.W,
            matrix.Row2.X * vector.X + matrix.Row2.Y * vector.Y + matrix.Row2.Z * vector.Z + matrix.Row2.W * vector.W,
            matrix.Row3.X * vector.X + matrix.Row3.Y * vector.Y + matrix.Row3.Z * vector.Z + matrix.Row3.W * vector.W
        );
    }

    /// <summary>
    /// Multiplies two matrices.
    /// </summary>
    public static Float4x4 operator *(Float4x4 left, Float4x4 right)
    {
        // Compute each element of the result matrix
        // Result[i,j] = sum of left.Row[i] * right.Column[j]
        return new Float4x4(
            new Float4(
                left.Row0.X * right.Row0.X + left.Row0.Y * right.Row1.X + left.Row0.Z * right.Row2.X + left.Row0.W * right.Row3.X,
                left.Row0.X * right.Row0.Y + left.Row0.Y * right.Row1.Y + left.Row0.Z * right.Row2.Y + left.Row0.W * right.Row3.Y,
                left.Row0.X * right.Row0.Z + left.Row0.Y * right.Row1.Z + left.Row0.Z * right.Row2.Z + left.Row0.W * right.Row3.Z,
                left.Row0.X * right.Row0.W + left.Row0.Y * right.Row1.W + left.Row0.Z * right.Row2.W + left.Row0.W * right.Row3.W
            ),
            new Float4(
                left.Row1.X * right.Row0.X + left.Row1.Y * right.Row1.X + left.Row1.Z * right.Row2.X + left.Row1.W * right.Row3.X,
                left.Row1.X * right.Row0.Y + left.Row1.Y * right.Row1.Y + left.Row1.Z * right.Row2.Y + left.Row1.W * right.Row3.Y,
                left.Row1.X * right.Row0.Z + left.Row1.Y * right.Row1.Z + left.Row1.Z * right.Row2.Z + left.Row1.W * right.Row3.Z,
                left.Row1.X * right.Row0.W + left.Row1.Y * right.Row1.W + left.Row1.Z * right.Row2.W + left.Row1.W * right.Row3.W
            ),
            new Float4(
                left.Row2.X * right.Row0.X + left.Row2.Y * right.Row1.X + left.Row2.Z * right.Row2.X + left.Row2.W * right.Row3.X,
                left.Row2.X * right.Row0.Y + left.Row2.Y * right.Row1.Y + left.Row2.Z * right.Row2.Y + left.Row2.W * right.Row3.Y,
                left.Row2.X * right.Row0.Z + left.Row2.Y * right.Row1.Z + left.Row2.Z * right.Row2.Z + left.Row2.W * right.Row3.Z,
                left.Row2.X * right.Row0.W + left.Row2.Y * right.Row1.W + left.Row2.Z * right.Row2.W + left.Row2.W * right.Row3.W
            ),
            new Float4(
                left.Row3.X * right.Row0.X + left.Row3.Y * right.Row1.X + left.Row3.Z * right.Row2.X + left.Row3.W * right.Row3.X,
                left.Row3.X * right.Row0.Y + left.Row3.Y * right.Row1.Y + left.Row3.Z * right.Row2.Y + left.Row3.W * right.Row3.Y,
                left.Row3.X * right.Row0.Z + left.Row3.Y * right.Row1.Z + left.Row3.Z * right.Row2.Z + left.Row3.W * right.Row3.Z,
                left.Row3.X * right.Row0.W + left.Row3.Y * right.Row1.W + left.Row3.Z * right.Row2.W + left.Row3.W * right.Row3.W
            )
        );
    }

    /// <inheritdoc/>
    public override string ToString() => $"[\n  {Row0}\n  {Row1}\n  {Row2}\n  {Row3}\n]";
}
