using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.Colors;
using Xui.Core.Canvas.SVG;
using System.Runtime.InteropServices;
using System.Net.Mime;

namespace Xui.Apps.TestApp.Examples;

public class TextMetricsExample : Example
{
    public TextMetricsExample()
    {
        this.Title = "TextMetrics";
        this.Content = new TextMetricsView();
    }

    public class TextMetricsView : View
    {
        protected override void RenderCore(IContext context)
        {
            context.SetFont(new()
            {
                FontFamily = ["Inter"],
                FontSize = 64,
                FontStretch = FontStretch.Normal,
                FontStyle = FontStyle.Normal,
                FontWeight = FontWeight.Normal
                // FontStyle = FontStyle.Italic,
                // FontWeight = FontWeight.SemiBold
            });

            context.TextAlign = TextAlign.Center;
            context.TextBaseline = TextBaseline.Alphabetic;

            var metrics = context.MeasureText("Hello World!");

            context.SetFill(Black);

            var c1 = this.Frame.Center;
            context.FillText("Hello World!", c1);

            context.BeginPath();
            context.MoveTo((c1.X - 200, c1.Y + metrics.Font.HangingBaseline));
            context.LineTo((c1.X + 200, c1.Y + metrics.Font.HangingBaseline));
            context.SetStroke(Blue);
            context.LineWidth = 2;
            context.Stroke();

            context.BeginPath();
            context.MoveTo((c1.X - 200, c1.Y + metrics.Font.AlphabeticBaseline));
            context.LineTo((c1.X + 200, c1.Y + metrics.Font.AlphabeticBaseline));
            context.SetStroke(Black);
            context.LineWidth = 2;
            context.Stroke();

            context.BeginPath();
            context.MoveTo((c1.X - 200, c1.Y + metrics.Font.IdeographicBaseline));
            context.LineTo((c1.X + 200, c1.Y + metrics.Font.IdeographicBaseline));
            context.SetStroke(Green);
            context.LineWidth = 2;
            context.Stroke();

            context.SetStroke(Green);
            context.LineWidth = 5;
            context.StrokeRect(new Rect(
                x: c1.X - metrics.Line.ActualBoundingBoxLeft,
                y: c1.Y - metrics.Font.EmHeightAscent,
                width: metrics.Line.ActualBoundingBoxLeft + metrics.Line.ActualBoundingBoxRight,
                height: metrics.Font.EmHeightAscent + metrics.Font.EmHeightDescent
            ));

            context.SetStroke(Orange);
            context.LineWidth = 3;
            context.StrokeRect(new Rect(
                x: c1.X - metrics.Line.ActualBoundingBoxLeft,
                y: c1.Y - metrics.Font.FontBoundingBoxAscent,
                width: metrics.Line.ActualBoundingBoxLeft + metrics.Line.ActualBoundingBoxRight,
                height: metrics.Font.FontBoundingBoxAscent + metrics.Font.FontBoundingBoxDescent
            ));

            context.SetStroke(Red);
            context.LineWidth = 1;
            context.StrokeRect(new Rect(
                x: c1.X - metrics.Line.ActualBoundingBoxLeft,
                y: c1.Y - metrics.Line.ActualBoundingBoxAscent,
                width: metrics.Line.ActualBoundingBoxLeft + metrics.Line.ActualBoundingBoxRight,
                height: metrics.Line.ActualBoundingBoxAscent + metrics.Line.ActualBoundingBoxDescent
            ));

            context.LineWidth = 3;
            context.SetStroke(Red);

            context.BeginPath();
            context.MoveTo((c1.X - 5, c1.Y));
            context.LineTo((c1.X + 5, c1.Y));
            context.MoveTo((c1.X, c1.Y - 5));
            context.LineTo((c1.X, c1.Y + 5));
            context.Stroke();
        }
    }
}