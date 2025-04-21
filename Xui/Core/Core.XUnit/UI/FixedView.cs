using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

public class FixedView : View
{
    public Size Size { get; set; }

    protected override Size MeasureCore(Size availableBorderEdgeSize) => this.Size;
}
