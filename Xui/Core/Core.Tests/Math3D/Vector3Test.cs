using Xui.Core.Math3D;

namespace Xui.Core.Math3D;

public class Vector3Test
{
    [Fact]
    public void Vector3_Constants()
    {
        Assert.Equal((0f, 0f, 0f), Vector3.Zero);
        Assert.Equal((1f, 1f, 1f), Vector3.One);
        Assert.Equal((1f, 0f, 0f), Vector3.Right);
        Assert.Equal((-1f, 0f, 0f), Vector3.Left);
        Assert.Equal((0f, 1f, 0f), Vector3.Up);
        Assert.Equal((0f, -1f, 0f), Vector3.Down);
        Assert.Equal((0f, 0f, -1f), Vector3.Forward);
        Assert.Equal((0f, 0f, 1f), Vector3.Back);
    }

    [Fact]
    public void Vector3_Magnitude()
    {
        Assert.Equal(0f, Vector3.Zero.Magnitude);
        Assert.Equal(1f, Vector3.Right.Magnitude, 1e-6f);
        Assert.Equal(1f, Vector3.Up.Magnitude, 1e-6f);
        Assert.Equal(1f, Vector3.Forward.Magnitude, 1e-6f);
        Assert.Equal(MathF.Sqrt(3f), Vector3.One.Magnitude, 1e-5f);
        Assert.Equal(5f, new Vector3(3f, 4f, 0f).Magnitude, 1e-5f);
    }

    [Fact]
    public void Vector3_MagnitudeSquared()
    {
        Assert.Equal(0f, Vector3.Zero.MagnitudeSquared);
        Assert.Equal(1f, Vector3.Right.MagnitudeSquared);
        Assert.Equal(14f, new Vector3(1f, 2f, 3f).MagnitudeSquared, 1e-6f);
    }

    [Fact]
    public void Vector3_Normalized()
    {
        Assert.Equal(Vector3.Zero, Vector3.Zero.Normalized);
        Assert.Equal(Vector3.Right, new Vector3(5f, 0f, 0f).Normalized);

        var n = new Vector3(1f, 1f, 1f).Normalized;
        Assert.Equal(1f, n.Magnitude, 1e-6f);

        var n2 = new Vector3(3f, 4f, 0f).Normalized;
        Assert.Equal(0.6f, n2.X, 1e-6f);
        Assert.Equal(0.8f, n2.Y, 1e-6f);
        Assert.Equal(0f, n2.Z, 1e-6f);
    }

    [Fact]
    public void Vector3_Dot()
    {
        Assert.Equal(0f, Vector3.Dot(Vector3.Right, Vector3.Up));
        Assert.Equal(0f, Vector3.Dot(Vector3.Right, Vector3.Forward));
        Assert.Equal(1f, Vector3.Dot(Vector3.Right, Vector3.Right));
        Assert.Equal(-1f, Vector3.Dot(Vector3.Right, Vector3.Left));
        Assert.Equal(32f, Vector3.Dot((1f, 2f, 3f), (4f, 5f, 6f)));
    }

    [Fact]
    public void Vector3_Cross()
    {
        Assert.Equal(Vector3.Up, Vector3.Cross(Vector3.Forward, Vector3.Left));
        Assert.Equal(Vector3.Back, Vector3.Cross(Vector3.Left, Vector3.Down));

        // Right-hand rule: Right × Up = Back (+Z), Right × Down = Forward (-Z)
        Assert.Equal(Vector3.Forward, Vector3.Cross(Vector3.Right, Vector3.Down));
        Assert.Equal(Vector3.Back, Vector3.Cross(Vector3.Left, Vector3.Down));

        // Anti-commutative
        var a = new Vector3(1f, 2f, 3f);
        var b = new Vector3(4f, 5f, 6f);
        var cross = Vector3.Cross(a, b);
        var neg = Vector3.Cross(b, a);
        Assert.Equal(cross, -neg);
        Assert.Equal(-3f, cross.X, 1e-5f);
        Assert.Equal(6f, cross.Y, 1e-5f);
        Assert.Equal(-3f, cross.Z, 1e-5f);
    }

