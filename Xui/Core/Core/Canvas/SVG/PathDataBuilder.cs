using Xui.Core.Math2D;

namespace Xui.Core.Canvas.SVG;

public ref struct PathDataBuilder
{
    public readonly IPathDrawingContext Sink;

    public Point? StartPoint;
    public Point CurrentPoint;
    public Point NextCubicControlPoint;
    public Point NextQuadraticControlPoint;

    public PathDataBuilder(IPathDrawingContext sink)
    {
        this.Sink = sink;

        this.StartPoint = null;
        this.CurrentPoint = (0, 0);
        this.NextCubicControlPoint = (0, 0);
        this.NextQuadraticControlPoint = (0, 0);
    }

    public PathDataBuilder Begin()
    {
        Sink.BeginPath();
        return this;
    }

    public PathDataBuilder M(Point point)
    {
        this.StartPoint = point;
        this.CurrentPoint = point;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.MoveTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder m(Vector vector) =>
        this.M(this.CurrentPoint + vector);

    public PathDataBuilder L(Point point)
    {
        this.CurrentPoint = point;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(point);
        return this;
    }

    public PathDataBuilder l(Vector vector)
    {
        this.CurrentPoint += vector;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder H(nfloat h)
    {
        this.CurrentPoint.X = h;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder h(nfloat h)
    {
        this.CurrentPoint.X += h;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder V(nfloat v)
    {
        this.CurrentPoint.Y = v;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder v(nfloat v)
    {
        this.CurrentPoint.Y += v;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.Sink.LineTo(this.CurrentPoint);
        return this;
    }

    public PathDataBuilder C(Point cp1, Point cp2, Point to)
    {
        this.CurrentPoint = to;
        this.Sink.CurveTo(cp1, cp2, this.CurrentPoint);
        this.NextQuadraticControlPoint = to;
        this.NextCubicControlPoint = this.CurrentPoint + (this.CurrentPoint - cp2);
        return this;
    }

    public PathDataBuilder c(Vector cv1, Vector cv2, Vector to) =>
        C(this.CurrentPoint + cv1, this.CurrentPoint + cv2, this.CurrentPoint + to);

    public PathDataBuilder S(Point cp, Point to) =>
        C(this.NextCubicControlPoint, cp, to);

    public PathDataBuilder s(Vector cv, Vector to) =>
        C(this.NextCubicControlPoint, this.CurrentPoint + cv, this.CurrentPoint + to);
    
    public PathDataBuilder Q(Point cp, Point to)
    {
        this.Sink.CurveTo(cp, to);
        this.NextQuadraticControlPoint = to + (to - cp);
        this.NextCubicControlPoint = to;
        this.CurrentPoint = to;
        return this;
    }

    public PathDataBuilder q(Vector cv, Vector to) =>
        Q(this.CurrentPoint + cv, this.CurrentPoint + to);
    
    public PathDataBuilder T(Point to) =>
        Q(this.NextQuadraticControlPoint, to);
    
    public PathDataBuilder t(Vector to) =>
        Q(this.NextQuadraticControlPoint, this.CurrentPoint + to);
    
    public PathDataBuilder A(Vector sr, nfloat xAxisRotationDeg, ArcFlag largeArcFlag, Winding sweepDirection, Point p2)
    {
        Vector p1 = this.CurrentPoint;

        // A rx ry x-axis-rotation large-arc-flag sweep-flag x y
        /// https://svgwg.org/svg2-draft/implnote.html#ArcConversionEndpointToCenter
        /// https://observablehq.com/@awhitty/svg-2-elliptical-arc-to-canvas-path2d

        var xAxisRotation = nfloat.DegreesToRadians(xAxisRotationDeg);
        var p = AffineTransform.Rotate(xAxisRotation) * (p2 - p1) * 0.5f;

        var absR = new Vector(nfloat.Abs(sr.X), nfloat.Abs(sr.Y));
        nfloat A = p.X * p.X / (absR.X * absR.X) + p.Y * p.Y / (absR.Y * absR.Y);
        var r = A > 1 ? nfloat.Sqrt(A) * absR : absR;

        var sign = ((largeArcFlag == ArcFlag.Large) != (sweepDirection == Winding.ClockWise)) ? 1 : -1;
        nfloat n = r.X*r.X * r.Y*r.Y - r.X*r.X * p.Y*p.Y - r.Y*r.Y * p.X*p.X;
        nfloat d = r.X*r.X * p.Y*p.Y + r.Y*r.Y * p.X*p.X;

        var cp = new Vector(r.X * p.Y / r.Y, (-r.Y * p.X) / r.X) * sign * nfloat.Sqrt(nfloat.Abs(n / d));

        var c = AffineTransform.Rotate(-xAxisRotation) * cp + ((p1 + p2) * 0.5f);

        var a = new Vector((p.X - cp.X) / r.X, (p.Y - cp.Y) / r.X);
        var b = new Vector((-p.X - cp.X) / r.X, (-p.Y - cp.Y) / r.Y);

        var startAngle = Vector.Angle((1, 0), a);
        var deltaAngle0 = Vector.Angle(a, b) % (2 * nfloat.Pi);

        var deltaAngle =
            sweepDirection == Winding.ClockWise && deltaAngle0 > 0
                ? deltaAngle0 - 2 * nfloat.Pi
                : sweepDirection == Winding.CounterClockWise && deltaAngle0 < 0
                ? deltaAngle0 + 2 * nfloat.Pi
                : deltaAngle0;
        
        var endAngle = startAngle + deltaAngle;

        this.Sink.Ellipse((Point)c, r.X, r.Y, xAxisRotation, startAngle, endAngle, deltaAngle >= 0 ? Winding.ClockWise : Winding.CounterClockWise);

        this.CurrentPoint = (Point)p2;
        this.NextCubicControlPoint = this.CurrentPoint;
        this.NextQuadraticControlPoint = this.CurrentPoint;

        return this;
    }

    public PathDataBuilder a(Vector v2, nfloat xAxisRotationDeg, ArcFlag largeArcFlag, Winding sweepDirection, Vector sr) =>
        A(this.CurrentPoint + v2, xAxisRotationDeg, largeArcFlag, sweepDirection, this.CurrentPoint + sr);

    public PathDataBuilder Z()
    {
        this.Sink.ClosePath();
        this.CurrentPoint = this.StartPoint ?? (0, 0);
        this.NextCubicControlPoint = this.CurrentPoint;
        this.NextQuadraticControlPoint = this.CurrentPoint;
        return this;
    }

    public PathDataBuilder z() => Z();
}
