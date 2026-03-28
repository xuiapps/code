using Xui.Core.Math3D;

namespace Xui.Core.Curves3D;

/// <summary>
/// Represents a quadratic (degree-2) Bézier curve in 3D space, defined by a start point,
/// one control point, and an end point.
/// </summary>
/// <remarks>
/// A quadratic Bézier provides smooth interpolation from <see cref="P0"/> to <see cref="P2"/>,
/// pulled toward the control point <see cref="P1"/>. Points are computed using
/// De Casteljau's algorithm for numerical stability.
/// </remarks>
public readonly struct QuadraticBezier3 : ICurve3
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point3 P0;

    /// <summary>The control point, which influences the curvature.</summary>
    public readonly Point3 P1;

    /// <summary>The ending point of the curve.</summary>
    public readonly Point3 P2;

    /// <summary>
    /// Initializes a new <see cref="QuadraticBezier3"/> with the specified control points.
    /// </summary>
    public QuadraticBezier3(Point3 p0, Point3 p1, Point3 p2)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
    }

    /// <summary>
    /// Evaluates the curve at parameter <paramref name="t"/> using De Casteljau's algorithm.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point3 Lerp(float t) =>
        Point3.Lerp(
            Point3.Lerp(P0, P1, t),
            Point3.Lerp(P1, P2, t),
            t
        );

    /// <summary>
    /// Returns the tangent vector of the curve at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Vector3 Tangent(float t) =>
        Vector3.Lerp(P1 - P0, P2 - P1, t) * 2f;

    /// <inheritdoc/>
    public Point3 this[float t] => Lerp(t);

    /// <summary>
    /// Approximates the arc length using 16 uniform samples.
    /// </summary>
    public float Length()
    {
        float length = 0f;
        var prev = Lerp(0f);
        for (int i = 1; i <= 16; i++)
        {
            var curr = Lerp(i / 16f);
            length += Point3.Distance(prev, curr);
            prev = curr;
        }
        return length;
    }

    /// <summary>
    /// Approximates the arc length using adaptive subdivision until the error per
    /// segment is within <paramref name="precision"/>.
    /// </summary>
    public float Length(float precision) =>
        AdaptiveLength(P0, P1, P2, precision);

    private static float AdaptiveLength(Point3 p0, Point3 p1, Point3 p2, float tolerance)
    {
        var chord = Point3.Distance(p0, p2);
        var control = Point3.Distance(p0, p1) + Point3.Distance(p1, p2);
        if (control - chord <= tolerance)
            return (chord + control) * 0.5f;

        // Subdivide at t = 0.5 using De Casteljau.
        var m01 = Point3.Lerp(p0, p1, 0.5f);
        var m12 = Point3.Lerp(p1, p2, 0.5f);
        var mid = Point3.Lerp(m01, m12, 0.5f);

        return AdaptiveLength(p0, m01, mid, tolerance * 0.5f)
             + AdaptiveLength(mid, m12, p2, tolerance * 0.5f);
    }

    /// <summary>
    /// Subdivides this curve at parameter <paramref name="t"/> using De Casteljau's algorithm,
    /// returning the left and right sub-curves.
    /// </summary>
    /// <param name="t">The split parameter in the range (0, 1).</param>
    public (QuadraticBezier3 left, QuadraticBezier3 right) Subdivide(float t)
    {
        var m01 = Point3.Lerp(P0, P1, t);
        var m12 = Point3.Lerp(P1, P2, t);
        var mid = Point3.Lerp(m01, m12, t);
        return (new(P0, m01, mid), new(mid, m12, P2));
    }

    /// <summary>
    /// Elevates this quadratic curve to an equivalent <see cref="CubicBezier3"/>.
    /// </summary>
    public CubicBezier3 ToCubic() =>
        new(
            P0,
            P0 + (P1 - P0) * (2f / 3f),
            P2 + (P1 - P2) * (2f / 3f),
            P2
        );

    /// <inheritdoc/>
    public override string ToString() => $"QuadraticBezier3 [{P0}, {P1}, {P2}]";
}
