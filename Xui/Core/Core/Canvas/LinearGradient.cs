using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public ref struct LinearGradient
{
    public Point StartPoint;
    public Point EndPoint;
    public ReadOnlySpan<GradientStop> GradientStops;

    public LinearGradient(Point start, Point end, ReadOnlySpan<GradientStop> gradient)
    {
        this.StartPoint = start;
        this.EndPoint = end;
        this.GradientStops = gradient;
    }
}
