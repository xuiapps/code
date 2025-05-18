using Xui.Core.Canvas;
using Xui.Core.Math2D;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.Math2D.Constants;
using static Xui.Core.Canvas.Winding;
using Xui.Core.Canvas.SVG;
using Xui.Core.UI;
using static Xui.Core.UI.HorizontalAlignment;

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

        SvgContextDemo();
        SvgXuiGradientsDemo();
        SvgXuiStackWithLabels();
    }

    private void SvgContextDemo()
    {
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

    private void SvgXuiGradientsDemo()
    {
        using var stream = WriteFile("svg-xui-logo-gradients.svg");

        using var context = (IContext)new Xui.Runtime.Software.Actual.SvgDrawingContext((64, 64), stream);

        // using the Xui.Core.Canvas.SVG extensions

        // Outer clip shape (rounded squircle)
        context.PathData().Begin()
            .M((0, 32))
            .C((0, 4.16f), (4.16f, 0), (32, 0))
            .C((59.84f, 0), (64, 4.16f), (64, 32))
            .C((64, 59.84f), (59.84f, 64), (32, 64))
            .C((4.16f, 64), (0, 59.84f), (0, 32))
            .Z();
        context.Clip();

        // Background gradient fill
        context.SetFill(new LinearGradient(
            start: (10, 5),
            end: (54, 59),
            gradient: [
                (0, 0xD642CDFF),
                (1, 0x8A05FFFF),
            ]));
        context.PathData().Begin()
            .M((0, 0)).H(64).V(64).H(0).Z(); // Equivalent to context.Rect((0, 0, 64, 64))
        context.Fill();

        // Left-side triangles
        context.SetFill(new LinearGradient(
            start: (0, 20),
            end: (64, 44),
            gradient: [
                (0, 0xE8BEEDFF),
                (1, 0xE4CFE5FF)
            ]));

        // Top triangle
        context.PathData().Begin()
            .M((5, 0)).L((15, 22)).L((25, 0)).Z();
        context.Fill();

        // Bottom triangle
        context.PathData().Begin()
            .M((5, 64)).L((15, 42)).L((25, 64)).Z();
        context.Fill();

        // Left triangle
        context.PathData().Begin()
            .M((0, 10)).L((10, 32)).L((0, 54)).Z();
        context.Fill();

        // Stylized "UI" strokes
        context.SetStroke(White);
        context.LineWidth = 5;

        // "U" shape
        context.PathData().Begin()
            .M((25, 24)).L((25, 35))
            .Q((25, 45.5f), (32, 45.5f))
            .Q((39, 45.5f), (39, 35)).L((39, 24));
        context.Stroke();

        // "I" vertical stroke
        context.PathData().Begin()
            .M((49, 24)).L((49, 47));
        context.Stroke();

        // Dot on the "I"
        context.SetFill(White);
        context.BeginPath();
        context.Ellipse((49, 18), 3, 3, 0, 0, π * 2, ClockWise);
        context.Fill();
    }

    private void SvgXuiStackWithLabels()
    {
        var size = new Size(120, 120);
        using var stream = WriteFile("svg-context-fonts.svg");

        using var context = (IContext)new Xui.Runtime.Software.Actual.SvgDrawingContext(
            size,
            stream,
            // List Xui.Core.Fonts embedded fonts
            Xui.Core.Fonts.Inter.URIs,
            // Maps embedded://Xui.Core.Fonts/* fonts to xuiapps.com/fonts/* web urls
            DocsGenerator.XuiDemoFontResolver.Instance);

        var root = new VerticalStack()
        {
            Content = [
                new Border() {
                    Margin = 5,
                    BorderThickness = 1,
                    BorderColor = Red,
                    CornerRadius = 3,
                    BackgroundColor = White,
                    HorizontalAlignment = Left,
                    Content = new Label() {
                        TextColor = Black,
                        FontFamily = ["Inter"],
                        Text = "Hello World"
                    }
                },
                new Border() {
                    Margin = 5,
                    BorderThickness = 1,
                    BorderColor = Red,
                    CornerRadius = 3,
                    BackgroundColor = White,
                    HorizontalAlignment = Left,
                    Content = new Label() {
                        TextColor = Black,
                        FontFamily = ["Inter"],
                        Text = "Xui"
                    }
                },
                new Border() {
                    Margin = 5,
                    BorderThickness = 1,
                    BorderColor = Red,
                    CornerRadius = 3,
                    BackgroundColor = White,
                    HorizontalAlignment = Left,
                    Content = new Label() {
                        TextColor = Black,
                        FontFamily = ["Inter"],
                        Text = ":)"
                    }
                }
            ]
        };

        root.Update(new LayoutGuide()
        {
            AvailableSize = size,
            Anchor = (0, 0),
            XAlign = LayoutGuide.Align.Start,
            YAlign = LayoutGuide.Align.Start,
            XSize = LayoutGuide.SizeTo.Exact,
            YSize = LayoutGuide.SizeTo.Exact,
            Pass = LayoutGuide.LayoutPass.Measure | LayoutGuide.LayoutPass.Arrange | LayoutGuide.LayoutPass.Render,
            MeasureContext = context,
            RenderContext = context
        });
    }
}
