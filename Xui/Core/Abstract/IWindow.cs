using Xui.Core.Abstract.Events;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    void Closed();
    bool Closing();
    void OnAnimationFrame(ref FrameEventRef animationFrame);
    void OnMouseMove(ref MouseMoveEventRef evRef);
    void OnMouseDown(ref MouseDownEventRef evRef);
    void OnMouseUp(ref MouseUpEventRef evRef);
    void OnScrollWheel(ref ScrollWheelEventRef evRef);
    void OnTouch(ref TouchEventRef touchEventRef);
    void Render(ref RenderEventRef render);
    void WindowHitTest(ref WindowHitTestEventRef evRef);
}
