using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.Grid;
using static Xui.Core.UI.Layout.Grid.TrackSize;

namespace Xui.Apps.TestApp.Pages.Grid.Tests;

/// <summary>
/// Grid demonstrating cells that span multiple columns and rows.
/// Uses RowSpan, ColumnSpan, RowEnd, and ColumnEnd attached properties.
/// </summary>
public class SpanningCellsTest : View
{
    private readonly global::Xui.Core.UI.Layout.Grid grid;

    public override int Count => 1;
    public override View this[int index] => grid;

    public SpanningCellsTest()
    {
        // 4 columns × 4 rows, with various spanning items
        var wide = new Border
        {
            [RowStart] = 1,
            [ColumnStart] = 1,
            [ColumnSpan] = 3,
            BackgroundColor = new Color(0x4A, 0x90, 0xD9, 0xFF),
            CornerRadius = 4,
            BorderThickness = 1,
            BorderColor = new Color(0x00, 0x00, 0x00, 0x30),
            Padding = 8,
            Content = new Label
            {
                Text = "col-span 3  (cols 1–3, row 1)",
                FontFamily = ["Inter"],
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
            }
        };

        var tall = new Border
        {
            [RowStart] = 1,
            [ColumnStart] = 4,
            [RowSpan] = 4,
            BackgroundColor = new Color(0x9B, 0x59, 0xB6, 0xFF),
            CornerRadius = 4,
            BorderThickness = 1,
            BorderColor = new Color(0x00, 0x00, 0x00, 0x30),
            Padding = 8,
            Content = new Label
            {
                Text = "row\nspan\n4\n(col 4)",
                FontFamily = ["Inter"],
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
            }
        };

        var big = new Border
        {
            [RowStart] = 2,
            [ColumnStart] = 1,
            [RowSpan] = 2,
            [ColumnSpan] = 2,
            BackgroundColor = new Color(0x1A, 0xBC, 0x9C, 0xFF),
            CornerRadius = 4,
            BorderThickness = 1,
            BorderColor = new Color(0x00, 0x00, 0x00, 0x30),
            Padding = 8,
            Content = new Label
            {
                Text = "2×2 span\n(rows 2–3, cols 1–2)",
                FontFamily = ["Inter"],
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
            }
        };

        grid = new global::Xui.Core.UI.Layout.Grid
        {
            TemplateColumns = [Fr(1), Fr(1), Fr(1), Fr(1)],
            TemplateRows = [50, 60, 60, 50],
            ColumnGap = 8,
            RowGap = 8,
            Content =
            [
                wide,
                tall,
                big,
                Cell("2,3", new Color(0xF0, 0xB2, 0x7A, 0xFF), 2, 3),
                Cell("3,3", new Color(0x85, 0xC1, 0xE9, 0xFF), 3, 3),
                Cell("4,1", new Color(0xF1, 0x94, 0x8A, 0xFF), 4, 1),
                Cell("4,2", new Color(0x82, 0xE0, 0xAA, 0xFF), 4, 2),
                Cell("4,3", new Color(0xFA, 0xD7, 0xA0, 0xFF), 4, 3),
            ]
        };
        AddProtectedChild(grid);
    }

    static Border Cell(string text, Color bg, int row, int col) => new Border
    {
        [RowStart] = row,
        [ColumnStart] = col,
        BackgroundColor = bg,
        CornerRadius = 4,
        BorderThickness = 1,
        BorderColor = new Color(0x00, 0x00, 0x00, 0x30),
        Padding = 6,
        Content = new Label
        {
            Text = text,
            FontFamily = ["Inter"],
            FontSize = 11,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Middle,
        }
    };

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        grid.Measure(availableSize, context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        NFloat padding = 24;
        grid.Arrange(new Rect(
            rect.X + padding,
            rect.Y + padding,
            rect.Width - padding * 2,
            rect.Height - padding * 2), context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(new Color(0xF5, 0xF5, 0xF5, 0xFF));
        context.FillRect(Frame);
        base.RenderCore(context);
    }
}
