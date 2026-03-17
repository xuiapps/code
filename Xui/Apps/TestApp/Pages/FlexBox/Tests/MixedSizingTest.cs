using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates mixing different sizing strategies: fixed-size items, flex-basis, flex-grow, and flex-shrink.
/// Shows how items with different flexibility interact in the same container.
/// </summary>
public class MixedSizingTest : View
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

    public MixedSizingTest()
    {
        container = new VerticalStack
        {
            Content =
            [
                FlexRow("All fixed (100px each)", CreateFixedItems()),
                FlexRow("Fixed + grow=1", CreateFixedPlusGrow()),
                FlexRow("Grow (1, 2, 1)", CreateGrowRatios()),
                FlexRow("Basis + grow", CreateBasisPlusGrow()),
                FlexRow("Mixed complex", CreateMixedComplex()),
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

    private View FlexRow(string label, View[] items)
    {
        return new LabeledFlexBox
        {
            Label = label,
            FlexBox = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = AlignItems.Center,
                Content = items
            }
        };
    }

    private View[] CreateFixedItems()
    {
        return
        [
            Box("100px", palette[0], 100, 45),
            Box("100px", palette[1], 100, 45),
            Box("100px", palette[2], 100, 45),
        ];
    }

    private View[] CreateFixedPlusGrow()
    {
        return
        [
            Box("80px", palette[0], 80, 45),
            FlexBox("grow=1", palette[1], 45, 1),
            Box("80px", palette[2], 80, 45),
        ];
    }

    private View[] CreateGrowRatios()
    {
        return
        [
            FlexBox("1", palette[3], 45, 1),
            FlexBox("2", palette[4], 45, 2),
            FlexBox("1", palette[5], 45, 1),
        ];
    }

    private View[] CreateBasisPlusGrow()
    {
        return
        [
            FlexBox("basis:100 g:1", palette[0], 45, 1),
            FlexBox("basis:150 g:2", palette[1], 45, 2),
            FlexBox("basis:80 g:1", palette[2], 45, 1),
        ];
    }

    private View[] CreateMixedComplex()
    {
        return
        [
            Box("60", palette[6], 60, 45),
            FlexBox("g:2", palette[7], 45, 2),
            Box("80", palette[0], 80, 45),
            FlexBox("g:1", palette[1], 45, 1),
            Box("50", palette[2], 50, 45),
        ];
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

    private static View FlexBox(string text, Color color, NFloat height, NFloat grow)
    {
        var box = new Border
        {
            BackgroundColor = color,
            BorderThickness = 1,
            BorderColor = new Color(0x00, 0x00, 0x00, 0x40),
            Padding = 4,
            Content = new Label
            {
                Text = text,
                FontFamily = ["Inter"],
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
            }
        };
        box[Grow] = grow;
        return box;
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
            FlexBox?.Measure(new Size(availableSize.Width - 140, 80), context);
            return new Size(availableSize.Width, 80);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            FlexBox?.Arrange(new Rect(rect.X + 140, rect.Y, rect.Width - 140, 80), context);
        }

        protected override void RenderCore(IContext context)
        {
            // Draw label background
            context.SetFill(new Color(0xA9, 0xA9, 0xA9, 0xFF));
            context.FillRect(new Rect(Frame.X, Frame.Y, 135, Frame.Height));

            // Draw label text
            context.SetFill(White);
            // TODO: Add text rendering

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
