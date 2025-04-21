using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Runtime.Windows.Win32;
using static Xui.Runtime.Windows.D2D1;
using static Xui.Runtime.Windows.DWrite;

namespace Xui.Runtime.Windows.Actual;

public partial class Direct2DContext : IDisposable, IContext
{
    private readonly RenderTarget RenderTarget;

    private readonly D2D1.Factory3 D2D1Factory;

    private readonly DWrite.Factory DWriteFactory;

    private PaintStruct stroke;

    private PaintStruct fill;

    private StrokeStyleStruct StrokeStyle;

    private PathStruct Path;

    private float lineWidth = 1f;

    private Stack<DrawingStateBlock> drawingStateBlocks;
    private Stack<AffineTransform> transforms;
    private int layersCount = 0;
    private Stack<State> states;

    private TextFormat? textFormat;

    private struct State
    {
        public int Blocks;

        public int Layers;

        public int Transforms { get; internal set; }
    }

    public Direct2DContext(RenderTarget renderTarget, D2D1.Factory3 d2d1Factory, DWrite.Factory dWriteFactory)
    {
        this.RenderTarget = renderTarget;
        this.RenderTarget.AddRef();

        this.D2D1Factory = d2d1Factory;
        this.D2D1Factory.AddRef();

        this.DWriteFactory = dWriteFactory;
        this.DWriteFactory.AddRef();

        this.stroke = new PaintStruct(this.RenderTarget, Colors.Black);
        this.fill = new PaintStruct(this.RenderTarget, Colors.White);

        this.StrokeStyle = new StrokeStyleStruct(this.D2D1Factory);
        this.Path = new PathStruct(this.D2D1Factory);

        this.drawingStateBlocks = new Stack<DrawingStateBlock>();
        this.transforms = new Stack<AffineTransform>();
        this.states = new Stack<State>();
    }

    NFloat IPenContext.GlobalAlpha { set => throw new NotImplementedException(); }
    LineCap IPenContext.LineCap { set => this.StrokeStyle.LineCap = value; }
    Xui.Core.Canvas.LineJoin IPenContext.LineJoin { set => this.StrokeStyle.LineJoin = value; }
    NFloat IPenContext.LineWidth { set => this.lineWidth = (float)value; }
    NFloat IPenContext.MiterLimit { set => this.StrokeStyle.MiterLimit = value; }

    NFloat IPenContext.LineDashOffset { set => this.StrokeStyle.LineDashOffset = value; }

    public TextAlign TextAlign { get; set; }

    public TextBaseline TextBaseline { get; set; }

    void IStateContext.Save()
    {
        this.states.Push(new State()
        {
            Blocks = this.drawingStateBlocks.Count,
            Transforms = this.transforms.Count,
            Layers = this.layersCount
        });

        var block = this.D2D1Factory.CreateDrawingStateBlock();
        this.RenderTarget.SaveDrawingState(block);
        this.drawingStateBlocks.Push(block);

        Matrix3X2F transform;
        this.RenderTarget.GetTransform(out transform);
        this.transforms.Push(transform);

        // TODO: Fill & Stroke Brush
        // TODO: Pen properties
    }

    void IStateContext.Restore()
    {
        if (this.states.Count > 0)
        {
            var state = this.states.Pop();

            while(this.drawingStateBlocks.Count > state.Blocks)
            {
                var block = this.drawingStateBlocks.Pop();
                this.RenderTarget.RestoreDrawingState(block);
                block.Dispose();
            }

            while(this.layersCount > state.Layers)
            {
                this.layersCount--;
                this.RenderTarget.PopLayer();
            }

            while(this.transforms.Count > state.Transforms)
            {
                var transform = this.transforms.Pop();
                this.RenderTarget.SetTransform(transform);
            }
        }
    }

    void IPenContext.SetLineDash(ReadOnlySpan<NFloat> segments) =>
        this.StrokeStyle.SetLineDash(segments);

