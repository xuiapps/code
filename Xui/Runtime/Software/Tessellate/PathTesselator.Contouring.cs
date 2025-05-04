using System.Collections.Generic;
using Xui.Core.Math2D;
using Xui.Core.Curves2D;
using Xui.Core.Canvas;

namespace Xui.Runtime.Software.Tessellate;

public partial class PathTesselator
{
    public class Contouring : IPathBuilder
    {
        private readonly List<ClosedContour> _contours = new();
        private readonly List<Point> _current = new();
        private Point _start;
        private Point _currentPos;
        private bool _open;
        private readonly nfloat _precision;

        public IReadOnlyList<ClosedContour> Contours => _contours;

        public Contouring()
        {
            _precision = (nfloat).25f;
        }

        public Contouring(nfloat precision)
        {
            _precision = precision;
        }

        public List<ClosedContour> Result => _contours;

        public static IReadOnlyList<ClosedContour> ContourPath(Path2D path, nfloat precision)
        {
            var contouring = new Contouring(precision);
            path.Visit(contouring);
            return contouring.Contours;
        }

        public void BeginPath()
        {
            _current.Clear();
            _open = false;
        }

        public void MoveTo(Point to)
        {
            FlushIfOpen();
            _start = _currentPos = to;
            _current.Add(to);
            _open = true;
        }

        public void LineTo(Point to)
        {
            EnsureOpen();
            _current.Add(to);
            _currentPos = to;
        }

        public void CurveTo(Point cp1, Point cp2, Point to)
        {
            EnsureOpen();
            AppendFlattened(new CubicBezier(_currentPos, cp1, cp2, to));
            _currentPos = to;
        }

        public void CurveTo(Point control, Point to)
        {
            EnsureOpen();
            AppendFlattened(new QuadraticBezier(_currentPos, control, to));
            _currentPos = to;
        }

        public void Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
        {
            EnsureOpen();
            AppendFlattened(new Arc(center, radius, radius, 0, startAngle, endAngle, winding));
            _currentPos = new Arc(center, radius, radius, 0, startAngle, endAngle, winding)[1]; // post-condition
        }

        public void Ellipse(Point center, nfloat rx, nfloat ry, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding = Winding.ClockWise)
        {
            EnsureOpen();
            AppendFlattened(new Arc(center, rx, ry, rotation, startAngle, endAngle, winding));
            _currentPos = new Arc(center, rx, ry, rotation, startAngle, endAngle, winding)[1];
        }

        public void ArcTo(Point cp1, Point cp2, nfloat radius)
        {
            EnsureOpen();

            var p0 = _currentPos;
            var p1 = cp1;
            var p2 = cp2;

            if (radius <= 0 || p0 == p1 || p1 == p2)
            {
                LineTo(p1);
                return;
            }

            var v1 = (p0 - p1).Normalized;
            var v2 = (p2 - p1).Normalized;

            var angle = nfloat.Abs(Vector.Angle(v1, v2));

            if (angle < 1e-5 || angle >= nfloat.Pi - 1e-5)
            {
                LineTo(p1);
                return;
            }

            var halfAngle = angle / 2;
            var sin = nfloat.Sin(halfAngle);
            var segmentLength = radius / sin;

            var tangentDir = ((v1 + v2) / 2).Normalized;
            var centerOffset = tangentDir.PerpendicularCCW * radius / nfloat.Tan(halfAngle);

            // Choose arc center
            var bisector = tangentDir.PerpendicularCW;
            var center = p1 + bisector.Normalized * (radius / nfloat.Tan(halfAngle));

            // Compute start/end points on the arc
            var start = p1 + v1.PerpendicularCW * radius;
            var end = p1 + v2.PerpendicularCCW * radius;

            // Fix winding based on cross product
            var cross = Vector.Cross(v1, v2);
            var winding = cross < 0 ? Winding.ClockWise : Winding.CounterClockWise;

            LineTo(start);
            AppendFlattened(new Arc(
                center,
                radius,
                radius,
                0,
                (start - center).ArcAngle,
                (end - center).ArcAngle,
                winding
            ));
            _currentPos = end;
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
            RoundRect(rect, new CornerRadius(radius));
        }

