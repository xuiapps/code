using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a linear spline segment between two points.
/// </summary>
/// <remarks>
/// A linear spline defines a straight-line interpolation between <see cref="P0"/> and <see cref="P1"/>.
/// It is the simplest form of spline, useful for polyline paths, control handles, and degenerate Bézier cases.
/// </remarks>
public readonly struct LinearSpline : ICurve
{
    /// <summary>
    /// The starting point of the line segment.
    /// </summary>
    public readonly Point P0;

    /// <summary>
    /// The ending point of the line segment.
    /// </summary>
    public readonly Point P1;

    /// <summary>
    /// Initializes a new <see cref="LinearSpline"/> segment between two points.
    /// </summary>
    /// <param name="p0">The starting point of the spline.</param>
    /// <param name="p1">The ending point of the spline.</param>
    public LinearSpline(Point p0, Point p1)
    {
        this.P0 = p0;
        this.P1 = p1;
    }

    /// <summary>
    /// Interpolates a point on the segment at the specified parameter.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The interpolated point on the line.</returns>
    public Point Lerp(nfloat t) => Point.Lerp(P0, P1, t);

    /// <summary>
    /// Gets the constant tangent vector of the line segment.
    /// </summary>
    /// <param name="t">A normalized parameter (unused, since the tangent is constant).</param>
    public Vector Tangent(nfloat t) => P1 - P0;

    /// <summary>
    /// Evaluates the point on the spline at the specified parameter.
    /// </summary>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Returns the length of the segment.
    /// </summary>
    public nfloat Length() => Point.Distance(P0, P1);

    /// <summary>
    /// Returns the length of the segment. Precision is ignored for linear splines.
    /// </summary>
    public nfloat Length(nfloat precision) => Length();

    /// <summary>
    /// Converts this linear spline to a <see cref="LinearBezier"/>.
    /// </summary>
    public static implicit operator LinearBezier(LinearSpline s) =>
        new(s.P0, s.P1);
}
