using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using static Xui.Core.Canvas.FontWeight;

namespace Xui.Apps.TestApp.Pages.Gestures.Tests;

/// <summary>
/// Vertical outer ScrollView containing horizontal inner ScrollViews. The horizontal
/// rows capture with <c>IDragHorizontalTentative</c> on Down, so they scroll
/// horizontally from the first move — but if the user's motion is dominantly vertical
/// when the 8pt threshold is crossed, the outer ScrollView steals capture and scrolls
/// vertically instead. Drag horizontally past 20pt and the inner row promotes itself
/// to a firm <c>IDrag</c>; from that point the outer can no longer steal.
/// </summary>
public class NestedScrollDemo : View
{
    private readonly ScrollView outer;

    public NestedScrollDemo()
    {
        outer = new ScrollView
        {
            Direction = ScrollDirection.Vertical,
            Content = new VerticalStack
            {
                Margin = 16,
                Content =
                [
                    Heading("Nested scroll (horizontal in vertical)"),
                    Paragraph(
                        "Outer is a vertical ScrollView; the colored rows are horizontal " +
                        "ScrollViews captured as IDragHorizontalTentative."),
                    Paragraph(
                        "• Drag horizontally → row scrolls.\n" +
                        "• Drag mostly vertically → outer steals at 8pt and scrolls vertically.\n" +
                        "• Drag horizontally past 20pt → row promotes to IDrag; outer can no longer steal."),
                    Spacer(12),
                    Caption("Row A — tiles 1..12"),
                    ColoredRow(0xFF3B82F6, 12),
                    Spacer(12),
                    Caption("Row B — tiles 1..12"),
                    ColoredRow(0xFFEF4444, 12),
                    Spacer(12),
                    Caption("Row C — tiles 1..12"),
                    ColoredRow(0xFF10B981, 12),
                    Spacer(12),
                    Paragraph(
                        "Filler below to make the outer ScrollView taller than the viewport."),
                    Filler(40),
                ]
            }
        };
        AddProtectedChild(outer);
    }

    public override int Count => 1;
    public override View this[int index] => index == 0 ? outer : throw new IndexOutOfRangeException();

    protected override Size MeasureCore(Size available, IMeasureContext context)
    {
        outer.Measure(available, context);
        return available;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        outer.Arrange(rect, context);
    }

    private static Label Heading(string text) =>
        new Label { Text = text, FontFamily = "Inter", FontSize = 18, FontWeight = SemiBold, Margin = (0, 0, 8, 0) };

    private static Label Paragraph(string text) =>
        new Label { Text = text, FontFamily = "Inter", FontSize = 13, Margin = (0, 0, 4, 0) };

    private static Label Caption(string text) =>
        new Label { Text = text, FontFamily = "Inter", FontSize = 14, FontWeight = SemiBold, Margin = (0, 0, 6, 0) };

    private static View Spacer(NFloat height) =>
        new Label { Text = "", FontFamily = "Inter", MinimumHeight = height };

    private static View Filler(int rows)
    {
        var stack = new VerticalStack();
        for (int i = 0; i < rows; i++)
            stack.Add(new Label { Text = $"Filler line {i + 1}", FontFamily = "Inter", FontSize = 13, Margin = (0, 0, 4, 0) });
        return stack;
    }

    private static ScrollView ColoredRow(uint baseRgba, int count)
    {
        var stack = new HorizontalStack();
        for (int i = 0; i < count; i++)
        {
            // Vary brightness slightly across the row so tiles read as distinct.
            var c = Tint(baseRgba, i, count);
            stack.Add(new Border
            {
                MinimumWidth = 120,
                MinimumHeight = 80,
                BackgroundColor = c,
                CornerRadius = 8,
                Margin = (0, 8, 0, 0),
                Content = new Label
                {
                    Text = $"#{i + 1}",
                    FontFamily = "Inter",
                    FontSize = 18,
                    FontWeight = Bold,
                    TextColor = Colors.White,
                    Margin = 16,
                },
            });
        }

        return new ScrollView
        {
            Direction = ScrollDirection.Horizontal,
            MinimumHeight = 96,
            Content = stack,
        };
    }

    private static Color Tint(uint baseRgba, int i, int n)
    {
        // Spread alpha-mixed brightness across the row.
        var r = (byte)((baseRgba >> 16) & 0xFF);
        var g = (byte)((baseRgba >> 8) & 0xFF);
        var b = (byte)(baseRgba & 0xFF);
        var t = (float)i / Math.Max(1, n - 1);     // 0..1
        var k = 0.55f + 0.45f * t;                  // 0.55..1.0
        return new Color((byte)(r * k), (byte)(g * k), (byte)(b * k), 0xFF);
    }
}
