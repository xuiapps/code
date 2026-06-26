using System.Text.RegularExpressions;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Runtime.Software.Actual;
using Xui.Runtime.Software.Font;

namespace Xui.Core.Tests.Font;

public class SvgEmbeddedFontsTests
{
    [Fact]
    public void TrimmedInterFontRetainsOnlySubsetOfTables()
    {
        var regularUri = Xui.Core.Fonts.Inter.URIs.First(x =>
            x.AbsolutePath.EndsWith("/Inter-Regular.ttf", StringComparison.OrdinalIgnoreCase));

        var original = Catalog.LoadEmbedded(regularUri);
        var originalFont = new TrueTypeFont(original, regularUri);
        var trimmed = originalFont.TrimToTouchedTablesForSvg();
        var trimmedFont = new TrueTypeFont(trimmed);

        Assert.True(trimmed.Length < original.Length);
        Assert.True(trimmedFont.Tables.Count < originalFont.Tables.Count);

        Assert.NotNull(trimmedFont.Head);
        Assert.NotNull(trimmedFont.Maxp);
        Assert.NotNull(trimmedFont.Cmap);
        Assert.NotNull(trimmedFont.Hhea);
        Assert.NotNull(trimmedFont.Hmtx);
    }

    [Fact]
    public void SvgContextEmbedsTrimmedFontDataForUsedFaceOnly()
    {
        using var stream = new MemoryStream();
        using (var ctx = new SvgDrawingContext(new Size(200, 60), stream, Xui.Core.Fonts.Inter.URIs, keepOpen: true))
        {
            var family = new[] { "Inter" };
            var font = new Xui.Core.Canvas.Font(16, family);
            ((ITextMeasureContext)ctx).SetFont(font);

            ((ITextDrawingContext)ctx).FillText("Hello", new Point(10, 20));
            ((ITextDrawingContext)ctx).FillText("World", new Point(10, 40));
        }

        stream.Position = 0;
        var svg = new StreamReader(stream).ReadToEnd();

        Assert.Contains("@font-face", svg);
        Assert.Contains("data:font/ttf;base64,", svg);

        var matches = Regex.Matches(svg, "@font-face");
        Assert.Single(matches);

        var dataMatch = Regex.Match(svg, "data:font/ttf;base64,([^)'\\\"]+)");
        Assert.True(dataMatch.Success);

        var embedded = Convert.FromBase64String(dataMatch.Groups[1].Value);
        var embeddedFont = new TrueTypeFont(embedded);

        var regularUri = Xui.Core.Fonts.Inter.URIs.First(x =>
            x.AbsolutePath.EndsWith("/Inter-Regular.ttf", StringComparison.OrdinalIgnoreCase));
        var original = Catalog.LoadEmbedded(regularUri);
        var originalFont = new TrueTypeFont(original);

        Assert.True(embedded.Length < original.Length);
        Assert.True(embeddedFont.Tables.Count < originalFont.Tables.Count);
    }
}
