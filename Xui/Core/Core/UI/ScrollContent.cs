using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// Internal content host for <see cref="ScrollView"/>. It owns the scroll-coordinate transform
/// so normal views need only understand their own coordinate mapping.
/// </summary>
internal sealed class ScrollContent : View
{
    private readonly ScrollView owner;
    private View? content;

    public ScrollContent(ScrollView owner, View content)
    {
        this.owner = owner;
        this.SetProtectedChild(ref this.content, content);
    }

    public View? Content => this.content;

    public void ClearContent() => this.SetProtectedChild(ref this.content, null);

    public override int Count => this.content is not null ? 1 : 0;

    public override View this[int index] => index == 0 && this.content is not null
        ? this.content : throw new IndexOutOfRangeException();

    public override Point TransformPoint(Point point) => point + this.owner.ScrollOffset;

    public override Point InverseTransformPoint(Point point) => point - this.owner.ScrollOffset;

    public override Rect TransformRect(Rect rect)
    {
        var offset = this.owner.ScrollOffset;
        return new Rect(rect.X + offset.X, rect.Y + offset.Y, rect.Width, rect.Height);
    }

    public override Rect InverseTransformRect(Rect rect)
    {
        var offset = this.owner.ScrollOffset;
        return new Rect(rect.X - offset.X, rect.Y - offset.Y, rect.Width, rect.Height);
    }

    protected override void RenderCore(IContext context)
    {
        context.Save();
        var offset = this.owner.ScrollOffset;
        context.Translate(new Vector(-offset.X, -offset.Y));
        base.RenderCore(context);
        context.Restore();
    }
}
