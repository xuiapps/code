using System.Runtime.CompilerServices;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Software;
using Xui.Runtime.Software.Actual;
using Xui.Tests.Docs.Canvas.Views;
using Xunit;

namespace Xui.Tests.Docs.Canvas;

/// <summary>
/// Generates SVG figures used in www/docs/canvas.md.
///
/// These are "generator" tests — they always write the output file and pass.
/// Run them locally after changing an example, then commit the updated SVGs.
///
/// Output: www/docs/img/canvas/*.svg  (relative to repo root via CallerFilePath)
/// </summary>
public class CanvasDocsTest
{
    private static readonly Size FigureSize = new Size(480, 240);

    [Fact] public void FillAndStroke() => Generate(new FillAndStrokeView(), "fill-and-stroke.svg");
    [Fact] public void Paths()         => Generate(new PathsView(),         "paths.svg");
    [Fact] public void Gradients()     => Generate(new GradientsView(),     "gradients.svg");
    [Fact] public void Clip()          => Generate(new ClipView(),          "clip.svg");
    [Fact] public void Transforms()    => Generate(new TransformsView(),    "transforms.svg");
    [Fact] public void TextMetrics()   => GenerateTextMetrics();

    private static void Generate(View view, string fileName, [CallerFilePath] string callerPath = "")
    {
        // Navigate from Xui/Tests/Docs/Canvas/ (4 levels) to repo root, then into www/docs/img/canvas/
        var sourceDir = Path.GetDirectoryName(callerPath)!;
        var repoRoot  = Path.GetFullPath(Path.Combine(sourceDir, "../../../.."));
        var outDir    = Path.Combine(repoRoot, "www", "docs", "img", "canvas");
        Directory.CreateDirectory(outDir);

        var svg = new SVGDocument
        {
            Content = view,
            Size = FigureSize,
            FontUris = Xui.Core.Fonts.Inter.URIs,
        }.ToSVG();
        File.WriteAllText(Path.Combine(outDir, fileName), svg);
    }

    private static void GenerateTextMetrics([CallerFilePath] string callerPath = "")
    {
        var sourceDir = Path.GetDirectoryName(callerPath)!;
        var repoRoot = Path.GetFullPath(Path.Combine(sourceDir, "../../../.."));
        var outDir = Path.Combine(repoRoot, "www", "docs", "img", "fonts");
        Directory.CreateDirectory(outDir);

        var svg = new SVGDocument
        {
            Content = new TextMetricsView(),
            Size = (600, 300),
            FontUris = Xui.Core.Fonts.Inter.URIs,
            FontResolver = new DocsFontResolver(),
        }.ToSVG();
        File.WriteAllText(Path.Combine(outDir, "text-metrics.svg"), svg);

        var fontSource = Path.Combine(repoRoot, "Xui", "Core", "Fonts", "Inter", "Inter-Regular.ttf");
        File.Copy(fontSource, Path.Combine(outDir, "Inter-Regular.ttf"), overwrite: true);
        File.Copy(Path.Combine(repoRoot, "Xui", "Core", "Fonts", "Inter", "LICENSE.txt"), Path.Combine(outDir, "LICENSE.txt"), overwrite: true);
    }

    private sealed class DocsFontResolver : SvgDrawingContext.SvgFontResolver
    {
        public override SvgDrawingContext.Resolved Resolve(Xui.Runtime.Software.Font.FontFace face, Uri? uri) =>
            string.Equals(face.Family, "Inter", StringComparison.OrdinalIgnoreCase)
                ? new(SvgDrawingContext.SvgFontMode.WebLink, new Uri("Inter-Regular.ttf", UriKind.Relative))
                : base.Resolve(face, uri);
    }
}
