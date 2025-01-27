namespace Xui.Core.Math2D;

/// <summary>
/// <code>
/// | A C Tx |
/// | B D Ty |
/// | 0 0 1  |
/// </code>
/// Transforming a vector:
/// <code>
/// | x' |   | A C Tx |   | x |
/// | y' | = | B D Ty | * | y |
/// | 1  |   | 0 0 1  |   | 1 |
/// </code>
/// </summary>
public struct AffineTransform
{
    public nfloat A, B, C, D, Tx, Ty;

    public static readonly AffineTransform Identity = new AffineTransform(1, 0, 0, 1, 0, 0);

    public static AffineTransform Rotate(nfloat angle)
    {
        var cos = nfloat.Cos(angle);
        var sin = nfloat.Sin(angle);
        return new AffineTransform(cos, sin, -sin, cos, 0, 0);
    }

    public static AffineTransform Translate(Vector v) =>
        new AffineTransform(1, 0, 0, 1, v.X, v.Y);
    
    public static AffineTransform Scale(Vector v) =>
        new AffineTransform(v.X, 0, 0, v.Y, 0, 0);

    public AffineTransform Inverse
    {
        get
        {
            var determinant = (A * D) - (C * B);
            return new(
                +D / determinant,
                -B / determinant,
                -C / determinant,
                +A / determinant,
                ((C * Ty) - (D * Tx)) / determinant,
                ((B * Tx) - (A * Ty)) / determinant
            );
        }
    }    

    [DebuggerStepThrough]
    public AffineTransform(nfloat a, nfloat b, nfloat c, nfloat d, nfloat tx, nfloat ty)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;
        this.Tx = tx;
        this.Ty = ty;
    }

    public static Vector operator *(AffineTransform t, Vector v) =>
        new (t.A * v.X + t.C * v.Y + t.Tx, t.B * v.X + t.D * v.Y + t.Ty);

    public static AffineTransform operator *(AffineTransform lhs, AffineTransform rhs) =>
        new (
            lhs.A * rhs.A + lhs.C * rhs.B,
            lhs.B * rhs.A + lhs.D * rhs.B,
            lhs.A * rhs.C + lhs.C * rhs.D,
            lhs.B * rhs.C + lhs.D * rhs.D,
            lhs.A * rhs.Tx + lhs.C * rhs.Ty + lhs.Tx,
            lhs.B * rhs.Tx + lhs.D * rhs.Ty + lhs.Ty
        );
}