using Xui.Core.UI;
using Xui.Core.UI.Layout;
using Xui.Core.UI.Tests;
using Xui.Core.Math2D;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Core.UI.Tests.Layout;

public class GridTests
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
    public void Grid_TwoColumn_FormLayout()
    {
        var grid = new Grid
        {
            TemplateColumns = [120, Fr(1)],
            TemplateRows   = [Auto, Auto, Auto],
            RowGap    = 8,
            ColumnGap = 12,
            Content =
            [
                new Label { Text = "Name",  [RowStart] = 1, [ColumnStart] = 1 },
                new Label { Text = "",      [RowStart] = 1, [ColumnStart] = 2 },
                new Label { Text = "Email", [RowStart] = 2, [ColumnStart] = 1 },
                new Label { Text = "",      [RowStart] = 2, [ColumnStart] = 2 },
                new Label { Text = "Send",  [RowStart] = 3, [ColumnStart] = 2 },
            ]
        };

        Assert.Equal(2, grid.TemplateColumns.Length);
        Assert.Equal(3, grid.TemplateRows.Length);
        Assert.Equal((nfloat)8,  grid.RowGap);
        Assert.Equal((nfloat)12, grid.ColumnGap);
        Assert.Equal(5, grid.Count);

        var name  = (Label)grid[0];
        var email = (Label)grid[2];
        var send  = (Label)grid[4];

        Assert.Equal((nint)1, name[RowStart]);
        Assert.Equal((nint)1, name[ColumnStart]);
        Assert.Equal((nint)2, email[RowStart]);
        Assert.Equal((nint)2, send[ColumnStart]);
    }

    [Fact]
    public void Grid_NamedAreas_PageLayout()
    {
        var grid = new Grid
        {
            TemplateColumns = [200, Fr(1)],
            TemplateAreas   =
            [
                "header  header",
                "sidebar content",
                "footer  footer",
            ],
            Content =
            [
                new Label { Text = "App Title",   [Area] = "header"  },
                new Label { Text = "Navigation",  [Area] = "sidebar" },
                new Label { Text = "Main",        [Area] = "content" },
                new Label { Text = "© 2026",      [Area] = "footer"  },
            ]
        };

        Assert.Equal(3, grid.TemplateAreas.Length);
        Assert.Equal(4, grid.Count);
        Assert.Equal("header",  ((Label)grid[0])[Area]);
        Assert.Equal("sidebar", ((Label)grid[1])[Area]);
        Assert.Equal("content", ((Label)grid[2])[Area]);
        Assert.Equal("footer",  ((Label)grid[3])[Area]);
    }

    [Fact]
    public void Grid_SimpleLayout_WithTestBox()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 200],
            TemplateRows = [50, 75],
            Content =
            [
                new TestBox { Width = 80, Height = 40, [RowStart] = 1, [ColumnStart] = 1 },
                new TestBox { Width = 150, Height = 60, [RowStart] = 1, [ColumnStart] = 2 },
                new TestBox { Width = 90, Height = 70, [RowStart] = 2, [ColumnStart] = 1 },
                new TestBox { Width = 180, Height = 65, [RowStart] = 2, [ColumnStart] = 2 },
            ]
        };

        // Measure the grid
        var size = grid.Measure((400, 800), null);

        // Grid should have 2 columns (100 + 200 = 300) and 2 rows (50 + 75 = 125)
        Assert.Equal(new Size(300, 125), size);

        // Arrange the grid
        grid.Arrange((0, 0, 400, 800), null);

        // Check child arrangements
        var box1 = grid[0];
        var box2 = grid[1];
        var box3 = grid[2];
        var box4 = grid[3];

        // Box 1 should be in first cell (0, 0, 100, 50)
        Assert.Equal(0, box1.Frame.X, precision: 1);
        Assert.Equal(0, box1.Frame.Y, precision: 1);
        
        // Box 2 should be in second column (100, 0, 200, 50)
        Assert.Equal(100, box2.Frame.X, precision: 1);
        Assert.Equal(0, box2.Frame.Y, precision: 1);
        
        // Box 3 should be in second row, first column (0, 50, 100, 75)
        Assert.Equal(0, box3.Frame.X, precision: 1);
        Assert.Equal(50, box3.Frame.Y, precision: 1);
        
        // Box 4 should be in second row, second column (100, 50, 200, 75)
        Assert.Equal(100, box4.Frame.X, precision: 1);
        Assert.Equal(50, box4.Frame.Y, precision: 1);
    }

    [Fact]
    public void Grid_WithTextLabel_SimpleMeasurement()
    {
        var grid = new Grid
        {
            TemplateColumns = [Auto, Auto],
            TemplateRows = [Auto, Auto],
            Content =
            [
                new TextLabel { Text = "Hello", FontSize = 20, [RowStart] = 1, [ColumnStart] = 1 },
                new TextLabel { Text = "World!", FontSize = 16, [RowStart] = 1, [ColumnStart] = 2 },
                new TextLabel { Text = "Grid", FontSize = 18, [RowStart] = 2, [ColumnStart] = 1 },
                new TextLabel { Text = "Layout", FontSize = 14, [RowStart] = 2, [ColumnStart] = 2 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // "Hello" = 5 chars * 0.75 * 20 = 75, height = 20
        // "World!" = 6 chars * 0.75 * 16 = 72, height = 16
        // "Grid" = 4 chars * 0.75 * 18 = 54, height = 18
        // "Layout" = 6 chars * 0.75 * 14 = 63, height = 14

        // First column should be max(75, 54) = 75
        // Second column should be max(72, 63) = 72
        // First row should be max(20, 16) = 20
        // Second row should be max(18, 14) = 18

        Assert.Equal(new Size(75 + 72, 20 + 18), size);
    }

    [Fact]
    public void Grid_WithGaps_AddsSpacing()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 100],
            TemplateRows = [50, 50],
            ColumnGap = 10,
            RowGap = 20,
            Content =
            [
                new TestBox { Width = 100, Height = 50, [RowStart] = 1, [ColumnStart] = 1 },
                new TestBox { Width = 100, Height = 50, [RowStart] = 1, [ColumnStart] = 2 },
                new TestBox { Width = 100, Height = 50, [RowStart] = 2, [ColumnStart] = 1 },
                new TestBox { Width = 100, Height = 50, [RowStart] = 2, [ColumnStart] = 2 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // Grid should be 100 + 10 + 100 = 210 wide
        // Grid should be 50 + 20 + 50 = 120 tall
        Assert.Equal(new Size(210, 120), size);

        grid.Arrange((0, 0, 400, 800), null);

        var box1 = grid[0];
        var box2 = grid[1];
        var box3 = grid[2];
        var box4 = grid[3];

        // Box 1 at (0, 0)
        Assert.Equal(0, box1.Frame.X, precision: 1);
        Assert.Equal(0, box1.Frame.Y, precision: 1);
        
        // Box 2 at (110, 0) - 100 + 10 gap
        Assert.Equal(110, box2.Frame.X, precision: 1);
        Assert.Equal(0, box2.Frame.Y, precision: 1);
        
        // Box 3 at (0, 70) - 50 + 20 gap
        Assert.Equal(0, box3.Frame.X, precision: 1);
        Assert.Equal(70, box3.Frame.Y, precision: 1);
        
        // Box 4 at (110, 70)
        Assert.Equal(110, box4.Frame.X, precision: 1);
        Assert.Equal(70, box4.Frame.Y, precision: 1);
    }

    [Fact]
    public void Grid_FractionalUnits_DistributeSpace()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, Fr(1), Fr(2)],
            TemplateRows = [50],
            Content =
            [
                new TestBox { [RowStart] = 1, [ColumnStart] = 1 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 2 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 3 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // Available width = 400
        // Fixed column = 100
        // Remaining = 300
        // Total fr = 1 + 2 = 3
        // 1fr = 300 / 3 = 100
        // Column 2 = 1 * 100 = 100
        // Column 3 = 2 * 100 = 200

        Assert.Equal(new Size(400, 50), size);

        grid.Arrange((0, 0, 400, 800), null);

        var box1 = grid[0];
        var box2 = grid[1];
        var box3 = grid[2];

        // Box 1 width should be 100
        Assert.Equal(100, box1.Frame.Width, precision: 1);
        
        // Box 2 width should be 100
        Assert.Equal(100, box2.Frame.Width, precision: 1);
        
        // Box 3 width should be 200
        Assert.Equal(200, box3.Frame.Width, precision: 1);
    }

    [Fact]
    public void Grid_AutoPlacement_NoExplicitPositioning()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 100],
            TemplateRows = [50, 50],
            Content =
            [
                new TestBox { Width = 80, Height = 40 },
                new TestBox { Width = 80, Height = 40 },
                new TestBox { Width = 80, Height = 40 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // Grid should auto-place items in row-major order
        Assert.Equal(new Size(200, 100), size);

        grid.Arrange((0, 0, 400, 800), null);

        var box1 = grid[0];
        var box2 = grid[1];
        var box3 = grid[2];

        // Box 1 should be at (0, 0) - first cell
        Assert.Equal(0, box1.Frame.X, precision: 1);
        Assert.Equal(0, box1.Frame.Y, precision: 1);
        
        // Box 2 should be at (100, 0) - second column
        Assert.Equal(100, box2.Frame.X, precision: 1);
        Assert.Equal(0, box2.Frame.Y, precision: 1);
        
        // Box 3 should be at (0, 50) - second row, first column
        Assert.Equal(0, box3.Frame.X, precision: 1);
        Assert.Equal(50, box3.Frame.Y, precision: 1);
    }

    [Fact]
    public void Grid_EmptyGrid_ReturnsZeroSize()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 200],
            TemplateRows = [50, 75],
        };

        var size = grid.Measure((400, 800), null);

        // Empty grid should have zero size
        Assert.Equal(new Size(0, 0), size);
    }

    [Fact]
    public void Grid_MinMaxTrackSize_ClampsToRange()
    {
        var grid = new Grid
        {
            TemplateColumns = [TrackSize.MinMax(50, 150)],
            TemplateRows = [50],
            Content =
            [
                new TestBox { Width = 200, Height = 40, [RowStart] = 1, [ColumnStart] = 1 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // MinMax(50, 150) with content width 200 should be clamped to 150
        Assert.Equal(new Size(150, 50), size);
    }

    [Fact]
    public void Grid_FitContentTrackSize_ClampsToMax()
    {
        var grid = new Grid
        {
            TemplateColumns = [TrackSize.FitContent(100)],
            TemplateRows = [50],
            Content =
            [
                new TestBox { Width = 200, Height = 40, [RowStart] = 1, [ColumnStart] = 1 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // FitContent(100) with content width 200 should be clamped to 100
        Assert.Equal(new Size(100, 50), size);
    }

    [Fact]
    public void Grid_RowSpan_SpansMultipleRows()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 100],
            TemplateRows = [50, 50, 50],
            Content =
            [
                new TestBox { [RowStart] = 1, [RowSpan] = 2, [ColumnStart] = 1 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 2 },
            ]
        };

        var size = grid.Measure((400, 800), null);
        Assert.Equal(new Size(200, 150), size);

        grid.Arrange((0, 0, 400, 800), null);

        var box1 = grid[0];
        var box2 = grid[1];

        // Box 1 should span 2 rows (height = 100)
        Assert.Equal(100, box1.Frame.Height, precision: 1);
        
        // Box 2 should only occupy first row (height = 50)
        Assert.Equal(50, box2.Frame.Height, precision: 1);
    }

    [Fact]
    public void Grid_ColumnSpan_SpansMultipleColumns()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 100, 100],
            TemplateRows = [50],
            Content =
            [
                new TestBox { [RowStart] = 1, [ColumnStart] = 1, [ColumnSpan] = 2 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 3 },
            ]
        };

        var size = grid.Measure((400, 800), null);
        Assert.Equal(new Size(300, 50), size);

        grid.Arrange((0, 0, 400, 800), null);

        var box1 = grid[0];
        var box2 = grid[1];

        // Box 1 should span 2 columns (width = 200)
        Assert.Equal(200, box1.Frame.Width, precision: 1);
        
        // Box 2 should only occupy third column (width = 100)
        Assert.Equal(100, box2.Frame.Width, precision: 1);
    }

    [Fact]
    public void Grid_AutoRows_CreatesImplicitRows()
    {
        var grid = new Grid
        {
            TemplateColumns = [100, 100],
            TemplateRows = [],
            AutoRows = 60,
            Content =
            [
                new TestBox { [RowStart] = 1, [ColumnStart] = 1 },
                new TestBox { [RowStart] = 2, [ColumnStart] = 1 },
                new TestBox { [RowStart] = 3, [ColumnStart] = 1 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // 3 implicit rows at 60 each = 180 height
        Assert.Equal(new Size(200, 180), size);
    }

    [Fact]
    public void Grid_AutoColumns_CreatesImplicitColumns()
    {
        var grid = new Grid
        {
            TemplateColumns = [],
            TemplateRows = [50],
            AutoColumns = 80,
            Content =
            [
                new TestBox { [RowStart] = 1, [ColumnStart] = 1 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 2 },
                new TestBox { [RowStart] = 1, [ColumnStart] = 3 },
            ]
        };

        var size = grid.Measure((400, 800), null);

        // 3 implicit columns at 80 each = 240 width
        Assert.Equal(new Size(240, 50), size);
    }
}
