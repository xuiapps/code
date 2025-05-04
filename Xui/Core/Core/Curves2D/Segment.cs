using Xui.Core.Math2D;

namespace Xui.Core.Curves2D;

/// <summary>
/// Represents a straight line segment from <see cref="P0"/> to <see cref="P1"/>.
/// Implements the <see cref="ICurve"/> interface for compatibility with other curve types.
/// </summary>
public readonly struct Segment : ICurve
{
    /// <summary>The starting point of the line segment.</summary>
    public readonly Point P0;

    /// <summary>The ending point of the line segment.</summary>
    public readonly Point P1;

    /// <summary>Initializes a new line from <paramref name="p0"/> to <paramref name="p1"/>.</summary>
    public Segment(Point p0, Point p1)
    {
        this.P0 = p0;
        this.P1 = p1;
    }

    /// <inheritdoc/>
    public Point Lerp(nfloat t) => Point.Lerp(P0, P1, t);

    /// <inheritdoc/>
    public Vector Tangent(nfloat t) => P1 - P0;

    /// <inheritdoc/>
    public Point this[nfloat t] => Lerp(t);

    /// <inheritdoc/>
    public nfloat Length() => Point.Distance(P0, P1);

    /// <inheritdoc/>
    public nfloat Length(nfloat precision) => Length();

    /// <summary>
    /// Converts a tuple to a <see cref="Segment"/>.
    /// </summary>
    public static implicit operator Segment((Point p0, Point p1) value) => new(value.p0, value.p1);
}
