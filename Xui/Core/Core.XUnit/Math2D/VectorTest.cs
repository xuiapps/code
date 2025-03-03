namespace Xui.Core.Math2D;

public class VectorTest
{
    [Fact]
    public void Vector_Normalized()
    {
        Assert.Equal((1, 0), new Vector(2, 0).Normalized);
        Assert.Equal((-1, 0), new Vector(-2, 0).Normalized);
        Assert.Equal((0, 1), new Vector(0, 2).Normalized);
        Assert.Equal((0, -1), new Vector(0, -2).Normalized);

        Assert.Equal((0, 0), new Vector(0, 0).Normalized);

        var d = nfloat.Cos(nfloat.Pi * 0.25f);
        var bottomRight = new Vector(2, 2).Normalized;
        Assert.Equal(d, bottomRight.X, 0.001f);
        Assert.Equal(d, bottomRight.Y, 0.001f);

        var topLeft = new Vector(-2, -2).Normalized;
        Assert.Equal(-d, topLeft.X, 0.001f);
        Assert.Equal(-d, topLeft.Y, 0.001f);
    }

    [Fact]
    public void Vector_Constants()
    {
        Assert.Equal((0, 0), Vector.Zero);
        Assert.Equal((1, 1), Vector.One);
        Assert.Equal((-1, 0), Vector.Left);
        Assert.Equal((0, -1), Vector.Up);
        Assert.Equal((1, 0), Vector.Right);
        Assert.Equal((0, 1), Vector.Down);
    }

    [Fact]
    public void Vector_Dot()
    {
        Assert.Equal(0, Vector.Dot(Vector.Right, Vector.Up));
        Assert.Equal(0, Vector.Dot(Vector.Right, Vector.Down));
        Assert.Equal(0, Vector.Dot(Vector.Left, Vector.Down));
        Assert.Equal(0, Vector.Dot(Vector.Left, Vector.Up));

        Assert.Equal(1, Vector.Dot(Vector.Right, Vector.Right));
        Assert.Equal(1, Vector.Dot(Vector.Down, Vector.Down));

        Assert.Equal(-7, Vector.Dot((2, 3), (4, -5)));
    }

    [Fact]
    public void Vector_Cross()
    {
        Assert.Equal(1, Vector.Cross(Vector.Up, Vector.Right));
        Assert.Equal(-1, Vector.Cross(Vector.Down, Vector.Right));
        Assert.Equal(-4, Vector.Cross((-2, 0), (-2, 2)));
    }

    [Fact]
    public void Vector_Angle()
    {
        Assert.Equal(nfloat.Pi * 0.5, Vector.Angle(Vector.Up, Vector.Right), 0.001);
        Assert.Equal(-nfloat.Pi * 0.5, Vector.Angle(Vector.Up, Vector.Left), 0.001);
        Assert.Equal(-nfloat.Pi * 0.5, Vector.Angle(Vector.Left, Vector.Down), 0.001);
        Assert.Equal(nfloat.Pi * 0.5, Vector.Angle(Vector.Down, Vector.Left), 0.001);
    }

    [Fact]
    public void Vector_Lerp()
    {
        Assert.Equal(Vector.Up, Vector.Lerp(Vector.Up, Vector.Right, 0));
        Assert.Equal((0.5f, -0.5f), Vector.Lerp(Vector.Up, Vector.Right, 0.5f));
        Assert.Equal(Vector.Right, Vector.Lerp(Vector.Up, Vector.Right, 1f));
    }

    [Fact]
    public void Vector_ConvertionAndBasicMath()
    {
        Assert.Equal(new Point(3, 2), (Point)new Vector(3, 2));
        Assert.Equal<Vector>((2, 2), new Point(2, 2));
        Assert.Equal((2, 3), new Vector(1, 1.5f) * 2f);
        Assert.Equal((2, 3), 2f * new Vector(1, 1.5f));

        Assert.Equal((3f, 4.5f), new Vector(1, 1.5f) + new Vector(2, 3));
        Assert.Equal((-1f, -1.5f), new Vector(1, 1.5f) - new Vector(2, 3));
    }
}
