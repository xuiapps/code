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
        var origin = new Point(350, 158);

        context.SetFill(White);
        context.FillRect(this.Frame);

        context.SetFont(new Font(64, ["Inter"], FontWeight.Normal));
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Alphabetic;
        var metrics = context.MeasureText(text);

        DrawBox(context, origin, metrics, metrics.Font.FontBoundingBoxAscent, metrics.Font.FontBoundingBoxDescent, Orange, 4);
        DrawBox(context, origin, metrics, metrics.Font.EmHeightAscent, metrics.Font.EmHeightDescent, Green, 2);
        DrawBox(context, origin, metrics, metrics.Line.ActualBoundingBoxAscent, metrics.Line.ActualBoundingBoxDescent, Red, 1);

        DrawLine(context, origin.Y + metrics.Font.HangingBaseline, Blue, 1, "hanging baseline");
        DrawLine(context, origin.Y + metrics.Font.AlphabeticBaseline, Black, 1, "alphabetic baseline");
        DrawLine(context, origin.Y + metrics.Font.IdeographicBaseline, Green, 1, "ideographic baseline");

        context.SetFill(Black);
        context.SetFont(new Font(64, ["Inter"], FontWeight.Normal));
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Alphabetic;
        context.FillText(text, origin);

        context.SetFont(new Font(13, ["Inter"], FontWeight.Normal));
        context.TextAlign = TextAlign.Left;
        context.TextBaseline = TextBaseline.Top;
        context.SetFill(Orange);
        context.FillText("font bounding box", (24, 18));
        context.SetFill(Green);
        context.FillText("em height", (24, 38));
        context.SetFill(Red);
        context.FillText("actual glyph bounds", (24, 58));
        context.SetFill(Black);
        context.FillText("Inter · 64 px", (24, 278));
    }

    private static void DrawBox(IContext context, Point origin, TextMetrics metrics, nfloat ascent, nfloat descent, Color color, nfloat lineWidth)
    {
        context.SetStroke(color);
        context.LineWidth = lineWidth;
        context.StrokeRect(new Rect(
            origin.X - metrics.Line.ActualBoundingBoxLeft,
            origin.Y - ascent,
            metrics.Line.ActualBoundingBoxLeft + metrics.Line.ActualBoundingBoxRight,
            ascent + descent));
    }

    private static void DrawLine(IContext context, nfloat y, Color color, nfloat lineWidth, string label)
    {
        context.BeginPath();
        context.MoveTo((140, y));
        context.LineTo((550, y));
        context.SetStroke(color);
        context.LineWidth = lineWidth;
        context.Stroke();

        context.SetFill(color);
        context.SetFont(new Font(11, ["Inter"], FontWeight.Normal));
        context.TextAlign = TextAlign.Left;
        context.TextBaseline = TextBaseline.Middle;
        context.FillText(label, (20, y));
    }
}
