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
    private View? focusedView;

    public EventRouter EventRouter { get; }

    public Window Window { get; }

    public View? FocusedView
    {
        get => this.focusedView;
        set
        {
            if (this.focusedView == value)
                return;

            this.focusedView = value;
            this.Window.Invalidate();
        }
    }

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

    void IContent.OnKeyDown(ref KeyEventRef e)
    {
        if (e.Key == VirtualKey.Tab)
        {
            this.MoveFocus(e.Shift ? -1 : 1);
            e.Handled = true;
            return;
        }

        this.focusedView?.OnKeyDown(ref e);
    }

    void IContent.OnChar(ref KeyEventRef e)
    {
        this.focusedView?.OnChar(ref e);
    }

    void IContent.OnScrollWheel(ref ScrollWheelEventRef e)
    {
    }

    void IContent.OnTouch(ref TouchEventRef e)
    {
        this.EventRouter.Dispatch(ref e);
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
                LayoutGuide.LayoutPass.Animate |
                LayoutGuide.LayoutPass.Arrange |
                LayoutGuide.LayoutPass.Measure |
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

    private void MoveFocus(int direction)
    {
        var focusable = new List<View>();
        CollectFocusable(this, focusable);

        if (focusable.Count == 0)
            return;

        var currentIndex = this.focusedView != null ? focusable.IndexOf(this.focusedView) : -1;
        var nextIndex = currentIndex + direction;

        // Wrap around
        if (nextIndex < 0)
            nextIndex = focusable.Count - 1;
        else if (nextIndex >= focusable.Count)
            nextIndex = 0;

        this.FocusedView = focusable[nextIndex];
    }

    private static void CollectFocusable(View view, List<View> result)
    {
        if (view.Focusable)
            result.Add(view);

        for (int i = 0; i < view.Count; i++)
            CollectFocusable(view[i], result);
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
