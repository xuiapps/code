using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;

/// <summary>
/// A layout container that arranges its children horizontally from left to right.
/// 
/// Each child is measured with unconstrained width and a constrained height.
/// The container expands to fit the combined width of all children.
/// </summary>
public class HorizontalStack : ViewCollection
{
    /// <inheritdoc/>
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        Size desiredBorderEdgeBoxSize = (0, 0);
        Size availableChildSize = (nfloat.PositiveInfinity, availableBorderEdgeSize.Height);

        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var desiredChildMarginEdgeBox = child.Measure(availableChildSize, context);
            desiredBorderEdgeBoxSize.Width += desiredChildMarginEdgeBox.Width;
            desiredBorderEdgeBoxSize.Height = nfloat.Max(desiredBorderEdgeBoxSize.Height, desiredChildMarginEdgeBox.Height);
        }

        return desiredBorderEdgeBoxSize;
    }

    /// <inheritdoc/>
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        nfloat x = rect.X;

        for (int i = 0; i < Count; i++)
        {
            var child = this[i];
            var desired = child.Measure((nfloat.PositiveInfinity, rect.Size.Height), context);

            // Arrange child at (X, Y), with width/height = desired
            var childRect = new Rect(x, rect.Y, desired.Width, rect.Height);
            child.Arrange(childRect, context);

            x += desired.Width;
        }
    }
}
