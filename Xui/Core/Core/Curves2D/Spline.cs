using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Provides factory methods for creating spline segments of various degrees.
/// </summary>
public static class Spline
{
    /// <summary>
    /// Creates a linear spline (degree 1) between two points.
    /// </summary>
    public static LinearSpline Linear(Point p0, Point p1) =>
        new(p0, p1);

    /// <summary>
    /// Creates a quadratic spline (degree 2) between three points.
    /// </summary>
    public static QuadraticSpline Quadratic(Point p0, Point p1, Point p2) =>
        new(p0, p1, p2);

    /// <summary>
    /// Creates a cubic spline (degree 3) using Catmull–Rom interpolation through P1 and P2.
    /// </summary>
    /// <remarks>
    /// This produces a smooth segment influenced by surrounding control points <paramref name="p0"/> and <paramref name="p3"/>.
    /// </remarks>
    public static CubicSpline Cubic(Point p0, Point p1, Point p2, Point p3) =>
        new(p0, p1, p2, p3);
}
