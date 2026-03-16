using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

/// <summary>
/// A simple box view for testing with predefined dimensions.
/// Uses Width/Height or MinWidth/MinHeight for layout measurement.
/// </summary>
public class TestBox : View
{
    /// <summary>
    /// Gets or sets the explicit width. If not set (NaN), uses MinWidth or available space.
    /// </summary>
    public nfloat Width { get; set; } = nfloat.NaN;

    /// <summary>
    /// Gets or sets the explicit height. If not set (NaN), uses MinHeight or available space.
    /// </summary>
    public nfloat Height { get; set; } = nfloat.NaN;

    /// <summary>
    /// Gets or sets the minimum width constraint.
    /// </summary>
    public nfloat MinWidth { get; set; } = 0;

    /// <summary>
    /// Gets or sets the minimum height constraint.
    /// </summary>
    public nfloat MinHeight { get; set; } = 0;

    /// <summary>
    /// Gets or sets the background color for the box.
    /// </summary>
    public Color BackgroundColor { get; set; } = Colors.Transparent;

    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        var width = nfloat.IsNaN(Width) ? MinWidth : Width;
        var height = nfloat.IsNaN(Height) ? MinHeight : Height;

        // Constrain to available size
        width = nfloat.Min(width, availableBorderEdgeSize.Width);
        height = nfloat.Min(height, availableBorderEdgeSize.Height);

        return new Size(width, height);
    }

    /// <inheritdoc/>
    protected override void RenderCore(IContext context)
    {
        if (BackgroundColor.Alpha > 0)
        {
            context.SetFill(BackgroundColor);
            context.FillRect(Frame);
        }
        base.RenderCore(context);
    }
}
