using Xui.Core.Math3D;

namespace Xui.Core.Math3D;

public class Point3Test
{
    [Fact]
    public void Point3_Constants()
    {
        Assert.Equal((0f, 0f, 0f), Point3.Zero);
        Assert.Equal((1f, 1f, 1f), Point3.One);
    }

    [Fact]
    public void Point3_Distance()
    {
        Assert.Equal(0f, Point3.Distance(Point3.Zero, Point3.Zero));
        Assert.Equal(1f, Point3.Distance(Point3.Zero, new Point3(1f, 0f, 0f)), 1e-6f);
        Assert.Equal(MathF.Sqrt(3f), Point3.Distance(Point3.Zero, Point3.One), 1e-5f);
        Assert.Equal(5f, Point3.Distance(new Point3(0f, 0f, 0f), new Point3(3f, 4f, 0f)), 1e-5f);
    }

    [Fact]
    public void Point3_SquaredDistance()
    {
        Assert.Equal(0f, Point3.SquaredDistance(Point3.Zero, Point3.Zero));
        Assert.Equal(1f, Point3.SquaredDistance(Point3.Zero, new Point3(1f, 0f, 0f)));
        Assert.Equal(25f, Point3.SquaredDistance(new Point3(0f, 0f, 0f), new Point3(3f, 4f, 0f)));
    }

    [Fact]
    public void Point3_TaxicabDistance()
    {
        Assert.Equal(6f, Point3.TaxicabDistance(Point3.Zero, new Point3(1f, 2f, 3f)), 1e-6f);
        Assert.Equal(6f, Point3.TaxicabDistance(new Point3(-1f, -2f, -3f), Point3.Zero), 1e-6f);
    }

    [Fact]
    public void Point3_Lerp()
    {
        Assert.Equal(Point3.Zero, Point3.Lerp(Point3.Zero, Point3.One, 0f));
        Assert.Equal(Point3.One, Point3.Lerp(Point3.Zero, Point3.One, 1f));

        var mid = Point3.Lerp(new Point3(0f, 0f, 0f), new Point3(2f, 4f, 6f), 0.5f);
        Assert.Equal(1f, mid.X, 1e-6f);
        Assert.Equal(2f, mid.Y, 1e-6f);
        Assert.Equal(3f, mid.Z, 1e-6f);
    }

    [Fact]
    public void Point3_Operators()
    {
        var p = new Point3(1f, 2f, 3f);
        var v = new Vector3(1f, 1f, 1f);

        Assert.Equal(new Point3(2f, 3f, 4f), p + v);
        Assert.Equal(new Point3(0f, 1f, 2f), p - v);
        Assert.Equal(new Vector3(1f, 1f, 1f), new Point3(3f, 4f, 5f) - new Point3(2f, 3f, 4f));
        Assert.Equal(new Point3(2f, 4f, 6f), p * 2f);
    }

    [Fact]
    public void Point3_Equality()
    {
        Assert.True(new Point3(1f, 2f, 3f) == new Point3(1f, 2f, 3f));
        Assert.False(new Point3(1f, 2f, 3f) == new Point3(1f, 2f, 4f));
        Assert.True(new Point3(1f, 2f, 3f) != new Point3(1f, 2f, 4f));
    }

    [Fact]
    public void Point3_TupleConversion()
    {
        Point3 p = (1f, 2f, 3f);
        Assert.Equal(1f, p.X);
        Assert.Equal(2f, p.Y);
        Assert.Equal(3f, p.Z);
    }

    [Fact]
    public void Point3_VectorConversion()
    {
        var v = new Vector3(4f, 5f, 6f);
        var p = (Point3)v;
        Assert.Equal(4f, p.X);
        Assert.Equal(5f, p.Y);
        Assert.Equal(6f, p.Z);
    }
}