    void IPenContext.SetStroke(Color color) =>
        this.stroke.SetSolidColor(color);

    void IPenContext.SetStroke(LinearGradient linearGradient) =>
        this.stroke.SetLinearGradient(linearGradient);

    void IPenContext.SetStroke(RadialGradient radialGradient) =>
        this.stroke.SetRadialGradient(radialGradient);

    void IPenContext.SetFill(Color color) =>
        this.fill.SetSolidColor(color);

    void IPenContext.SetFill(LinearGradient linearGradient) =>
        this.fill.SetLinearGradient(linearGradient);

    void IPenContext.SetFill(RadialGradient radialGradient) =>
        this.fill.SetRadialGradient(radialGradient);

    void IPathDrawingContext.BeginPath() =>
        this.Path.BeginPath();

    void IPathDrawingContext.MoveTo(Point to) => this.Path.MoveTo(to);

    void IPathDrawingContext.LineTo(Point to) => this.Path.LineTo(to);

    void IPathDrawingContext.ClosePath() => this.Path.ClosePath();

    void IPathDrawingContext.CurveTo(Point cp1, Point to) => this.Path.CurveTo(cp1, to);

    void IPathDrawingContext.CurveTo(Point cp1, Point cp2, Point to) => this.Path.CurveTo(cp1, cp2, to);

    void IPathDrawingContext.Arc(Point center, NFloat radius, NFloat startAngle, NFloat endAngle, Winding winding) =>
        this.Path.Arc(center, radius, startAngle, endAngle, winding);

    void IPathDrawingContext.ArcTo(Point cp1, Point cp2, NFloat radius) =>
        this.Path.ArcTo(cp1, cp2, radius);

    void IPathDrawingContext.Ellipse(Point center, NFloat radiusX, NFloat radiusY, NFloat rotation, NFloat startAngle, NFloat endAngle, Winding winding) =>
        this.Path.Ellipse(center, radiusX, radiusY, rotation, startAngle, endAngle, winding);

    void IPathDrawingContext.Rect(Rect rect)
    {
        throw new NotImplementedException();
    }

    void IPathDrawingContext.RoundRect(Rect rect, NFloat radius) =>
        this.Path.RoundRect(rect, radius);

    void IPathDrawingContext.RoundRect(Rect rect, CornerRadius radius) =>
        this.Path.RoundRect(rect, radius);

    void IPathDrawingContext.Fill(FillRule rule)
    {
        var path = this.Path.PrepareToUse();
        if (path != null)
        {
            this.RenderTarget.FillGeometry(path, this.fill.Brush);
        }
        this.Path.ClearAfterUse();
    }

    void IPathDrawingContext.Stroke()
    {
        var path = this.Path.PrepareToUse();
        if (path != null)
        {
            this.RenderTarget.DrawGeometry(path, this.stroke.Brush, this.lineWidth, this.StrokeStyle.StrokeStyle);
        }
        this.Path.ClearAfterUse();
    }

    void IPathDrawingContext.Clip()
    {
        unsafe
        {
            Geometry? geometry = this.Path.PrepareToUse();
            if (geometry != null)
            {
                LayerParameters layerParameters = new LayerParameters();
                layerParameters.GeometricMask = geometry;
                this.RenderTarget.PushLayer(layerParameters);
                this.layersCount++;
            }
            this.Path.ClearAfterUse();
        }
    }

    void IRectDrawingContext.StrokeRect(Rect rect) =>
        this.RenderTarget.DrawRectangle(rect, this.stroke.Brush, this.lineWidth, this.StrokeStyle.StrokeStyle);

    void IRectDrawingContext.FillRect(Rect rect) =>
        this.RenderTarget.FillRectangle(rect, this.fill.Brush);

    void ITextDrawingContext.FillText(string text, Point pos)
    {
        if (this.textFormat != null)
        {
            this.RenderTarget.DrawText(text, this.textFormat, new RectF((float)pos.X, (float)pos.Y, float.PositiveInfinity, float.PositiveInfinity), this.fill.Brush);
        }
    }

