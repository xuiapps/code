using System.Runtime.CompilerServices;

namespace Xui.Core.Math3D;

/// <summary>
/// Represents a 4×4 column-major transformation matrix for 3D geometry.
/// </summary>
/// <remarks>
/// <para>
/// Vectors and points are treated as column vectors and transformed by pre-multiplication:
/// <c>v' = M * v</c>. Translation is stored in the last column (M14, M24, M34).
/// </para>
/// <para>
/// The matrix is stored in row-major memory order (M_row_col) as 16 sequential floats
/// (64 bytes), fitting exactly into four 128-bit SIMD registers.
/// </para>
/// <para>
/// To interoperate with <see cref="System.Numerics.Matrix4x4"/> (which uses row-vector
/// convention) use the explicit cast operators, which perform the necessary transpose.
/// </para>
/// </remarks>
public struct Matrix4x4
{
    /// <summary>Row 1, Column 1 — X basis X component (scale X for pure-scale matrices).</summary>
    public float M11;
    /// <summary>Row 1, Column 2 — X basis Y shear.</summary>
    public float M12;
    /// <summary>Row 1, Column 3 — X basis Z shear.</summary>
    public float M13;
    /// <summary>Row 1, Column 4 — Translation along X.</summary>
    public float M14;

    /// <summary>Row 2, Column 1 — Y basis X shear.</summary>
    public float M21;
    /// <summary>Row 2, Column 2 — Y basis Y component (scale Y for pure-scale matrices).</summary>
    public float M22;
    /// <summary>Row 2, Column 3 — Y basis Z shear.</summary>
    public float M23;
    /// <summary>Row 2, Column 4 — Translation along Y.</summary>
    public float M24;

    /// <summary>Row 3, Column 1 — Z basis X shear.</summary>
    public float M31;
    /// <summary>Row 3, Column 2 — Z basis Y shear.</summary>
    public float M32;
    /// <summary>Row 3, Column 3 — Z basis Z component (scale Z for pure-scale matrices).</summary>
    public float M33;
    /// <summary>Row 3, Column 4 — Translation along Z.</summary>
    public float M34;

    /// <summary>Row 4, Column 1 — Perspective X component (0 for affine transforms).</summary>
    public float M41;
    /// <summary>Row 4, Column 2 — Perspective Y component (0 for affine transforms).</summary>
    public float M42;
    /// <summary>Row 4, Column 3 — Perspective Z component (0 for affine transforms).</summary>
    public float M43;
    /// <summary>Row 4, Column 4 — Homogeneous W scale (1 for affine transforms).</summary>
    public float M44;

    /// <summary>The identity matrix.</summary>
    public static readonly Matrix4x4 Identity = new(
        1f, 0f, 0f, 0f,
        0f, 1f, 0f, 0f,
        0f, 0f, 1f, 0f,
        0f, 0f, 0f, 1f
    );

