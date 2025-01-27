using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

/// <summary>
/// A C# implementation of web canvas Path in CanvasRenderingContext2D.
/// https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D#paths
/// </summary>
public interface IPathDrawingContext
{
    public void BeginPath();
    public void MoveTo(Point to);
    public void LineTo(Point to);
    public void ClosePath();

    public void CurveTo(Point cp1, Point to);
    public void CurveTo(Point cp1, Point cp2, Point to);

    public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);

    public void ArcTo(Point cp1, Point cp2, nfloat radius);

    public void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise);

    public void Rect(Rect rect);

    public void RoundRect(Rect rect, nfloat radius);

    public void RoundRect(Rect rect, CornerRadius radius);

    public void Fill(FillRule rule = FillRule.NonZero);
    public void Stroke();

    public void Clip();
}