    Vector ITextMeasureContext.MeasureText(string text)
    {
        if (this.textFormat != null)
        {
            using var layout = this.DWriteFactory.CreateTextLayout(text, this.textFormat, float.PositiveInfinity, float.PositiveInfinity);
            var textMetrics = layout.GetTextMetrics();
            return (textMetrics.Width, textMetrics.Height);
        }

        return (0, 0);
    }

    void ITextMeasureContext.SetFont(Xui.Core.Canvas.Font font)
    {
        string fontFamilyName = font.FontFamily[0];
        // Use system default
        FontCollection? fontCollection = null;
        FontWeight fontWeight = (FontWeight)(uint)font.FontWeight;
        DWrite.FontStyle fontStyle = font.FontStyle.IsItalic ? DWrite.FontStyle.Italic : (font.FontStyle.IsOblique ? DWrite.FontStyle.Oblique : DWrite.FontStyle.Normal);
        float fontSize = (float)font.FontSize;
        FontStretch fontStretch = FontStretch.Normal;

        this.textFormat?.Dispose();
        this.textFormat =  this.DWriteFactory.CreateTextFormat(fontFamilyName, fontCollection, fontWeight, fontStyle, fontStretch, fontSize, "en-US");
    }

    void ITransformContext.Translate(Vector vector)
    {
        Matrix3X2F matrix = new();
        this.RenderTarget.GetTransform(out matrix);
        AffineTransform transform = matrix;
        transform = transform * AffineTransform.Translate(vector);
        matrix = transform;
        this.RenderTarget.SetTransform(matrix);
    }

    void ITransformContext.Rotate(NFloat angle)
    {
        Matrix3X2F matrix = new();
        this.RenderTarget.GetTransform(out matrix);
        AffineTransform transform = matrix;
        transform = transform * AffineTransform.Rotate(angle);
        matrix = transform;
        this.RenderTarget.SetTransform(matrix);
    }

    void ITransformContext.Scale(Vector vector)
    {
        Matrix3X2F matrix = new();
        this.RenderTarget.GetTransform(out matrix);
        AffineTransform transform = matrix;
        transform = transform * AffineTransform.Scale(vector);
        matrix = transform;
        this.RenderTarget.SetTransform(matrix);
    }

    void ITransformContext.SetTransform(AffineTransform transform)
    {
        Matrix3X2F matrix = transform;
        this.RenderTarget.SetTransform(matrix);
    }

    void ITransformContext.Transform(AffineTransform matrix)
    {
        Matrix3X2F m = new();
        this.RenderTarget.GetTransform(out m);
        AffineTransform transform = m;
        transform = transform * matrix;
        m = transform;
        this.RenderTarget.SetTransform(m);
    }

    public void Dispose()
    {
        // If the RenderTarget dies, and we have to reacreate the Direct2DContext, we will really need to call these...
        // this.fill.Dispose();
        // this.stroke.Dispose();

        // TODO: The IContext is disposable, but it has different meaning from the Dispose here...
        // Make IContext IContext.Scope() that can dispose a scope, but doesn't wipe out the RenderTarget...
        // this.RenderTarget.Dispose();
        // GC.SuppressFinalize(this);
    }

    public void BeginDraw()
    {
    }

    public void EndDraw()
    {
        this.states.Clear();

        while(this.drawingStateBlocks.Count > 0)
        {
            var block = this.drawingStateBlocks.Pop();
            this.RenderTarget.RestoreDrawingState(block);
            block.Dispose();
        }

        while(this.layersCount > 0)
        {
            this.layersCount--;
            this.RenderTarget.PopLayer();
        }

        while(this.transforms.Count > 0)
        {
            Matrix3X2F transorm = this.transforms.Pop();
            this.RenderTarget.SetTransform(in transorm);
        }
        this.RenderTarget.SetTransform(in Matrix3X2F.Identity);
    }

