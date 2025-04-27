using Xui.Core.Math2D;
using Xui.Core.Canvas;

namespace Xui.Core.UI;

/// <summary>
/// A layout container that arranges its children in a vertical stack,
/// giving each child the same height.
/// </summary>
/// <remarks>
/// <para>
/// If the parent provides a constrained height, the container divides
/// the available height equally among all children.
/// </para>
/// <para>
/// If the height is unconstrained (infinite), the container measures each child
/// to determine the maximum height, and uses that height for all rows.
/// </para>
/// </remarks>
public class VerticalUniformStack : ViewCollection
{
    /// <inheritdoc />
    protected override Size MeasureCore(Size availableBorderEdgeSize, IMeasureContext context)
    {
        int childCount = this.Count;
        if (childCount == 0)
            return Size.Empty;

        bool fixedHeight = nfloat.IsFinite(availableBorderEdgeSize.Height);
        nfloat uniformHeight = 0;
        nfloat maxWidth = 0;

        if (fixedHeight)
        {
            // Split the available height evenly among children
            uniformHeight = availableBorderEdgeSize.Height / childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childSize = child.Measure((availableBorderEdgeSize.Width, uniformHeight), context);
                maxWidth = nfloat.Max(maxWidth, childSize.Width);
            }
        }
        else
        {
            // Unconstrained height: use the maximum child height
            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childSize = child.Measure((availableBorderEdgeSize.Width, nfloat.PositiveInfinity), context);
                uniformHeight = nfloat.Max(uniformHeight, childSize.Height);
                maxWidth = nfloat.Max(maxWidth, childSize.Width);
            }
        }

        return new Size(maxWidth, uniformHeight * childCount);
    }

    /// <inheritdoc />
    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        int childCount = this.Count;
        if (childCount == 0)
            return;

        nfloat uniformHeight = rect.Height / childCount;
        nfloat y = rect.Y;

        for (int i = 0; i < childCount; i++)
        {
            var child = this[i];
            var childRect = new Rect(rect.X, y, rect.Width, uniformHeight);
            child.Arrange(childRect, context);
            y += uniformHeight;
        }
    }
}
