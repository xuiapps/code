using System;
using System.IO;
using System.Text;
using Xui.Core.Canvas;
using Xui.Core.Curves2D;
using Xui.Core.Math2D;

namespace Xui.Runtime.Software.Actual;

public sealed class SvgDrawingContext : IContext, IDisposable
{
    private readonly Stream stream;
    private readonly StreamWriter writer;
    private readonly bool keepOpen;
    private readonly Size canvasSize;
    private bool disposed = false;

    private StringBuilder? currentPath;
    private Color? currentFillColor;
    private Color? currentStrokeColor;
    private FillRule currentFillRule = FillRule.NonZero;
    private bool hasOpenPath = false;

    public nfloat GlobalAlpha { get; set; } = 1f;
    public LineCap LineCap { get; set; } = LineCap.Butt;
    public LineJoin LineJoin { get; set; } = LineJoin.Miter;
    public nfloat LineWidth { get; set; } = 1f;
    public nfloat MiterLimit { get; set; } = 10f;
    public nfloat LineDashOffset { get; set; } = 0f;
    public TextAlign TextAlign { get; set; } = TextAlign.Start;
    public TextBaseline TextBaseline { get; set; } = TextBaseline.Alphabetic;

    public SvgDrawingContext(Size canvasSize, Stream output, bool keepOpen = false)
    {
        this.stream = output ?? throw new ArgumentNullException(nameof(output));
        this.writer = new StreamWriter(output, Encoding.UTF8, leaveOpen: keepOpen);
        this.keepOpen = keepOpen;
        this.canvasSize = canvasSize;

        WriteSvgHeader();
    }

