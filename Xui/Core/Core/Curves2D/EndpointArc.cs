using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a circular or elliptical arc defined by radii, rotation, and endpoint parameters.
/// </summary>
/// <remarks>
/// This arc representation uses start and end points along with size and sweep flags.
/// It is commonly used in vector graphics where the arc shape is inferred from geometry and direction flags.
/// </remarks>
public readonly struct EndpointArc : ICurve
{
    /// <summary>The start point of the arc.</summary>
    public readonly Point Start;

    /// <summary>The end point of the arc.</summary>
    public readonly Point End;

    /// <summary>The horizontal radius of the arc.</summary>
    public readonly nfloat RadiusX;

    /// <summary>The vertical radius of the arc.</summary>
    public readonly nfloat RadiusY;

    /// <summary>The clockwise rotation (in radians) applied to the arc's ellipse relative to the X axis.</summary>
    public readonly nfloat Rotation;

    /// <summary>If true, the arc is the longer (greater than 180°) of the two possible arcs between points.</summary>
    public readonly bool LargeArc;

    /// <summary>The direction in which the arc is drawn (clockwise or counter-clockwise).</summary>
    public readonly Winding Winding;

    /// <summary>
    /// Initializes a new <see cref="EndpointArc"/> with the given geometry and flags.
    /// </summary>
    /// <param name="start">The start point of the arc.</param>
    /// <param name="end">The end point of the arc.</param>
    /// <param name="rx">The horizontal radius.</param>
    /// <param name="ry">The vertical radius.</param>
    /// <param name="rotation">The clockwise rotation in radians applied to the ellipse.</param>
    /// <param name="largeArc">Whether the arc should be the larger of the two possible arcs.</param>
    /// <param name="winding">The sweep direction (clockwise or counter-clockwise).</param>
    public EndpointArc(Point start, Point end, nfloat rx, nfloat ry, nfloat rotation, bool largeArc, Winding winding)
    {
        Start = start;
        End = end;
        RadiusX = rx;
        RadiusY = ry;
        Rotation = rotation;
        LargeArc = largeArc;
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
    /// <returns>The corresponding point on the arc.</returns>
    public Point Evaluate(nfloat t)
    {
        // Conversion to center-based arc will be used internally
        var centerArc = ToCenterArc();
        return centerArc.Evaluate(t);
    }

    /// <summary>
    /// Computes the tangent vector at parameter <paramref name="t"/> on the arc.
    /// </summary>
    /// <param name="t">A normalized value in the range [0, 1].</param>
    /// <returns>The normalized tangent vector at the evaluated point on the arc.</returns>
    public Vector Tangent(nfloat t)
    {
        var centerArc = ToCenterArc();
        return centerArc.Tangent(t);
    }

    /// <summary>
    /// Approximates the arc length using uniform sampling with 16 segments.
    /// </summary>
    /// <returns>The approximate total arc length.</returns>
    public nfloat Length()
    {
        var arc = ToCenterArc();
        return arc.Length();
    }

    /// <summary>
    /// Approximates the arc length using recursive adaptive subdivision.
    /// </summary>
    /// <param name="flatness">The maximum allowed error per segment for arc approximation.</param>
    /// <returns>The computed arc length within the specified flatness tolerance.</returns>
    public nfloat Length(nfloat flatness)
    {
        var arc = ToCenterArc();
        return arc.Length(flatness);
    }

    /// <summary>
    /// Converts this endpoint-parameterized arc to a center-based <see cref="Arc"/>.
    /// </summary>
    /// <returns>A new <see cref="Arc"/> representing the same shape.</returns>
    public Arc ToCenterArc()
    {
        // Conversion adapted from the SVG spec and D2D documentation
        // https://www.w3.org/TR/SVG/implnote.html#ArcImplementationNotes

        nfloat x1 = Start.X;
        nfloat y1 = Start.Y;
        nfloat x2 = End.X;
        nfloat y2 = End.Y;

        nfloat rx = nfloat.Abs(RadiusX);
        nfloat ry = nfloat.Abs(RadiusY);
        nfloat phi = Rotation;

        nfloat cosPhi = nfloat.Cos(phi);
        nfloat sinPhi = nfloat.Sin(phi);

        nfloat dx2 = (x1 - x2) / 2;
        nfloat dy2 = (y1 - y2) / 2;

        nfloat x1p = cosPhi * dx2 + sinPhi * dy2;
        nfloat y1p = -sinPhi * dx2 + cosPhi * dy2;

        nfloat rx2 = rx * rx;
        nfloat ry2 = ry * ry;
        nfloat x1p2 = x1p * x1p;
        nfloat y1p2 = y1p * y1p;

        // Adjust radii if necessary
        nfloat lambda = (x1p2 / rx2) + (y1p2 / ry2);
        if (lambda > 1)
        {
            nfloat scale = nfloat.Sqrt(lambda);
            rx *= scale;
            ry *= scale;
            rx2 = rx * rx;
            ry2 = ry * ry;
        }

        nfloat sign = (LargeArc == (Winding == Winding.ClockWise)) ? -1 : 1;
        nfloat num = rx2 * ry2 - rx2 * y1p2 - ry2 * x1p2;
        nfloat denom = rx2 * y1p2 + ry2 * x1p2;
        nfloat coef = (num < 0 || denom == 0) ? 0 : sign * nfloat.Sqrt(num / denom);

        nfloat cxp = coef * (rx * y1p) / ry;
        nfloat cyp = coef * -(ry * x1p) / rx;

        nfloat cx = cosPhi * cxp - sinPhi * cyp + (x1 + x2) / 2;
        nfloat cy = sinPhi * cxp + cosPhi * cyp + (y1 + y2) / 2;

        nfloat startAngle = AngleOnUnitEllipse(1, 0, (x1p - cxp) / rx, (y1p - cyp) / ry);
        nfloat endAngle = AngleOnUnitEllipse((x1p - cxp) / rx, (y1p - cyp) / ry, (-x1p - cxp) / rx, (-y1p - cyp) / ry);

        return new Arc(new Point(cx, cy), rx, ry, phi, startAngle, startAngle + endAngle, Winding);
    }

    private static nfloat AngleOnUnitEllipse(nfloat ux, nfloat uy, nfloat vx, nfloat vy)
    {
        nfloat dot = ux * vx + uy * vy;
        nfloat len = nfloat.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
        nfloat angle = nfloat.Acos(nfloat.Clamp(dot / len, -1f, 1f));
        if (ux * vy - uy * vx < 0)
            angle = -angle;
        return angle;
    }
}
