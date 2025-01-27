using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

public ref struct WindowHitTestEventRef
{
    public Point Point;
    public Rect Window;

    public WindowArea Area;

    public WindowHitTestEventRef(Point point, Rect window)
    {
        this.Point = point;
        this.Window = window;
        this.Area = WindowArea.Default;
    }

    public enum WindowArea : uint
    {
        Default = 0,

        Transparent,
        Client,
        Title,

        BorderTopLeft,
        BorderTop,
        BorderTopRight,
        BorderRight,
        BorderBottomRight,
        BorderBottom,
        BorderBottomLeft,
        BorderLeft
    }
}