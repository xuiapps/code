using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IContent
{
    Window Window { get; }

    void OnMouseDown(ref MouseDownEventRef e);

    void OnMouseMove(ref MouseMoveEventRef e);

    void OnMouseUp(ref MouseUpEventRef e);

    void OnScrollWheel(ref ScrollWheelEventRef e);

    void OnTouch(ref TouchEventRef e);

    void Update(Rect rect, IContext context);
}
