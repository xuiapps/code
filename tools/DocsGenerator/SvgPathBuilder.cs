using System.Globalization;
using System.Text;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace DocsGenerator;

/// <summary>
/// Converts a <see cref="Path2D"/> into an SVG path data string ("d" attribute).
/// Handles move, line, and curve segments with culture-invariant formatting.
/// </summary>
public class SvgPathBuilder : IPathBuilder
{
    private readonly StringBuilder _sb;
    private readonly CultureInfo _culture;

    public SvgPathBuilder(StringBuilder sb, CultureInfo culture)
    {
        _sb = sb;
        _culture = culture;
    }

    private string F(nfloat value) => value.ToString(_culture);

    public void BeginPath() { }

    public void MoveTo(Point to)
    {
        _sb.Append($"M {F(to.X)} {F(to.Y)} ");
    }

    public void LineTo(Point to)
    {
        _sb.Append($"L {F(to.X)} {F(to.Y)} ");
    }

    public void CurveTo(Point control, Point to)
    {
        _sb.Append($"Q {F(control.X)} {F(control.Y)} {F(to.X)} {F(to.Y)} ");
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
        _sb.Append($"C {F(cp1.X)} {F(cp1.Y)} {F(cp2.X)} {F(cp2.Y)} {F(to.X)} {F(to.Y)} ");
    }

    public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        // SVG elliptical arc approximation is not implemented here.
        // Replace with straight line to approximate arc visually in raw path.
        var p = center + Vector.Angle(endAngle) * radius;
        _sb.Append($"L {F(p.X)} {F(p.Y)} ");
    }

    public void Ellipse(Point center, nfloat rx, nfloat ry, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        // SVG elliptical arc approximation is not implemented here.
        var p = center + Vector.Angle(endAngle) * rx;
        _sb.Append($"L {F(p.X)} {F(p.Y)} ");
    }

    public void ArcTo(Point cp1, Point cp2, nfloat radius)
    {
        // Approximated as a line to the first control point
        _sb.Append($"L {F(cp1.X)} {F(cp1.Y)} ");
    }

    public void Rect(Rect rect)
    {
        MoveTo(new Point(rect.X, rect.Y));
        LineTo(new Point(rect.X + rect.Width, rect.Y));
        LineTo(new Point(rect.X + rect.Width, rect.Y + rect.Height));
        LineTo(new Point(rect.X, rect.Y + rect.Height));
        ClosePath();
    }

    public void RoundRect(Rect rect, nfloat radius)
    {
        Rect(rect); // fallback to normal rect for simplicity
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
        Rect(rect); // fallback to normal rect for simplicity
    }

    public void ClosePath()
    {
        _sb.Append("Z ");
    }
}
