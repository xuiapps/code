namespace Xui.Core.Canvas;

public struct CornerRadius
{
    public nfloat TopLeft;
    public nfloat TopRight;
    public nfloat BottomRight;
    public nfloat BottomLeft;

    [DebuggerStepThrough]
    public CornerRadius(nfloat radius) : this()
    {
        this.TopLeft = this.TopRight = this.BottomRight = this.BottomLeft = radius;
    }

    [DebuggerStepThrough]
    public CornerRadius(nfloat topLeft, nfloat topRight, nfloat bottomRight, nfloat bottomLeft)
    {
        this.TopLeft = topLeft;
        this.TopRight = topRight;
        this.BottomRight = bottomRight;
        this.BottomLeft = bottomLeft;
    }
}