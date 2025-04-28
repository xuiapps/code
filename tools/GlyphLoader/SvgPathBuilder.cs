using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

public class SvgPathBuilder : IGlyphPathBuilder
{
    private readonly StringBuilder _sb = new();
    private bool _started = false;

    public NFloat Scale { get; set; } = 1;
    public Vector Translate { get; set; } = default;

    public string Path => _sb.ToString().TrimEnd();
    public bool HasCommands => _sb.Length > 0;

    private Point Transform(Point p) => (p * Scale) + Translate;

    private static string F(NFloat v) => v.ToString("0.###", CultureInfo.InvariantCulture);

    public void MoveTo(Point to)
    {
        var p = Transform(to);
        _sb.Append($"M{F(p.X)} {F(p.Y)} ");
        _started = true;
    }

    public void LineTo(Point to)
    {
        var p = Transform(to);
        _sb.Append($"L{F(p.X)} {F(p.Y)} ");
    }

    public void CurveTo(Point control, Point to)
    {
        var c = Transform(control);
        var p = Transform(to);
        _sb.Append($"Q{F(c.X)} {F(c.Y)}, {F(p.X)} {F(p.Y)} ");
    }

    public void ClosePath()
    {
        if (_started)
        {
            _sb.Append("Z ");
            _started = false;
        }
    }

    public void Clear()
    {
        _sb.Clear();
        _started = false;
    }
}
