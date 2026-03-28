using System.Runtime.CompilerServices;

namespace Xui.Core.Math3D;

/// <summary>
/// Represents a point in 3D space defined by its X, Y, and Z coordinates.
/// </summary>
/// <remarks>
/// A <see cref="Point3"/> denotes a position. The difference of two points yields a
/// <see cref="Vector3"/>. Adding a <see cref="Vector3"/> to a <see cref="Point3"/> offsets it.
/// </remarks>
public struct Point3
{
    /// <summary>The X coordinate of the point.</summary>
    public float X;

    /// <summary>The Y coordinate of the point.</summary>
    public float Y;

    /// <summary>The Z coordinate of the point.</summary>
    public float Z;

    /// <summary>A point at the origin (0, 0, 0).</summary>
    public static readonly Point3 Zero = (0f, 0f, 0f);

    /// <summary>A point at (1, 1, 1).</summary>
    public static readonly Point3 One = (1f, 1f, 1f);

    /// <summary>
    /// Initializes a new <see cref="Point3"/> with the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="z">The Z coordinate.</param>
    [DebuggerStepThrough]
    public Point3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Linearly interpolates between two points by the factor <paramref name="t"/>.
    /// </summary>
    /// <param name="start">The start point at t = 0.</param>
    /// <param name="end">The end point at t = 1.</param>
    /// <param name="t">The interpolation factor, typically in the range [0, 1].</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 Lerp(Point3 start, Point3 end, float t)
    {
        float inv = 1f - t;
        return new(start.X * inv + end.X * t, start.Y * inv + end.Y * t, start.Z * inv + end.Z * t);
    }

    /// <summary>
    /// Returns the Euclidean distance between two points.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Point3 a, Point3 b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        var dz = b.Z - a.Z;
        return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    /// <summary>
    /// Returns the squared Euclidean distance between two points (no square root).
    /// Useful for performance comparisons or ordering by proximity.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SquaredDistance(Point3 a, Point3 b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        var dz = b.Z - a.Z;
        return dx * dx + dy * dy + dz * dz;
    }

    /// <summary>
    /// Returns the Manhattan (taxicab) distance between two points.
    /// This is the sum of the absolute differences of their coordinates.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TaxicabDistance(Point3 a, Point3 b) =>
        MathF.Abs(b.X - a.X) + MathF.Abs(b.Y - a.Y) + MathF.Abs(b.Z - a.Z);

    /// <summary>
    /// Returns the vector from <paramref name="rhs"/> to <paramref name="lhs"/>.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Point3 lhs, Point3 rhs) =>
        new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);

    /// <summary>
    /// Offsets a point by a vector.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator +(Point3 p, Vector3 v) =>
        new(p.X + v.X, p.Y + v.Y, p.Z + v.Z);

    /// <summary>
    /// Offsets a point in the opposite direction of a vector.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator -(Point3 p, Vector3 v) =>
        new(p.X - v.X, p.Y - v.Y, p.Z - v.Z);

    /// <summary>
    /// Applies a uniform scale to a point relative to the origin.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point3 operator *(Point3 p, float f) =>
        new(p.X * f, p.Y * f, p.Z * f);

    /// <summary>Determines whether two points have equal coordinates.</summary>
    [DebuggerStepThrough]
    public static bool operator ==(Point3 lhs, Point3 rhs) =>
        lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

    /// <summary>Determines whether two points differ in any coordinate.</summary>
    [DebuggerStepThrough]
    public static bool operator !=(Point3 lhs, Point3 rhs) =>
        lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

    /// <summary>Implicitly converts a tuple to a <see cref="Point3"/>.</summary>
    [DebuggerStepThrough]
    public static implicit operator Point3((float x, float y, float z) t) =>
        new(t.x, t.y, t.z);

    /// <summary>Explicitly converts a <see cref="Vector3"/> to a <see cref="Point3"/>.</summary>
    [DebuggerStepThrough]
    public static explicit operator Point3(Vector3 v) =>
        new(v.X, v.Y, v.Z);

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Point3 other && this == other;

    /// <summary>Returns true if this point has the same coordinates as another.</summary>
    public bool Equals(Point3 other) => this == other;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc/>
    public override string ToString() => $"Point3 [X={X}, Y={Y}, Z={Z}]";
}
