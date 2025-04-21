using Xui.Core.Math2D;

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
    /// <summary>
    /// Measures the desired size of this layout container and its children,
    /// based on the available space provided by the parent.
    /// </summary>
    /// <param name="availableBorderEdgeSize">The space available for layout, excluding padding and borders.</param>
    /// <returns>The desired size of this container based on its layout strategy.</returns>
    protected override Size MeasureCore(Size availableBorderEdgeSize)
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
                var childSize = child.Measure((availableBorderEdgeSize.Width, uniformHeight));
                maxWidth = nfloat.Max(maxWidth, childSize.Width);
            }
        }
        else
        {
            // Unconstrained height: use the maximum child height
            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childSize = child.Measure((availableBorderEdgeSize.Width, nfloat.PositiveInfinity));
                uniformHeight = nfloat.Max(uniformHeight, childSize.Height);
                maxWidth = nfloat.Max(maxWidth, childSize.Width);
            }
        }

        return new Size(maxWidth, uniformHeight * childCount);
    }

    /// <summary>
    /// Arranges the children into vertically stacked rows of equal height.
    /// </summary>
    /// <param name="rect">The rectangle within which to arrange children.</param>
    protected override void ArrangeCore(Rect rect)
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
            child.Arrange(childRect);
            y += uniformHeight;
        }
    }
}