    ~Direct2DContext()
    {
        // Debug.WriteLine($"Reached to finalizer for {this.GetType().FullName}. Treat as resource and call Dispose.");
        // this.RenderTarget.Dispose();
    }

    private struct PaintStruct : IDisposable
    {
        public readonly RenderTarget RenderTarget;

        public PaintStyle PaintStyle;

        public Brush Brush;

        public PaintStruct(RenderTarget renderTarget, Color color)
        {
            this.PaintStyle = PaintStyle.SolidColor;
            this.RenderTarget = renderTarget;
            this.Brush = this.RenderTarget.CreateSolidColorBrush(color);
        }

        public void Dispose()
        {
            this.Brush?.Dispose();
        }

        public void SetSolidColor(Color color)
        {
            this.Brush?.Dispose();
            this.Brush = this.RenderTarget.CreateSolidColorBrush(color);
            this.PaintStyle = PaintStyle.SolidColor;
        }

        public void SetLinearGradient(LinearGradient linearGradient)
        {
            this.Brush?.Dispose();

            LinearGradientBrush.Properties linearGradientBrushProperties = new()
            {
                StartPoint = linearGradient.StartPoint,
                EndPoint = linearGradient.EndPoint
            };
            BrushProperties brushProperties = new ()
            {
                Opacity = 1f,
                Transform = new() { _11 = 1f, _12 = 0f, _21 = 0f, _22 = 1f, _31 = 0f, _32 = 0f }
            };

            var abstractGradientStops = linearGradient.GradientStops;
            Span<D2D1.GradientStop> gradientStops = stackalloc D2D1.GradientStop[abstractGradientStops.Length];
            for(var i = 0; i < abstractGradientStops.Length; i++)
            {
                var abstr = abstractGradientStops[i];
                gradientStops[i] = new()
                {
                    Position = (float)abstr.Offset,
                    Color = abstr.Color
                };
            }

            this.Brush = this.RenderTarget.CreateLinearGradientBrush(linearGradientBrushProperties, brushProperties, gradientStops, Gamma.Gamma_2_2, ExtendMode.Clamp);
            this.PaintStyle = PaintStyle.LinearGradient;
        }

        public void SetRadialGradient(RadialGradient radialGradient)
        {
            this.Brush?.Dispose();

            RadialGradientBrush.Properties radialGradientBrushProperties = new()
            {
                GradientOriginOffset = radialGradient.StartCenter - radialGradient.EndCenter,
                Center = radialGradient.EndCenter,
                Radius = new Point2F() { X = (float)radialGradient.StartRadius, Y = (float)radialGradient.StartRadius },
                RadiusX = (float)radialGradient.EndRadius,
                RadiusY = (float)radialGradient.EndRadius,
            };

            BrushProperties brushProperties = new ()
            {
                Opacity = 1f,
                Transform = new() { _11 = 1f, _12 = 0f, _21 = 0f, _22 = 1f, _31 = 0f, _32 = 0f }
            };

            var abstractGradientStops = radialGradient.GradientStops;
            Span<D2D1.GradientStop> gradientStops = stackalloc D2D1.GradientStop[abstractGradientStops.Length];
            for(var i = 0; i < abstractGradientStops.Length; i++)
            {
                var abstr = abstractGradientStops[i];
                gradientStops[i] = new()
                {
                    Position = (float)abstr.Offset,
                    Color = abstr.Color
                };
            }

            this.Brush = this.RenderTarget.CreateRadialGradientBrush(radialGradientBrushProperties, brushProperties, gradientStops, Gamma.Gamma_2_2, ExtendMode.Clamp);
            this.PaintStyle = PaintStyle.LinearGradient;
        }
    }

    private struct StrokeStyleStruct
    {
        public readonly Factory3 Factory;

        private StrokeStyle? strokeStyle;

        private bool strokeStyleValid = false;

        private List<float> dashList;

        private LineCap lineCap;

        private Xui.Core.Canvas.LineJoin lineJoin;

