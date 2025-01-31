namespace Xui.Core.Math2D;

public struct Point
{
    public nfloat X;
    public nfloat Y;

    public static Point Zero => new (0, 0);

    [DebuggerStepThrough]
    public Point(nfloat x, nfloat y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Vector operator-(Point lhs, Point rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    public static Point operator+(Point lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Point operator-(Point lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    public static implicit operator Point(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);
}