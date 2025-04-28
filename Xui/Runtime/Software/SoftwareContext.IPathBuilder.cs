using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software;

public partial class SoftwareContext : IPathBuilder
{
    protected Xui.Core.Canvas.Path2D path { get; } = new ();

    public void Arc(Point center, NFloat radius, NFloat startAngle, NFloat endAngle, Winding winding = Winding.ClockWise)
    {
        ((IPathBuilder)path).Arc(center, radius, startAngle, endAngle, winding);
    }

    public void ArcTo(Point cp1, Point cp2, NFloat radius)
    {
        ((IPathBuilder)path).ArcTo(cp1, cp2, radius);
    }

    public void BeginPath()
    {
        ((IPathBuilder)path).BeginPath();
    }

    public void ClosePath()
    {
        ((IPathBuilder)path).ClosePath();
    }

    public void CurveTo(Point cp1, Point to)
    {
        ((IPathBuilder)path).CurveTo(cp1, to);
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
        ((IPathBuilder)path).CurveTo(cp1, cp2, to);
    }

    public void Ellipse(Point center, NFloat radiusX, NFloat radiusY, NFloat rotation, NFloat startAngle, NFloat endAngle, Winding winding = Winding.ClockWise)
    {
        ((IPathBuilder)path).Ellipse(center, radiusX, radiusY, rotation, startAngle, endAngle, winding);
    }

    public void LineTo(Point to)
    {
        ((IPathBuilder)path).LineTo(to);
    }

    public void MoveTo(Point to)
    {
        ((IPathBuilder)path).MoveTo(to);
    }

    public void Rect(Rect rect)
    {
        ((IPathBuilder)path).Rect(rect);
    }

    public void RoundRect(Rect rect, NFloat radius)
    {
        ((IPathBuilder)path).RoundRect(rect, radius);
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
        ((IPathBuilder)path).RoundRect(rect, radius);
    }
}
