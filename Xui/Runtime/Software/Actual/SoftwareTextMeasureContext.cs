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
    private Xui.Core.Canvas.Font font;

    public SoftwareTextMeasureContext(Catalog catalog)
    {
        this.catalog = catalog;
    }

    public TextMetrics MeasureText(string text)
    {
        return this.catalog.MeasureText(in font, text);
    }

    public void SetFont(Xui.Core.Canvas.Font font)
    {
        this.font = font;
    }
}
