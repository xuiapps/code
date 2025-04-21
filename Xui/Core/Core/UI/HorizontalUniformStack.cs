using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// A layout container that arranges its children in a horizontal stack,
/// assigning each child the same width.
/// </summary>
/// <remarks>
/// <para>
/// If the parent provides a constrained width, the container divides
/// the available width equally among all children.
/// </para>
/// <para>
/// If the width is unconstrained (infinite), the container measures each child
/// to determine the maximum width, and assigns that uniform width to all columns.
/// </para>
/// <para>
/// The height of the container is based on the tallest child.
/// </para>
/// </remarks>
public class HorizontalUniformStack : ViewCollection
{
    /// <summary>
    /// Measures the desired size of this layout container and its children,
    /// based on the available space provided by the parent.
    /// </summary>
    /// <param name="availableBorderEdgeSize">
    /// The space available for layout, excluding padding and borders.
    /// </param>
    /// <returns>
    /// The desired size of this container based on its layout strategy.
    /// </returns>
    protected override Size MeasureCore(Size availableBorderEdgeSize)
    {
        int childCount = this.Count;
        if (childCount == 0)
            return Size.Empty;

        bool fixedWidth = nfloat.IsFinite(availableBorderEdgeSize.Width);
        nfloat uniformWidth = 0;
        nfloat maxHeight = 0;

        if (fixedWidth)
        {
            // If width is fixed, we split it equally
            uniformWidth = availableBorderEdgeSize.Width / childCount;

            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childSize = child.Measure((uniformWidth, availableBorderEdgeSize.Height));
                maxHeight = nfloat.Max(maxHeight, childSize.Height);
            }
        }
        else
        {
            // If width is unconstrained, we find the widest child
            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childSize = child.Measure((nfloat.PositiveInfinity, availableBorderEdgeSize.Height));
                uniformWidth = nfloat.Max(uniformWidth, childSize.Width);
                maxHeight = nfloat.Max(maxHeight, childSize.Height);
            }
        }

        return new Size(uniformWidth * childCount, maxHeight);
    }

    /// <summary>
    /// Arranges the children into horizontally stacked columns of equal width.
    /// </summary>
    /// <param name="rect">
    /// The rectangle within which to arrange children.
    /// </param>
    protected override void ArrangeCore(Rect rect)
    {
        int childCount = this.Count;
        if (childCount == 0)
            return;

        nfloat uniformWidth = rect.Width / childCount;
        nfloat x = rect.X;

        for (int i = 0; i < childCount; i++)
        {
            var child = this[i];
            var childRect = new Rect(x, rect.Y, uniformWidth, rect.Height);
            child.Arrange(childRect);
            x += uniformWidth;
        }
    }
}
