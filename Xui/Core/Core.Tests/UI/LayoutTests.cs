using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

public class LayoutTests
{
    public static readonly LayoutGuide DeviceScreen = new LayoutGuide()
    {
        Pass = LayoutGuide.LayoutPass.Measure | LayoutGuide.LayoutPass.Arrange,
        Anchor = (0, 0),
        AvailableSize = (400, 800),
        XAlign = LayoutGuide.Align.Start,
        YAlign = LayoutGuide.Align.Start,
        XSize = LayoutGuide.SizeTo.Exact,
        YSize = LayoutGuide.SizeTo.Exact
    };

    [Fact]
    public void FixedView_Should_Be_Stretched_By_Exact_DeviceScreen()
    {
        var view = new FixedView { Size = (100, 50) };
        var result = view.Update(DeviceScreen);

        // DeviceScreen forces an exact 400x800 layout, so FixedView is overridden
        Assert.Equal(new Size(400, 800), result.DesiredSize);
        Assert.Equal(new Rect(0, 0, 400, 800), result.ArrangedRect);
    }

    [Fact]
    public void FixedView_Should_Account_For_Margin_When_Arranged()
    {
        var view = new FixedView { Size = (100, 50), Margin = (10, 20) };
        var result = view.Update(DeviceScreen);
        Assert.Equal(new Size(400, 800), result.DesiredSize);
        Assert.Equal(new Rect(10, 20, 380, 760), result.ArrangedRect);
    }

    [Fact]
    public void FixedView_With_HorizontalAlignment_Center_Should_Not_Stretch()
    {
        var view = new FixedView { Size = (100, 50), HorizontalAlignment = HorizontalAlignment.Center };
        var result = view.Update(DeviceScreen);
        Assert.Equal(new Size(100, 800), result.DesiredSize);
        Assert.Equal(new Rect(150, 0, 100, 800), result.ArrangedRect);
    }

    [Fact]
    public void FixedView_With_VerticalAlignment_Bottom_Should_Not_Stretch()
    {
        var view = new FixedView { Size = (100, 50), VerticalAlignment = VerticalAlignment.Bottom };
        var result = view.Update(DeviceScreen);

        // Height should be taken from the view (50), width still stretches to 400
        Assert.Equal(new Size(400, 50), result.DesiredSize);
        Assert.Equal(new Rect(0, 750, 400, 50), result.ArrangedRect); // Bottom-aligned
    }

    [Fact]
    public void FixedView_Should_Align_TopRight()
    {
        var view = new FixedView
        {
            Size = (100, 50),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };

        var result = view.Update(DeviceScreen);

        Assert.Equal(new Size(100, 50), result.DesiredSize);
        Assert.Equal(new Rect(300, 0, 100, 50), result.ArrangedRect); // X = 400 - 100
    }

    [Fact]
    public void FixedView_Should_Align_BottomLeft()
    {
        var view = new FixedView
        {
            Size = (100, 50),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        var result = view.Update(DeviceScreen);

        Assert.Equal(new Size(100, 50), result.DesiredSize);
        Assert.Equal(new Rect(0, 750, 100, 50), result.ArrangedRect); // Y = 800 - 50
    }

    [Fact]
    public void FixedView_Should_Align_BottomRight()
    {
        var view = new FixedView
        {
            Size = (100, 50),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        var result = view.Update(DeviceScreen);

        Assert.Equal(new Size(100, 50), result.DesiredSize);
        Assert.Equal(new Rect(300, 750, 100, 50), result.ArrangedRect);
    }

    [Fact]
    public void FixedView_Should_Align_CenterCenter()
    {
        var view = new FixedView
        {
            Size = (100, 50),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Middle
        };

        var result = view.Update(DeviceScreen);

        Assert.Equal(new Size(100, 50), result.DesiredSize);
        Assert.Equal(new Rect(150, 375, 100, 50), result.ArrangedRect); // center of 400x800
    }

    [Fact]
    public void FixedView_Should_Align_BottomRight_With_Margin()
    {
        var view = new FixedView
        {
            Size = (100, 50),
            Margin = (10, 20),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom
        };
        var result = view.Update(DeviceScreen);
        Assert.Equal(new Size(120, 90), result.DesiredSize);
        Assert.Equal(new Rect(290, 730, 100, 50), result.ArrangedRect);
    }
}