using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates flex-grow and flex-shrink properties.
/// Items grow or shrink to fill available space.
/// </summary>
public class GrowAndShrinkTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox flexBox;

    private static readonly Color[] palette =
    [
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
        new Color(0xF5, 0xA6, 0x23, 0xFF), // Yellow
    ];

    public override int Count => 1;
    public override View this[int index] => flexBox;

    public GrowAndShrinkTest()
    {
        flexBox = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                Box("Fixed 80px", palette[0], 80, 60),
                Box("Grow 1", palette[1], 50, 60, grow: 1),
                Box("Grow 2", palette[2], 50, 60, grow: 2),
                Box("Grow 1", palette[3], 50, 60, grow: 1),
            ]
        };
        AddProtectedChild(flexBox);
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        flexBox.Measure(availableSize, context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        flexBox.Arrange(rect, context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(new Color(0xF5, 0xF5, 0xF5, 0xFF));
        context.FillRect(Frame);
        base.RenderCore(context);
    }

    private static View Box(string text, Color color, NFloat width, NFloat height, NFloat grow = default)
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
        var sizedBox = new SizedBox(border, width, height);
        if (grow > 0)
            sizedBox[Grow] = grow;
        return sizedBox;
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
