using Xui.Core.Set;

namespace Xui.Core.Math2D;

public struct Frame
{
    public nfloat Left;
    public nfloat Top;
    public nfloat Right;
    public nfloat Bottom;

    public static readonly Frame Zero = new Frame();

    [DebuggerStepThrough]
    public Frame()
    {
        this.Left = this.Top = this.Right = this.Bottom = 0;
    }

    [DebuggerStepThrough]
    public Frame(int left, int top, int right, int bottom)
    {
        this.Left = left;
        this.Top = top;
        this.Bottom = bottom;
        this.Right = right;
    }
}