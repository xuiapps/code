using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates row-gap and column-gap properties with wrapping layouts.
/// Shows how gaps create consistent spacing between flex items both horizontally and vertically.
/// </summary>
public class GapsTest : View
{
    private readonly VerticalStack container;

    private static readonly Color[] palette =
    [
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
        new Color(0xF5, 0xA6, 0x23, 0xFF), // Yellow
        new Color(0x9B, 0x59, 0xB6, 0xFF), // Purple
        new Color(0xF3, 0x9C, 0x12, 0xFF), // Orange
        new Color(0x1A, 0xBC, 0x9C, 0xFF), // Cyan
        new Color(0xE7, 0x4C, 0x3C, 0xFF), // Pink
    ];

    public override int Count => 1;
    public override View this[int index] => container;

    public GapsTest()
    {
        container = new VerticalStack
        {
            Content =
            [
                FlexRow("No gaps", 0, 0),
                FlexRow("column-gap: 20", 20, 0),
                FlexRow("row-gap: 20", 0, 20),
                FlexRow("both: 20", 20, 20),
                FlexRow("col: 5, row: 15", 5, 15),
            ]
        };
        AddProtectedChild(container);
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        container.Measure(availableSize, context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        container.Arrange(rect, context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(new Color(0xF5, 0xF5, 0xF5, 0xFF));
        context.FillRect(Frame);
        base.RenderCore(context);
    }

    private View FlexRow(string label, NFloat columnGap, NFloat rowGap)
    {
        return new LabeledFlexBox
        {
            Label = label,
            FlexBox = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
                FlexWrap = Wrap.Wrap,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = AlignItems.FlexStart,
                FlexAlignContent = AlignContent.FlexStart,
                ColumnGap = columnGap,
                RowGap = rowGap,
                Content =
                [
                    Box("1", palette[0], 65, 35),
                    Box("2", palette[1], 65, 35),
                    Box("3", palette[2], 65, 35),
                    Box("4", palette[3], 65, 35),
                    Box("5", palette[4], 65, 35),
                    Box("6", palette[5], 65, 35),
                ]
            }
        };
    }

    private static View Box(string text, Color color, NFloat width, NFloat height)
    {
        var border = new Border
        {
            BackgroundColor = color,
            BorderThickness = 1,
            BorderColor = new Color(0x00, 0x00, 0x00, 0x40),
            Padding = 4,
            Content = new Label
            {
                Text = text,
                FontFamily = ["Inter"],
                FontSize = 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
            }
        };
        return new SizedBox(border, width, height);
    }

    private class LabeledFlexBox : View
    {
        public string Label { get; set; } = "";
        public global::Xui.Core.UI.Layout.FlexBox? FlexBox { get; set; }

        public override int Count => FlexBox != null ? 1 : 0;
        public override View this[int index] => FlexBox!;

        public LabeledFlexBox()
        {
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            FlexBox?.Measure(new Size(availableSize.Width - 140, 100), context);
            return new Size(availableSize.Width, 100);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            FlexBox?.Arrange(new Rect(rect.X + 140, rect.Y, rect.Width - 140, 100), context);
        }

        protected override void RenderCore(IContext context)
        {
            // Draw label background
            context.SetFill(new Color(0xA9, 0xA9, 0xA9, 0xFF));
            context.FillRect(new Rect(Frame.X, Frame.Y, 135, Frame.Height));

            // Draw label text
            context.SetFont(new Font { FontFamily = ["Inter"], FontSize = 12 });
            context.TextBaseline = TextBaseline.Middle;
            context.TextAlign = TextAlign.Center;
            context.SetFill(White);
            context.FillText(Label, new Point(Frame.X + 67.5f, Frame.Y + Frame.Height / 2));

            // Draw flex container background
            context.SetFill(new Color(0xC0, 0xC0, 0xC0, 0xFF));
            context.FillRect(new Rect(Frame.X + 140, Frame.Y, Frame.Width - 140, Frame.Height));

            base.RenderCore(context);
        }
    }

    private class SizedBox : View
    {
        private readonly View _content;
        public NFloat Width { get; set; }
        public NFloat Height { get; set; }

        public override int Count => _content != null ? 1 : 0;
        public override View this[int index] => _content;

        public SizedBox(View content, NFloat width, NFloat height)
        {
            _content = content;
            Width = width;
            Height = height;
            if (_content != null)
                AddProtectedChild(_content);
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            var size = new Size(Width > 0 ? Width : availableSize.Width, Height > 0 ? Height : availableSize.Height);
            _content?.Measure(size, context);
            return size;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            _content?.Arrange(rect, context);
        }
    }
}
