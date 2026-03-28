using System.Runtime.CompilerServices;

namespace Xui.Core.Math3D;

/// <summary>
/// Represents a unit quaternion used to encode a 3D rotation.
/// </summary>
/// <remarks>
/// <para>
/// A quaternion is stored as four floats (X, Y, Z, W) where (X, Y, Z) is the
/// vector part and W is the scalar part. For a rotation of angle θ around a
/// normalized axis <b>n</b>: X = n.X·sin(θ/2), Y = n.Y·sin(θ/2),
/// Z = n.Z·sin(θ/2), W = cos(θ/2).
/// </para>
/// <para>
/// The 16-byte memory layout (four sequential floats) makes <see cref="Quaternion"/>
/// compatible with SIMD registers and GPU constant buffers.
/// </para>
/// </remarks>
public struct Quaternion
{
    /// <summary>The X component of the vector part.</summary>
    public float X;

    /// <summary>The Y component of the vector part.</summary>
    public float Y;

    /// <summary>The Z component of the vector part.</summary>
    public float Z;

    /// <summary>The scalar (real) part of the quaternion.</summary>
    public float W;

    /// <summary>The identity quaternion representing no rotation.</summary>
    public static readonly Quaternion Identity = new(0f, 0f, 0f, 1f);

    /// <summary>
    /// Initializes a new <see cref="Quaternion"/> with the specified components.
    /// </summary>
    /// <param name="x">The X component of the vector part.</param>
    /// <param name="y">The Y component of the vector part.</param>
    /// <param name="z">The Z component of the vector part.</param>
    /// <param name="w">The scalar (real) part.</param>
    [DebuggerStepThrough]
    public Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// Returns the magnitude (norm) of the quaternion.
    /// </summary>
    public float Magnitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    /// <summary>
    /// Returns the squared magnitude of the quaternion, avoiding a square root.
    /// </summary>
    public float MagnitudeSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z + W * W;
    }

    /// <summary>
    /// Returns a normalized (unit) copy of this quaternion.
    /// Returns <see cref="Identity"/> if the magnitude is zero.
    /// </summary>
    public Quaternion Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var len = Magnitude;
            return len == 0f ? Identity : new(X / len, Y / len, Z / len, W / len);
        }
    }

    /// <summary>
    /// Returns the conjugate of the quaternion (negates the vector part).
    /// For a unit quaternion, the conjugate equals the inverse.
    /// </summary>
    public Quaternion Conjugate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(-X, -Y, -Z, W);
    }

    /// <summary>
    /// Returns the inverse of the quaternion.
    /// For a unit quaternion, prefer <see cref="Conjugate"/> which is faster.
    /// </summary>
    public Quaternion Inverse
    {
        get
        {
            var mag2 = MagnitudeSquared;
            if (mag2 == 0f) return Identity;
            return new(-X / mag2, -Y / mag2, -Z / mag2, W / mag2);
        }
    }

    /// <summary>
    /// Creates a quaternion representing a rotation of <paramref name="angle"/> radians
    /// around the given <paramref name="axis"/>.
    /// </summary>
    /// <param name="axis">The axis of rotation (should be normalized).</param>
    /// <param name="angle">The rotation angle in radians.</param>
    [DebuggerStepThrough]
    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
        float half = angle * 0.5f;
        float s = MathF.Sin(half);
        float c = MathF.Cos(half);
        var n = axis.Normalized;
        return new(n.X * s, n.Y * s, n.Z * s, c);
    }

    /// <summary>
    /// Creates a quaternion from Euler angles (in radians) applied in
    /// Z (roll) → X (pitch) → Y (yaw) order (intrinsic rotations).
    /// </summary>
    /// <param name="pitch">Rotation around the X axis in radians.</param>
    /// <param name="yaw">Rotation around the Y axis in radians.</param>
    /// <param name="roll">Rotation around the Z axis in radians.</param>
    [DebuggerStepThrough]
    public static Quaternion FromEuler(float pitch, float yaw, float roll)
    {
        float hp = pitch * 0.5f;
        float hy = yaw * 0.5f;
        float hr = roll * 0.5f;

        float sp = MathF.Sin(hp), cp = MathF.Cos(hp);
        float sy = MathF.Sin(hy), cy = MathF.Cos(hy);
        float sr = MathF.Sin(hr), cr = MathF.Cos(hr);

        return new(
            cy * sp * cr + sy * cp * sr,
            sy * cp * cr - cy * sp * sr,
            cy * cp * sr - sy * sp * cr,
            cy * cp * cr + sy * sp * sr
        );
    }

    /// <summary>
    /// Returns the dot product of two quaternions.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Quaternion a, Quaternion b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

    /// <summary>
    /// Linearly interpolates between two quaternions and normalizes the result.
    /// </summary>
    /// <param name="a">The start quaternion at t = 0.</param>
    /// <param name="b">The end quaternion at t = 1.</param>
    /// <param name="t">The interpolation factor, typically in the range [0, 1].</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
    {
        float inv = 1f - t;
        var result = new Quaternion(
            a.X * inv + b.X * t,
            a.Y * inv + b.Y * t,
            a.Z * inv + b.Z * t,
            a.W * inv + b.W * t
        );
        return result.Normalized;
    }

    /// <summary>
    /// Performs spherical linear interpolation (Slerp) between two unit quaternions.
    /// Produces a constant-speed rotation along the shortest arc on the unit 4-sphere.
    /// </summary>
    /// <param name="a">The start rotation at t = 0.</param>
    /// <param name="b">The end rotation at t = 1.</param>
    /// <param name="t">The interpolation factor, typically in the range [0, 1].</param>
    [DebuggerStepThrough]
    public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
    {
        float dot = Dot(a, b);

        // Ensure shortest path by negating b when dot is negative.
        if (dot < 0f)
        {
            b = new(-b.X, -b.Y, -b.Z, -b.W);
            dot = -dot;
        }

        // Fall back to linear interpolation for nearly parallel quaternions.
        const float threshold = 0.9995f;
        if (dot > threshold)
            return Lerp(a, b, t);

        float angle = MathF.Acos(dot);
        float sinAngle = MathF.Sin(angle);
        float sa = MathF.Sin((1f - t) * angle) / sinAngle;
        float sb = MathF.Sin(t * angle) / sinAngle;

        return new(
            sa * a.X + sb * b.X,
            sa * a.Y + sb * b.Y,
            sa * a.Z + sb * b.Z,
            sa * a.W + sb * b.W
        );
    }

    /// <summary>
    /// Multiplies two quaternions, composing their rotations.
    /// The result applies <paramref name="rhs"/> first, then <paramref name="lhs"/>.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion operator *(Quaternion lhs, Quaternion rhs) =>
        new(
            lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y,
            lhs.W * rhs.Y - lhs.X * rhs.Z + lhs.Y * rhs.W + lhs.Z * rhs.X,
            lhs.W * rhs.Z + lhs.X * rhs.Y - lhs.Y * rhs.X + lhs.Z * rhs.W,
            lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z
        );

    /// <summary>
    /// Rotates a <see cref="Vector3"/> by this quaternion using the sandwich product:
    /// <c>v' = q * v * q⁻¹</c>.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector3 operator *(Quaternion q, Vector3 v)
    {
        // Optimized rotation: v' = v + 2w(q.xyz × v) + 2(q.xyz × (q.xyz × v))
        var qv = new Vector3(q.X, q.Y, q.Z);
        var t = 2f * Vector3.Cross(qv, v);
        return v + q.W * t + Vector3.Cross(qv, t);
    }

    /// <summary>
    /// Rotates a <see cref="Point3"/> by this quaternion.
    /// </summary>
    [DebuggerStepThrough]
    public static Point3 operator *(Quaternion q, Point3 p) =>
        (Point3)(q * (Vector3)p);

    /// <summary>
    /// Converts this quaternion to an equivalent 4×4 rotation matrix.
    /// </summary>
    [DebuggerStepThrough]
    public Matrix4x4 ToMatrix4x4()
    {
        float xx = X * X, yy = Y * Y, zz = Z * Z;
        float xy = X * Y, xz = X * Z, yz = Y * Z;
        float wx = W * X, wy = W * Y, wz = W * Z;

        return new Matrix4x4(
            1f - 2f * (yy + zz),  2f * (xy - wz),        2f * (xz + wy),        0f,
            2f * (xy + wz),        1f - 2f * (xx + zz),  2f * (yz - wx),        0f,
            2f * (xz - wy),        2f * (yz + wx),        1f - 2f * (xx + yy),  0f,
            0f,                    0f,                    0f,                    1f
        );
    }

    /// <summary>
    /// Decomposes this quaternion into Euler angles (pitch, yaw, roll) in radians.
    /// </summary>
    /// <remarks>
    /// The angles are in the range [-π, π] for pitch, yaw, and roll.
    /// Gimbal lock may occur near the poles of pitch (±90°).
    /// </remarks>
    [DebuggerStepThrough]
    public (float pitch, float yaw, float roll) ToEuler()
    {
        // Pitch (X axis)
        float sinP = 2f * (W * X - Y * Z);
        float pitch = MathF.Abs(sinP) >= 1f
            ? MathF.CopySign(MathF.PI / 2f, sinP)
            : MathF.Asin(sinP);

        // Yaw (Y axis)
        float yaw = MathF.Atan2(
            2f * (W * Y + X * Z),
            1f - 2f * (X * X + Y * Y));

        // Roll (Z axis)
        float roll = MathF.Atan2(
            2f * (W * Z + X * Y),
            1f - 2f * (X * X + Z * Z));

        return (pitch, yaw, roll);
    }

    /// <summary>Returns true if two quaternions have equal components.</summary>
    [DebuggerStepThrough]
    public static bool operator ==(Quaternion lhs, Quaternion rhs) =>
        lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;

    /// <summary>Returns true if any component of two quaternions differs.</summary>
    [DebuggerStepThrough]
    public static bool operator !=(Quaternion lhs, Quaternion rhs) =>
        lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;

    /// <summary>
    /// Implicitly converts this quaternion to a <see cref="System.Numerics.Quaternion"/>
    /// for use with SIMD-accelerated operations.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Quaternion(Quaternion q) =>
        new(q.X, q.Y, q.Z, q.W);

    /// <summary>
    /// Implicitly converts a <see cref="System.Numerics.Quaternion"/> to a <see cref="Quaternion"/>.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Quaternion(System.Numerics.Quaternion q) =>
        new(q.X, q.Y, q.Z, q.W);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Quaternion other && this == other;

    /// <summary>Returns true if this quaternion equals another.</summary>
    public bool Equals(Quaternion other) => this == other;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    /// <inheritdoc/>
    public override string ToString() => $"Quaternion [X={X}, Y={Y}, Z={Z}, W={W}]";
}
