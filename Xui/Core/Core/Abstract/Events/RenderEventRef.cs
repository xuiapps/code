using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

public ref struct RenderEventRef
{
    public Rect Rect;

    public FrameEventRef Frame;

    public RenderEventRef(Rect rect, FrameEventRef frame)
    {
        this.Frame = frame;
        this.Rect = rect;
    }
}