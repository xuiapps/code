namespace Xui.Core.Math2D;

public struct Vector
{
    public nfloat X;
    public nfloat Y;

    [DebuggerStepThrough]
    public Vector(nfloat x, nfloat y)
    {
        this.X = x;
        this.Y = y;
    }

    public static readonly Vector Zero = (0, 0);
    public static readonly Vector One = (1, 1);

    public static readonly Vector Left = (-1, 0);
    public static readonly Vector Up = (0, -1);
    public static readonly Vector Right = (1, 0);
    public static readonly Vector Down = (0, 1);

    public Vector Normalized
    {
        get
        {
            if (this.X == 0 && this.Y == 0)
            {
                return Zero;
            }

            var len = this.Magnitude;
            return new (this.X / len, this.Y / len);
        }
    }

    public nfloat Magnitude => nfloat.Sqrt(this.X * this.X + this.Y * this.Y);

    [DebuggerStepThrough]
    public static nfloat Dot(Vector lhs, Vector rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

    [DebuggerStepThrough]
    public static nfloat Cross(Vector lhs, Vector rhs) => lhs.X * rhs.Y - lhs.Y * rhs.X;

    [DebuggerStepThrough]
    public static nfloat Angle(Vector u, Vector v) =>
        (u.X * v.Y - u.Y * v.X >= 0 ? 1 : -1) * nfloat.Acos(Vector.Dot(u, v) / (u.Magnitude * v.Magnitude));
        // => nfloat.Asin(Dot(lhs, rhs) / (lhs.Magnitude * rhs.Magnitude));

    [DebuggerStepThrough]
    public static Vector Lerp(Vector start, Vector end, nfloat step)
    {
        nfloat stepMinusOne = 1f - step;
        return new (start.X * stepMinusOne + end.X * step, start.Y * stepMinusOne + end.Y * step);
    }

    [DebuggerStepThrough]
    public static implicit operator Vector(Point point) => new (point.X, point.Y);

    [DebuggerStepThrough]
    public static Vector operator*(Vector v, nfloat f) => new (v.X * f, v.Y * f);
    [DebuggerStepThrough]
    public static Vector operator*(nfloat f, Vector v) => new (v.X * f, v.Y * f);

    [DebuggerStepThrough]
    public static Vector operator+(Vector lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);
    [DebuggerStepThrough]
    public static Vector operator-(Vector lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    [DebuggerStepThrough]
    public static implicit operator Vector(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);
}