using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IContent
{
    // Actual
    void Invalidate();

    // Abstract
    void OnMouseDown(ref MouseDownEventRef e);

    void OnMouseMove(ref MouseMoveEventRef e);

    void OnMouseUp(ref MouseUpEventRef e);

    void OnScrollWheel(ref ScrollWheelEventRef e);

    void OnTouch(ref TouchEventRef e);

    void OnAnimationFrame(ref FrameEventRef e);

    void Update(ref RenderEventRef @event, IContext context);

    void OnKeyDown(ref KeyEventRef e);

    void OnChar(ref KeyEventRef e);
}
