namespace Xui.Core.Math2D;

public class PointTest
{
    [Fact]
    public void Point_Consts()
    {
        Assert.Equal((0, 0), Point.Zero);
    }

    [Fact]
    public void Point_Lerp()
    {
        Assert.Equal((1, 0), Point.Lerp((1, 0), (0, 1), 0f));
        Assert.Equal((0.5f, 0.5f), Point.Lerp((1, 0), (0, 1), 0.5f));
        Assert.Equal((0, 1f), Point.Lerp((1, 0), (0, 1), 1f));
    }

    [Fact]
    public void Distance()
    {
        Vector distance = new Point(3, 2) - new Point(1, 1);
        Assert.Equal(new Vector(2, 1), distance);
    }

    [Fact]
    public void Point_PlusVector()
    {
        Assert.Equal((2, 2), new Point(3, -1) + new Vector(-1, 3));
        Assert.Equal((2, 2), new Point(3, -1) - new Vector(1, -3));
    }
}
