using System.Collections.Generic;
using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Catalog = Xui.Runtime.Software.Font.Catalog;

namespace Xui.Runtime.Software.Actual;

/// <summary>
/// A lightweight <see cref="ITextMeasureContext"/> backed by the software font
/// <see cref="Catalog"/>. Used to provide text hit-testing for pointer events
/// without needing a full drawing context.
/// </summary>
public sealed class SoftwareTextMeasureContext : ITextMeasureContext
{
    private readonly Catalog catalog;
    private readonly List<string> fontFamily = new();
    private nfloat fontSize;
    private FontWeight fontWeight;
    private FontStyle fontStyle;
    private FontStretch fontStretch;
    private nfloat lineHeight;

    public SoftwareTextMeasureContext(Catalog catalog)
    {
        this.catalog = catalog;
    }

    public TextMetrics MeasureText(string text)
    {
        var font = new Xui.Core.Canvas.Font(
            fontSize,
            CollectionsMarshal.AsSpan(fontFamily),
            fontWeight,
            fontStyle,
            fontStretch,
            lineHeight);
        return this.catalog.MeasureText(in font, text);
    }

    public void SetFont(Xui.Core.Canvas.Font font)
    {
        this.fontSize = font.FontSize;
        this.fontWeight = font.FontWeight;
        this.fontStyle = font.FontStyle;
        this.fontStretch = font.FontStretch;
        this.lineHeight = font.LineHeight;

        this.fontFamily.Clear();
        var span = font.FontFamily;
        for (int i = 0; i < span.Length; i++)
            this.fontFamily.Add(span[i]);
    }
}
