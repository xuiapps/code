namespace Xui.Core.Math2D;

public struct Size
{
    public nfloat Width;
    public nfloat Height;

    public static readonly Size Empty = new Size(0, 0);

    [DebuggerStepThrough]
    public Size()
    {
        this.Width = 0;
        this.Height = 0;
    }

    [DebuggerStepThrough]
    public Size(nfloat width, nfloat height)
    {
        this.Width = width;
        this.Height = height;
    }

    [DebuggerStepThrough]
    public static implicit operator Size(ValueTuple<nfloat, nfloat> tuple) =>
        new (tuple.Item1, tuple.Item2);

    [DebuggerStepThrough]
    public static Size operator+(Size size, Frame frame) =>
        (frame.Left + size.Width + frame.Right, frame.Top + size.Height + frame.Bottom);
}