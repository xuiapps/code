using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

namespace Xui.Apps.TestApp.Pages.FlexBox.Tests;

/// <summary>
/// Demonstrates different justify-content values for main-axis alignment.
/// Shows how items are distributed along the main axis.
/// </summary>
public class JustifyContentTest : View
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

    public JustifyContentTest()
    {
        container = new VerticalStack
        {
            Content =
            [
                FlexRow("flex-start", JustifyContent.FlexStart),
                FlexRow("flex-end", JustifyContent.FlexEnd),
                FlexRow("center", JustifyContent.Center),
                FlexRow("space-between", JustifyContent.SpaceBetween),
                FlexRow("space-around", JustifyContent.SpaceAround),
                FlexRow("space-evenly", JustifyContent.SpaceEvenly),
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

    private View FlexRow(string label, JustifyContent justify)
    {
        return new LabeledFlexBox
        {
            Label = label,
            FlexBox = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
                FlexJustifyContent = justify,
                FlexAlignItems = AlignItems.Center,
                Content =
                [
                    Box("1", palette[0], 60, 40),
                    Box("2", palette[1], 60, 40),
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
            FlexBox?.Measure(new Size(availableSize.Width - 100, 50), context);
            return new Size(availableSize.Width, 50);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            FlexBox?.Arrange(new Rect(rect.X + 100, rect.Y, rect.Width - 100, 50), context);
        }

        protected override void RenderCore(IContext context)
        {
            // Draw label background
            context.SetFill(new Color(0xA9, 0xA9, 0xA9, 0xFF));
            context.FillRect(new Rect(Frame.X, Frame.Y, 95, Frame.Height));

            // Draw label text
            context.SetFont(new Font { FontFamily = ["Inter"], FontSize = 12 });
            context.TextBaseline = TextBaseline.Middle;
            context.TextAlign = TextAlign.Center;
            context.SetFill(White);
            context.FillText(Label, new Point(Frame.X + 47.5f, Frame.Y + Frame.Height / 2));

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
