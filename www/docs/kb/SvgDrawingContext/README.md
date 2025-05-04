# SvgDrawingContext

This article demonstrates how to use Xui’s `SvgDrawingContext` to draw vector graphics directly into an SVG file, using the same APIs available on native platforms. This backend is ideal for static output, testing, or generating documentation visuals without relying on a GPU or rasterization.

The example below includes:

* A filled rounded rectangle
* A heart-shaped Bézier path with stroke
* A small filled circle

The output is written to an SVG file using `System.IO.Stream`, and the same drawing commands could run on any supported platform (Direct2D, CoreGraphics, Skia) without changes.

## SVG Output

![SVG Preview](svg-context-demo.svg)

## Example Code

```csharp
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using static Xui.Core.Canvas.Colors;

var size = new Size(240, 120);
using var stream = File.Create("svg-context-demo.svg");
using var context = (IContext)new Xui.Runtime.Software.Actual.SvgDrawingContext(size, stream);

// Filled rounded rect background
context.SetFill(Purple);
context.BeginPath();
context.RoundRect(new Rect(10, 10, 220, 100), 16);
context.Fill(FillRule.NonZero);

// Heart path with stroke
context.SetStroke(DeepPink);
context.LineWidth = 2.5f;
context.BeginPath();
context.MoveTo(new Point(120, 35));
context.CurveTo(new Point(150, 5), new Point(180, 60), new Point(120, 90));
context.CurveTo(new Point(60, 60), new Point(90, 5), new Point(120, 35));
context.ClosePath();
context.Stroke();

// Small filled dot
context.SetFill(Blue);
context.BeginPath();
context.Ellipse(new Point(120, 35), 4, 4, 0, 0, 2 * nfloat.Pi, Winding.ClockWise);
context.Fill(FillRule.NonZero);
```

> 🔪 This is also a great tool for unit tests and documentation—allowing you to validate and visualize rendering output in a headless environment.
