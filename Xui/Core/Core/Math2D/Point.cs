using System.Runtime.CompilerServices;

namespace Xui.Core.Math2D;

/// <summary>
/// Represents a point in 2D space, defined by its <see cref="X"/> and <see cref="Y"/> coordinates.
/// </summary>
public struct Point
{
    /// <summary>The X-coordinate of the point.</summary>
    public nfloat X;

    /// <summary>The Y-coordinate of the point.</summary>
    public nfloat Y;

    /// <summary>A point at the origin (0, 0).</summary>
    public static readonly Point Zero = (0, 0);

    /// <summary>
    /// A point at (1, 1), useful for normalized coordinates or vector math.
    /// </summary>
    public static readonly Point One = (1, 1);

    /// <summary>
    /// Initializes a new point with the specified coordinates.
    /// </summary>
    [DebuggerStepThrough]
    public Point(nfloat x, nfloat y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Linearly interpolates between two points by <paramref name="step"/>.
    /// </summary>
    /// <param name="start">The start point.</param>
    /// <param name="end">The end point.</param>
    /// <param name="step">A value from 0 to 1 indicating the interpolation position.</param>
    [DebuggerStepThrough]
    public static Point Lerp(Point start, Point end, nfloat step)
    {
        nfloat stepMinusOne = 1f - step;
        return new (start.X * stepMinusOne + end.X * step, start.Y * stepMinusOne + end.Y * step);
    }

    /// <summary>
    /// Returns the Euclidean distance between two points.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat Distance(Point a, Point b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        return nfloat.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Returns the squared Euclidean distance between two points (no square root).
    /// Useful for performance comparisons or ordering by proximity.
    /// </summary>
    [DebuggerStepThrough]
    public static nfloat SquaredDistance(Point a, Point b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Returns the Manhattan (taxicab) distance between two points.
    /// </summary>
    /// <remarks>
    /// The sum of the absolute horizontal and vertical distances. 
    /// Often used in grid-based or discrete movement systems.
    /// </remarks>
    [DebuggerStepThrough]
    public static nfloat TaxicabDistance(Point a, Point b)
    {
        return nfloat.Abs(b.X - a.X) + nfloat.Abs(b.Y - a.Y);
    }

    /// <summary>
    /// Returns the vector difference from one point to another.
    /// </summary>
    [DebuggerStepThrough]
    public static Vector operator-(Point lhs, Point rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    /// <summary>
    /// Offsets a point by a vector.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator+(Point lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);

    /// <summary>
    /// Subtracts a vector from a point, offsetting it in the opposite direction.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator-(Point lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    /// <summary>
    /// Converts a tuple to a point.
    /// </summary>
    [DebuggerStepThrough]
    public static implicit operator Point(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);

    /// <summary>
    /// Explicitly converts a vector to a point (drops directional semantics).
    /// </summary>
    [DebuggerStepThrough]
    public static explicit operator Point(Vector vector) => new (vector.X, vector.Y);

    /// <summary>
    /// Determines whether two points have equal coordinates.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator==(Point lhs, Point rhs) =>
        lhs.X == rhs.X && lhs.Y == rhs.Y;

    /// <summary>
    /// Determines whether two points differ in any coordinate.
    /// </summary>
    [DebuggerStepThrough]
    public static bool operator!=(Point lhs, Point rhs) =>
        lhs.X != rhs.X || lhs.Y != rhs.Y;

    /// <summary>
    /// Apply an uniform scale to a point.
    /// </summary>
    [DebuggerStepThrough]
    public static Point operator*(Point v, nfloat f) => new (v.X * f, v.Y * f);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Point [X={X}, Y={Y}]";

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Point other && this == other;

    /// <summary>
    /// Returns true if this point has the same coordinates as another.
    /// </summary>
    public bool Equals(Point other) =>
        this == other;

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(X, Y);
}