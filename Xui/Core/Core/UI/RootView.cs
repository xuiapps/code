using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Actual;
using Xui.Core.Canvas;
using Xui.Core.Debug;
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

    void IContent.OnAnimationFrame(ref FrameEventRef e)
    {
        if ((this.Flags & (ViewFlags.Animated | ViewFlags.DescendantAnimated)) != 0)
        {
            this.Animate(e.Previous, e.Next);
        }
    }

    void IContent.Update(ref RenderEventRef @event, IContext context)
    {
        var instruments = Runtime.CurrentInstruments;
        var rect = @event.Rect;
        using var _ = instruments.Trace(Scope.Rendering, LevelOfDetail.Essential,
            $"RootView.Update Rect({rect.X:F1}, {rect.Y:F1}, {rect.Width:F1}, {rect.Height:F1})");

        this.Update(new LayoutGuide()
        {
            Anchor = @event.Rect.TopLeft,
            PreviousTime = @event.Frame.Previous,
            CurrentTime = @event.Frame.Next,
            Pass =
                LayoutGuide.LayoutPass.Measure |
                LayoutGuide.LayoutPass.Arrange |
                LayoutGuide.LayoutPass.Render,
            AvailableSize = @event.Rect.Size,
            MeasureContext = context,
            XAlign = LayoutGuide.Align.Start,
            YAlign = LayoutGuide.Align.Start,
            XSize = LayoutGuide.SizeTo.Exact,
            YSize = LayoutGuide.SizeTo.Exact,
            RenderContext = context,
            Instruments = instruments,
        });

        instruments.DumpVisualTree(this, LevelOfDetail.Diagnostic);
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
