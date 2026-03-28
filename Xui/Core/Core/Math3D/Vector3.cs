using System.Runtime.CompilerServices;

namespace Xui.Core.Math3D;

/// <summary>
/// Represents a 3D vector with X, Y, and Z components.
/// Commonly used for geometric operations, physics, lighting, and 3D movement.
/// </summary>
/// <remarks>
/// The struct is laid out sequentially as three 32-bit floats (12 bytes), making it
/// directly compatible with SIMD operations and GPU shader inputs.
/// Use <see cref="System.Numerics.Vector3"/> for hardware-accelerated batch processing.
/// </remarks>
public struct Vector3
{
    /// <summary>The X component of the vector.</summary>
    public float X;

    /// <summary>The Y component of the vector.</summary>
    public float Y;

    /// <summary>The Z component of the vector.</summary>
    public float Z;

    /// <summary>
    /// Initializes a new <see cref="Vector3"/> with the specified components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    [DebuggerStepThrough]
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>A zero vector (0, 0, 0).</summary>
    public static readonly Vector3 Zero = (0f, 0f, 0f);

    /// <summary>A unit vector (1, 1, 1).</summary>
    public static readonly Vector3 One = (1f, 1f, 1f);

    /// <summary>A unit vector pointing right along the positive X axis (1, 0, 0).</summary>
    public static readonly Vector3 Right = (1f, 0f, 0f);

    /// <summary>A unit vector pointing left along the negative X axis (-1, 0, 0).</summary>
    public static readonly Vector3 Left = (-1f, 0f, 0f);

    /// <summary>A unit vector pointing up along the positive Y axis (0, 1, 0).</summary>
    public static readonly Vector3 Up = (0f, 1f, 0f);

    /// <summary>A unit vector pointing down along the negative Y axis (0, -1, 0).</summary>
    public static readonly Vector3 Down = (0f, -1f, 0f);

    /// <summary>
    /// A unit vector pointing forward along the negative Z axis (0, 0, -1) in a right-handed coordinate system.
    /// </summary>
    public static readonly Vector3 Forward = (0f, 0f, -1f);

    /// <summary>A unit vector pointing backward along the positive Z axis (0, 0, 1).</summary>
    public static readonly Vector3 Back = (0f, 0f, 1f);