        private float miterLimit;

        private float lineDashOffset;

        public StrokeStyleStruct(Factory3 factory)
        {
            this.Factory = factory;
            this.dashList = new List<float>();
            this.miterLimit = 10f;
            this.lineDashOffset = 0f;
        }

        public StrokeStyle? StrokeStyle => this.UpdateStrokeStyle();

        public LineCap LineCap
        {
            set
            {
                if (this.lineCap != value)
                {
                    this.lineCap = value;
                    this.InvalidateStroke();
                }
            }
        }

        public Xui.Core.Canvas.LineJoin LineJoin
        {
            set
            {
                if (this.lineJoin != value)
                {
                    this.lineJoin = value;
                    this.InvalidateStroke();
                }
            }
        }

        public NFloat MiterLimit
        {
            set
            {
                if (this.miterLimit != value)
                {
                    this.miterLimit = (float)value;
                    this.InvalidateStroke();
                }
            }
        }

        public NFloat LineDashOffset
        {
            set
            {
                if (this.lineDashOffset != value)
                {
                    this.lineDashOffset = (float)value;
                    this.InvalidateStroke();
                }
            }
        }

        public void SetLineDash(ReadOnlySpan<NFloat> segments)
        {
            this.dashList.Clear();
            foreach(var f in segments)
            {
                this.dashList.Add((float)f);
            }
            this.InvalidateStroke();
        }

        private void InvalidateStroke()
        {
            this.strokeStyle?.Dispose();
            this.strokeStyle = null;
            this.strokeStyleValid = false;
        }

        private StrokeStyle? UpdateStrokeStyle()
        {
            if (!this.strokeStyleValid)
            {
                if (this.dashList.Count > 0 ||
                    this.lineCap != LineCap.Butt ||
                    this.lineJoin != Xui.Core.Canvas.LineJoin.Miter ||
                    this.miterLimit != 10 ||
                    this.lineDashOffset != 0)
                {
                    // There are non-default values, build StrokeStyle
                    CapStyle capStyle = Map(this.lineCap);
                    StrokeStyleProperties strokeStyleProperties = new ()
                    {
                        StartCap = capStyle,
                        EndCap = capStyle,
                        DashCap = capStyle,
                        LineJoin = Map(this.lineJoin),
                        MiterLimit = this.miterLimit,
                        DashStyle = this.dashList.Count == 0 ? DashStyle.Solid : DashStyle.Custom,
                        DashOffset = this.lineDashOffset
                    };
                    this.strokeStyle = this.Factory.CreateStrokeStyle(strokeStyleProperties, CollectionsMarshal.AsSpan(this.dashList));
                }

                this.strokeStyleValid = true;
            }

            return this.strokeStyle;
        }

        private static D2D1.LineJoin Map(Xui.Core.Canvas.LineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case Xui.Core.Canvas.LineJoin.Bevel: return D2D1.LineJoin.Bevel;
                case Xui.Core.Canvas.LineJoin.Round: return D2D1.LineJoin.Round;
                default: return D2D1.LineJoin.Miter;
            }
        }

