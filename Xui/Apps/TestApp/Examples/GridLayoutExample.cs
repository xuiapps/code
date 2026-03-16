using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Apps.TestApp.Examples;

/// <summary>
/// Demonstrates CSS Grid layout with various features including:
/// - Template columns and rows with different track sizes (px, fr, auto)
/// - Named grid areas for page layout
/// - Gaps between grid items
/// - Explicit positioning with grid-row-start/end and grid-column-start/end
/// - Fractional units for flexible sizing
/// </summary>
public class GridLayoutExample : Example
{
    public GridLayoutExample()
    {
        this.Title = "Grid Layout";

        this.Content = new VerticalStack
        {
            Content =
            [
                CreateBasicGridExample(),
                CreateNamedAreasExample(),
                CreateFractionalUnitsExample(),
            ]
        };
    }

    /// <summary>
    /// Basic grid with explicit column/row positioning
    /// </summary>
    private static View CreateBasicGridExample()
    {
        return new Border
        {
            Margin = 8,
            Padding = 8,
            CornerRadius = 8,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = White,
            Content = new VerticalStack
            {
                Content =
                [
                    new Label
                    {
                        Text = "Basic Grid (2x2 with gaps)",
                        FontFamily = ["Inter"],
                        FontWeight = Xui.Core.Canvas.FontWeight.Bold,
                        Margin = (0, 0, 0, 8)
                    },
                    new Grid
                    {
                        TemplateColumns = [120, 120],
                        TemplateRows = [60, 60],
                        ColumnGap = 8,
                        RowGap = 8,
                        Content =
                        [
                            CreateGridItem("Cell 1,1", LightBlue, 1, 1),
                            CreateGridItem("Cell 1,2", LightGreen, 1, 2),
                            CreateGridItem("Cell 2,1", LightCoral, 2, 1),
                            CreateGridItem("Cell 2,2", LightYellow, 2, 2),
                        ]
                    }
                ]
            }
        };
    }

    /// <summary>
    /// Grid using named areas for a classic page layout
    /// </summary>
    private static View CreateNamedAreasExample()
    {
        return new Border
        {
            Margin = 8,
            Padding = 8,
            CornerRadius = 8,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = White,
            Content = new VerticalStack
            {
                Content =
                [
                    new Label
                    {
                        Text = "Named Grid Areas (Page Layout)",
                        FontFamily = ["Inter"],
                        FontWeight = Xui.Core.Canvas.FontWeight.Bold,
                        Margin = (0, 0, 0, 8)
                    },
                    new Grid
                    {
                        TemplateColumns = [150, Fr(1)],
                        TemplateRows = [50, Fr(1), 40],
                        TemplateAreas =
                        [
                            "header  header",
                            "sidebar content",
                            "footer  footer",
                        ],
                        RowGap = 4,
                        ColumnGap = 4,
                        Content =
                        [
                            CreateAreaItem("Header", LightBlue, "header"),
                            CreateAreaItem("Sidebar", LightGreen, "sidebar"),
                            CreateAreaItem("Content", White, "content"),
                            CreateAreaItem("Footer", LightGray, "footer"),
                        ]
                    }
                ]
            }
        };
    }

    /// <summary>
    /// Grid demonstrating fractional units (fr) for flexible layouts
    /// </summary>
    private static View CreateFractionalUnitsExample()
    {
        return new Border
        {
            Margin = 8,
            Padding = 8,
            CornerRadius = 8,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = White,
            Content = new VerticalStack
            {
                Content =
                [
                    new Label
                    {
                        Text = "Fractional Units (100px, 1fr, 2fr)",
                        FontFamily = ["Inter"],
                        FontWeight = Xui.Core.Canvas.FontWeight.Bold,
                        Margin = (0, 0, 0, 8)
                    },
                    new Grid
                    {
                        TemplateColumns = [100, Fr(1), Fr(2)],
                        TemplateRows = [50],
                        ColumnGap = 8,
                        Content =
                        [
                            CreateGridItem("100px", LightBlue, 1, 1),
                            CreateGridItem("1fr", LightGreen, 1, 2),
                            CreateGridItem("2fr", LightCoral, 1, 3),
                        ]
                    }
                ]
            }
        };
    }

    private static View CreateGridItem(string text, Xui.Core.Canvas.Color bgColor, int row, int col)
    {
        return new Border
        {
            [RowStart] = row,
            [ColumnStart] = col,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = bgColor,
            CornerRadius = 4,
            Padding = 8,
            Content = new Label
            {
                Text = text,
                FontFamily = ["Inter"],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle
            }
        };
    }

    private static View CreateAreaItem(string text, Xui.Core.Canvas.Color bgColor, string area)
    {
        return new Border
        {
            [Area] = area,
            BorderThickness = 1,
            BorderColor = Gray,
            BackgroundColor = bgColor,
            CornerRadius = 4,
            Padding = 8,
            Content = new Label
            {
                Text = text,
                FontFamily = ["Inter"],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle
            }
        };
    }
}
