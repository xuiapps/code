using Xui.Core.Math3D;

namespace Xui.Core.Curves3D;

/// <summary>
/// Represents a linear (degree-1) Bézier curve in 3D space — a straight line segment
/// from <see cref="P0"/> to <see cref="P1"/>.
/// </summary>
public readonly struct LinearBezier3 : ICurve3
{
    /// <summary>The starting point of the segment.</summary>
    public readonly Point3 P0;

    /// <summary>The ending point of the segment.</summary>
    public readonly Point3 P1;

    /// <summary>
    /// Initializes a new <see cref="LinearBezier3"/> with the specified endpoints.
    /// </summary>
    public LinearBezier3(Point3 p0, Point3 p1)
    {
        P0 = p0;
        P1 = p1;
    }

    /// <summary>
    /// Evaluates the segment at parameter <paramref name="t"/>.
    /// </summary>
    /// <param name="t">A normalized parameter in the range [0, 1].</param>
    public Point3 Lerp(float t) => Point3.Lerp(P0, P1, t);

    /// <summary>
    /// Returns the constant tangent (direction) of the segment.
    /// </summary>
    /// <param name="t">Ignored — the tangent is constant for a linear segment.</param>
    public Vector3 Tangent(float t) => P1 - P0;

    /// <inheritdoc/>
    public Point3 this[float t] => Lerp(t);

    /// <summary>
    /// Returns the length of the segment (the Euclidean distance from <see cref="P0"/> to <see cref="P1"/>).
    /// </summary>
    public float Length() => Point3.Distance(P0, P1);

    /// <summary>
    /// Returns the length of the segment.
    /// The <paramref name="precision"/> parameter is ignored for linear segments.
    /// </summary>
    public float Length(float precision) => Length();

    /// <summary>
    /// Converts this linear segment to a <see cref="QuadraticBezier3"/> with a midpoint control.
    /// </summary>
    public QuadraticBezier3 ToQuadratic() =>
        new(P0, Point3.Lerp(P0, P1, 0.5f), P1);

    /// <summary>
    /// Converts this linear segment to a <see cref="CubicBezier3"/> with collinear controls.
    /// </summary>
    public CubicBezier3 ToCubic() =>
        new(P0, Point3.Lerp(P0, P1, 1f / 3f), Point3.Lerp(P0, P1, 2f / 3f), P1);

    /// <inheritdoc/>
    public override string ToString() => $"LinearBezier3 [{P0} → {P1}]";
}
