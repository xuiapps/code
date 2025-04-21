using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a linear Bézier curve, equivalent to a straight line between two points.
/// </summary>
/// <remarks>
/// This is the simplest form of Bézier curve, defined by two points <see cref="P0"/> and <see cref="P1"/>.
/// It is often used for edges of polygonal paths or degenerate forms of higher-order Béziers.
/// </remarks>
public readonly struct LinearBezier : ICurve
{
    /// <summary>The start point of the curve.</summary>
    public readonly Point P0;

    /// <summary>The end point of the curve.</summary>
    public readonly Point P1;

    /// <summary>
    /// Initializes a new <see cref="LinearBezier"/> curve with the given start and end points.
    /// </summary>
    /// <param name="p0">The starting point.</param>
    /// <param name="p1">The ending point.</param>
    public LinearBezier(Point p0, Point p1)
    {
        this.P0 = p0;
        this.P1 = p1;
    }

    /// <summary>
    /// Computes the point on the line at parameter <paramref name="t"/> using linear interpolation.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t) =>
        Point.Lerp(P0, P1, t);

    /// <summary>
    /// Returns the constant tangent vector of the line segment.
    /// </summary>
    /// <param name="t">The curve parameter (ignored since the tangent is constant).</param>
    public Vector Tangent(nfloat t) =>
        P1 - P0;

    /// <summary>
    /// Returns the exact length of the line segment.
    /// </summary>
    public nfloat Length() =>
        Point.Distance(P0, P1);

    /// <summary>
    /// Returns the length of the line segment. This overload ignores the <paramref name="precision"/> parameter.
    /// </summary>
    public nfloat Length(nfloat precision) =>
        Length();

    /// <summary>
    /// Indexer-style access to the interpolated point at parameter <paramref name="t"/>.
    /// </summary>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Implicitly converts a tuple of two points into a <see cref="LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier((Point p0, Point p1) value) =>
        new(value.p0, value.p1);

    /// <summary>
    /// Implicitly converts this <see cref="LinearBezier"/> to a <see cref="QuadraticBezier"/> curve.
    /// The intermediate point is set to the midpoint for visual smoothness.
    /// </summary>
    public static implicit operator QuadraticBezier(LinearBezier linear) =>
        new QuadraticBezier(
            linear.P0,
            linear[0.5f],
            linear.P1);

    /// <summary>
    /// Implicitly converts this <see cref="LinearBezier"/> to a <see cref="CubicBezier"/> curve.
    /// Intermediate control points are interpolated at 1/3 and 2/3 for smooth spacing.
    /// </summary>
    public static implicit operator CubicBezier(LinearBezier linear) =>
        new CubicBezier(
            linear.P0,
            linear[1 / (nfloat)3],
            linear[2 / (nfloat)3],
            linear.P1
        );

    /// <summary>
    /// Implicitly converts a <see cref="LinearSpline"/> to a <see cref="LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier(LinearSpline spline) =>
        new(spline.P0, spline.P1);
}
