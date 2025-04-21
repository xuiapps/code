using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a quadratic Bézier curve defined by three control points.
/// </summary>
/// <remarks>
/// A quadratic Bézier curve smoothly interpolates between <see cref="P0"/> and <see cref="P2"/>.
/// The curve is influenced by a single control point <see cref="P1"/>.
/// Quadratic Béziers are widely used in vector graphics, font rendering, and easing functions.
/// </remarks>
public readonly struct QuadraticBezier : ICurve
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point P0;

    /// <summary>The control point that influences the curvature of the segment.</summary>
    public readonly Point P1;

    /// <summary>The ending point of the curve.</summary>
    public readonly Point P2;

    /// <summary>
    /// Initializes a new quadratic Bézier curve with the specified control points.
    /// </summary>
    public QuadraticBezier(Point p0, Point p1, Point p2)
    {
        this.P0 = p0;
        this.P1 = p1;
        this.P2 = p2;
    }

    /// <summary>
    /// Evaluates the curve at parameter <paramref name="t"/> using De Casteljau’s algorithm.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The interpolated point on the curve.</returns>
    public Point Lerp(nfloat t) =>
        Point.Lerp(
            Point.Lerp(P0, P1, t),
            Point.Lerp(P1, P2, t),
            t);

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    /// <returns>The tangent vector at the specified point.</returns>
    public Vector Tangent(nfloat t) =>
        Vector.Lerp(P1 - P0, P2 - P1, t) * 2;

    /// <summary>
    /// Approximates the arc length using uniform sampling over 16 segments.
    /// </summary>
    /// <returns>The approximate total length of the curve.</returns>
    public nfloat Length()
    {
        nfloat length = 0;
        var previous = Lerp(0);
        for (int i = 1; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var current = Lerp(t);
            length += Point.Distance(previous, current);
            previous = current;
        }
        return length;
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name="precision">The maximum allowed error for curve flatness.</param>
    public nfloat Length(nfloat precision) =>
        Length(P0, P1, P2, precision);

    private static nfloat Length(Point a, Point b, Point c, nfloat tolerance)
    {
        var chord = Point.Distance(a, c);
        var control = Point.Distance(a, b) + Point.Distance(b, c);

        if (control - chord <= tolerance)
            return (chord + control) / 2;

        var ab = Point.Lerp(a, b, 0.5f);
        var bc = Point.Lerp(b, c, 0.5f);
        var mid = Point.Lerp(ab, bc, 0.5f);

        return
            Length(a, ab, mid, tolerance) +
            Length(mid, bc, c, tolerance);
    }

    /// <summary>
    /// Evaluates the point on the curve at the specified parameter.
    /// </summary>
    public Point this[nfloat t] => Lerp(t);

    /// <summary>
    /// Converts a tuple of 3 points to a <see cref="QuadraticBezier"/>.
    /// </summary>
    public static implicit operator QuadraticBezier((Point p0, Point p1, Point p2) value) =>
        new(value.p0, value.p1, value.p2);

    /// <summary>
    /// Converts a <see cref="QuadraticSpline"/> to a <see cref="QuadraticBezier"/>.
    /// </summary>
    public static implicit operator QuadraticBezier(QuadraticSpline spline) =>
        new(spline.P0, spline.P1, spline.P2);

    /// <summary>
    /// Adjusts the control point to drag the curve at <paramref name="t"/> by the given delta.
    /// Keeps <see cref="P0"/> and <see cref="P2"/> fixed.
    /// </summary>
    public QuadraticBezier Drag(nfloat t, Vector delta)
    {
        nfloat w = 2 * t * (1 - t); // Influence weight of P1 at t
        return new QuadraticBezier(
            P0,
            P1 + delta / w,
            P2
        );
    }

    /// <summary>
    /// Adjusts the control point so that the curve is moved near <paramref name="origin"/> by <paramref name="delta"/>.
    /// Uses coarse sampling to find the closest point.
    /// </summary>
    public QuadraticBezier DragAt(Point origin, Vector delta)
    {
        var t = ClosestT(origin);
        return Drag(t, delta);
    }

    /// <summary>
    /// Adjusts the control point so that the curve is moved near <paramref name="origin"/> by <paramref name="delta"/>.
    /// Uses adaptive precision to locate the closest point.
    /// </summary>
    public QuadraticBezier DragAt(Point origin, Vector delta, nfloat precision)
    {
        var t = ClosestT(origin, precision);
        return Drag(t, delta);
    }

    /// <summary>
    /// Finds the <c>t</c> value where the curve is closest to <paramref name="target"/>.
    /// Uses uniform sampling across 16 points.
    /// </summary>
    public nfloat ClosestT(Point target)
    {
        nfloat bestT = 0;
        nfloat bestDist = nfloat.MaxValue;

        for (int i = 0; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var pt = this[t];
            var dist = Point.SquaredDistance(pt, target);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestT = t;
            }
        }

        return bestT;
    }

    /// <summary>
    /// Finds the <c>t</c> value where the curve is closest to <paramref name="target"/>.
    /// Uses recursive ternary subdivision with the specified <paramref name="precision"/>.
    /// </summary>
    public nfloat ClosestT(Point target, nfloat precision)
    {
        return ClosestTRecursive(this, target, 0, 1, precision);
    }

    private static nfloat ClosestTRecursive(QuadraticBezier curve, Point target, nfloat t0, nfloat t1, nfloat precision)
    {
        var m0 = t0 + (t1 - t0) / 3;
        var m1 = t1 - (t1 - t0) / 3;

        var p0 = curve[m0];
        var p1 = curve[m1];

        var d0 = Point.SquaredDistance(p0, target);
        var d1 = Point.SquaredDistance(p1, target);

        if ((t1 - t0) < precision)
            return (d0 < d1) ? m0 : m1;

        return (d0 < d1)
            ? ClosestTRecursive(curve, target, t0, m1, precision)
            : ClosestTRecursive(curve, target, m0, t1, precision);
    }
}
