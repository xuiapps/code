using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Runtime.Browser.Actual;

public partial class BrowserDrawingContext : IContext
{
    [JSImport("Xui.Runtime.Browser.Actual.BrowserDrawingContext.setFillStyle", "main.js")]
    internal static partial void CanvasSetFillStyle(string fillStyle);

    [JSImport("Xui.Runtime.Browser.Actual.BrowserDrawingContext.fillRect", "main.js")]
    internal static partial void CanvasFillRect(double x, double y, double width, double height);

    public static readonly BrowserDrawingContext Instance = new BrowserDrawingContext();

    public NFloat GlobalAlpha { set => throw new NotImplementedException(); }
    public LineCap LineCap { set => throw new NotImplementedException(); }
    public LineJoin LineJoin { set => throw new NotImplementedException(); }
    public NFloat LineWidth { set => throw new NotImplementedException(); }
    public NFloat MitterLimit { set => throw new NotImplementedException(); }
    public NFloat LineDashOffset { set => throw new NotImplementedException(); }

    public void Arc(Point center, NFloat radius, NFloat startAngle, NFloat endAngle, Winding winding = Winding.ClockWise)
    {
        throw new NotImplementedException();
    }

    public void ArcTo(Point cp1, Point cp2, NFloat radius)
    {
        throw new NotImplementedException();
    }

    public void BeginPath()
    {
        throw new NotImplementedException();
    }

    public void Clip()
    {
        throw new NotImplementedException();
    }

    public void ClosePath()
    {
        throw new NotImplementedException();
    }

    public void CurveTo(Point cp1, Point to)
    {
        throw new NotImplementedException();
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
    }

    public void Ellipse(Point center, NFloat radiusX, NFloat radiusY, NFloat rotation, NFloat startAngle, NFloat endAngle, Winding winding = Winding.ClockWise)
    {
        throw new NotImplementedException();
    }

    public void Fill(FillRule rule = FillRule.NonZero)
    {
        throw new NotImplementedException();
    }

    public void FillRect(Rect rect) =>
        CanvasFillRect(rect.X, rect.Y, rect.Width, rect.Height);

    public void FillText(string text, Point pos)
    {
        throw new NotImplementedException();
    }

    public void LineTo(Point to)
    {
        throw new NotImplementedException();
    }

    public Vector MeasureText(string text)
    {
        throw new NotImplementedException();
    }

    public void MoveTo(Point to)
    {
        throw new NotImplementedException();
    }

    public void Rect(Rect rect)
    {
        throw new NotImplementedException();
    }

    public void Restore()
    {
        throw new NotImplementedException();
    }

    public void Rotate(NFloat angle)
    {
        throw new NotImplementedException();
    }

    public void RoundRect(Rect rect, NFloat radius)
    {
        throw new NotImplementedException();
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Scale(Vector vector)
    {
        throw new NotImplementedException();
    }

    public void SetFill(Color color) =>
        CanvasSetFillStyle($"rgba({color.Red * 255}, {color.Green * 255}, {color.Blue * 255}, {color.Alpha})");

    public void SetFill(LinearGradient linearGradient)
    {
        throw new NotImplementedException();
    }

    public void SetFill(RadialGradient radialGradient)
    {
        throw new NotImplementedException();
    }

    public void SetFont(Font font)
    {
        throw new NotImplementedException();
    }

    public void SetLineDash(ReadOnlySpan<NFloat> segments)
    {
        throw new NotImplementedException();
    }

    public void SetStroke(Color color)
    {
        throw new NotImplementedException();
    }

    public void SetStroke(LinearGradient linearGradient)
    {
        throw new NotImplementedException();
    }

    public void SetStroke(RadialGradient radialGradient)
    {
        throw new NotImplementedException();
    }

    public void SetTransform(AffineTransform transform)
    {
        throw new NotImplementedException();
    }

    public void Stroke()
    {
        throw new NotImplementedException();
    }

    public void StrokeRect(Rect rect)
    {
        throw new NotImplementedException();
    }

    public void Transform(AffineTransform matrix)
    {
        throw new NotImplementedException();
    }

    public void Translate(Vector vector)
    {
        throw new NotImplementedException();
    }
}
