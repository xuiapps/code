using Xui.Core.Math2D;
using Xui.Core.UI.Layout;

namespace Xui.Core.UI;

public class VerticalStack : ViewCollection
{
    public nfloat Gap { get; set; }

    public override Size SizeToFit(Size constraint)
    {
        Size size = (0, 0);
        for (var i = 0; i < this.Count; i++)
        {
            var child = this[i];
            var childSize = child.SizeToFit((constraint.Width, nfloat.PositiveInfinity));
            size.Width = nfloat.Max(size.Width, childSize.Width);
            size.Height += childSize.Height;
        }
        size.Height += this.Count * this.Gap;

        return size;
    }

    public override void Layout(Constraint constraint)
    {
        // TODO: Try with a single pass first...
        if (constraint.VerticalAlign == VerticalAlign.Top)
        {

        }
        else
        {
            base.Layout(constraint);

            nfloat top = this.Frame.Top;
            for (var i = 0; i < this.Count; i++)
            {
                var child = this[i];
                child.Layout(new Constraint() {
                    HorizontalGuide = this.Frame.Left,
                    HorizontalSize = SizeTo.Parent,
                    HorizontalAlign = HorizontalAlign.Left,
                    VerticalGuide = top,
                    VerticalSize = SizeTo.Content,
                    VerticalAlign = VerticalAlign.Top,
                    Size = (this.Frame.Width, nfloat.PositiveInfinity)
                });
                top = child.Frame.Bottom + this.Gap;
            }
        }
    }
}