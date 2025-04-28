using System.Runtime.CompilerServices;
using Xui.Core.Math2D;

namespace Xui.Core.Canvas;

public class Path2D : IPathBuilder
{
    private byte[] _data;
    private int _length;

    public Path2D(int initialCapacity = 256)
    {
        _data = new byte[initialCapacity];
        _length = 0;
    }

    public void BeginPath()
    {
        _length = 0;
    }

    public void MoveTo(Point to)
    {
        WriteCommand(PathCommandType.MoveTo);
        WritePoint(to);
    }

    public void LineTo(Point to)
    {
        WriteCommand(PathCommandType.LineTo);
        WritePoint(to);
    }

    public void ClosePath()
    {
        WriteCommand(PathCommandType.ClosePath);
    }

    public void CurveTo(Point cp1, Point to)
    {
        WriteCommand(PathCommandType.QuadraticCurveTo);
        WritePoint(cp1);
        WritePoint(to);
    }

    public void CurveTo(Point cp1, Point cp2, Point to)
    {
        WriteCommand(PathCommandType.CubicCurveTo);
        WritePoint(cp1);
        WritePoint(cp2);
        WritePoint(to);
    }

    public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        WriteCommand(PathCommandType.Arc);
        WritePoint(center);
        WriteNFloat(radius);
        WriteNFloat(startAngle);
        WriteNFloat(endAngle);
        WriteByte((byte)winding);
    }

    public void ArcTo(Point cp1, Point cp2, nfloat radius)
    {
        WriteCommand(PathCommandType.ArcTo);
        WritePoint(cp1);
        WritePoint(cp2);
        WriteNFloat(radius);
    }

    public void Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
    {
        WriteCommand(PathCommandType.Ellipse);
        WritePoint(center);
        WriteNFloat(radiusX);
        WriteNFloat(radiusY);
        WriteNFloat(rotation);
        WriteNFloat(startAngle);
        WriteNFloat(endAngle);
        WriteByte((byte)winding);
    }

    public void Rect(Rect rect)
    {
        WriteCommand(PathCommandType.Rect);
        WriteRect(rect);
    }

    public void RoundRect(Rect rect, nfloat radius)
    {
        WriteCommand(PathCommandType.RoundRectUniform);
        WriteRect(rect);
        WriteNFloat(radius);
    }

    public void RoundRect(Rect rect, CornerRadius radius)
    {
        WriteCommand(PathCommandType.RoundRectCorners);
        WriteRect(rect);
        WriteCornerRadius(radius);
    }

    private void WriteCommand(PathCommandType cmd)
    {
        EnsureCapacity(1);
        _data[_length++] = (byte)cmd;
    }

    private void WritePoint(Point p)
    {
        WriteNFloat(p.X);
        WriteNFloat(p.Y);
    }

    private void WriteRect(Rect r)
    {
        WriteNFloat(r.X);
        WriteNFloat(r.Y);
        WriteNFloat(r.Width);
        WriteNFloat(r.Height);
    }

    private void WriteCornerRadius(CornerRadius r)
    {
        WriteNFloat(r.TopLeft);
        WriteNFloat(r.TopRight);
        WriteNFloat(r.BottomRight);
        WriteNFloat(r.BottomLeft);
    }

    private void WriteNFloat(nfloat value)
    {
        EnsureCapacity(nfloat.Size);
        Unsafe.As<byte, nfloat>(ref _data[_length]) = value;
        _length += nfloat.Size;
    }

    private void WriteByte(byte value)
    {
        EnsureCapacity(1);
        _data[_length++] = value;
    }

    private void EnsureCapacity(int sizeHint)
    {
        if (_length + sizeHint > _data.Length)
        {
            Array.Resize(ref _data, Math.Max(_data.Length * 2, _length + sizeHint));
        }
    }

    enum PathCommandType : byte
    {
        MoveTo,
        LineTo,
        ClosePath,
        QuadraticCurveTo,
        CubicCurveTo,
        Arc,
        ArcTo,
        Ellipse,
        Rect,
        RoundRectUniform,
        RoundRectCorners
    }

    public void Visit(IPathBuilder sink)
    {
        int pos = 0;
        while (pos < _length)
        {
            var cmd = (PathCommandType)_data[pos++];
            switch (cmd)
            {
                case PathCommandType.MoveTo:
                    sink.MoveTo(ReadPoint(ref pos));
                    break;

                case PathCommandType.LineTo:
                    sink.LineTo(ReadPoint(ref pos));
                    break;

                case PathCommandType.ClosePath:
                    sink.ClosePath();
                    break;

                case PathCommandType.QuadraticCurveTo:
                    {
                        var cp1 = ReadPoint(ref pos);
                        var to = ReadPoint(ref pos);
                        sink.CurveTo(cp1, to);
                    }
                    break;

                case PathCommandType.CubicCurveTo:
                    {
                        var cp1 = ReadPoint(ref pos);
                        var cp2 = ReadPoint(ref pos);
                        var to = ReadPoint(ref pos);
                        sink.CurveTo(cp1, cp2, to);
                    }
                    break;

                case PathCommandType.Arc:
                    {
                        var center = ReadPoint(ref pos);
                        var radius = ReadNFloat(ref pos);
                        var start = ReadNFloat(ref pos);
                        var end = ReadNFloat(ref pos);
                        var winding = (Winding)_data[pos++];
                        sink.Arc(center, radius, start, end, winding);
                    }
                    break;

                case PathCommandType.ArcTo:
                    {
                        var cp1 = ReadPoint(ref pos);
                        var cp2 = ReadPoint(ref pos);
                        var radius = ReadNFloat(ref pos);
                        sink.ArcTo(cp1, cp2, radius);
                    }
                    break;

                case PathCommandType.Ellipse:
                    {
                        var center = ReadPoint(ref pos);
                        var rx = ReadNFloat(ref pos);
                        var ry = ReadNFloat(ref pos);
                        var rotation = ReadNFloat(ref pos);
                        var start = ReadNFloat(ref pos);
                        var end = ReadNFloat(ref pos);
                        var winding = (Winding)_data[pos++];
                        sink.Ellipse(center, rx, ry, rotation, start, end, winding);
                    }
                    break;

                case PathCommandType.Rect:
                    sink.Rect(ReadRect(ref pos));
                    break;

                case PathCommandType.RoundRectUniform:
                    sink.RoundRect(ReadRect(ref pos), ReadNFloat(ref pos));
                    break;

                case PathCommandType.RoundRectCorners:
                    sink.RoundRect(ReadRect(ref pos), ReadCornerRadius(ref pos));
                    break;
            }
        }
    }

    private Point ReadPoint(ref int pos)
    {
        var x = ReadNFloat(ref pos);
        var y = ReadNFloat(ref pos);
        return new Point(x, y);
    }

    private Rect ReadRect(ref int pos)
    {
        var x = ReadNFloat(ref pos);
        var y = ReadNFloat(ref pos);
        var w = ReadNFloat(ref pos);
        var h = ReadNFloat(ref pos);
        return new Rect(x, y, w, h);
    }

    private CornerRadius ReadCornerRadius(ref int pos)
    {
        var tl = ReadNFloat(ref pos);
        var tr = ReadNFloat(ref pos);
        var br = ReadNFloat(ref pos);
        var bl = ReadNFloat(ref pos);
        return new CornerRadius(tl, tr, br, bl);
    }

    private nfloat ReadNFloat(ref int pos)
    {
        var value = Unsafe.As<byte, nfloat>(ref _data[pos]);
        pos += nfloat.Size;
        return value;
    }
}
