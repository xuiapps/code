using Xui.Core.UI;
using Xui.Core.UI.Layout;
using Xui.Core.UI.Tests;
using Xui.Core.Math2D;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Core.UI.Tests.Layout;

public class FlexBoxTests
{
    private static readonly LayoutGuide DeviceScreen = new LayoutGuide()
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
    public void FlexBox_Toolbar_RowWithGrowingSpacer()
    {
        var toolbar = new FlexBox
        {
            FlexDirection      = FlexBox.Direction.Row,
            FlexAlignItems     = FlexBox.AlignItems.Center,
            FlexJustifyContent = FlexBox.JustifyContent.FlexStart,
            ColumnGap = 8,
            Content =
            [
                new Label { Text = "File"   },
                new Label { Text = "Edit"   },
                new Label { Text = "View"   },
                new Label { Text = "spacer", [Grow] = 1 },
                new Label { Text = "Help"   },
            ]
        };

        Assert.Equal(FlexBox.Direction.Row,            toolbar.FlexDirection);
        Assert.Equal(FlexBox.AlignItems.Center,        toolbar.FlexAlignItems);
        Assert.Equal(FlexBox.JustifyContent.FlexStart, toolbar.FlexJustifyContent);
        Assert.Equal((nfloat)8,                        toolbar.ColumnGap);
        Assert.Equal(5,                                toolbar.Count);

        var spacer = (Label)toolbar[3];
        Assert.Equal((nfloat)1, spacer[Grow]);
        Assert.Equal((nfloat)1, spacer[Shrink]);   // default per CSS spec
    }

    [Fact]
    public void FlexBox_Card_ColumnWithShrinkingBody()
    {
        var card = new FlexBox
        {
            FlexDirection  = FlexBox.Direction.Column,
            FlexWrap       = FlexBox.Wrap.NoWrap,
            FlexAlignItems = FlexBox.AlignItems.Stretch,
            RowGap = 4,
            Content =
            [
                new Label { Text = "Title"  },
                new Label { Text = "Body", [Grow] = 1, [Shrink] = 1 },
                new Label { Text = "Footer" },
            ]
        };

        Assert.Equal(FlexBox.Direction.Column,   card.FlexDirection);
        Assert.Equal(FlexBox.Wrap.NoWrap,        card.FlexWrap);
        Assert.Equal(FlexBox.AlignItems.Stretch, card.FlexAlignItems);
        Assert.Equal(3,                          card.Count);

        var body = (Label)card[1];
        Assert.Equal("Body",    body.Text);
        Assert.Equal((nfloat)1, body[Grow]);
        Assert.Equal((nfloat)1, body[Shrink]);
    }