    /// <summary>
    /// Initializes a new <see cref="Matrix4x4"/> with the specified row-ordered coefficients.
    /// </summary>
    [DebuggerStepThrough]
    public Matrix4x4(
        float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        M11 = m11; M12 = m12; M13 = m13; M14 = m14;
        M21 = m21; M22 = m22; M23 = m23; M24 = m24;
        M31 = m31; M32 = m32; M33 = m33; M34 = m34;
        M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    /// <summary>Returns <c>true</c> when this matrix equals the identity matrix.</summary>
    public bool IsIdentity =>
        M11 == 1f && M12 == 0f && M13 == 0f && M14 == 0f &&
        M21 == 0f && M22 == 1f && M23 == 0f && M24 == 0f &&
        M31 == 0f && M32 == 0f && M33 == 1f && M34 == 0f &&
        M41 == 0f && M42 == 0f && M43 == 0f && M44 == 1f;

    /// <summary>
    /// Returns the transpose of this matrix (swaps rows and columns).
    /// </summary>
    public Matrix4x4 Transposed =>
        new(
            M11, M21, M31, M41,
            M12, M22, M32, M42,
            M13, M23, M33, M43,
            M14, M24, M34, M44
        );

    /// <summary>
    /// Returns the determinant of the matrix.
    /// A determinant of zero means the matrix is not invertible.
    /// </summary>
    public float Determinant
    {
        get
        {
            // Cofactor expansion along the first row.
            float c11 = M22 * (M33 * M44 - M34 * M43) - M23 * (M32 * M44 - M34 * M42) + M24 * (M32 * M43 - M33 * M42);
            float c12 = M21 * (M33 * M44 - M34 * M43) - M23 * (M31 * M44 - M34 * M41) + M24 * (M31 * M43 - M33 * M41);
            float c13 = M21 * (M32 * M44 - M34 * M42) - M22 * (M31 * M44 - M34 * M41) + M24 * (M31 * M42 - M32 * M41);
            float c14 = M21 * (M32 * M43 - M33 * M42) - M22 * (M31 * M43 - M33 * M41) + M23 * (M31 * M42 - M32 * M41);
            return M11 * c11 - M12 * c12 + M13 * c13 - M14 * c14;
        }
    }

    /// <summary>
    /// Returns the inverse of this matrix.
    /// </summary>
    /// <remarks>
    /// For rotation-only or TRS (translation-rotation-scale) matrices, consider computing
    /// the inverse analytically for better performance.
    /// Throws a division-by-zero style error if the matrix is singular (determinant ≈ 0).
    /// </remarks>
    public Matrix4x4 Inverse
    {
        get
        {
            // Compute cofactors for all 16 elements.
            float a11 = M22 * (M33 * M44 - M34 * M43) - M23 * (M32 * M44 - M34 * M42) + M24 * (M32 * M43 - M33 * M42);
            float a12 = -(M21 * (M33 * M44 - M34 * M43) - M23 * (M31 * M44 - M34 * M41) + M24 * (M31 * M43 - M33 * M41));
            float a13 = M21 * (M32 * M44 - M34 * M42) - M22 * (M31 * M44 - M34 * M41) + M24 * (M31 * M42 - M32 * M41);
            float a14 = -(M21 * (M32 * M43 - M33 * M42) - M22 * (M31 * M43 - M33 * M41) + M23 * (M31 * M42 - M32 * M41));

            float det = M11 * a11 + M12 * a12 + M13 * a13 + M14 * a14;
            float invDet = 1f / det;

            float a21 = -(M12 * (M33 * M44 - M34 * M43) - M13 * (M32 * M44 - M34 * M42) + M14 * (M32 * M43 - M33 * M42));
            float a22 = M11 * (M33 * M44 - M34 * M43) - M13 * (M31 * M44 - M34 * M41) + M14 * (M31 * M43 - M33 * M41);
            float a23 = -(M11 * (M32 * M44 - M34 * M42) - M12 * (M31 * M44 - M34 * M41) + M14 * (M31 * M42 - M32 * M41));
            float a24 = M11 * (M32 * M43 - M33 * M42) - M12 * (M31 * M43 - M33 * M41) + M13 * (M31 * M42 - M32 * M41);

            float a31 = M12 * (M23 * M44 - M24 * M43) - M13 * (M22 * M44 - M24 * M42) + M14 * (M22 * M43 - M23 * M42);
            float a32 = -(M11 * (M23 * M44 - M24 * M43) - M13 * (M21 * M44 - M24 * M41) + M14 * (M21 * M43 - M23 * M41));
            float a33 = M11 * (M22 * M44 - M24 * M42) - M12 * (M21 * M44 - M24 * M41) + M14 * (M21 * M42 - M22 * M41);
            float a34 = -(M11 * (M22 * M43 - M23 * M42) - M12 * (M21 * M43 - M23 * M41) + M13 * (M21 * M42 - M22 * M41));

            float a41 = -(M12 * (M23 * M34 - M24 * M33) - M13 * (M22 * M34 - M24 * M32) + M14 * (M22 * M33 - M23 * M32));
            float a42 = M11 * (M23 * M34 - M24 * M33) - M13 * (M21 * M34 - M24 * M31) + M14 * (M21 * M33 - M23 * M31);
            float a43 = -(M11 * (M22 * M34 - M24 * M32) - M12 * (M21 * M34 - M24 * M31) + M14 * (M21 * M32 - M22 * M31));
            float a44 = M11 * (M22 * M33 - M23 * M32) - M12 * (M21 * M33 - M23 * M31) + M13 * (M21 * M32 - M22 * M31);

            // Adjugate (transpose of cofactor matrix) divided by determinant.
            return new Matrix4x4(
                a11 * invDet, a21 * invDet, a31 * invDet, a41 * invDet,
                a12 * invDet, a22 * invDet, a32 * invDet, a42 * invDet,
                a13 * invDet, a23 * invDet, a33 * invDet, a43 * invDet,
                a14 * invDet, a24 * invDet, a34 * invDet, a44 * invDet
            );
        }
    }

    // ── Factory methods ────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a translation matrix that moves a point by the given vector.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 Translate(Vector3 v) =>
        new(
            1f, 0f, 0f, v.X,
            0f, 1f, 0f, v.Y,
            0f, 0f, 1f, v.Z,
            0f, 0f, 0f, 1f
        );

    /// <summary>
    /// Creates a uniform scale matrix.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 Scale(float s) =>
        new(
            s,  0f, 0f, 0f,
            0f, s,  0f, 0f,
            0f, 0f, s,  0f,
            0f, 0f, 0f, 1f
        );

    /// <summary>
    /// Creates a non-uniform scale matrix.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 Scale(Vector3 v) =>
        new(
            v.X, 0f,  0f,  0f,
            0f,  v.Y, 0f,  0f,
            0f,  0f,  v.Z, 0f,
            0f,  0f,  0f,  1f
        );

    /// <summary>
    /// Creates a rotation matrix around the X axis by the given angle in radians.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 RotateX(float angle)
    {
        float c = MathF.Cos(angle);
        float s = MathF.Sin(angle);
        return new(
            1f, 0f, 0f, 0f,
            0f, c, -s,  0f,
            0f, s,  c,  0f,
            0f, 0f, 0f, 1f
        );
    }

    /// <summary>
    /// Creates a rotation matrix around the Y axis by the given angle in radians.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 RotateY(float angle)
    {
        float c = MathF.Cos(angle);
        float s = MathF.Sin(angle);
        return new(
             c, 0f, s, 0f,
            0f, 1f, 0f, 0f,
            -s, 0f, c, 0f,
            0f, 0f, 0f, 1f
        );
    }

    /// <summary>
    /// Creates a rotation matrix around the Z axis by the given angle in radians.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 RotateZ(float angle)
    {
        float c = MathF.Cos(angle);
        float s = MathF.Sin(angle);
        return new(
            c, -s,  0f, 0f,
            s,  c,  0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );
    }

    /// <summary>
    /// Creates a rotation matrix from a unit quaternion.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 Rotate(Quaternion q) => q.ToMatrix4x4();

    /// <summary>
    /// Creates a symmetric perspective projection matrix (right-handed, depth range [-1, 1]).
    /// </summary>
    /// <param name="fovY">Vertical field of view in radians.</param>
    /// <param name="aspect">Aspect ratio (width / height).</param>
    /// <param name="near">Distance to the near clipping plane (must be positive).</param>
    /// <param name="far">Distance to the far clipping plane (must be greater than <paramref name="near"/>).</param>
    [DebuggerStepThrough]
    public static Matrix4x4 Perspective(float fovY, float aspect, float near, float far)
    {
        float f = 1f / MathF.Tan(fovY * 0.5f);
        float d = near - far;
        return new(
            f / aspect, 0f, 0f,                       0f,
            0f,         f,  0f,                       0f,
            0f,         0f, (far + near) / d,         2f * far * near / d,
            0f,         0f, -1f,                      0f
        );
    }

    /// <summary>
    /// Creates a look-at view matrix (right-handed coordinate system).
    /// </summary>
    /// <param name="eye">The position of the camera.</param>
    /// <param name="target">The position the camera is looking at.</param>
    /// <param name="up">The world up direction (typically <see cref="Vector3.Up"/>).</param>
    [DebuggerStepThrough]
    public static Matrix4x4 LookAt(Point3 eye, Point3 target, Vector3 up)
    {
        var forward = (eye - target).Normalized;  // Points from target to eye (-Z)
        var right = Vector3.Cross(up, forward).Normalized;
        var upDir = Vector3.Cross(forward, right);
        var eyeVec = (Vector3)eye;

        return new(
            right.X,   right.Y,   right.Z,   -Vector3.Dot(right, eyeVec),
            upDir.X,   upDir.Y,   upDir.Z,   -Vector3.Dot(upDir, eyeVec),
            forward.X, forward.Y, forward.Z, -Vector3.Dot(forward, eyeVec),
            0f,        0f,        0f,         1f
        );
    }

    /// <summary>
    /// Creates a symmetric orthographic projection matrix (right-handed, depth range [-1, 1]).
    /// </summary>
    /// <param name="width">The width of the view volume.</param>
    /// <param name="height">The height of the view volume.</param>
    /// <param name="near">Distance to the near clipping plane.</param>
    /// <param name="far">Distance to the far clipping plane.</param>
    [DebuggerStepThrough]
    public static Matrix4x4 Orthographic(float width, float height, float near, float far)
    {
        float d = near - far;
        return new(
            2f / width, 0f,          0f,       0f,
            0f,         2f / height, 0f,       0f,
            0f,         0f,          2f / d,   (near + far) / d,
            0f,         0f,          0f,       1f
        );
    }

    // ── Operators ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Composes two transformation matrices.
    /// The result applies <paramref name="rhs"/> first, then <paramref name="lhs"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs) =>
        new(
            lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31 + lhs.M14 * rhs.M41,
            lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32 + lhs.M14 * rhs.M42,
            lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33 + lhs.M14 * rhs.M43,
            lhs.M11 * rhs.M14 + lhs.M12 * rhs.M24 + lhs.M13 * rhs.M34 + lhs.M14 * rhs.M44,

            lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31 + lhs.M24 * rhs.M41,
            lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32 + lhs.M24 * rhs.M42,
            lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33 + lhs.M24 * rhs.M43,
            lhs.M21 * rhs.M14 + lhs.M22 * rhs.M24 + lhs.M23 * rhs.M34 + lhs.M24 * rhs.M44,

            lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31 + lhs.M34 * rhs.M41,
            lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32 + lhs.M34 * rhs.M42,
            lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33 + lhs.M34 * rhs.M43,
            lhs.M31 * rhs.M14 + lhs.M32 * rhs.M24 + lhs.M33 * rhs.M34 + lhs.M34 * rhs.M44,

            lhs.M41 * rhs.M11 + lhs.M42 * rhs.M21 + lhs.M43 * rhs.M31 + lhs.M44 * rhs.M41,
            lhs.M41 * rhs.M12 + lhs.M42 * rhs.M22 + lhs.M43 * rhs.M32 + lhs.M44 * rhs.M42,
            lhs.M41 * rhs.M13 + lhs.M42 * rhs.M23 + lhs.M43 * rhs.M33 + lhs.M44 * rhs.M43,
            lhs.M41 * rhs.M14 + lhs.M42 * rhs.M24 + lhs.M43 * rhs.M34 + lhs.M44 * rhs.M44
        );

    /// <summary>
    /// Transforms a <see cref="Vector3"/> by this matrix, ignoring the translation component.
    /// Use this for directions and normals.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Matrix4x4 m, Vector3 v) =>
        new(
            m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
            m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
            m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z
        );

    /// <summary>
    /// Transforms a <see cref="Point3"/> by this matrix, including the translation component.
    /// Use this for positions.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator *(Matrix4x4 m, Point3 p) =>
        new(
            m.M11 * p.X + m.M12 * p.Y + m.M13 * p.Z + m.M14,
            m.M21 * p.X + m.M22 * p.Y + m.M23 * p.Z + m.M24,
            m.M31 * p.X + m.M32 * p.Y + m.M33 * p.Z + m.M34
        );

    /// <summary>Returns true if two matrices have equal coefficients.</summary>
    [DebuggerStepThrough]
    public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs) =>
        lhs.M11 == rhs.M11 && lhs.M12 == rhs.M12 && lhs.M13 == rhs.M13 && lhs.M14 == rhs.M14 &&
        lhs.M21 == rhs.M21 && lhs.M22 == rhs.M22 && lhs.M23 == rhs.M23 && lhs.M24 == rhs.M24 &&
        lhs.M31 == rhs.M31 && lhs.M32 == rhs.M32 && lhs.M33 == rhs.M33 && lhs.M34 == rhs.M34 &&
        lhs.M41 == rhs.M41 && lhs.M42 == rhs.M42 && lhs.M43 == rhs.M43 && lhs.M44 == rhs.M44;

