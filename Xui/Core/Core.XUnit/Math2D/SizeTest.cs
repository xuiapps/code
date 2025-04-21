namespace Xui.Core.Math2D;

public class SizeTest
{
    [Fact]
    public void Size_EmptyConstructor()
    {
        Assert.Equal((0, 0), new Size());
    }

    [Fact]
    public void Size_TupleOperator()
    {
        Size size = (3, 5);
        Assert.Equal(new Size(3, 5), size);
    }

    [Fact]
    public void Size_PlusFrame()
    {
        Size size = (3, 5);
        Frame frame = new Frame(2, 1, 2, 1);
        Assert.Equal(new Size(5, 9), size + frame);
    }
}