    [Fact]
    public void FlexBox_BasicRow_WithTestBox()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            ColumnGap = 10,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 75, Height = 40 },
                new TestBox { Width = 60, Height = 35 },
            ]
        };

        // Measure
        var size = flexBox.Measure((400, 800), null);

        // Total width with gaps: 50 + 10 + 75 + 10 + 60 = 205
        // Max height: 40
        Assert.Equal((nfloat)(50 + 75 + 60 + 2 * 10), size.Width); // 205
        Assert.Equal((nfloat)40, size.Height);

        // Arrange
        flexBox.Arrange((0, 0, 400, 800), null);

        // Verify arranged positions
        Assert.Equal((nfloat)0, flexBox[0].Frame.X);
        Assert.Equal((nfloat)60, flexBox[1].Frame.X); // 50 + 10 gap
        Assert.Equal((nfloat)145, flexBox[2].Frame.X); // 50 + 10 + 75 + 10
    }

    [Fact]
    public void FlexBox_BasicColumn_WithTestBox()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Column,
            RowGap = 8,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 60, Height = 40 },
                new TestBox { Width = 55, Height = 25 },
            ]
        };

        // Measure
        var size = flexBox.Measure((400, 800), null);

        // Max width: 60
        // Total height with gaps: 30 + 8 + 40 + 8 + 25 = 111
        Assert.Equal((nfloat)60, size.Width);
        Assert.Equal((nfloat)(30 + 40 + 25 + 2 * 8), size.Height); // 111

        // Arrange
        flexBox.Arrange((0, 0, 400, 800), null);

        // Verify arranged positions
        Assert.Equal((nfloat)0, flexBox[0].Frame.Y);
        Assert.Equal((nfloat)38, flexBox[1].Frame.Y); // 30 + 8 gap
        Assert.Equal((nfloat)86, flexBox[2].Frame.Y); // 30 + 8 + 40 + 8
    }

    [Fact]
    public void FlexBox_GrowItems_InRow()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            Content =
            [
                new TestBox { Width = 50, Height = 30, [Grow] = 1 },
                new TestBox { Width = 50, Height = 30, [Grow] = 2 },
                new TestBox { Width = 50, Height = 30, [Grow] = 1 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Total grow = 4, available = 400, base = 150, remaining = 250
        // Item 0: 50 + 250 * 1/4 = 112.5
        // Item 1: 50 + 250 * 2/4 = 175
        // Item 2: 50 + 250 * 1/4 = 112.5
        // All items should be wider than their base width of 50
        Assert.True(flexBox[0].Frame.Width > 50, $"Expected >50, got {flexBox[0].Frame.Width}");
        Assert.True(flexBox[1].Frame.Width > flexBox[0].Frame.Width, $"Expected item1.Width ({flexBox[1].Frame.Width}) > item0.Width ({flexBox[0].Frame.Width})");
        Assert.Equal(flexBox[0].Frame.Width, flexBox[2].Frame.Width, precision: 1);
        
        // Check approximate expected values
        Assert.True(flexBox[0].Frame.Width > 100 && flexBox[0].Frame.Width < 125, $"Item0 width should be ~112.5, got {flexBox[0].Frame.Width}");
        Assert.True(flexBox[1].Frame.Width > 165 && flexBox[1].Frame.Width < 185, $"Item1 width should be ~175, got {flexBox[1].Frame.Width}");
    }

    [Fact]
    public void FlexBox_ShrinkItems_InRow()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            Content =
            [
                new TestBox { Width = 200, Height = 30, [Shrink] = 1 },
                new TestBox { Width = 200, Height = 30, [Shrink] = 2 },
                new TestBox { Width = 200, Height = 30, [Shrink] = 1 },
            ]
        };

        // Available width is 400, but items want 600
        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Items should shrink proportionally
        Assert.True(flexBox[0].Frame.Width < 200);
        Assert.True(flexBox[1].Frame.Width < flexBox[0].Frame.Width);
        Assert.Equal(flexBox[0].Frame.Width, flexBox[2].Frame.Width, precision: 1);
    }

    [Fact]
    public void FlexBox_JustifyContent_SpaceBetween()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            FlexJustifyContent = FlexBox.JustifyContent.SpaceBetween,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 50, Height = 30 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Total item width = 150, available = 400, space = 250
        // Space between 3 items = 250 / 2 = 125
        Assert.Equal((nfloat)0, flexBox[0].Frame.X);
        Assert.True(flexBox[1].Frame.X > 50); // Should have space before
        Assert.True(flexBox[2].Frame.X > flexBox[1].Frame.X + 50); // Should have space before
    }

    [Fact]
    public void FlexBox_JustifyContent_Center()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            FlexJustifyContent = FlexBox.JustifyContent.Center,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 50, Height = 30 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Total item width = 100, available = 400, offset = (400-100)/2 = 150
        Assert.True(flexBox[0].Frame.X > 100); // Should be centered
        Assert.Equal(flexBox[0].Frame.X + 50, flexBox[1].Frame.X);
    }

    [Fact]
    public void FlexBox_AlignItems_Center()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            FlexAlignItems = FlexBox.AlignItems.Center,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 50, Height = 60 },
                new TestBox { Width = 50, Height = 40 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Max height = 60, items should be centered vertically
        var maxHeight = flexBox[1].Frame.Height; // 60
        Assert.True(flexBox[0].Frame.Y > 0); // Should be offset from top
        Assert.True(flexBox[0].Frame.Y < (maxHeight - 30) / 2 + 5); // Approximately centered (allow tolerance)
    }

    [Fact]
    public void FlexBox_AlignItems_Stretch()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            FlexAlignItems = FlexBox.AlignItems.Stretch,
            Content =
            [
                new TestBox { Width = 50, MinHeight = 30 },
                new TestBox { Width = 50, Height = 60 },
                new TestBox { Width = 50, MinHeight = 40 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Items with no explicit height should stretch to line cross size
        // The line cross size is determined by the max of all items (60)
        // But in a single-line horizontal flexbox, the line height equals the container
        // Items 0 and 2 (with MinHeight) should stretch, item 1 has explicit height
        Assert.True(flexBox[0].Frame.Height >= 30); // At least MinHeight
        Assert.Equal((nfloat)60, flexBox[1].Frame.Height); // Has explicit height
        Assert.True(flexBox[2].Frame.Height >= 40); // At least MinHeight
    }

    [Fact]
    public void FlexBox_Wrap_MultiLine()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            FlexWrap = FlexBox.Wrap.Wrap,
            ColumnGap = 10,
            RowGap = 5,
            Content =
            [
                new TestBox { Width = 150, Height = 30 },
                new TestBox { Width = 150, Height = 30 },
                new TestBox { Width = 150, Height = 30 },
                new TestBox { Width = 150, Height = 30 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Width 400, items are 150 each with 10 gap
        // Line 1: 150 + 10 + 150 = 310 (fits)
        // Line 2: 150 + 10 + 150 = 310 (fits)
        // Items 0,1 on first line, 2,3 on second line
        Assert.Equal(flexBox[0].Frame.Y, flexBox[1].Frame.Y); // Same line
        Assert.True(flexBox[2].Frame.Y > flexBox[0].Frame.Y); // Second line
        Assert.Equal(flexBox[2].Frame.Y, flexBox[3].Frame.Y); // Same line
    }

    [Fact]
    public void FlexBox_ReverseRow()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.RowReverse,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 60, Height = 30 },
                new TestBox { Width = 70, Height = 30 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Items should appear right-to-left
        Assert.True(flexBox[0].Frame.X > flexBox[1].Frame.X);
        Assert.True(flexBox[1].Frame.X > flexBox[2].Frame.X);
    }

    [Fact]
    public void FlexBox_ReverseColumn()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.ColumnReverse,
            Content =
            [
                new TestBox { Width = 50, Height = 30 },
                new TestBox { Width = 50, Height = 40 },
                new TestBox { Width = 50, Height = 50 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Items should appear bottom-to-top
        Assert.True(flexBox[0].Frame.Y > flexBox[1].Frame.Y);
        Assert.True(flexBox[1].Frame.Y > flexBox[2].Frame.Y);
    }

    [Fact]
    public void FlexBox_FlexBasis_OverridesWidth()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            Content =
            [
                new TestBox { Width = 100, Height = 30, [Basis] = 50 },
                new TestBox { Width = 100, Height = 30, [Basis] = 75 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Flex basis should override the width measurement
        Assert.Equal((nfloat)50, flexBox[0].Frame.Width);
        Assert.Equal((nfloat)75, flexBox[1].Frame.Width);
    }

    [Fact]
    public void FlexBox_Order_ReordersItems()
    {
        var flexBox = new FlexBox
        {
            FlexDirection = FlexBox.Direction.Row,
            Content =
            [
                new TestBox { Width = 50, Height = 30, [Order] = 3 },
                new TestBox { Width = 60, Height = 30, [Order] = 1 },
                new TestBox { Width = 70, Height = 30, [Order] = 2 },
            ]
        };

        flexBox.Measure((400, 800), null);
        flexBox.Arrange((0, 0, 400, 800), null);

        // Visual order should be: Item 1 (order=1), Item 2 (order=2), Item 0 (order=3)
        // Item at index 1 should be leftmost
        Assert.True(flexBox[1].Frame.X < flexBox[2].Frame.X);
        Assert.True(flexBox[2].Frame.X < flexBox[0].Frame.X);
    }
}