    /// <summary>Returns true if any coefficient of two matrices differs.</summary>
    [DebuggerStepThrough]
    public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs) =>
        !(lhs == rhs);

    // ── System.Numerics interop ───────────────────────────────────────────────────

    /// <summary>
    /// Explicitly converts to a <see cref="System.Numerics.Matrix4x4"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="System.Numerics.Matrix4x4"/> uses row-vector convention, which is the
    /// transpose of the column-vector convention used by this type.
    /// The conversion transposes the matrix so that row-vector math on the result
    /// produces the same geometric transformation.
    /// </remarks>
    [DebuggerStepThrough]
    public static explicit operator System.Numerics.Matrix4x4(Matrix4x4 m) =>
        new(
            m.M11, m.M21, m.M31, m.M41,
            m.M12, m.M22, m.M32, m.M42,
            m.M13, m.M23, m.M33, m.M43,
            m.M14, m.M24, m.M34, m.M44
        );

    /// <summary>
    /// Explicitly converts from a <see cref="System.Numerics.Matrix4x4"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="System.Numerics.Matrix4x4"/> uses row-vector convention. This conversion
    /// transposes the input matrix so that it works correctly with the column-vector
    /// convention used by this type.
    /// </remarks>
    [DebuggerStepThrough]
    public static explicit operator Matrix4x4(System.Numerics.Matrix4x4 m) =>
        new(
            m.M11, m.M21, m.M31, m.M41,
            m.M12, m.M22, m.M32, m.M42,
            m.M13, m.M23, m.M33, m.M43,
            m.M14, m.M24, m.M34, m.M44
        );

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Matrix4x4 other && this == other;

    /// <summary>Returns true if this matrix equals another <see cref="Matrix4x4"/>.</summary>
    public bool Equals(Matrix4x4 other) => this == other;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(
            HashCode.Combine(M11, M12, M13, M14),
            HashCode.Combine(M21, M22, M23, M24),
            HashCode.Combine(M31, M32, M33, M34),
            HashCode.Combine(M41, M42, M43, M44)
        );

    /// <inheritdoc/>
    public override string ToString() =>
        $"Matrix4x4 [{M11}, {M12}, {M13}, {M14} | {M21}, {M22}, {M23}, {M24} | {M31}, {M32}, {M33}, {M34} | {M41}, {M42}, {M43}, {M44}]";
}
