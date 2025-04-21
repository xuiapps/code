using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a 4-point spline segment using Catmull–Rom interpolation.
/// The curve interpolates smoothly between <see cref="P1"/> and <see cref="P2"/>.
/// </summary>
/// <remarks>
/// Catmull–Rom splines are useful for generating smooth curves through a sequence of points
/// with minimal configuration. This implementation produces a C¹-continuous cubic curve,
/// influenced by neighboring control points <see cref="P0"/> and <see cref="P3"/>.
/// The segment can also be converted into a <see cref="CubicBezier"/> for drawing or compatibility.
/// </remarks>
public readonly struct CubicSpline : ICurve
{
    /// <summary>The control point before the start of the curve.</summary>
    public readonly Point P0;

    /// <summary>The starting point of the interpolated segment.</summary>
    public readonly Point P1;

    /// <summary>The ending point of the interpolated segment.</summary>
    public readonly Point P2;

    /// <summary>The control point after the end of the curve.</summary>
    public readonly Point P3;

    /// <summary>
    /// Initializes a new Catmull–Rom spline segment from four control points.
    /// </summary>
    public CubicSpline(Point p0, Point p1, Point p2, Point p3)
    {
        this.P0 = p0;
        this.P1 = p1;
        this.P2 = p2;
        this.P3 = p3;
    }

    /// <summary>
    /// Evaluates the point on the spline at the given parameter <paramref name="t"/> using the Catmull–Rom formula.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point Lerp(nfloat t)
    {
        var t2 = t * t;
        var t3 = t2 * t;

        return new Point(
            0.5f * ((2 * P1.X) +
                   (-P0.X + P2.X) * t +
                   (2 * P0.X - 5 * P1.X + 4 * P2.X - P3.X) * t2 +
                   (-P0.X + 3 * P1.X - 3 * P2.X + P3.X) * t3),
            0.5f * ((2 * P1.Y) +
                   (-P0.Y + P2.Y) * t +
                   (2 * P0.Y - 5 * P1.Y + 4 * P2.Y - P3.Y) * t2 +
                   (-P0.Y + 3 * P1.Y - 3 * P2.Y + P3.Y) * t3)
        );
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name="t"/> by differentiating the spline.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Vector Tangent(nfloat t)
    {
        var t2 = t * t;

        return new Vector(
            0.5f * (
                (-P0.X + P2.X) +
                2 * (2 * P0.X - 5 * P1.X + 4 * P2.X - P3.X) * t +
                3 * (-P0.X + 3 * P1.X - 3 * P2.X + P3.X) * t2
            ),
            0.5f * (
                (-P0.Y + P2.Y) +
                2 * (2 * P0.Y - 5 * P1.Y + 4 * P2.Y - P3.Y) * t +
                3 * (-P0.Y + 3 * P1.Y - 3 * P2.Y + P3.Y) * t2
            )
        );
    }

    /// <summary>
    /// Approximates the arc length of the curve using uniform sampling with 16 segments.
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
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name="precision">The tolerance used to decide subdivision depth.</param>
    public nfloat Length(nfloat precision) =>
        Length(this[0], this[0.33f], this[0.66f], this[1], precision);

    private static nfloat Length(Point p0, Point p1, Point p2, Point p3, nfloat tolerance)
    {
        var chord = Point.Distance(p0, p3);
        var control = Point.Distance(p0, p1) + Point.Distance(p1, p2) + Point.Distance(p2, p3);

        if (control - chord <= tolerance)
            return (chord + control) / 2;

        var ab = Point.Lerp(p0, p1, 0.5f);
        var bc = Point.Lerp(p1, p2, 0.5f);
        var cd = Point.Lerp(p2, p3, 0.5f);

        var abbc = Point.Lerp(ab, bc, 0.5f);
        var bccd = Point.Lerp(bc, cd, 0.5f);
        var mid = Point.Lerp(abbc, bccd, 0.5f);

        return
            Length(p0, ab, abbc, mid, tolerance) +
            Length(mid, bccd, cd, p3, tolerance);
    }

    /// <summary>
    /// Indexer-style access to evaluate the curve at a given parameter.
    /// </summary>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Converts this Catmull–Rom spline segment to an equivalent <see cref="CubicBezier"/>.
    /// The resulting curve interpolates between <see cref="P1"/> and <see cref="P2"/>.
    /// </summary>
    public static implicit operator CubicBezier(CubicSpline s)
    {
        var c1 = s.P1 + (s.P2 - s.P0) * (1f / 6f);
        var c2 = s.P2 - (s.P3 - s.P1) * (1f / 6f);
        return new CubicBezier(s.P1, c1, c2, s.P2);
    }
}
