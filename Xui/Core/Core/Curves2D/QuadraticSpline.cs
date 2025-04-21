using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a 3-point quadratic spline segment (parabolic arc) defined by a start point, control point, and end point.
/// </summary>
/// <remarks>
/// This spline uses the De Casteljau algorithm for interpolation and arc length approximation.
/// The curve passes through <see cref="P0"/> and <see cref="P2"/>, with <see cref="P1"/> acting as the control point that defines curvature.
/// </remarks>
public readonly struct QuadraticSpline : ICurve
{
    /// <summary>The start point of the spline.</summary>
    public readonly Point P0;

    /// <summary>The control point that defines the curvature of the arc.</summary>
    public readonly Point P1;

    /// <summary>The end point of the spline.</summary>
    public readonly Point P2;

    /// <summary>
    /// Initializes a new <see cref="QuadraticSpline"/> with the given start, control, and end points.
    /// </summary>
    public QuadraticSpline(Point p0, Point p1, Point p2)
    {
        this.P0 = p0;
        this.P1 = p1;
        this.P2 = p2;
    }

    /// <summary>
    /// Computes the point on the spline at parameter <paramref name="t"/> using De Casteljau interpolation.
    /// </summary>
    /// <param name="t">A normalized parameter between 0 and 1.</param>
    public Point Lerp(nfloat t) =>
        Point.Lerp(
            Point.Lerp(P0, P1, t),
            Point.Lerp(P1, P2, t),
            t
        );

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name="t"/> on the spline.
    /// </summary>
    /// <param name="t">A normalized parameter between 0 and 1.</param>
    public Vector Tangent(nfloat t) =>
        Vector.Lerp(P1 - P0, P2 - P1, t) * 2;

    /// <summary>
    /// Approximates the total arc length of the spline using a fixed 16-sample De Casteljau subdivision.
    /// </summary>
    public nfloat Length()
    {
        nfloat length = 0;
        var previous = this[0];
        for (int i = 1; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var current = this[t];
            length += Point.Distance(previous, current);
            previous = current;
        }
        return length;
    }

    /// <summary>
    /// Approximates the arc length with adaptive subdivision using the specified precision tolerance.
    /// </summary>
    /// <param name="precision">The maximum allowed error in the approximation.</param>
    public nfloat Length(nfloat precision) =>
        Length(P0, P1, P2, precision);

    private static nfloat Length(Point a, Point b, Point c, nfloat tolerance)
    {
        var chord = Point.Distance(a, c);
        var control = Point.Distance(a, b) + Point.Distance(b, c);

        if (control - chord <= tolerance)
            return (chord + control) / 2;

        // Subdivide using De Casteljau
        var ab = Point.Lerp(a, b, 0.5f);
        var bc = Point.Lerp(b, c, 0.5f);
        var mid = Point.Lerp(ab, bc, 0.5f);

        return
            Length(a, ab, mid, tolerance) +
            Length(mid, bc, c, tolerance);
    }

    /// <summary>
    /// Evaluates the point on the spline at parameter <paramref name="t"/>.
    /// </summary>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Converts this spline to a <see cref="QuadraticBezier"/> curve with the same control points.
    /// </summary>
    public static implicit operator QuadraticBezier(QuadraticSpline s) =>
        new(s.P0, s.P1, s.P2);
}
