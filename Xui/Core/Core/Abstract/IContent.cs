using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

/// <summary>
/// Defines the contract for UI content that participates in layout, rendering, and input dispatch.
/// </summary>
public partial interface IContent
{
    /// <summary>Requests a visual redraw of this content.</summary>
    void Invalidate();

    /// <summary>Invoked when a mouse button is pressed within this content's area.</summary>
    void OnMouseDown(ref MouseDownEventRef e);

    /// <summary>Invoked when the mouse pointer moves within this content's area.</summary>
    void OnMouseMove(ref MouseMoveEventRef e);

    /// <summary>Invoked when a mouse button is released.</summary>
    void OnMouseUp(ref MouseUpEventRef e);

    /// <summary>Invoked when the scroll wheel is used over this content.</summary>
    void OnScrollWheel(ref ScrollWheelEventRef e);

    /// <summary>Invoked when a touch event occurs.</summary>
    void OnTouch(ref TouchEventRef e);

    /// <summary>Invoked on each animation frame tick.</summary>
    void OnAnimationFrame(ref FrameEventRef e);

    /// <summary>Performs layout and renders this content into the provided drawing context.</summary>
    void Update(ref RenderEventRef @event, IContext context);

    /// <summary>Invoked when a keyboard key is pressed.</summary>
    void OnKeyDown(ref KeyEventRef e);

    /// <summary>Invoked when a character is typed.</summary>
    void OnChar(ref KeyEventRef e);
}
