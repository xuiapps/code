using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    /// <summary>
    /// The overall area displaying application UI.
    /// May be partially hidden by cutouts, rounded corners or status elements.
    /// </summary>
    public Rect DisplayArea { get; set; }

    /// <summary>
    /// Fully visible area, place important elements within this rectangle.
    /// </summary>
    public Rect SafeArea { get; set; }

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