    [Fact]
    public void Vector3_Reflect()
    {
        var reflected = Vector3.Reflect(Vector3.Down, Vector3.Up);
        Assert.Equal(Vector3.Up.X, reflected.X, 1e-6f);
        Assert.Equal(Vector3.Up.Y, reflected.Y, 1e-6f);
        Assert.Equal(Vector3.Up.Z, reflected.Z, 1e-6f);
    }

    [Fact]
    public void Vector3_Angle()
    {
        Assert.Equal(MathF.PI / 2f, Vector3.Angle(Vector3.Right, Vector3.Up), 1e-5f);
        Assert.Equal(MathF.PI, Vector3.Angle(Vector3.Right, Vector3.Left), 1e-5f);
        Assert.Equal(0f, Vector3.Angle(Vector3.Up, Vector3.Up), 1e-5f);
    }

    [Fact]
    public void Vector3_Lerp()
    {
        Assert.Equal(Vector3.Zero, Vector3.Lerp(Vector3.Zero, Vector3.One, 0f));
        Assert.Equal(Vector3.One, Vector3.Lerp(Vector3.Zero, Vector3.One, 1f));

        var mid = Vector3.Lerp(Vector3.Zero, new Vector3(2f, 4f, 6f), 0.5f);
        Assert.Equal(1f, mid.X, 1e-6f);
        Assert.Equal(2f, mid.Y, 1e-6f);
        Assert.Equal(3f, mid.Z, 1e-6f);
    }

    [Fact]
    public void Vector3_Operators()
    {
        Assert.Equal((3f, 3f, 3f), new Vector3(1f, 2f, 3f) + new Vector3(2f, 1f, 0f));
        Assert.Equal((-1f, 1f, 3f), new Vector3(1f, 2f, 3f) - new Vector3(2f, 1f, 0f));
        Assert.Equal((2f, 4f, 6f), new Vector3(1f, 2f, 3f) * 2f);
        Assert.Equal((2f, 4f, 6f), 2f * new Vector3(1f, 2f, 3f));
        Assert.Equal((0.5f, 1f, 1.5f), new Vector3(1f, 2f, 3f) / 2f);
        Assert.Equal((-1f, -2f, -3f), -new Vector3(1f, 2f, 3f));
        Assert.Equal((4f, 10f, 18f), new Vector3(1f, 2f, 3f) * new Vector3(4f, 5f, 6f));
    }

    [Fact]
    public void Vector3_Equality()
    {
        var a = new Vector3(1f, 2f, 3f);
        var b = new Vector3(1f, 2f, 3f);
        var c = new Vector3(1f, 2f, 4f);
        Assert.True(a == b);
        Assert.False(a == c);
        Assert.True(a != c);
        Assert.False(a != b);
    }

    [Fact]
    public void Vector3_TupleConversion()
    {
        Vector3 v = (1f, 2f, 3f);
        Assert.Equal(1f, v.X);
        Assert.Equal(2f, v.Y);
        Assert.Equal(3f, v.Z);
    }

    [Fact]
    public void Vector3_Point3Conversion()
    {
        var p = new Point3(1f, 2f, 3f);
        Vector3 v = p;
        Assert.Equal(1f, v.X);
        Assert.Equal(2f, v.Y);
        Assert.Equal(3f, v.Z);
    }

    [Fact]
    public void Vector3_NumericsInterop()
    {
        var v = new Vector3(1f, 2f, 3f);
        System.Numerics.Vector3 nv = v;
        Assert.Equal(1f, nv.X);
        Assert.Equal(2f, nv.Y);
        Assert.Equal(3f, nv.Z);

        Vector3 back = nv;
        Assert.Equal(v, back);
    }

    [Fact]
    public void Vector3_MinMax()
    {
        var a = new Vector3(1f, 5f, 3f);
        var b = new Vector3(4f, 2f, 6f);
        Assert.Equal((1f, 2f, 3f), Vector3.Min(a, b));
        Assert.Equal((4f, 5f, 6f), Vector3.Max(a, b));
    }

    [Fact]
    public void Vector3_Project()
    {
        var proj = Vector3.Project(new Vector3(3f, 4f, 0f), Vector3.Right);
        Assert.Equal(3f, proj.X, 1e-5f);
        Assert.Equal(0f, proj.Y, 1e-5f);
        Assert.Equal(0f, proj.Z, 1e-5f);
    }
}
