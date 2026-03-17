using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

/// <summary>
/// A simple text label for testing.
/// Calculates desired width as: number of characters × 0.75 × font size.
/// Calculates desired height as: font size.
/// Uses the Xui base layout mechanism without relying on actual text measurement.
/// </summary>
public class TextLabel : View
{
    /// <summary>
    /// Gets or sets the text content.
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// Gets or sets the font size. Determines both height and width calculation.
    /// </summary>
    public nfloat FontSize { get; set; } = 14;

    /// <summary>
    /// Gets or sets the text color.
    /// </summary>
    public Color TextColor { get; set; } = Colors.Black;

    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        // Simple text measurement formula from problem statement:
        // - Width: number of characters × 0.75 × font size
        // - Height: font size
        var width = (nfloat)(Text.Length * 0.75) * FontSize;
        var height = FontSize;

        // Constrain to available size
        width = nfloat.Min(width, availableBorderEdgeSize.Width);
        height = nfloat.Min(height, availableBorderEdgeSize.Height);

        return new Size(width, height);
    }

    /// <inheritdoc/>
    protected override void RenderCore(IContext context)
    {
        // Note: This is a simplified test view, actual text rendering would use context.FillText
        // For testing purposes, we just need the measurement behavior
        base.RenderCore(context);
    }
}
