using Xui.Core.Canvas;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using static Xui.Core.Canvas.Colors;
using Xui.Core.Abstract;

namespace Xui.Apps.TestApp.Pages;

public class SdkExampleBackButton : Label
{
    bool hover = false;
    bool pressed = false;

    public SdkExampleBackButton()
    {
        this.Text = "< Back";
    }

    private void NavigateToExample()
    {
        if (this.TryFindParent<SdkNavigation>(out var navigation))
        {
            navigation.PopExample();
        }
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType == PointerType.Mouse)
        {
            if (e.Type == PointerEventType.Enter)
            {
                Window.OpenWindows[0].Invalidate();
                hover = true;
            }
            else if (e.Type == PointerEventType.Leave)
            {
                Window.OpenWindows[0].Invalidate();
                hover = false;
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
            {
                Window window = Window.OpenWindows[0];
                window.Invalidate();
                window.EventRouter?.CapturePointer(this, e.PointerId);

                pressed = true;
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
            {
                Window window = Window.OpenWindows[0];
                window.Invalidate();
                window.EventRouter?.ReleasePointer(this, e.PointerId);

                pressed = false;

                this.NavigateToExample();
            }
        }

        base.OnPointerEvent(ref e, phase);
    }

    protected override void RenderCore(IContext context)
    {
        if (this.pressed)
        {
            context.SetFill(Yellow);
            context.FillRect(this.Frame);
        }
        else if (this.hover)
        {
            context.SetFill(LightGray);
            context.FillRect(this.Frame);
        }

        base.RenderCore(context);
    }
}
