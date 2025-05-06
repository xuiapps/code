using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a circular arc defined by three points: start, mid (on-curve), and end.
/// </summary>
/// <remarks>
/// The arc is guaranteed to pass through all three control points <see cref="P0"/>, <see cref="P1"/>, and <see cref="P2"/>.
/// The arc lies on the unique circle that intersects these points and is useful for geometric primitives,
/// shader-based SDF rendering, and polygonal arc approximation.
/// </remarks>
public readonly struct Arc3Point : ICurve
{
    /// <summary>The starting point of the arc.</summary>
    public readonly Point P0;

    /// <summary>A point on the arc between <see cref="P0"/> and <see cref="P2"/>.</summary>
    public readonly Point P1;

    /// <summary>The ending point of the arc.</summary>
    public readonly Point P2;

    /// <summary>
    /// Initializes a new <see cref="Arc3Point"/> defined by three on-curve points.
    /// </summary>
    public Arc3Point(Point p0, Point p1, Point p2)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
    }

    /// <summary>
    /// Converts this three-point arc into a center-based <see cref="Arc"/> if the arc is valid.
    /// </summary>
    /// <returns>
    /// A valid <see cref="Arc"/> if the input points define a circular segment; otherwise <c>null</c>.
    /// If the result is <c>null</c>, consider falling back to a straight <see cref="Segment"/> from <see cref="P0"/> to <see cref="P2"/>.
    /// </returns>
    public Arc? ToCenterArc()
    {
        var a = P0;
        var b = P1;
        var c = P2;

        var abMid = (a + b) * (nfloat).5;
        var bcMid = (b + c) * (nfloat).5;

        var abDir = (b - a).PerpendicularCCW.Normalized;
        var bcDir = (c - b).PerpendicularCCW.Normalized;

        if (abDir == (0, 0) || bcDir == (0, 0))
            return null;

        if (!TryLineIntersection(abMid, abMid + abDir, bcMid, bcMid + bcDir, out var center))
            return null;

        var radius = (center - a).Magnitude;

        var θ0 = nfloat.Atan2(P0.Y - center.Y, P0.X - center.X);
        var θm = nfloat.Atan2(P1.Y - center.Y, P1.X - center.X);
        var θ1 = nfloat.Atan2(P2.Y - center.Y, P2.X - center.X);

        var winding = GetWinding(θ0, θm, θ1);
        return new Arc(center, radius, radius, 0, θ0, θ1, winding);
    }

    /// <summary>
    /// Converts this arc to an endpoint-based <see cref="ArcEndpoint"/> format, if valid.
    /// </summary>
    public ArcEndpoint? ToEndpointArc()
    {
        var arc = ToCenterArc();
        return arc?.ToEndpointArc();
    }

    /// <inheritdoc/>
    public Point Lerp(nfloat t) => ToCenterArc()?.Lerp(t) ?? Point.Lerp(P0, P2, t);

    /// <inheritdoc/>
    public Vector Tangent(nfloat t) => ToCenterArc()?.Tangent(t) ?? (P2 - P0).Normalized;

    /// <inheritdoc/>
    public Point this[nfloat t] => Lerp(t);

    /// <inheritdoc/>
    public nfloat Length() => ToCenterArc()?.Length() ?? (P2 - P0).Magnitude;

    /// <inheritdoc/>
    public nfloat Length(nfloat precision) => ToCenterArc()?.Length(precision) ?? (P2 - P0).Magnitude;

    /// <summary>
    /// Implicitly converts a tuple of 3 points to an <see cref="Arc3Point"/>.
    /// </summary>
    public static implicit operator Arc3Point((Point p0, Point p1, Point p2) value) =>
        new(value.p0, value.p1, value.p2);

    private static bool TryLineIntersection(Point p1, Point p2, Point q1, Point q2, out Point intersection)
    {
        var a1 = p2.Y - p1.Y;
        var b1 = p1.X - p2.X;
        var c1 = a1 * p1.X + b1 * p1.Y;

        var a2 = q2.Y - q1.Y;
        var b2 = q1.X - q2.X;
        var c2 = a2 * q1.X + b2 * q1.Y;

        var det = a1 * b2 - a2 * b1;
        if (nfloat.Abs(det) < nfloat.Epsilon)
        {
            intersection = default;
            return false;
        }

        intersection = new Point(
            (b2 * c1 - b1 * c2) / det,
            (a1 * c2 - a2 * c1) / det
        );
        return true;
    }

    private static Winding GetWinding(nfloat θ0, nfloat θm, nfloat θ1)
    {
        nfloat Normalize(nfloat θ) => (θ + 2 * nfloat.Pi) % (2 * nfloat.Pi);
        θ0 = Normalize(θ0);
        θm = Normalize(θm);
        θ1 = Normalize(θ1);

        bool isCCW = (θ0 < θ1)
            ? (θ0 < θm && θm < θ1)
            : (θ0 < θm || θm < θ1);

        return isCCW ? Winding.CounterClockWise : Winding.ClockWise;
    }

    /// <summary>
    /// Splits the arc into one or two Y-monotonic <see cref="Arc3Point"/> segments.
    /// </summary>
    /// <remarks>
    /// This method finds the angle θ where the Y value of the arc reaches an extremum.
    /// If this point lies within the arc, the arc is split at that angle into two Y-monotonic sub-arcs.
    /// If no vertical extremum is found within the arc range, the arc is already monotonic.
    /// </remarks>
    public MonotonicArc3Point SplitIntoYMonotonicCurves()
    {
        if ((P1.Y >= P0.Y && P1.Y <= P2.Y) || (P1.Y <= P0.Y && P1.Y >= P2.Y))
        {
            return new MonotonicArc3Point(this); // Already Y-monotonic
        }

        var arc = ToCenterArc();
        if (arc == null)
            return new MonotonicArc3Point(this); // fallback segment will be linear, still monotonic

        var a = arc.Value;

        // θ at which Y extrema occur in an ellipse: tan(θ) = -b/a · tan(φ)
        // But we're only interested in θ = π/2 and 3π/2 — i.e., vertical tangent points (dy/dθ = 0)
        var criticalY = new[] { nfloat.Pi / 2, 3 * nfloat.Pi / 2 };

        foreach (var θ in criticalY)
        {
            var θAbs = NormalizeAngle(θ);
            var start = NormalizeAngle(a.StartAngle);
            var end = NormalizeAngle(a.EndAngle);

            bool inArc =
                a.Winding == Winding.ClockWise
                    ? AngleIsBetweenCW(θAbs, start, end)
                    : AngleIsBetweenCCW(θAbs, start, end);

            if (inArc)
            {
                var mid = a.EvaluateAtAngle(θAbs); // a method that returns the point at a given θ
                var first = new Arc3Point(P0, mid, P2);
                var second = new Arc3Point(mid, P1, P2); // Choose which P to keep in which segment
                return new MonotonicArc3Point(first, second);
            }
        }

        return new MonotonicArc3Point(this);
    }

    private static nfloat NormalizeAngle(nfloat θ)
    {
        var twoPi = 2 * nfloat.Pi;
        return ((θ % twoPi) + twoPi) % twoPi;
    }

    private static bool AngleIsBetweenCW(nfloat θ, nfloat from, nfloat to)
    {
        return from >= to ? (θ >= from || θ <= to) : (θ >= from && θ <= to);
    }

    private static bool AngleIsBetweenCCW(nfloat θ, nfloat from, nfloat to)
    {
        return from <= to ? (θ >= from && θ <= to) : (θ >= from || θ <= to);
    }
}
