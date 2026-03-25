using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates a basic horizontal flexbox layout with fixed-size items.
/// </summary>
public class BasicRowTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox flexBox;

    private static readonly Color[] palette =
    [
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
        new Color(0xF5, 0xA6, 0x23, 0xFF), // Yellow
        new Color(0x9B, 0x59, 0xB6, 0xFF), // Purple
    ];

    public override int Count => 1;
    public override View this[int index] => flexBox;

    public BasicRowTest()
    {
        flexBox = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.FlexStart,
            Content =
            [
                Box("Item 1", palette[0], 100, 60),
                Box("Item 2", palette[1], 120, 80),
                Box("Item 3", palette[2], 90, 70),
                Box("Item 4", palette[3], 110, 65),
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
            // When width/height is 0 or unset, measure content to get its size
            // But clamp infinite available sizes to prevent returning infinity
            if (Width > 0 && Height > 0)
            {
                // Both dimensions specified
                _content?.Measure(new Size(Width, Height), context);
                return new Size(Width, Height);
            }
            else if (Width > 0)
            {
                // Only width specified, get height from content
                var contentSize = _content?.Measure(new Size(Width, NFloat.IsFinite(availableSize.Height) ? availableSize.Height : NFloat.PositiveInfinity), context) ?? Size.Empty;
                return new Size(Width, NFloat.IsFinite(contentSize.Height) ? contentSize.Height : 0);
            }
            else if (Height > 0)
            {
                // Only height specified, get width from content
                var contentSize = _content?.Measure(new Size(NFloat.IsFinite(availableSize.Width) ? availableSize.Width : NFloat.PositiveInfinity, Height), context) ?? Size.Empty;
                return new Size(NFloat.IsFinite(contentSize.Width) ? contentSize.Width : 0, Height);
            }
            else
            {
                // No dimensions specified, measure content with clamped available size
                var availWidth = NFloat.IsFinite(availableSize.Width) ? availableSize.Width : NFloat.PositiveInfinity;
                var availHeight = NFloat.IsFinite(availableSize.Height) ? availableSize.Height : NFloat.PositiveInfinity;
                var contentSize = _content?.Measure(new Size(availWidth, availHeight), context) ?? Size.Empty;
                // Clamp infinity to 0 - don't return infinity from measurement
                return new Size(
                    NFloat.IsFinite(contentSize.Width) ? contentSize.Width : 0,
                    NFloat.IsFinite(contentSize.Height) ? contentSize.Height : 0
                );
            }
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            _content?.Arrange(rect, context);
        }
    }
}
