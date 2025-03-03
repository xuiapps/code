using Xui.Core.Math2D;

namespace Xui.Core.UI.Layout;

public class ConstraintTest
{
    [Fact]
    public void SizeToParent_TopLeft_100x100()
    {
        Constraint constraint = new Constraint()
        {
            HorizontalAlign = HorizontalAlign.Left,
            HorizontalGuide = 0,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Top,
            VerticalGuide = 0,
            VerticalSize = SizeTo.Parent,
            Size = (100, 100)
        };

        Assert.True(constraint.IsSizeToParent(out var rect));
        Assert.Equal((0, 0, 100, 100), rect);
    }

    [Fact]
    public void SizeToParent_BottomRight_100x100()
    {
        Constraint constraint = new Constraint()
        {
            HorizontalAlign = HorizontalAlign.Right,
            HorizontalGuide = 100,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Bottom,
            VerticalGuide = 100,
            VerticalSize = SizeTo.Parent,
            Size = (100, 100)
        };

        Assert.True(constraint.IsSizeToParent(out var rect));
        Assert.Equal((0, 0, 100, 100), rect);
    }

    [Fact]
    public void SizeToParent_Middle_100x100()
    {
        Constraint constraint = new Constraint()
        {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 50,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Middle,
            VerticalGuide = 50,
            VerticalSize = SizeTo.Parent,
            Size = (100, 100)
        };

        Assert.True(constraint.IsSizeToParent(out var rect));
        Assert.Equal((0, 0, 100, 100), rect);
    }

    [Fact]
    public void NotSizedToParent_TopAlignContent()
    {
        Constraint constraint = new Constraint()
        {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 50,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Top,
            VerticalGuide = 0,
            VerticalSize = SizeTo.Content,
            Size = (100, 100)
        };

        Assert.False(constraint.IsSizeToParent(out var rect));
    }

    [Fact]
    public void NotSizedToParent_Unbound()
    {
        Constraint constraint = new Constraint()
        {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 50,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Middle,
            VerticalGuide = 05,
            VerticalSize = SizeTo.Content,
            Size = (nfloat.PositiveInfinity, nfloat.PositiveInfinity)
        };

        Assert.False(constraint.IsSizeToParent(out var rect));
    }

    [Fact]
    public void AlignToParent_TopRight()
    {
        var expected = new Constraint() {
            HorizontalAlign = HorizontalAlign.Right,
            HorizontalGuide = 300,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Top,
            VerticalGuide = 100,
            VerticalSize = SizeTo.Parent,
            Size = (200, 200)
        };
        var actual = Constraint.AlignToParent(
            rect: (100, 100, 200, 200),
            horizontalAlign: HorizontalAlign.Right,
            verticalAlign: VerticalAlign.Top
        );
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AlignToParent_MiddleLeft()
    {
        var expected = new Constraint() {
            HorizontalAlign = HorizontalAlign.Left,
            HorizontalGuide = -100,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Middle,
            VerticalGuide = 0,
            VerticalSize = SizeTo.Parent,
            Size = (200, 200)
        };
        var actual = Constraint.AlignToParent(
            rect: (-100, -100, 200, 200),
            horizontalAlign: HorizontalAlign.Left,
            verticalAlign: VerticalAlign.Middle
        );
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AlignContent_TopToContent_MiddleToContent()
    {
        var constraint = new Constraint() {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 150,
            HorizontalSize = SizeTo.Content,
            VerticalAlign = VerticalAlign.Top,
            VerticalGuide = 50,
            VerticalSize = SizeTo.Content,
            Size = (nfloat.PositiveInfinity, nfloat.PositiveInfinity)
        };
        var actual = constraint.AlignContent((200, 100));
        Assert.Equal((50, 50, 200, 100), actual);
    }

    [Fact]
    public void AlignContent_MiddleToInfParent_MiddleToInfParent()
    {
        var constraint = new Constraint() {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 150,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Middle,
            VerticalGuide = 150,
            VerticalSize = SizeTo.Parent,
            Size = (nfloat.PositiveInfinity, nfloat.PositiveInfinity)
        };
        var actual = constraint.AlignContent((200, 100));
        Assert.Equal((50, 100, 200, 100), actual);
    }

    [Fact]
    public void AlignContent_MiddleToParent_MiddleToParent()
    {
        var constraint = new Constraint() {
            HorizontalAlign = HorizontalAlign.Middle,
            HorizontalGuide = 150,
            HorizontalSize = SizeTo.Parent,
            VerticalAlign = VerticalAlign.Middle,
            VerticalGuide = 150,
            VerticalSize = SizeTo.Parent,
            Size = (300, 300)
        };
        var actual = constraint.AlignContent((200, 100));
        Assert.Equal((0, 0, 300, 300), actual);
    }
}
