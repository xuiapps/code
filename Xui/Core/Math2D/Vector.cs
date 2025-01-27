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

    public static Vector Zero => new (0, 0);
    public static Vector One => new (1, 1);

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

    public static nfloat Dot(Vector lhs, Vector rhs) => lhs.X * rhs.X + lhs.Y * rhs.Y;

    public static nfloat Angle(Vector u, Vector v) =>
        (u.X * v.Y - u.Y * v.X >= 0 ? 1 : -1) * nfloat.Acos(Vector.Dot(u, v) / (u.Magnitude * v.Magnitude));
        // => nfloat.Asin(Dot(lhs, rhs) / (lhs.Magnitude * rhs.Magnitude));

    public static Vector Lerp(Vector start, Vector end, nfloat step)
    {
        nfloat stepMinusOne = 1f - step;
        return new (start.X * stepMinusOne + end.X * step, start.Y * stepMinusOne + end.Y * step);
    }

    public static nfloat Cross(Vector lhs, Vector rhs) => lhs.X * rhs.Y - lhs.Y * rhs.X;

    public static implicit operator Vector(Point point) => new (point.X, point.Y);

    public static Vector operator*(Vector v, nfloat f) => new (v.X * f, v.Y * f);
    public static Vector operator*(nfloat f, Vector v) => new (v.X * f, v.Y * f);

    public static Vector operator+(Vector lhs, Vector rhs) => new (lhs.X + rhs.X, lhs.Y + rhs.Y);
    public static Vector operator-(Vector lhs, Vector rhs) => new (lhs.X - rhs.X, lhs.Y - rhs.Y);

    public static implicit operator Vector(ValueTuple<nfloat, nfloat> tuple) => new (tuple.Item1, tuple.Item2);
}