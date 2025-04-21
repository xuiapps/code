using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Represents a linear gradient brush that transitions between colors along a straight line,
/// following the same behavior as the HTML5 Canvas <c>createLinearGradient()</c>.
/// </summary>
public ref struct LinearGradient
{
    /// <summary>
    /// The starting point of the gradient, in user space coordinates.
    /// </summary>
    public Point StartPoint;

    /// <summary>
    /// The ending point of the gradient, in user space coordinates.
    /// </summary>
    public Point EndPoint;

    /// <summary>
    /// The sequence of gradient stops that define color transitions along the line.
    /// Offsets are typically in the range [0, 1].
    /// </summary>
    public ReadOnlySpan<GradientStop> GradientStops;

    /// <summary>
    /// Creates a new <see cref="LinearGradient"/> with the given start and end points and gradient stops.
    /// </summary>
    /// <param name="start">The start point of the gradient.</param>
    /// <param name="end">The end point of the gradient.</param>
    /// <param name="gradient">A span of gradient stops sorted by offset.</param>
    public LinearGradient(Point start, Point end, ReadOnlySpan<GradientStop> gradient)
    {
        this.StartPoint = start;
        this.EndPoint = end;
        this.GradientStops = gradient;
    }
}
