using Xui.Core.Set;

namespace Xui.Core.Math2D;

public struct Rect : INonEnumerableSet<Point>
{
    public nfloat X;
    public nfloat Y;
    public nfloat Width;
    public nfloat Height;

    [DebuggerStepThrough]
    public Rect(nfloat x, nfloat y, nfloat width, nfloat height)
    {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
    }

    [DebuggerStepThrough]
    public Rect(Point topLeft, Size size)
    {
        this.X = topLeft.X;
        this.Y = topLeft.Y;
        this.Width = size.Width;
        this.Height = size.Height;
    }

    [DebuggerStepThrough]
    public Rect(Vector topLeft, Size size)
    {
        this.X = topLeft.X;
        this.Y = topLeft.Y;
        this.Width = size.Width;
        this.Height = size.Height;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point TopLeft => new (X, Y);
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point TopRight => new (X + Width, Y);
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point BottomRight => new (X + Width, Y + Height);
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Point BottomLeft => new (X, Y + Height);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Right => this.X + this.Width;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Bottom => this.Y + this.Height;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Left => this.X;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nfloat Top => this.Y;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Size Size
    {
        get => new (this.Width, this.Height);
        set
        {
            this.Width = value.Width;
            this.Height = value.Height;
        }
    }

    [DebuggerStepThrough]
    public Rect Inset(nfloat inset) =>
        new (
            X + inset,
            Y + inset,
            Width - inset - inset,
            Height - inset - inset
        );

    [DebuggerStepThrough]
    public bool Contains(Point point) =>
        point.X >= this.X &&
        point.Y >= this.Y &&
        point.X <= this.X + this.Width &&
        point.Y <= this.Y + this.Height;

    [DebuggerStepThrough]
    public Rect Expand(nfloat h, nfloat v) =>
        new (
            X - h,
            Y - v,
            Width + h + h,
            Height + v + v
        );

    [DebuggerStepThrough]
    public Rect Expand(nfloat expand) =>
        new (
            X - expand,
            Y - expand,
            Width + expand + expand,
            Height + expand + expand
        );

    public static implicit operator Rect(ValueTuple<nfloat, nfloat, nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);

    public static Rect operator+(Rect rect, Frame frame) =>
        new Rect(rect.X - frame.Left, rect.Y - frame.Top, rect.Width + frame.Left + frame.Right, rect.Height + frame.Top + frame.Bottom);        
    
    public static Rect operator-(Rect rect, Frame frame) =>
        new Rect(rect.X + frame.Left, rect.Y + frame.Top, rect.Width - frame.Left - frame.Right, rect.Height - frame.Top - frame.Bottom);
}