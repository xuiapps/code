using System.Diagnostics;

namespace Xui.Core.Math2D;

/// <summary>
/// Represents a 2D affine transformation matrix in the form:
/// <code>
/// | A C Tx |
/// | B D Ty |
/// | 0 0  1 |
/// </code>
/// Used for rotating, scaling, translating, and skewing 2D vectors or points.
/// </summary>
/// <remarks>
/// Transformation is applied as:
/// <code>
/// | x' |   | A C Tx |   | x |
/// | y' | = | B D Ty | * | y |
/// | 1  |   | 0 0  1 |   | 1 |
/// </code>
/// </remarks>
public struct AffineTransform
{
    /// <summary>The matrix component in the first row, first column (scale X / rotate).</summary>
    public nfloat A;

    /// <summary>The matrix component in the second row, first column (shear Y / rotate).</summary>
    public nfloat B;

    /// <summary>The matrix component in the first row, second column (shear X / rotate).</summary>
    public nfloat C;

    /// <summary>The matrix component in the second row, second column (scale Y / rotate).</summary>
    public nfloat D;

    /// <summary>The translation component along the X axis.</summary>
    public nfloat Tx;

    /// <summary>The translation component along the Y axis.</summary>
    public nfloat Ty;

    /// <summary>The identity transform (no scale, rotation, or translation).</summary>
    public static readonly AffineTransform Identity = new AffineTransform(1, 0, 0, 1, 0, 0);

    /// <summary>Returns <c>true</c> if the current transform is the identity matrix.</summary>
    public bool IsIdentity =>
        A == 1 && B == 0 &&
        C == 0 && D == 1 &&
        Tx == 0 && Ty == 0;

    /// <summary>
    /// Returns the inverse of this affine transform.
    /// </summary>
    /// <remarks>
    /// This operation assumes the transform is invertible. Division by a zero determinant will cause a runtime exception.
    /// </remarks>
    public AffineTransform Inverse
    {
        [DebuggerStepThrough]
        get
        {
            var determinant = this.Determinant;
            return new(
                +D / determinant,
                -B / determinant,
                -C / determinant,
                +A / determinant,
                ((C * Ty) - (D * Tx)) / determinant,
                ((B * Tx) - (A * Ty)) / determinant
            );
        }
    }

    /// <summary>
    /// Returns the determinant of the 2×2 linear portion of the matrix.
    /// </summary>
    /// <remarks>
    /// This value determines if the matrix is invertible. A value of 0 means the matrix cannot be inverted.
    /// </remarks>
    public nfloat Determinant
    {
        [DebuggerStepThrough]
        get => (A * D) - (C * B);
    }

    /// <summary>
    /// Creates a rotation transform around the origin using the given angle in radians.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Rotate(nfloat angle)
    {
        var cos = nfloat.Cos(angle);
        var sin = nfloat.Sin(angle);
        return new AffineTransform(cos, sin, -sin, cos, 0, 0);
    }

    /// <summary>
    /// Creates a translation transform using the specified offset vector.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Translate(Vector v) =>
        new AffineTransform(1, 0, 0, 1, v.X, v.Y);

    /// <summary>
    /// Creates a scaling transform using the specified scale vector.
    /// </summary>
    [DebuggerStepThrough]
    public static AffineTransform Scale(Vector v) =>
        new AffineTransform(v.X, 0, 0, v.Y, 0, 0);

    /// <summary>
    /// Creates a shear (skew) transformation along the X and Y axes.
    /// </summary>
    /// <param name="shearX">The shear factor in the X direction.</param>
    /// <param name="shearY">The shear factor in the Y direction.</param>
    [DebuggerStepThrough]
    public static AffineTransform Shear(nfloat shearX, nfloat shearY) =>
        new AffineTransform(1, shearY, shearX, 1, 0, 0);

    /// <summary>
    /// Constructs an affine transform with the specified matrix coefficients.
    /// </summary>
    [DebuggerStepThrough]
    public AffineTransform(nfloat a, nfloat b, nfloat c, nfloat d, nfloat tx, nfloat ty)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
        this.Tx = tx;
        this.Ty = ty;
    }

    /// <summary>
    /// Applies only the linear portion (scale, rotation, shear) of this transform to a <see cref="Vector"/>.
    /// </summary>
    /// <remarks>
    /// Translation is not applied. Use this to transform directions, movement deltas, or normals.
    /// </remarks>
    /// <param name="t">The affine transform.</param>
    /// <param name="v">The vector to transform.</param>
    /// <returns>A new <see cref="Vector"/> with the linear transform applied.</returns>
    [DebuggerStepThrough]
    public static Vector operator *(AffineTransform t, Vector v) =>
        new(t.A * v.X + t.C * v.Y, t.B * v.X + t.D * v.Y);

    /// <summary>
    /// Applies this transform to a <see cref="Point"/>, including translation.
    /// </summary>
    /// <remarks>
    /// This operation applies the full affine matrix, including scale, rotation, shear, and translation.
    /// Use this when transforming coordinates in space (e.g., an element's position).
    /// </remarks>
    /// <param name="t">The affine transform.</param>
    /// <param name="p">The point to transform.</param>
    /// <returns>A new <see cref="Point"/> with the transform applied.</returns>
    [DebuggerStepThrough]
    public static Point operator *(AffineTransform t, Point p) =>
        new(t.A * p.X + t.C * p.Y + t.Tx, t.B * p.X + t.D * p.Y + t.Ty);

    /// <summary>
    /// Composes two affine transforms using matrix multiplication.
    /// </summary>
    /// <returns>A new transform representing the application of <paramref name="rhs"/> followed by <paramref name="lhs"/>.</returns>
    [DebuggerStepThrough]
    public static AffineTransform operator *(AffineTransform lhs, AffineTransform rhs) =>
        new(
            lhs.A * rhs.A + lhs.C * rhs.B,
            lhs.B * rhs.A + lhs.D * rhs.B,
            lhs.A * rhs.C + lhs.C * rhs.D,
            lhs.B * rhs.C + lhs.D * rhs.D,
            lhs.A * rhs.Tx + lhs.C * rhs.Ty + lhs.Tx,
            lhs.B * rhs.Tx + lhs.D * rhs.Ty + lhs.Ty
        );

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override string ToString() =>
        $"AffineTransform [A={A}, B={B}, C={C}, D={D}, Tx={Tx}, Ty={Ty}]";
}
