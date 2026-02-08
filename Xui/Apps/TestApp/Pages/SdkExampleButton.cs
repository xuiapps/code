using Xui.Core.Canvas;
using Xui.Core.UI;
using Xui.Core.UI.Input;
using static Xui.Core.Canvas.Colors;
using Xui.Apps.TestApp.Examples;

namespace Xui.Apps.TestApp.Pages;

public class SdkExampleButton<TPage> : Label
    where TPage : Example, new()
{
    bool hover = false;
    bool pressed = false;

    public SdkExampleButton()
    {
        this.FontFamily = ["Inter"];
    }

    private void NavigateToExample()
    {
        if (this.TryFindParent<SdkNavigation>(out var navigation))
        {
            navigation.PushExamplePage<TPage>();
        }
    }

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType == PointerType.Mouse)
        {
            if (e.Type == PointerEventType.Enter)
            {
                hover = true;
                this.InvalidateRender();
            }
            else if (e.Type == PointerEventType.Leave)
            {
                hover = false;
                this.InvalidateRender();
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down)
            {
                this.CapturePointer(e.PointerId);

                pressed = true;
                this.InvalidateRender();
            }
            else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up)
            {
                this.ReleasePointer(e.PointerId);

                this.NavigateToExample();

                pressed = false;
                this.InvalidateRender();
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