    /// <summary>
    /// Returns a normalized (unit length) copy of this vector.
    /// Returns <see cref="Zero"/> if the vector has zero magnitude.
    /// </summary>
    public Vector3 Normalized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var len = Magnitude;
            return len == 0f ? Zero : new(X / len, Y / len, Z / len);
        }
    }

    /// <summary>
    /// Returns the magnitude (length) of the vector.
    /// </summary>
    public float Magnitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MathF.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Returns the squared magnitude of the vector, avoiding a square root computation.
    /// Useful for distance comparisons where the exact length is not needed.
    /// </summary>
    public float MagnitudeSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z;
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    /// <param name="lhs">The left-hand vector.</param>
    /// <param name="rhs">The right-hand vector.</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector3 lhs, Vector3 rhs) =>
        lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;

    /// <summary>
    /// Returns the cross product of two vectors.
    /// The result is a vector perpendicular to both inputs,
    /// with direction determined by the right-hand rule.
    /// </summary>
    /// <param name="lhs">The left-hand vector.</param>
    /// <param name="rhs">The right-hand vector.</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Cross(Vector3 lhs, Vector3 rhs) =>
        new(
            lhs.Y * rhs.Z - lhs.Z * rhs.Y,
            lhs.Z * rhs.X - lhs.X * rhs.Z,
            lhs.X * rhs.Y - lhs.Y * rhs.X
        );

    /// <summary>
    /// Projects <paramref name="lhs"/> onto <paramref name="rhs"/>.
    /// Returns <see cref="Zero"/> if <paramref name="rhs"/> has zero magnitude.
    /// </summary>
    /// <param name="lhs">The vector to project.</param>
    /// <param name="rhs">The target direction vector.</param>
    [DebuggerStepThrough]
    public static Vector3 Project(Vector3 lhs, Vector3 rhs)
    {
        var denominator = rhs.MagnitudeSquared;
        if (denominator == 0f) return Zero;
        return rhs * (Dot(lhs, rhs) / denominator);
    }

    /// <summary>
    /// Reflects the <paramref name="vector"/> across the given surface <paramref name="normal"/>.
    /// The normal is assumed to be unit length.
    /// </summary>
    /// <param name="vector">The incident vector to reflect.</param>
    /// <param name="normal">The surface normal (should be normalized).</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Reflect(Vector3 vector, Vector3 normal) =>
        vector - 2f * Dot(vector, normal) * normal;

    /// <summary>
    /// Returns the angle in radians between two vectors.
    /// Returns 0 if either vector has zero magnitude.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    [DebuggerStepThrough]
    public static float Angle(Vector3 lhs, Vector3 rhs)
    {
        var denominator = MathF.Sqrt(lhs.MagnitudeSquared * rhs.MagnitudeSquared);
        if (denominator == 0f) return 0f;
        var cosAngle = Math.Clamp(Dot(lhs, rhs) / denominator, -1f, 1f);
        return MathF.Acos(cosAngle);
    }

    /// <summary>
    /// Linearly interpolates between two vectors by the factor <paramref name="t"/>.
    /// </summary>
    /// <param name="start">The start vector at t = 0.</param>
    /// <param name="end">The end vector at t = 1.</param>
    /// <param name="t">The interpolation factor, typically in the range [0, 1].</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Lerp(Vector3 start, Vector3 end, float t)
    {
        float inv = 1f - t;
        return new(start.X * inv + end.X * t, start.Y * inv + end.Y * t, start.Z * inv + end.Z * t);
    }

    /// <summary>
    /// Returns the component-wise minimum of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Min(Vector3 lhs, Vector3 rhs) =>
        new(MathF.Min(lhs.X, rhs.X), MathF.Min(lhs.Y, rhs.Y), MathF.Min(lhs.Z, rhs.Z));

    /// <summary>
    /// Returns the component-wise maximum of two vectors.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Max(Vector3 lhs, Vector3 rhs) =>
        new(MathF.Max(lhs.X, rhs.X), MathF.Max(lhs.Y, rhs.Y), MathF.Max(lhs.Z, rhs.Z));

    /// <summary>Adds two vectors.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 lhs, Vector3 rhs) =>
        new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);

    /// <summary>Subtracts one vector from another.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 lhs, Vector3 rhs) =>
        new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);

    /// <summary>Negates a vector.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 v) =>
        new(-v.X, -v.Y, -v.Z);

    /// <summary>Multiplies a vector by a scalar.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 v, float f) =>
        new(v.X * f, v.Y * f, v.Z * f);

    /// <summary>Multiplies a scalar by a vector.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(float f, Vector3 v) =>
        new(v.X * f, v.Y * f, v.Z * f);

    /// <summary>Divides a vector by a scalar.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 v, float f) =>
        new(v.X / f, v.Y / f, v.Z / f);

    /// <summary>Multiplies two vectors component-wise.</summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 lhs, Vector3 rhs) =>
        new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);

    /// <summary>Returns true if two vectors have equal components.</summary>
    [DebuggerStepThrough]
    public static bool operator ==(Vector3 lhs, Vector3 rhs) =>
        lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

    /// <summary>Returns true if any component of two vectors differs.</summary>
    [DebuggerStepThrough]
    public static bool operator !=(Vector3 lhs, Vector3 rhs) =>
        lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

    /// <summary>Implicitly converts a tuple to a <see cref="Vector3"/>.</summary>
    [DebuggerStepThrough]
    public static implicit operator Vector3((float x, float y, float z) t) =>
        new(t.x, t.y, t.z);

    /// <summary>Implicitly converts a <see cref="Point3"/> to a <see cref="Vector3"/>.</summary>
    [DebuggerStepThrough]
    public static implicit operator Vector3(Point3 p) =>
        new(p.X, p.Y, p.Z);

    /// <summary>
    /// Implicitly converts this vector to a <see cref="System.Numerics.Vector3"/>
    /// for use with SIMD-accelerated operations.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator System.Numerics.Vector3(Vector3 v) =>
        new(v.X, v.Y, v.Z);

    /// <summary>
    /// Implicitly converts a <see cref="System.Numerics.Vector3"/> to a <see cref="Vector3"/>.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(System.Numerics.Vector3 v) =>
        new(v.X, v.Y, v.Z);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Vector3 other && this == other;

    /// <summary>Returns true if this vector equals another <see cref="Vector3"/>.</summary>
    public bool Equals(Vector3 other) => this == other;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc/>
    public override string ToString() => $"Vector3 [X={X}, Y={Y}, Z={Z}]";
}
