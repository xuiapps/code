using System;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Input;

namespace Xui.Middleware.Emulator.Actual;

internal sealed class EmulatorControlRootView : View
{
    private readonly EventRouter eventRouter;
    private readonly EmulatorHeaderButtonView rotateCcwButton;
    private readonly EmulatorHeaderButtonView rotateCwButton;

    public EmulatorControlRootView()
    {
        eventRouter = new EventRouter(this);
        rotateCcwButton = new EmulatorHeaderButtonView("Rotate CCW");
        rotateCwButton = new EmulatorHeaderButtonView("Rotate CW");
        AddProtectedChild(rotateCcwButton);
        AddProtectedChild(rotateCwButton);
    }

    public override int Count => 2;
    public override View this[int index] => index switch
    {
        0 => rotateCcwButton,
        1 => rotateCwButton,
        _ => throw new IndexOutOfRangeException()
    };

    public void OnMouseDown(ref MouseDownEventRef e) => eventRouter.Dispatch(ref e);
    public void OnMouseMove(ref MouseMoveEventRef e) => eventRouter.Dispatch(ref e);
    public void OnMouseUp(ref MouseUpEventRef e) => eventRouter.Dispatch(ref e);

    public void UpdateLayout(Rect titleRect)
    {
        Frame = titleRect;

        var buttonHeight = 28f;
        var buttonWidth = 92f;
        var gap = 8f;
        var rightPadding = 12f;
        var y = titleRect.Y + (titleRect.Height - buttonHeight) * 0.5f;
        var cwX = titleRect.Right - rightPadding - buttonWidth;
        var ccwX = cwX - gap - buttonWidth;

        rotateCcwButton.SetFrame(new Rect(ccwX, y, buttonWidth, buttonHeight));
        rotateCwButton.SetFrame(new Rect(cwX, y, buttonWidth, buttonHeight));
    }

    public bool IsInteractive(Point point) =>
        rotateCcwButton.Frame.Contains(point) || rotateCwButton.Frame.Contains(point);

    protected override void RenderCore(IContext context)
    {
        context.BeginPath();
        context.RoundRect(Frame, 10f);
        context.SetFill(new Color(0x333333FF));
        context.Fill();
        base.RenderCore(context);
    }
}

internal sealed class EmulatorHeaderButtonView : View
{
    private readonly string label;
    private bool hover;
    private bool pressed;

    public EmulatorHeaderButtonView(string label)
    {
        this.label = label;
    }

    public override int Count => 0;
    public override View this[int index] => throw new IndexOutOfRangeException();

    public void SetFrame(Rect frame) => Frame = frame;

    public override void OnPointerEvent(ref PointerEventRef e, EventPhase phase)
    {
        if (e.State.PointerType != PointerType.Mouse)
            return;

        if (e.Type == PointerEventType.Enter)
        {
            hover = true;
            InvalidateRender();
        }
        else if (e.Type == PointerEventType.Leave)
        {
            hover = false;
            pressed = false;
            InvalidateRender();
        }
        else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Down && e.State.Button == PointerButton.Left)
        {
            pressed = true;
            InvalidateRender();
        }
        else if (phase == EventPhase.Tunnel && e.Type == PointerEventType.Up && pressed)
        {
            pressed = false;
            InvalidateRender();
        }
    }

    protected override void RenderCore(IContext context)
    {
        var bg = pressed
            ? new Color(0x4D4D4DFF)
            : hover ? new Color(0x5A5A5AFF) : new Color(0x474747FF);

        context.BeginPath();
        context.RoundRect(Frame, 6f);
        context.SetFill(bg);
        context.Fill();

        context.BeginPath();
        context.RoundRect(Frame, 6f);
        context.SetStroke(new Color(0x8A8A8AFF));
        context.LineWidth = 1f;
        context.Stroke();

        context.SetFont(new Font
        {
            FontFamily = "Inter",
            FontSize = 12,
            FontWeight = 600,
            FontStyle = FontStyle.Normal,
            LineHeight = 16
        });
        context.TextAlign = TextAlign.Center;
        context.TextBaseline = TextBaseline.Middle;
        context.SetFill(Colors.White);
        context.FillText(label, Frame.Center);
    }
}
