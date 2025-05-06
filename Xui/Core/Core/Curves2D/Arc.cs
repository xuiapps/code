using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a circular or elliptical arc defined by a center point, radii, rotation, and sweep angles.
/// </summary>
/// <remarks>
/// Arcs are useful for describing elliptical curves in vector graphics, layout paths, and stroke outlines.
/// They interpolate smoothly from <see cref="StartAngle"/> to <see cref="EndAngle"/>, optionally applying
/// rotation and winding direction.
/// </remarks>
public readonly struct Arc : ICurve
{
    /// <summary>The center point of the arc's ellipse.</summary>
    public readonly Point Center;

    /// <summary>The horizontal radius of the arc.</summary>
    public readonly nfloat RadiusX;

    /// <summary>The vertical radius of the arc.</summary>
    public readonly nfloat RadiusY;

    /// <summary>The clockwise rotation (in radians) applied to the arc relative to the X axis.</summary>
    public readonly nfloat Rotation;

    /// <summary>The angle (in radians) at which the arc starts, before rotation is applied.</summary>
    public readonly nfloat StartAngle;

    /// <summary>The angle (in radians) at which the arc ends, before rotation is applied.</summary>
    public readonly nfloat EndAngle;

    /// <summary>Specifies whether the arc is drawn clockwise or counter-clockwise.</summary>
    public readonly Winding Winding;

    /// <summary>
    /// Initializes a new <see cref="Arc"/> with the given center, radii, rotation, angles, and winding direction.
    /// </summary>
    /// <param name="center">The center point of the arc's ellipse.</param>
    /// <param name="rx">The horizontal radius.</param>
    /// <param name="ry">The vertical radius.</param>
    /// <param name="rotation">The clockwise rotation in radians applied to the ellipse.</param>
    /// <param name="startAngle">The start angle of the arc in radians (before rotation).</param>
    /// <param name="endAngle">The end angle of the arc in radians (before rotation).</param>
    /// <param name="winding">Whether the arc is drawn clockwise or counter-clockwise.</param>
    public Arc(Point center, nfloat rx, nfloat ry, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        Center = center;
        RadiusX = rx;
        RadiusY = ry;
        Rotation = rotation;
        StartAngle = startAngle;
        EndAngle = endAngle;
        Winding = winding;
    }

    /// <summary>
    /// Gets the interpolated point on the arc at the specified parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized value from 0 to 1 representing the progression along the arc.</param>
    public Point this[nfloat t] => Evaluate(t);

    /// <summary>
    /// Computes the interpolated point on the arc at a given parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized value in the range [0, 1].</param>
    public Point Lerp(nfloat t) => Evaluate(t);

    /// <summary>
    /// Evaluates the arc at a normalized parameter <paramref name="t"/>, returning the corresponding point.
    /// </summary>
    /// <param name="t">A normalized value in the range [0, 1].</param>
    /// <returns>The corresponding point on the rotated ellipse arc.</returns>
    public Point Evaluate(nfloat t)
    {
        nfloat sweep = SweepAngle;
        nfloat angle = StartAngle + t * sweep;

        nfloat cos = nfloat.Cos(angle);
        nfloat sin = nfloat.Sin(angle);

        nfloat xr = cos * RadiusX;
        nfloat yr = sin * RadiusY;

        nfloat x = Center.X + xr * nfloat.Cos(Rotation) - yr * nfloat.Sin(Rotation);
        nfloat y = Center.Y + xr * nfloat.Sin(Rotation) + yr * nfloat.Cos(Rotation);

        return new Point(x, y);
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name="t"/> on the arc.
    /// </summary>
    /// <param name="t">A normalized value in the range [0, 1].</param>
    /// <returns>The normalized tangent vector at the evaluated point on the arc.</returns>
    public Vector Tangent(nfloat t)
    {
        nfloat sweep = SweepAngle;
        nfloat angle = StartAngle + t * sweep;

        nfloat dx = -RadiusX * nfloat.Sin(angle);
        nfloat dy = RadiusY * nfloat.Cos(angle);

        nfloat tx = dx * nfloat.Cos(Rotation) - dy * nfloat.Sin(Rotation);
        nfloat ty = dx * nfloat.Sin(Rotation) + dy * nfloat.Cos(Rotation);

        return new Vector(tx, ty).Normalized;
    }

    /// <summary>
    /// Approximates the arc length using uniform sampling with 16 segments.
    /// </summary>
    /// <returns>The approximate total arc length.</returns>
    public nfloat Length()
    {
        nfloat length = 0;
        var prev = this[0];
        for (int i = 1; i <= 16; i++)
        {
            var t = (nfloat)i / 16;
            var curr = this[t];
            length += Point.Distance(prev, curr);
            prev = curr;
        }
        return length;
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name="flatness">The maximum allowed error per segment for arc approximation.</param>
    /// <returns>The computed arc length within the specified flatness tolerance.</returns>
    public nfloat Length(nfloat flatness)
    {
        return LengthRecursive(0, 1, this[0], this[1], flatness);
    }

    private nfloat LengthRecursive(nfloat t0, nfloat t1, Point p0, Point p1, nfloat tol)
    {
        var tm = (t0 + t1) * 0.5f;
        var pm = this[tm];

        var chord = Point.Distance(p0, p1);
        var curve = Point.Distance(p0, pm) + Point.Distance(pm, p1);

        if (curve - chord <= tol)
            return (chord + curve) / 2;

        return LengthRecursive(t0, tm, p0, pm, tol) + LengthRecursive(tm, t1, pm, p1, tol);
    }

    /// <summary>
    /// Gets the signed sweep angle of the arc, in radians.
    /// Adjusts automatically for the specified <see cref="Winding"/>.
    /// </summary>
    public nfloat SweepAngle
    {
        get
        {
            nfloat sweep = EndAngle - StartAngle;
            if (Winding == Winding.CounterClockWise && sweep > 0)
                sweep -= 2 * MathF.PI;
            if (Winding == Winding.ClockWise && sweep < 0)
                sweep += 2 * MathF.PI;
            return sweep;
        }
    }

    /// <summary>
    /// Converts this arc to an endpoint-parameterized <see cref="ArcEndpoint"/> representation.
    /// </summary>
    /// <remarks>
    /// This method generates a single arc segment between the start and end points of the original arc,
    /// preserving the sweep direction and determining whether the arc is the larger of the two possible segments.
    /// If the arc forms a complete circle (i.e., the start and end points match and the sweep covers 360°),
    /// the endpoint position is nudged slightly to avoid rendering ambiguities in formats that do not support full-circle arcs directly.
    /// </remarks>
    /// <returns>
    /// An <see cref="ArcEndpoint"/> structure that approximates this arc using endpoint-based parametrization.
    /// </returns>
    public ArcEndpoint ToEndpointArc()
    {
        Point start = Evaluate(0);
        Point end = Evaluate(1);

        // Full-circle: avoid start == end ambiguity
        bool isFullCircle = Point.Distance(start, end) < 0.001f && Math.Abs(SweepAngle) >= 2 * MathF.PI;

        if (isFullCircle)
        {
            // Shrink end slightly along the path to make it renderable
            end = Evaluate(0.999f); // Just short of full circle
        }

        bool largeArc = Math.Abs(SweepAngle) > MathF.PI;
        return new ArcEndpoint(start, end, RadiusX, RadiusY, Rotation, largeArc, Winding);
    }

    /// <summary>
    /// Converts this arc to one or two endpoint-parameterized arcs, depending on the arc's sweep angle.
    /// </summary>
    /// <remarks>
    /// If the arc forms a complete or nearly complete circle (i.e., the sweep angle approaches ±360° and the start and end points match),
    /// the arc is split into two half-circle segments to avoid rendering issues in formats that do not support full-circle arcs directly.
    /// Otherwise, a single <see cref="ArcEndpoint"/> is returned with appropriate flags set for direction and size.
    /// </remarks>
    /// <returns>
    /// A tuple of one or two <see cref="ArcEndpoint"/> instances that collectively represent the same elliptical arc:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <c>Item1</c> – The first arc segment (always non-null).
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <c>Item2</c> – The second arc segment if the arc is split, or <c>null</c> if the arc fits in a single segment.
    ///     </description>
    ///   </item>
    /// </list>
    /// </returns>
    public (ArcEndpoint, ArcEndpoint?) ToEndpointArcs()
    {
        Point start = Evaluate(0);
        Point end = Evaluate(1);

        nfloat sweep = SweepAngle;
        nfloat absSweep = nfloat.Abs(sweep);
        bool isFullCircle = Point.Distance(start, end) < 0.001f && absSweep >= 2 * MathF.PI * 0.999f;

        if (isFullCircle)
        {
            // Split into two 180-degree arcs
            nfloat midAngle = StartAngle + sweep * 0.5f;
            var midPoint = Evaluate(0.5f);

            var arc1 = new ArcEndpoint(start, midPoint, RadiusX, RadiusY, Rotation, false, Winding);
            var arc2 = new ArcEndpoint(midPoint, end, RadiusX, RadiusY, Rotation, false, Winding);
            return (arc1, arc2);
        }
        else
        {
            bool largeArc = absSweep > MathF.PI;
            var arc = new ArcEndpoint(start, end, RadiusX, RadiusY, Rotation, largeArc, Winding);
            return (arc, null);
        }
    }

    /// <summary>
    /// Evaluates the position on the ellipse at a given angle <paramref name="θ"/> (in radians),
    /// relative to the unrotated ellipse center.
    /// </summary>
    /// <param name="θ">
    /// The angle in radians, measured from the X axis of the unrotated ellipse.
    /// This is not a normalized parameter like <c>t</c>, but an absolute angle used in parametric arc equations.
    /// </param>
    /// <returns>
    /// The corresponding <see cref="Point"/> on the arc's ellipse at the given angle,
    /// after applying the arc's rotation and translating from its center.
    /// </returns>
    /// <remarks>
    /// The point is computed using the parametric equation of an axis-aligned ellipse:
    /// <c>(x, y) = (rx * cos(θ), ry * sin(θ))</c>, then rotated by <see cref="Rotation"/> and translated to <see cref="Center"/>.
    /// This method is useful when working directly with angle-based metrics like vertical extrema (e.g., θ = π/2 or 3π/2).
    /// </remarks>
    public Point EvaluateAtAngle(nfloat θ)
    {
        // Assume θ is already inside the arc range
        var cos = nfloat.Cos(θ);
        var sin = nfloat.Sin(θ);

        var x = RadiusX * cos;
        var y = RadiusY * sin;

        var rotatedX = x * nfloat.Cos(Rotation) - y * nfloat.Sin(Rotation);
        var rotatedY = x * nfloat.Sin(Rotation) + y * nfloat.Cos(Rotation);

        return new Point(Center.X + rotatedX, Center.Y + rotatedY);
    }

    /// <summary>
    /// Splits this arc into a series of <see cref="Arc3Point"/> segments,
    /// each spanning ≤ 90° and aligned so that one edge is axis-aligned.
    /// </summary>
    /// <param name="buffer">
    /// A span to receive the output segments. Must have space for up to 4 entries.
    /// </param>
    /// <param name="count">
    /// The number of segments written to <paramref name="buffer"/>.
    /// </param>
    /// <remarks>
    /// This method ensures consistent arc tessellation by aligning segment edges with
    /// X or Y axes and bounding each by ≤ 90°, suitable for round rects or shader-compatible curves.
    /// </remarks>
    public void ToArc3Segments(Span<Arc3Point> buffer, out int count)
    {
        count = 0;

        var Δθ = EndAngle - StartAngle;
        var direction = Winding == Winding.ClockWise ? 1 : -1;
        var steps = (int)Math.Ceiling(nfloat.Abs(Δθ) / (π / 2));
        if (steps > buffer.Length) return;

        for (int i = 0; i < steps; i++)
        {
            var θ0 = StartAngle + direction * (π / 2) * i;
            var θ1 = StartAngle + direction * (π / 2) * Math.Min(i + 1, steps);

            var p0 = EvaluateAtAngle(θ0);
            var p2 = EvaluateAtAngle(θ1);
            var p1 = EvaluateAtAngle((θ0 + θ1) * 0.5f);

            buffer[count++] = new Arc3Point(p0, p1, p2);
        }
    }
}
