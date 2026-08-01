using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;

namespace Xui.Tests.Docs.Canvas.Views;

/// <summary>Visualizes the metrics returned for Inter 64px text.</summary>
public sealed class TextMetricsView : View
{
    protected override void RenderCore(IContext context)
    {
        const string text = "Hello World!";
        var origin = new Point(this.Frame.Center.X, this.Frame.Center.Y + 8);

        context.SetFill(White);
        context.FillRect(this.Frame);

        context.SetFont(new Font(64, "Inter", FontWeight.Normal));
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Alphabetic;
        var metrics = context.MeasureText(text);

        DrawAdvanceBox(context, origin, metrics, metrics.Font.FontBoundingBoxAscent, metrics.Font.FontBoundingBoxDescent, Orange, 4);
        DrawActualGlyphBox(context, origin, metrics, Red, 1);

        DrawLine(context, origin, metrics.Line.Width, origin.Y + metrics.Font.HangingBaseline, Blue, "hanging baseline");
        DrawLine(context, origin, metrics.Line.Width, origin.Y + metrics.Font.AlphabeticBaseline, Black, "alphabetic baseline");
        DrawLine(context, origin, metrics.Line.Width, origin.Y + metrics.Font.IdeographicBaseline, Green, "ideographic baseline");

        context.SetFill(Black);
        context.SetFont(new Font(64, "Inter", FontWeight.Normal));
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Alphabetic;
        context.FillText(text, origin);

    }

    private static void DrawAdvanceBox(IContext context, Point origin, TextMetrics metrics, nfloat ascent, nfloat descent, Color color, nfloat lineWidth)
    {
        context.SetStroke(color);
        context.LineWidth = lineWidth;
        context.StrokeRect(new Rect(
            origin.X - metrics.Line.Width / 2,
            origin.Y - ascent,
            metrics.Line.Width,
            ascent + descent));
    }

    private static void DrawActualGlyphBox(IContext context, Point origin, TextMetrics metrics, Color color, nfloat lineWidth)
    {
        context.SetStroke(color);
        context.LineWidth = lineWidth;
        context.StrokeRect(new Rect(
            origin.X - metrics.Line.ActualBoundingBoxLeft,
            origin.Y - metrics.Line.ActualBoundingBoxAscent,
            metrics.Line.ActualBoundingBoxLeft + metrics.Line.ActualBoundingBoxRight,
            metrics.Line.ActualBoundingBoxAscent + metrics.Line.ActualBoundingBoxDescent));
    }

    private static void DrawLine(IContext context, Point origin, nfloat width, nfloat y, Color color, string label)
    {
        context.BeginPath();
        context.MoveTo((origin.X - width / 2, y));
        context.LineTo((origin.X + width / 2, y));
        context.SetStroke(color);
        context.LineWidth = 1;
        context.Stroke();

        context.SetFill(color);
        context.SetFont(new Font(11, "Inter", FontWeight.Normal));
        context.TextAlign = TextAlign.Right;
        context.TextBaseline = TextBaseline.Middle;
        context.FillText(label, (origin.X - width / 2 - 8, y));
    }
}
