using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public ref struct RadialGradient
{
    public Point StartCenter;

    public nfloat StartRadius;

    public Point EndCenter;

    public nfloat EndRadius;

    public ReadOnlySpan<GradientStop> GradientStops;

    public RadialGradient(Point startCenter, nfloat startRadius, Point endCenter, nfloat endRadius, ReadOnlySpan<GradientStop> gradientStops)
    {
        this.StartCenter = startCenter;
        this.StartRadius = startRadius;
        this.EndCenter = endCenter;
        this.EndRadius = endRadius;
        this.GradientStops = gradientStops;
    }

    public RadialGradient(Point center, nfloat radius, ReadOnlySpan<GradientStop> gradientStops)
    {
        this.StartCenter = center;
        this.StartRadius = 0;
        this.EndCenter = center;
        this.EndRadius = radius;
        this.GradientStops = gradientStops;
    }
}