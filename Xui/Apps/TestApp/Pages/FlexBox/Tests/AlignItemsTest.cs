using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates different align-items values for cross-axis alignment.
/// Shows how items are aligned along the cross axis within their line.
/// </summary>
public class AlignItemsTest : View
{
    private readonly VerticalStack container;

    private static readonly Color[] palette =
    [
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
    ];

    public override int Count => 1;
    public override View this[int index] => container;

    public AlignItemsTest()
    {
        container = new VerticalStack
        {
            Content =
            [
                FlexRow("flex-start", AlignItems.FlexStart),
                FlexRow("flex-end", AlignItems.FlexEnd),
                FlexRow("center", AlignItems.Center),
                FlexRow("stretch", AlignItems.Stretch),
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

    private View FlexRow(string label, AlignItems align)
    {
        return new LabeledFlexBox
        {
            Label = label,
            FlexBox = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = align,
                Content =
                [
                    Box("1", palette[0], 60, 30),
                    Box("2", palette[1], 60, 50),
                    Box("3", palette[2], 60, 40),
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
            FlexBox?.Measure(new Size(availableSize.Width - 100, 80), context);
            return new Size(availableSize.Width, 80);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            FlexBox?.Arrange(new Rect(rect.X + 100, rect.Y, rect.Width - 100, 80), context);
        }

        protected override void RenderCore(IContext context)
        {
            // Draw label background
            context.SetFill(new Color(0xA9, 0xA9, 0xA9, 0xFF));
            context.FillRect(new Rect(Frame.X, Frame.Y, 95, Frame.Height));

            // Draw label text
            context.SetFill(White);
            // TODO: Add text rendering

            // Draw flex container background
            context.SetFill(new Color(0xC0, 0xC0, 0xC0, 0xFF));
            context.FillRect(new Rect(Frame.X + 100, Frame.Y, Frame.Width - 100, Frame.Height));

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
