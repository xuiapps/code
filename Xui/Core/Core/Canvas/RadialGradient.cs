using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// Represents a radial gradient brush that transitions colors outward from one circle to another,
/// similar to <c>createRadialGradient()</c> in the HTML5 Canvas API.
/// </summary>
public ref struct RadialGradient
{
    /// <summary>
    /// The center point of the starting circle (inner circle).
    /// </summary>
    public Point StartCenter;

    /// <summary>
    /// The radius of the starting circle (inner circle), usually 0 for solid centers.
    /// </summary>
    public nfloat StartRadius;

    /// <summary>
    /// The center point of the ending circle (outer circle).
    /// </summary>
    public Point EndCenter;

    /// <summary>
    /// The radius of the ending circle (outer circle), defining the spread of the gradient.
    /// </summary>
    public nfloat EndRadius;

    /// <summary>
    /// A sequence of color stops defining how colors are interpolated from the inner to the outer circle.
    /// Offsets are typically in the range [0, 1].
    /// </summary>
    public ReadOnlySpan<GradientStop> GradientStops;

    /// <summary>
    /// Initializes a radial gradient that transitions between two circles.
    /// </summary>
    /// <param name="startCenter">Center of the inner circle.</param>
    /// <param name="startRadius">Radius of the inner circle.</param>
    /// <param name="endCenter">Center of the outer circle.</param>
    /// <param name="endRadius">Radius of the outer circle.</param>
    /// <param name="gradientStops">Span of color stops defining the gradient transition.</param>
    public RadialGradient(Point startCenter, nfloat startRadius, Point endCenter, nfloat endRadius, ReadOnlySpan<GradientStop> gradientStops)
    {
        this.StartCenter = startCenter;
        this.StartRadius = startRadius;
        this.EndCenter = endCenter;
        this.EndRadius = endRadius;
        this.GradientStops = gradientStops;
    }

    /// <summary>
    /// Initializes a radial gradient from a single point and radius, fading outward from a solid center.
    /// This is equivalent to setting the start radius to zero and both centers to the same point.
    /// </summary>
    /// <param name="center">Center point of the gradient.</param>
    /// <param name="radius">Radius of the gradient spread.</param>
    /// <param name="gradientStops">Span of color stops defining the gradient transition.</param>
    public RadialGradient(Point center, nfloat radius, ReadOnlySpan<GradientStop> gradientStops)
    {
        this.StartCenter = center;
        this.StartRadius = 0;
        this.EndCenter = center;
        this.EndRadius = radius;
        this.GradientStops = gradientStops;
    }
}