        private static CapStyle Map(LineCap lineCap)
        {
            switch(lineCap)
            {
                case LineCap.Round: return CapStyle.Round;
                case LineCap.Square: return CapStyle.Square;
                default: return CapStyle.Flat;
            }
        }
    }

    private struct PathStruct
    {
        public static readonly float HalfPI = float.Pi / 2f;

        public readonly Factory3 Factory;

        public PathGeometry? PathGeometry;

        public GeometrySink? GeometrySink;

        private Point point = Point.Zero;
        private bool hasOpenFigure;

        public PathStruct(Factory3 factory)
        {
            this.Factory = factory;
            this.PathGeometry = null;
            this.GeometrySink = null;
        }

        public void Dispose()
        {
            this.GeometrySink?.Dispose();
            this.GeometrySink = null;
            this.PathGeometry?.Dispose();
            this.PathGeometry = null;
        }

        public void BeginPath()
        {
            this.CloseFigureIfAny();
            this.GeometrySink?.Close();
            this.ClearAfterUse();
        }

        public void CurveTo(Point cp1, Point to)
        {
            this.CreatePathOnDemand();
            this.BeginFigureOnDemand();

            this.GeometrySink!.AddQuadraticBezier(new QuadraticBezierSegment()
            {
                Point1 = cp1,
                Point2 = to
            });

            this.point = to;
        }

        public void CurveTo(Point cp1, Point cp2, Point to)
        {
            this.CreatePathOnDemand();
            this.BeginFigureOnDemand();

            this.GeometrySink!.AddBezier(new BezierSegment()
            {
                Point1 = cp1,
                Point2 = cp2,
                Point3 = to
            });

            this.point = to;
        }

        public void LineTo(Point to)
        {
            this.CreatePathOnDemand();
            this.BeginFigureOnDemand();

            this.GeometrySink!.AddLine(to);

            this.point = to;
        }

        public void MoveTo(Point to)
        {
            this.CloseFigureIfAny();
            this.point = to;
        }

        public void ClosePath()
        {
            if (this.hasOpenFigure)
            {
                this.hasOpenFigure = false;
                this.GeometrySink!.EndFigure(FigureEnd.Closed);
            }
        }

        public void CreatePathOnDemand()
        {
            if (this.PathGeometry == null)
            {
                this.PathGeometry = this.Factory.CreatePathGeometry();
                this.GeometrySink = this.PathGeometry.Open();
            }
        }

        public void BeginFigureOnDemand()
        {
            if (!this.hasOpenFigure)
            {
                this.GeometrySink!.BeginFigure(this.point, FigureBegin.Filled);
                this.hasOpenFigure = true;
            }
        }

        public void BeginFigureOnDemandOrLineTo(Point startPoint)
        {
            if (this.hasOpenFigure)
            {
                this.LineTo(startPoint);
            }
            else
            {
                this.MoveTo(startPoint);
                this.GeometrySink!.BeginFigure(this.point, FigureBegin.Filled);
                this.hasOpenFigure = true;
            }
        }

        public void CloseFigureIfAny()
        {
            if (this.hasOpenFigure)
            {
                this.hasOpenFigure = false;
                this.GeometrySink!.EndFigure(FigureEnd.Open);
            }
        }

        public Geometry? PrepareToUse()
        {
            this.CloseFigureIfAny();
            this.GeometrySink?.Close();
            return this.PathGeometry;
        }

        public void ClearAfterUse()
        {
            this.GeometrySink?.Dispose();
            this.GeometrySink = null;
            this.PathGeometry?.Dispose();
            this.PathGeometry = null;
        }

        public void Arc(Point center, NFloat radius, NFloat startAngle, NFloat endAngle, Winding winding)
        {
            Point startPoint = Point.Zero + new Vector(float.Cos((float)startAngle), float.Sin((float)startAngle)) * radius;
            Point endPoint = Point.Zero + new Vector(float.Cos((float)endAngle), float.Sin((float)endAngle)) * radius;

            this.CreatePathOnDemand();
            this.BeginFigureOnDemandOrLineTo(startPoint);

            ArcSegment arcSegment = new()
            {
                Point = new Vector(float.Cos((float)endAngle), float.Sin((float)endAngle)) * radius,
                Size = new SizeF((float)radius),
                RotationAngle = 0f,
                SweepDirection = Map(winding),
                ArcSize = float.Abs((float)endAngle - (float)startAngle) > float.Pi ? ArcSize.Large : ArcSize.Small
            };

            this.GeometrySink!.AddArc(arcSegment);
        }

        private SweepDirection Map(Winding winding)
        {
            if (winding == Winding.ClockWise)
            {
                return SweepDirection.Clockwise;
            }
            else
            {
                return SweepDirection.CounterClockwise;
            }
        }

        public void ArcTo(Point cp1, Point cp2, NFloat radius)
        {
            var from = this.point - cp1;
            var to = cp2 - cp1;
            var median = (from + to).Normalized;

            var a = Vector.Angle(to, median);
            var mDist = radius / NFloat.Sin(a);
            var sDist = NFloat.Sin(a) * mDist;
            
            var center = cp1 + median * mDist;
            var startPoint = cp1 + from.Normalized * sDist;
            var endPoint = cp1 + to.Normalized * sDist;

            var cross = Vector.Cross(from, to);

            this.CreatePathOnDemand();
            this.BeginFigureOnDemandOrLineTo(this.point);
            this.LineTo(startPoint);
            this.GeometrySink!.AddArc(new ()
            {
                Point = endPoint,
                Size = new SizeF((float)radius),
                RotationAngle = 2f * (HalfPI - (float)a),
                SweepDirection = cross <= 0 ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
                ArcSize = ArcSize.Small
            });
            this.point = endPoint;
        }

        public void Ellipse(Point center, NFloat radiusX, NFloat radiusY, NFloat rotation, NFloat startAngle, NFloat endAngle, Winding winding)
        {
            var right = radiusX * new Vector(NFloat.Cos(rotation), NFloat.Sin(rotation));
            var bottom = radiusY * new Vector(-NFloat.Sin(rotation), NFloat.Cos(rotation));

            var startPoint = center + NFloat.Cos(startAngle) * right + NFloat.Sin(startAngle) * bottom;
            var endPoint = center + NFloat.Cos(endAngle) * right + NFloat.Sin(endAngle) * bottom;

            this.CreatePathOnDemand();
            this.MoveTo(startPoint);
            this.BeginFigureOnDemand();

            this.GeometrySink!.AddArc(new()
            {
                Point = endPoint,
                Size = new SizeF((float)radiusX, (float)radiusY),
                RotationAngle = float.RadiansToDegrees((float)rotation),
                SweepDirection = Map(winding),
                ArcSize = ArcSize.Small
            });
        }

        public void RoundRect(Rect rect, NFloat radius) =>
            this.RoundRect(rect, new CornerRadius(radius));

        public void RoundRect(Rect rect, CornerRadius radius)
        {
            this.CreatePathOnDemand();
            this.BeginFigureOnDemandOrLineTo(rect.TopLeft + (0, radius.TopLeft));

            this.GeometrySink!.AddArc(new()
            {
                Point = rect.TopLeft + (radius.TopLeft, 0),
                Size = new SizeF((float)radius.TopLeft),
                RotationAngle = HalfPI,
                SweepDirection = SweepDirection.Clockwise,
                ArcSize = ArcSize.Small
            });
            this.GeometrySink.AddLine(rect.TopRight + (-radius.TopRight, 0));
            this.GeometrySink!.AddArc(new()
            {
                Point = rect.TopRight + (0, radius.TopRight),
                Size = new SizeF((float)radius.TopRight),
                RotationAngle = HalfPI,
                SweepDirection = SweepDirection.Clockwise,
                ArcSize = ArcSize.Small
            });
            this.GeometrySink.AddLine(rect.BottomRight + (0, -radius.BottomRight));
            this.GeometrySink!.AddArc(new()
            {
                Point = rect.BottomRight + (-radius.BottomRight, 0),
                Size = new SizeF((float)radius.BottomRight),
                RotationAngle = HalfPI,
                SweepDirection = SweepDirection.Clockwise,
                ArcSize = ArcSize.Small
            });
            this.GeometrySink.AddLine(rect.BottomLeft + (radius.BottomLeft, 0));
            this.GeometrySink!.AddArc(new()
            {
                Point = rect.BottomLeft + (0, -radius.BottomLeft),
                Size = new SizeF((float)radius.BottomLeft),
                RotationAngle = HalfPI,
                SweepDirection = SweepDirection.Clockwise,
                ArcSize = ArcSize.Small
            });
            this.ClosePath();
        }
    }
}