        public void RoundRect(Rect rect, CornerRadius radius)
        {
            EnsureOpen();

            var l = rect.X;
            var t = rect.Y;
            var r = rect.X + rect.Width;
            var b = rect.Y + rect.Height;

            var rx = radius.TopLeft;
            var ry = radius.TopLeft;

            // Clamp radii to half of width/height to prevent overlap
            var maxRadius = nfloat.Min(rect.Width, rect.Height) / 2;
            rx = ry = nfloat.Min(rx, maxRadius);

            // Start at top-left corner after radius
            MoveTo(new Point(l + rx, t));

            // Top edge to top-right corner
            LineTo(new Point(r - radius.TopRight, t));
            AppendFlattened(new Arc(
                center: new Point(r - radius.TopRight, t + radius.TopRight),
                rx: radius.TopRight,
                ry: radius.TopRight,
                startAngle: -nfloat.Pi / 2,
                endAngle: 0,
                rotation: 0,
                winding: Winding.ClockWise
            ));

            // Right edge to bottom-right
            LineTo(new Point(r, b - radius.BottomRight));
            AppendFlattened(new Arc(
                center: new Point(r - radius.BottomRight, b - radius.BottomRight),
                rx: radius.BottomRight,
                ry: radius.BottomRight,
                startAngle: 0,
                endAngle: nfloat.Pi / 2,
                rotation: 0,
                winding: Winding.ClockWise
            ));

            // Bottom edge to bottom-left
            LineTo(new Point(l + radius.BottomLeft, b));
            AppendFlattened(new Arc(
                center: new Point(l + radius.BottomLeft, b - radius.BottomLeft),
                rx: radius.BottomLeft,
                ry: radius.BottomLeft,
                startAngle: nfloat.Pi / 2,
                endAngle: nfloat.Pi,
                rotation: 0,
                winding: Winding.ClockWise
            ));

            // Left edge to top-left
            LineTo(new Point(l, t + radius.TopLeft));
            AppendFlattened(new Arc(
                center: new Point(l + radius.TopLeft, t + radius.TopLeft),
                rx: radius.TopLeft,
                ry: radius.TopLeft,
                startAngle: nfloat.Pi,
                endAngle: 3 * nfloat.Pi / 2,
                rotation: 0,
                winding: Winding.ClockWise
            ));

            ClosePath();
        }

        public void ClosePath()
        {
            if (_open && _current.Count >= 2)
            {
                if (_current[^1] != _start)
                    _current.Add(_start);
                _contours.Add(new ClosedContour(new List<Point>(_current)));
            }
            _current.Clear();
            _open = false;
        }

        private void FlushIfOpen()
        {
            if (_open) ClosePath();
        }

        private void EnsureOpen()
        {
            if (!_open)
                MoveTo(_currentPos);
        }

        private void AppendFlattened(CubicBezier curve)
        {
            AppendFlattenedRecursive(curve, 0, 1);
        }

        private void AppendFlattenedRecursive(CubicBezier curve, nfloat t0, nfloat t1)
        {
            var p0 = curve[t0];
            var p1 = curve[t1];
            var mid = curve[(t0 + t1) * 0.5f];
            var linearMid = Point.Lerp(p0, p1, 0.5f);
            var error = Point.SquaredDistance(mid, linearMid);

            if (error <= _precision * _precision)
            {
                _current.Add(p1);
                return;
            }

            var tm = (t0 + t1) * 0.5f;
            AppendFlattenedRecursive(curve, t0, tm);
            AppendFlattenedRecursive(curve, tm, t1);
        }

        private void AppendFlattened(QuadraticBezier curve)
        {
            AppendFlattenedRecursive(curve, 0, 1);
        }

        private void AppendFlattenedRecursive(QuadraticBezier curve, nfloat t0, nfloat t1)
        {
            var p0 = curve[t0];
            var p1 = curve[t1];
            var mid = curve[(t0 + t1) * 0.5f];
            var linearMid = Point.Lerp(p0, p1, 0.5f);
            var error = Point.SquaredDistance(mid, linearMid);

            if (error <= _precision * _precision)
            {
                _current.Add(p1);
                return;
            }

            var tm = (t0 + t1) * 0.5f;
            AppendFlattenedRecursive(curve, t0, tm);
            AppendFlattenedRecursive(curve, tm, t1);
        }

        private void AppendFlattened(Arc arc)
        {
            AppendFlattenedRecursive(arc, 0, 1);
        }

        private void AppendFlattenedRecursive(Arc arc, nfloat t0, nfloat t1)
        {
            var p0 = arc[t0];
            var p1 = arc[t1];
            var mid = arc[(t0 + t1) * 0.5f];
            var linearMid = Point.Lerp(p0, p1, 0.5f);
            var error = Point.SquaredDistance(mid, linearMid);

            if (error <= _precision * _precision)
            {
                _current.Add(p1);
                return;
            }

            var tm = (t0 + t1) * 0.5f;
            AppendFlattenedRecursive(arc, t0, tm);
            AppendFlattenedRecursive(arc, tm, t1);
        }
    }
}
