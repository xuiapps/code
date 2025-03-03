using Xui.Core.Math2D;
using Xui.Core.Canvas;
using Xui.Core.UI.Layout;

namespace Xui.Core.UI;

public class View
{
    public Rect Frame { get; private set; }

    public Frame Margin { get; set; }

    public virtual Size SizeToFit(Size constrain) => Size.Empty + this.Margin;

    public virtual void Layout(Constraint constraint)
    {
        if (constraint.IsSizeToParent(out var rect))
        {
            this.Frame = rect;
        }

        Size content = this.SizeToFit(constraint.Size);
        this.Frame = constraint.AlignContent(content);
    }

    public virtual void Render(IContext canvas)
    {
    }
}
