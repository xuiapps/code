using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;

/// <summary>
/// A layout container that arranges its children vertically from top to bottom.
/// 
/// Each child is measured with an unconstrained height and is allowed to take up as much vertical space as needed.
/// The container expands to fit the combined height of all children.
/// </summary>
public class VerticalStack : ViewCollection
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        Size desiredBorderEdgeBoxSize = (0, 0);
        Size availableChildSize = (availableBorderEdgeSize.Width, nfloat.PositiveInfinity);
        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var desiredChildMarginEdgeBox = child.Measure(availableChildSize, context);
            desiredBorderEdgeBoxSize.Width = nfloat.Max(desiredBorderEdgeBoxSize.Width, desiredChildMarginEdgeBox.Width);
            desiredBorderEdgeBoxSize.Height += desiredChildMarginEdgeBox.Height;
        }
        return desiredBorderEdgeBoxSize;
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        nfloat y = rect.Y;
        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var desired = child.Measure((rect.Size.Width, nfloat.PositiveInfinity), context);

            // Arrange child at (X, Y), with width/height = desired
            var childRect = new Rect(rect.X, y, rect.Width, desired.Height);
            child.Arrange(childRect, context);

            y += desired.Height;
        }
    }
}
