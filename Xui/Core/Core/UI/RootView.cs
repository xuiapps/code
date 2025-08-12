using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI.Input;

namespace Xui.Core.UI;

public class RootView : View, IContent
{
    private View? content;

    public EventRouter EventRouter { get; }

    public Window Window { get; }

    public View? Content
    {
        get => this.content;
        set
        {
            if (this.content is not null)
            {
                this.content.Parent = null;
                this.content = null;
            }

            this.content = value;
            if (this.content is not null)
            {
                this.content.Parent = this;
            }
        }
    }

    public override int Count => this.Content is not null ? 1 : 0;

    public override View this[int index] => index == 0 && this.Content is not null ? this.Content : throw new IndexOutOfRangeException();

    public RootView(Window window)
    {
        this.Window = window;
        this.EventRouter = new EventRouter(this);
    }

    void IContent.OnMouseDown(ref MouseDownEventRef e)
    {
        this.EventRouter.Dispatch(ref e);
    }

    void IContent.OnMouseMove(ref MouseMoveEventRef e)
    {
        this.EventRouter.Dispatch(ref e);
    }

    void IContent.OnMouseUp(ref MouseUpEventRef e)
    {
        this.EventRouter.Dispatch(ref e);
    }

    void IContent.OnScrollWheel(ref ScrollWheelEventRef e)
    {
    }

    void IContent.OnTouch(ref TouchEventRef e)
    {
        this.EventRouter.Dispatch(ref e);
    }

    void IContent.Update(Rect rect, IContext context)
    {
        this.Update(new LayoutGuide()
        {
            Anchor = rect.TopLeft,
            Pass =
                LayoutGuide.LayoutPass.Arrange |
                LayoutGuide.LayoutPass.Measure |
                LayoutGuide.LayoutPass.Render,
            AvailableSize = rect.Size,
            MeasureContext = context,
            XAlign = LayoutGuide.Align.Start,
            YAlign = LayoutGuide.Align.Start,
            XSize = LayoutGuide.SizeTo.Exact,
            YSize = LayoutGuide.SizeTo.Exact,
            RenderContext = context,
        });
    }

    protected override void OnChildRenderChanged(View child)
    {
        base.OnChildRenderChanged(child);
        ((IContent)this).Invalidate();
    }

    void IContent.Invalidate()
    {
        this.Window.Invalidate();
    }
}
