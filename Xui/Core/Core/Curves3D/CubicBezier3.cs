using Xui.Core.Math3D;

namespace Xui.Core.Curves3D;

/// <summary>
/// Represents a cubic (degree-3) Bézier curve in 3D space, defined by a start point,
/// two control points, and an end point.
/// </summary>
/// <remarks>
/// A cubic Bézier provides smooth interpolation from <see cref="P0"/> to <see cref="P3"/>,
/// shaped by the two interior control points <see cref="P1"/> and <see cref="P2"/>.
/// Points are computed using De Casteljau's algorithm for numerical stability.
/// This is the standard curve type for 3D animation paths and splines.
/// </remarks>
public readonly struct CubicBezier3 : ICurve3
{
    /// <summary>The starting point of the curve.</summary>
    public readonly Point3 P0;

    /// <summary>The first control point, influencing the curve near <see cref="P0"/>.</summary>
    public readonly Point3 P1;

    /// <summary>The second control point, influencing the curve near <see cref="P3"/>.</summary>
    public readonly Point3 P2;

    /// <summary>The ending point of the curve.</summary>
    public readonly Point3 P3;

    /// <summary>
    /// Initializes a new <see cref="CubicBezier3"/> with the specified control points.
    /// </summary>
    public CubicBezier3(Point3 p0, Point3 p1, Point3 p2, Point3 p3)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    /// <summary>
    /// Evaluates the curve at parameter <paramref name="t"/> using De Casteljau's algorithm.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point3 Lerp(float t) =>
        Point3.Lerp(
            Point3.Lerp(
                Point3.Lerp(P0, P1, t),
                Point3.Lerp(P1, P2, t),
                t),
            Point3.Lerp(
                Point3.Lerp(P1, P2, t),
                Point3.Lerp(P2, P3, t),
                t),
            t
        );

    /// <summary>
    /// Returns the tangent vector of the curve at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Vector3 Tangent(float t) =>
        Vector3.Lerp(
            Vector3.Lerp(P1 - P0, P2 - P1, t),
            Vector3.Lerp(P2 - P1, P3 - P2, t),
            t
        ) * 3f;

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
        AdaptiveLength(P0, P1, P2, P3, precision);

    private static float AdaptiveLength(Point3 p0, Point3 p1, Point3 p2, Point3 p3, float tolerance)
    {
        var chord = Point3.Distance(p0, p3);
        var control = Point3.Distance(p0, p1) + Point3.Distance(p1, p2) + Point3.Distance(p2, p3);
        if (control - chord <= tolerance)
            return (chord + control) * 0.5f;

        // Subdivide at t = 0.5 using De Casteljau.
        var m01 = Point3.Lerp(p0, p1, 0.5f);
        var m12 = Point3.Lerp(p1, p2, 0.5f);
        var m23 = Point3.Lerp(p2, p3, 0.5f);
        var m012 = Point3.Lerp(m01, m12, 0.5f);
        var m123 = Point3.Lerp(m12, m23, 0.5f);
        var mid = Point3.Lerp(m012, m123, 0.5f);

        return AdaptiveLength(p0, m01, m012, mid, tolerance * 0.5f)
             + AdaptiveLength(mid, m123, m23, p3, tolerance * 0.5f);
    }

    /// <summary>
    /// Subdivides this curve at parameter <paramref name="t"/> using De Casteljau's algorithm,
    /// returning the left and right sub-curves.
    /// </summary>
    /// <param name="t">The split parameter in the range (0, 1).</param>
    public (CubicBezier3 left, CubicBezier3 right) Subdivide(float t)
    {
        var m01 = Point3.Lerp(P0, P1, t);
        var m12 = Point3.Lerp(P1, P2, t);
        var m23 = Point3.Lerp(P2, P3, t);
        var m012 = Point3.Lerp(m01, m12, t);
        var m123 = Point3.Lerp(m12, m23, t);
        var mid = Point3.Lerp(m012, m123, t);
        return (new(P0, m01, m012, mid), new(mid, m123, m23, P3));
    }

    /// <summary>
    /// Returns a quadratic Bézier that approximates this cubic by averaging the
    /// endpoint tangents. Suitable as a fast approximation when subdivision is not required.
    /// </summary>
    public QuadraticBezier3 QuadraticApproximation
    {
        get
        {
            var v0 = (Vector3)P0;
            var v3 = (Vector3)P3;
            var v1 = (Vector3)P1;
            var v2 = (Vector3)P2;

            var tangentStart = 3f * (v1 - v0);
            var tangentEnd = 3f * (v3 - v2);
            var avgTangent = (tangentStart + tangentEnd) * 0.25f;
            var midpoint = Point3.Lerp(P0, P3, 0.5f);
            var approxControl = midpoint + avgTangent;

            return new(P0, approxControl, P3);
        }
    }

    /// <inheritdoc/>
    public override string ToString() => $"CubicBezier3 [{P0}, {P1}, {P2}, {P3}]";
}
