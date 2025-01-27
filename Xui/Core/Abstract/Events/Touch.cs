using Xui.Core.Math2D;

namespace Xui.Core.Abstract.Events;

public struct Touch
{
    public long Index;
    public Point Position;
    public nfloat Radius;
    public TouchPhase Phase;
}