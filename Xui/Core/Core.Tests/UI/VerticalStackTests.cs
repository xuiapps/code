using Xui.Core.Math2D;

namespace Xui.Core.UI.Tests;

public class VerticalStackTests
{
    public FixedView Box(
            out FixedView view,
            Size size,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment verticalAlignment = VerticalAlignment.Stretch) =>
        view = new FixedView()
        {
            Size = size,
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment
        };

    public VerticalStack VerticalStack(
            out VerticalStack view,
            ReadOnlySpan<View> content,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment verticalAlignment = VerticalAlignment.Stretch) => 
        view = new VerticalStack()
        {
            Content = content,
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment
        }; 

    [Fact]
    public void VerticalStack_Should_Account_For_Largest_Child_Width_And_Sum_Heights()
    {
        VerticalStack(
                out var stack,
                content: [
            Box(out var b1, (100, 40)),
            Box(out var b2, (150, 30)),
            Box(out var b3, (120, 50)),
        ]);
        
        var size = stack.Measure((400, 800), null);
        Assert.Equal(new Size(150, 120), size); // width = widest, height = sum
        var rect = stack.Arrange((0, 0, 400, 800), null);
        Assert.Equal(new Rect(0, 0, 400, 800), rect);
    }

    [Fact]
    public void VerticalStack_Should_Sum_Child_Heights_And_Use_Max_Width()
    {
        VerticalStack(
                out var stack,
                content: [
            Box(out var b1, (100, 40)),
            Box(out var b2, (120, 30)),
            Box(out var b3, (90, 50)),
        ]);

        stack.Update(LayoutTests.DeviceScreen);

        Assert.Equal((0, 0, 400, 800), stack.Frame);
        Assert.Equal((0, 0, 100, 40), b1.Frame);
        Assert.Equal((0, 40, 120, 30), b2.Frame);
        Assert.Equal((0, 70, 90, 50), b3.Frame);
    }

    [Fact]
    public void VerticalStack_Should_Center_Children()
    {
        VerticalStack(
                out var stack,
                horizontalAlignment: HorizontalAlignment.Center,
                verticalAlignment: VerticalAlignment.Middle,
                content: [
            Box(out var b1, (100, 40)),
            Box(out var b2, (120, 30)),
            Box(out var b3, (90, 50)),
        ]);

        stack.Update(LayoutTests.DeviceScreen);

        Assert.Equal((140, 340, 120, 120), stack.Frame);
        Assert.Equal((140, 340, 100, 40), b1.Frame);
        Assert.Equal((140, 380, 120, 30), b2.Frame);
        Assert.Equal((140, 410, 90, 50), b3.Frame);
    }
}