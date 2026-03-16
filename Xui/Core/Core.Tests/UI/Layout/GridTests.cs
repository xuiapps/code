using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Core.UI.Tests.Layout;

public class GridTests
{
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
}
