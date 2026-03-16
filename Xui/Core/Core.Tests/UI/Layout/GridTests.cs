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
            RowGap    = 0,
            ColumnGap = 0,
            Content =
            [
                new Label { Text = "App Title",   [Area] = "header"  },
                new Label { Text = "Navigation",  [Area] = "sidebar" },
                new Label { Text = "Main",        [Area] = "content" },
                new Label { Text = "© 2026",      [Area] = "footer"  },
            ]
        };
    }
}
