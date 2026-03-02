// File: Xui/Core/UI/Layers/DockRight.cs
using Xui.Core.Math2D;
using Xui.Core.UI;

namespace Xui.Core.UI.Layers;

/// <summary>
/// A two-child horizontal layout layer. The right child is measured at its natural (desired) width;
/// the left child fills the remaining horizontal space. Both children receive the full allocated height.
/// Use <see cref="Gap"/> to insert horizontal space between the two children.
/// </summary>
public struct DockRight<TLeft, TRight> : ILayer
    where TLeft : struct, ILayer
    where TRight : struct, ILayer
{
    public TLeft Left;
    public TRight Right;

    /// <summary>Horizontal gap in pixels between the left and right children.</summary>
    public nfloat Gap;

    // Stored from the last Measure pass so Arrange/Render can split the rect correctly.
    private nfloat _rightWidth;

    public LayoutGuide Update(LayoutGuide guide)
    {
        if (guide.IsMeasure)
        {
            var rightGuide = guide;
            rightGuide = Right.Update(rightGuide);
            _rightWidth = rightGuide.DesiredSize.Width;

            var leftGuide = guide;
            leftGuide.AvailableSize = new Size(
                guide.AvailableSize.Width - _rightWidth - Gap,
                guide.AvailableSize.Height);
            leftGuide = Left.Update(leftGuide);

            var height = leftGuide.DesiredSize.Height >= rightGuide.DesiredSize.Height
                ? leftGuide.DesiredSize.Height
                : rightGuide.DesiredSize.Height;

            guide.DesiredSize = new Size(leftGuide.DesiredSize.Width + Gap + _rightWidth, height);
        }

        if (guide.IsArrange)
        {
            var rect = guide.ArrangedRect;

            var leftGuide = guide;
            leftGuide.ArrangedRect = new Rect(rect.X, rect.Y, rect.Width - _rightWidth - Gap, rect.Height);
            Left.Update(leftGuide);

            var rightGuide = guide;
            rightGuide.ArrangedRect = new Rect(rect.Right - _rightWidth, rect.Y, _rightWidth, rect.Height);
            Right.Update(rightGuide);
        }

        if (guide.IsRender)
        {
            var rect = guide.ArrangedRect;

            var leftGuide = guide;
            leftGuide.ArrangedRect = new Rect(rect.X, rect.Y, rect.Width - _rightWidth - Gap, rect.Height);
            Left.Update(leftGuide);

            var rightGuide = guide;
            rightGuide.ArrangedRect = new Rect(rect.Right - _rightWidth, rect.Y, _rightWidth, rect.Height);
            Right.Update(rightGuide);
        }

        if (guide.IsAnimate && !guide.IsRender && !guide.IsArrange && !guide.IsMeasure)
        {
            Left.Update(guide);
            Right.Update(guide);
        }

        return guide;
    }
}
