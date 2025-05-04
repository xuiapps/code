using System.Globalization;
using System.Reflection;
using System.Text;
using Xui.Core.Canvas;
using Xui.Runtime.Software.Font;

namespace DocsGenerator.FontReader;

/// <summary>
/// A documentation article that showcases the Xui TrueTypeFont reader,
/// including reading glyph outlines and visualizing text layout.
/// </summary>
public class FontReaderArticle : Article
{
    public FontReaderArticle() : base("FontReader") {}

    public override void Build()
    {
        base.Build();
        AddHelloWorldOutlines();
    }

    private void AddHelloWorldOutlines()
    {
        using var fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DocsGenerator.Inter-Regular.ttf")!;
        using var stream = WriteFile("hello-world-outlines.svg");
        using var writer = new StreamWriter(stream, Encoding.UTF8);

        var sb = new StringBuilder();
        var pathBuilder = new SvgPathBuilder(sb, CultureInfo.InvariantCulture);
        LoadFontAndGenerateTextLayoutGlyphsPath(fontStream, "Hello World!", 160f, pathBuilder);

        writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        writer.WriteLine("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"1600\" height=\"240\" viewBox=\"0 0 1600 240\">");
        writer.WriteLine("  <g transform=\"translate(0, 192)\">");
        writer.WriteLine($"    <path d=\"{sb}\" fill=\"none\" stroke=\"black\" stroke-width=\"2\" />");
        writer.WriteLine("  </g>");
        writer.WriteLine("</svg>");
    }

    /// <summary>
    /// Loads a font from a stream, lays out the given text at the specified size,
    /// and feeds glyph outlines into the given IPathBuilder.
    /// </summary>
    public static void LoadFontAndGenerateTextLayoutGlyphsPath(Stream fontStream, string text, float fontSize, IPathBuilder pathBuilder)
    {
        using var mem = new MemoryStream();
        fontStream.CopyTo(mem);
        var fontData = new ReadOnlyMemory<byte>(mem.ToArray());

        var font = new TrueTypeFont(fontData);
        var layout = new TextLayout(font, text, fontSize);
        layout.Visit(pathBuilder);
    }
}
