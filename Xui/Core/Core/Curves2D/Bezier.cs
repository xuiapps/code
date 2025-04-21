using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Static helper methods for constructing Bézier curves.
/// </summary>
public static class Bezier
{
    /// <summary>
    /// Creates a linear Bézier curve from two points.
    /// </summary>
    public static LinearBezier Linear(Point p0, Point p1) =>
        new(p0, p1);

    /// <summary>
    /// Creates a quadratic Bézier curve from three control points.
    /// </summary>
    public static QuadraticBezier Quadratic(Point p0, Point p1, Point p2) =>
        new(p0, p1, p2);

    /// <summary>
    /// Creates a cubic Bézier curve from four control points.
    /// </summary>
    public static CubicBezier Cubic(Point p0, Point p1, Point p2, Point p3) =>
        new(p0, p1, p2, p3);
}