using Xui.Core.Math3D;

namespace Xui.Core.Curves3D;

/// <summary>
/// Provides factory methods for constructing 3D Bézier curves.
/// </summary>
public static class Bezier3
{
    /// <summary>
    /// Creates a linear (degree-1) Bézier curve — a straight line from
    /// <paramref name="p0"/> to <paramref name="p1"/>.
    /// </summary>
    public static LinearBezier3 Linear(Point3 p0, Point3 p1) =>
        new(p0, p1);

    /// <summary>
    /// Creates a quadratic (degree-2) Bézier curve with one control point.
    /// </summary>
    /// <param name="p0">The start point.</param>
    /// <param name="p1">The control point.</param>
    /// <param name="p2">The end point.</param>
    public static QuadraticBezier3 Quadratic(Point3 p0, Point3 p1, Point3 p2) =>
        new(p0, p1, p2);

    /// <summary>
    /// Creates a cubic (degree-3) Bézier curve with two control points.
    /// </summary>
    /// <param name="p0">The start point.</param>
    /// <param name="p1">The first control point.</param>
    /// <param name="p2">The second control point.</param>
    /// <param name="p3">The end point.</param>
    public static CubicBezier3 Cubic(Point3 p0, Point3 p1, Point3 p2, Point3 p3) =>
        new(p0, p1, p2, p3);
}