    private void WriteSvgHeader()
    {
        writer.WriteLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{F(canvasSize.Width)}\" height=\"{F(canvasSize.Height)}\" viewBox=\"0 0 {F(canvasSize.Width)} {F(canvasSize.Height)}\">");
    }

    private void WriteSvgFooter()
    {
        writer.WriteLine("</svg>");
    }

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            WriteSvgFooter();
            writer.Flush();

            if (!keepOpen)
                writer.Dispose();
        }
    }

    void IStateContext.Save() => throw new NotImplementedException();
    void IStateContext.Restore() => throw new NotImplementedException();
    void IPenContext.SetLineDash(ReadOnlySpan<nfloat> segments) => throw new NotImplementedException();
    void IPenContext.SetStroke(Color color) => currentStrokeColor = color;
    void IPenContext.SetStroke(LinearGradient linearGradient) => throw new NotImplementedException();
    void IPenContext.SetStroke(RadialGradient radialGradient) => throw new NotImplementedException();
    void IPenContext.SetFill(Color color) => currentFillColor = color;
    void IPenContext.SetFill(LinearGradient linearGradient) => throw new NotImplementedException();
    void IPenContext.SetFill(RadialGradient radialGradient) => throw new NotImplementedException();

    void IPathBuilder.BeginPath()
    {
        currentPath = new StringBuilder();
        hasOpenPath = false;
    }

    void IGlyphPathBuilder.MoveTo(Point to)
    {
        EnsureClosedSubpath();
        currentPath ??= new StringBuilder();
        currentPath.Append($"M {F(to.X)} {F(to.Y)} ");
        hasOpenPath = true;
    }

    void IGlyphPathBuilder.LineTo(Point to)
    {
        currentPath?.Append($"L {F(to.X)} {F(to.Y)} ");
    }

    void IGlyphPathBuilder.CurveTo(Point control, Point to)
    {
        currentPath?.Append($"Q {F(control.X)} {F(control.Y)}, {F(to.X)} {F(to.Y)} ");
    }

    void IGlyphPathBuilder.ClosePath()
    {
        if (currentPath != null)
        {
            currentPath.Append("Z ");
            hasOpenPath = false;
        }
    }

    void IPathBuilder.Rect(Rect rect)
    {
        EnsureClosedSubpath();
        currentPath ??= new StringBuilder();

        currentPath.Append($"M {F(rect.X)} {F(rect.Y)} ");
        currentPath.Append($"H {F(rect.X + rect.Width)} ");
        currentPath.Append($"V {F(rect.Y + rect.Height)} ");
        currentPath.Append($"H {F(rect.X)} ");
        currentPath.Append("Z ");

        hasOpenPath = false;
    }

    void IPathBuilder.RoundRect(Rect rect, nfloat radius)
    {
        EnsureClosedSubpath();
        currentPath ??= new StringBuilder();

        var x = rect.X;
        var y = rect.Y;
        var w = rect.Width;
        var h = rect.Height;
        var r = nfloat.Min(radius, nfloat.Min(w / 2, h / 2));

        currentPath.Append($"M {F(x + r)} {F(y)} ");
        currentPath.Append($"H {F(x + w - r)} ");
        currentPath.Append($"A {F(r)} {F(r)} 0 0 1 {F(x + w)} {F(y + r)} ");
        currentPath.Append($"V {F(y + h - r)} ");
        currentPath.Append($"A {F(r)} {F(r)} 0 0 1 {F(x + w - r)} {F(y + h)} ");
        currentPath.Append($"H {F(x + r)} ");
        currentPath.Append($"A {F(r)} {F(r)} 0 0 1 {F(x)} {F(y + h - r)} ");
        currentPath.Append($"V {F(y + r)} ");
        currentPath.Append($"A {F(r)} {F(r)} 0 0 1 {F(x + r)} {F(y)} ");
        currentPath.Append("Z ");

        hasOpenPath = false;
    }

    void IPathDrawing.Fill(FillRule rule)
    {
        if (currentPath == null || currentFillColor == null)
            return;

        string fill = ToSvgColor(currentFillColor.Value, out nfloat? opacity);
        string fillRule = rule == FillRule.EvenOdd ? "evenodd" : "nonzero";

        writer.Write($"<path d=\"{currentPath}\" fill=\"{fill}\" fill-rule=\"{fillRule}\"");

        if (opacity.HasValue)
            writer.Write($" fill-opacity=\"{F(opacity.Value)}\"");

        writer.WriteLine(" />");

        currentPath = null;
        hasOpenPath = false;
    }

    void IPathDrawing.Stroke()
    {
        if (currentPath == null || currentStrokeColor == null)
            return;

        string stroke = ToSvgColor(currentStrokeColor.Value, out nfloat? opacity);

        writer.Write($"<path d=\"{currentPath}\" fill=\"none\" stroke=\"{stroke}\"");

        if (LineWidth != 1f)
            writer.Write($" stroke-width=\"{F(LineWidth)}\"");

        if (LineCap != LineCap.Butt)
            writer.Write($" stroke-linecap=\"{LineCap.ToString().ToLowerInvariant()}\"");

        if (LineJoin != LineJoin.Miter)
            writer.Write($" stroke-linejoin=\"{LineJoin.ToString().ToLowerInvariant()}\"");

        if (LineJoin == LineJoin.Miter && MiterLimit != 10f)
            writer.Write($" stroke-miterlimit=\"{F(MiterLimit)}\"");

        if (opacity.HasValue)
            writer.Write($" stroke-opacity=\"{F(opacity.Value)}\"");

        writer.WriteLine(" />");

        currentPath = null;
        hasOpenPath = false;
    }


    void IPathClipping.Clip() => throw new NotImplementedException();
    void IRectDrawingContext.StrokeRect(Rect rect) => throw new NotImplementedException();
    void IRectDrawingContext.FillRect(Rect rect) => throw new NotImplementedException();
    void ITextDrawingContext.FillText(string text, Point pos) => throw new NotImplementedException();
    Vector ITextMeasureContext.MeasureText(string text) => throw new NotImplementedException();
    void ITextMeasureContext.SetFont(Core.Canvas.Font font) => throw new NotImplementedException();
    void ITransformContext.Translate(Vector vector) => throw new NotImplementedException();
    void ITransformContext.Rotate(nfloat angle) => throw new NotImplementedException();
    void ITransformContext.Scale(Vector vector) => throw new NotImplementedException();
    void ITransformContext.SetTransform(AffineTransform transform) => throw new NotImplementedException();
    void ITransformContext.Transform(AffineTransform matrix) => throw new NotImplementedException();

    private static string ToSvgColor(Color color, out nfloat? opacity)
    {
        byte r = (byte)(color.Red * 255);
        byte g = (byte)(color.Green * 255);
        byte b = (byte)(color.Blue * 255);

        opacity = color.Alpha < 1f ? color.Alpha : null;
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private void EnsureClosedSubpath()
    {
        if (hasOpenPath && currentPath != null)
        {
            currentPath.Append("Z ");
            hasOpenPath = false;
        }
    }

    void IPathBuilder.CurveTo(Point cp1, Point cp2, Point to)
    {
        currentPath ??= new StringBuilder();
        currentPath.Append($"C {F(cp1.X)} {F(cp1.Y)}, {F(cp2.X)} {F(cp2.Y)}, {F(to.X)} {F(to.Y)} ");
    }

    void IPathBuilder.Arc(Point center, nfloat radius, nfloat startAngle, nfloat endAngle, Winding winding)
    {
        currentPath ??= new StringBuilder();

        var arc = new Arc(center, radius, radius, 0f, startAngle, endAngle, winding);
        var (arc1, arc2) = arc.ToEndpointArcs();

        AppendEndpointArcToPath(arc1);

        if (arc2 is EndpointArc second)
            AppendEndpointArcToPath(second);
    }

    private void AppendEndpointArcToPath(EndpointArc arc)
    {
        currentPath ??= new StringBuilder();

        int largeArcFlag = arc.LargeArc ? 1 : 0;
        int sweepFlag = arc.Winding == Winding.ClockWise ? 1 : 0;

        // If the path is not open yet, move to start
        if (!hasOpenPath)
        {
            currentPath.Append($"M {F(arc.Start.X)} {F(arc.Start.Y)} ");
            hasOpenPath = true;
        }

        currentPath.Append($"A {F(arc.RadiusX)} {F(arc.RadiusY)} {F(arc.Rotation * 180 / nfloat.Pi)} {F(largeArcFlag)} {F(sweepFlag)} {F(arc.End.X)} {F(arc.End.Y)} ");
    }

    void IPathBuilder.ArcTo(Point cp1, Point cp2, nfloat radius)
    {
        throw new NotImplementedException();
    }

    void IPathBuilder.Ellipse(Point center, nfloat radiusX, nfloat radiusY, nfloat rotation, nfloat startAngle, nfloat endAngle, Winding winding)
    {
        currentPath ??= new StringBuilder();

        var arc = new Arc(center, radiusX, radiusY, rotation, startAngle, endAngle, winding);
        var (arc1, arc2) = arc.ToEndpointArcs();

        AppendEndpointArcToPath(arc1);

        if (arc2 is EndpointArc second)
            AppendEndpointArcToPath(second);
    }

    void IPathBuilder.RoundRect(Rect rect, CornerRadius radius)
    {
        currentPath ??= new StringBuilder();
        EnsureClosedSubpath();

        nfloat x = rect.X;
        nfloat y = rect.Y;
        nfloat w = rect.Width;
        nfloat h = rect.Height;

        nfloat tl = nfloat.Min(radius.TopLeft, nfloat.Min(w, h) * (nfloat).5);
        nfloat tr = nfloat.Min(radius.TopRight, nfloat.Min(w, h) * (nfloat).5);
        nfloat br = nfloat.Min(radius.BottomRight, nfloat.Min(w, h) * (nfloat).5);
        nfloat bl = nfloat.Min(radius.BottomLeft, nfloat.Min(w, h) * (nfloat).5);

        // Top-left corner start
        currentPath.Append($"M {F(x + tl)} {F(y)} ");
        hasOpenPath = true;

        // Top edge
        currentPath.Append($"H {F(x + w - tr)} ");
        // Top-right corner arc
        AppendEndpointArcToPath(new EndpointArc(
            new Point(x + w - tr, y),
            new Point(x + w, y + tr),
            tr, tr, 0f, false, Winding.ClockWise));

        // Right edge
        currentPath.Append($"V {F(y + h - br)} ");
        // Bottom-right arc
        AppendEndpointArcToPath(new EndpointArc(
            new Point(x + w, y + h - br),
            new Point(x + w - br, y + h),
            br, br, 0f, false, Winding.ClockWise));

        // Bottom edge
        currentPath.Append($"H {F(x + bl)} ");
        // Bottom-left arc
        AppendEndpointArcToPath(new EndpointArc(
            new Point(x + bl, y + h),
            new Point(x, y + h - bl),
            bl, bl, 0f, false, Winding.ClockWise));

        // Left edge
        currentPath.Append($"V {F(y + tl)} ");
        // Top-left arc
        AppendEndpointArcToPath(new EndpointArc(
            new Point(x, y + tl),
            new Point(x + tl, y),
            tl, tl, 0f, false, Winding.ClockWise));

        currentPath.Append("Z ");
        hasOpenPath = false;
    }

    private static string F(nfloat value) =>
        value.ToString("G", System.Globalization.CultureInfo.InvariantCulture);
}
