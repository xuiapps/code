using Xui.Core.Canvas;
using Xui.Core.Math2D;
using static Xui.Core.Canvas.Colors;

namespace DocsGenerator.SvgDrawingContext;

/// <summary>
/// Demonstrates basic vector drawing using SvgDrawingContext,
/// including fills, strokes, rounded rectangles, and Bézier paths.
/// </summary>
public class SvgDrawingContextArticle : Article
{
    public SvgDrawingContextArticle() : base("SvgDrawingContext") { }

    public override void Build()
    {
        base.Build();

        var size = new Size(240, 120);
        using var stream = WriteFile("svg-context-demo.svg");
        using var context = (IContext)new Xui.Runtime.Software.Actual.SvgDrawingContext(size, stream);

        // Filled rounded rect background
        context.SetFill(Purple);
        context.BeginPath();  // 🔁 Begin a new path
        context.RoundRect(new Rect(10, 10, 220, 100), 16);
        context.Fill(FillRule.NonZero);

        // Heart path with stroke
        context.SetStroke(DeepPink);
        context.LineWidth = 2.5f;
        context.BeginPath();  // 🔁 Begin a new path
        context.MoveTo(new Point(120, 35));
        context.CurveTo(new Point(150, 5), new Point(180, 60), new Point(120, 90));
        context.CurveTo(new Point(60, 60), new Point(90, 5), new Point(120, 35));
        context.ClosePath();
        context.Stroke();

        // Small filled dot
        context.SetFill(Blue);
        context.BeginPath();  // 🔁 Begin a new path
        context.Ellipse(new Point(120, 35), 4, 4, 0, 0, 2 * nfloat.Pi, Winding.ClockWise);
        context.Fill(FillRule.NonZero);
    }
}
