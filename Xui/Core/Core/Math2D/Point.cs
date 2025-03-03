namespace Xui.Core.Math2D;

public struct Point
{
    public nfloat X;
    public nfloat Y;

    public static readonly Point Zero = (0, 0);

    [DebuggerStepThrough]
    public Point(nfloat x, nfloat y)
    {
        this.X = x;
        this.Y = y;
    }

    [DebuggerStepThrough]
    public static Point Lerp(Point start, Point end, nfloat step)
    {
        nfloat stepMinusOne = 1f - step;
        return new (start.X * stepMinusOne + end.X * step, start.Y * stepMinusOne + end.Y * step);
    }

    [DebuggerStepThrough]
    public static Vector operator-(Point lhs, Point rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    [DebuggerStepThrough]
    public static Point operator+(Point lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);
    [DebuggerStepThrough]
    public static Point operator-(Point lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    [DebuggerStepThrough]
    public static implicit operator Point(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);

    [DebuggerStepThrough]
    public static explicit operator Point(Vector vector) => new (vector.X, vector.Y);
}