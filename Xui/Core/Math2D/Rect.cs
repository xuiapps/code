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
    public Rect(Point topLeft, Vector size)
    {
        this.X = topLeft.X;
        this.Y = topLeft.Y;
        this.Width = size.X;
        this.Height = size.Y;
    }

    [DebuggerStepThrough]
    public Rect(Vector topLeft, Vector size)
    {
        this.X = topLeft.X;
        this.Y = topLeft.Y;
        this.Width = size.X;
        this.Height = size.Y;
    }

    public Point TopLeft => new (X, Y);
    public Point TopRight => new (X + Width, Y);
    public Point BottomRight => new (X + Width, Y + Height);
    public Point BottomLeft => new (X, Y + Height);

    public nfloat Right => this.X + this.Width;
    public nfloat Bottom => this.Y + this.Height;
    public nfloat Left => this.X;
    public nfloat Top => this.Y;

    public Vector Size
    {
        get => new (this.Width, this.Height);
        set
        {
            this.Width = value.X;
            this.Height = value.Y;
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
}