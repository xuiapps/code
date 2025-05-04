using System.Text;
using System.Globalization;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Runtime.Software.Tessellate;

namespace DocsGenerator.Tessellate;

/// <summary>
/// A documentation article showcasing the tessellation of vector paths into polygons,
/// including intermediate steps like flattening, contour extraction, and triangle generation.
/// </summary>
public class TesselateArticle : Article
{
    static nfloat precision = (nfloat)1.0f;

    public TesselateArticle() : base("Tesselate") {}

    public override void Build()
    {
        base.Build();
        AddHeartStages();
        AddHeartHighPrecisionStage();
        AddOShapeStages();
        AddFigure8Stages();
    }

    private void AddHeartStages()
    {
        var path = new Path2D();
        path.MoveTo(new Point(60, 20));
        path.CurveTo(new Point(100, 0), new Point(100, 60), new Point(60, 80));
        path.CurveTo(new Point(20, 60), new Point(20, 0), new Point(60, 20));
        path.ClosePath();

        var tess = PathTesselator.Fill(path, FillRule.EvenOdd, precision);
        ExportStages(path, tess, "tessellation-process.svg", 3, 120, 100);
    }

    private void AddHeartHighPrecisionStage()
    {
        var path = new Path2D();
        path.MoveTo(new Point(60, 20));
        path.CurveTo(new Point(100, 0), new Point(100, 60), new Point(60, 80));
        path.CurveTo(new Point(20, 60), new Point(20, 0), new Point(60, 20));
        path.ClosePath();

        var tess = PathTesselator.Fill(path, FillRule.EvenOdd, 0.25f);
        ExportStages(path, tess, "tessellation-heart-precise.svg", 3, 120, 100);
    }

    private void AddOShapeStages()
    {
        var path = CreateOPath();

        var evenOdd = PathTesselator.Fill(path, FillRule.EvenOdd, precision);
        ExportStages(path, evenOdd, "tessellation-o-even-odd.svg", 3, 120, 100);

        var nonZero = PathTesselator.Fill(path, FillRule.NonZero, precision);
        ExportStages(path, nonZero, "tessellation-o-non-zero.svg", 3, 120, 100);
    }

    private void AddFigure8Stages()
    {
        var path = CreateFigure8Path();

        var evenOdd = PathTesselator.Fill(path, FillRule.EvenOdd, precision);
        ExportStages(path, evenOdd, "tessellation-8-even-odd.svg", 3, 120, 100);

        var nonZero = PathTesselator.Fill(path, FillRule.NonZero, precision);
        ExportStages(path, nonZero, "tessellation-8-non-zero.svg", 3, 120, 100);
    }

    private Path2D CreateOPath()
    {
        var path = new Path2D();

        // Outer circle (clockwise)
        path.MoveTo(new Point(60, 20));
        path.CurveTo(new Point(100, 20), new Point(100, 80), new Point(60, 80));
        path.CurveTo(new Point(20, 80), new Point(20, 20), new Point(60, 20));
        path.ClosePath();

        // Inner circle (same winding, for fill rule comparison)
        path.MoveTo(new Point(60, 35));
        path.CurveTo(new Point(75, 35), new Point(75, 65), new Point(60, 65));
        path.CurveTo(new Point(45, 65), new Point(45, 35), new Point(60, 35));
        path.ClosePath();

        return path;
    }

    private Path2D CreateFigure8Path()
    {
        var path = new Path2D();

        path.MoveTo((60, 20));
        path.CurveTo((100, 20), (100, 50), (60, 50));
        path.CurveTo((0, 50), (0, 100), (60, 100));
        path.CurveTo((120, 100), (120, 50), (60, 50));
        path.CurveTo((20, 50), (20, 20), (60, 20));
        path.ClosePath();

        path.MoveTo((60, 0));
        path.CurveTo((120, 0), (120, 60), (60, 60));
        path.CurveTo((20, 60), (20, 80), (60, 80));
        path.CurveTo((100, 80), (100, 60), (60, 60));
        path.CurveTo((0, 60), (0, 0), (60, 0));
        path.ClosePath();

        return path;
    }

    private void ExportStages(Path2D path, PathTesselator tess, string filename, int stages, int w, int h)
    {
        int spacing = 10;
        int blockWidth = w;
        int blockHeight = h;
        int totalWidth = stages * (blockWidth + spacing) - spacing;
        int labelHeight = 20;

        using var stream = WriteFile(filename);
        using var writer = new StreamWriter(stream, Encoding.UTF8);

        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        writer.WriteLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{totalWidth}\" height=\"{blockHeight + labelHeight}\" viewBox=\"0 0 {totalWidth} {blockHeight + labelHeight}\">");

        void Stage(string label, int i, Action drawFn)
        {
            int tx = i * (blockWidth + spacing);

            writer.WriteLine($"  <!-- {label} -->");
            writer.WriteLine($"  <g transform=\"translate({tx},0)\">");

            writer.WriteLine($"    <g transform=\"translate(0,0)\">");
            drawFn();
            writer.WriteLine($"    </g>");

            writer.WriteLine($"    <text x=\"{blockWidth / 2}\" y=\"{blockHeight + 15}\" font-family=\"sans-serif\" font-size=\"10\" fill=\"#333\" text-anchor=\"middle\">{label}</text>");
            writer.WriteLine("  </g>");
        }

        Stage("Path2D", 0, () =>
        {
            writer.WriteLine($"      <path d=\"{SvgPathFrom(path)}\" fill=\"none\" stroke=\"#444\" stroke-width=\"1.5\" />");
        });

        Stage("Contours", 1, () =>
        {
            foreach (var contour in tess.Contours)
            {
                var points = string.Join(" ", contour.Points.Select(p => $"{F(p.X)},{F(p.Y)}"));
                writer.WriteLine($"      <polyline points=\"{points}\" fill=\"none\" stroke=\"#3366cc\" stroke-width=\"1\" />");
            }
        });

        Stage("Triangles", 2, () =>
        {
            foreach (var poly in tess.Polygons)
            {
                var points = string.Join(" ", poly.Select(p => $"{F(p.X)},{F(p.Y)}"));
                writer.WriteLine($"      <polygon points=\"{points}\" fill=\"#f08080\" stroke=\"#990000\" stroke-width=\"1\" />");
            }
        });

        writer.WriteLine("</svg>");
    }

    private static string F(nfloat value) => value.ToString(CultureInfo.InvariantCulture);

    private static string SvgPathFrom(Path2D path)
    {
        var sb = new StringBuilder();
        var culture = CultureInfo.InvariantCulture;
        path.Visit(new SvgPathBuilder(sb, culture));
        return sb.ToString();
    }
}
