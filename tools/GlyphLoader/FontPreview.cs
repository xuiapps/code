using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Xui.Core.Math2D;
using Xui.Runtime.Software.Font;

public static class FontPreview
{
    public static void WriteSVG(TrueTypeFont font, string text, string filePath, float fontSize = 64f, float padding = 32f)
    {
        var layout = new TextLayout(font, text, fontSize);
        var builder = new SvgPathBuilder
        {
            Scale = 1,
            Translate = default
        };

        layout.Visit(builder);

        // Compute bounds
        NFloat minX = NFloat.MaxValue, minY = NFloat.MaxValue;
        NFloat maxX = NFloat.MinValue, maxY = NFloat.MinValue;

        foreach (var pos in layout.Glyphs)
        {
            if (font.Hmtx != null)
            {
                var metric = font.Hmtx.GetMetric(pos.GlyphId);

                var x = pos.X; // Already scaled inside TextLayout
                var yTop = -fontSize * 0.25f; // Approximate ascender height
                var yBottom = fontSize;       // Approximate descender

                minX = NFloat.Min(minX, x);
                maxX = NFloat.Max(maxX, x + metric.AdvanceWidth * (fontSize / font.Head!.UnitsPerEm));
                minY = NFloat.Min(minY, yTop);
                maxY = NFloat.Max(maxY, yBottom);
            }
        }

        var width = maxX - minX + padding * 2;
        var height = maxY - minY + padding * 2;

        var tx = padding - minX;
        var ty = padding - minY;

        builder.Translate = new Vector(tx, ty);
        builder.Clear();
        layout.Visit(builder);

        var x1 = minX + tx;
        var x2 = maxX + tx;
        var y = 0 + ty; // baseline Y is logical 0 + vertical shift

        string F(NFloat v) => v.ToString("0.###", CultureInfo.InvariantCulture);

        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
        writer.WriteLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{F(width)}\" height=\"{F(height)}\" viewBox=\"0 0 {F(width)} {F(height)}\">");

        // Draw the baseline
        writer.WriteLine($"  <line x1=\"{F(x1)}\" y1=\"{F(y)}\" x2=\"{F(x2)}\" y2=\"{F(y)}\" stroke=\"gray\" stroke-width=\"1\" />");

        // Draw the glyph path
        writer.WriteLine($"  <path d=\"{builder.Path}\" fill=\"black\" rule=\"nonzero\" />");

        writer.WriteLine("</svg>");
        Console.WriteLine($"✅ SVG written: {filePath}");
    }
}
