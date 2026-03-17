using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;
#pragma warning disable CS8981
using nfloat = System.Runtime.InteropServices.NFloat;
#pragma warning restore CS8981

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
        Blue5, Green5, Red5
    ];

    public override int Count => 1;
    public override View this[int index] => container;

    public AlignItemsTest()
    {
        container = new VerticalStack
        {
            Gap = 20,
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
        context.SetFill(Gray10);
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
                FlexDirection = Direction.Row,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = align,
                ColumnGap = 10,
                Content =
                [
                    Box("1", palette[0], 60, 30),
                    Box("2", palette[1], 60, 50),
                    Box("3", palette[2], 60, 40),
                ]
            }
        };
    }

    private static View Box(string text, Color color, nfloat width, nfloat height)
    {
        return new BoxView
        {
            Text = text,
            BackgroundColor = color,
            Width = width,
            Height = height
        };
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
            context.SetFill(Gray9);
            context.FillRect(new Rect(Frame.X, Frame.Y, 95, Frame.Height));

            // Draw label text
            context.SetFill(White);
            context.SetFont(["Inter"], 11, FontWeight.Normal, FontSlant.Normal);
            context.FillText(Label, Frame.X + 5, Frame.Y + Frame.Height / 2 + 4);

            // Draw flex container background
            context.SetFill(Gray8);
            context.FillRect(new Rect(Frame.X + 100, Frame.Y, Frame.Width - 100, Frame.Height));

            base.RenderCore(context);
        }
    }

    private class BoxView : View
    {
        public string Text { get; set; } = "";
        public Color BackgroundColor { get; set; }
        public nfloat Width { get; set; }
        public nfloat Height { get; set; }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            return new Size(Width, Height);
        }

        protected override void RenderCore(IContext context)
        {
            context.SetFill(BackgroundColor);
            context.FillRect(Frame);

            context.SetFill(White);
            context.SetFont(["Inter"], 14, FontWeight.Normal, FontSlant.Normal);
            context.FillText(Text, Frame.X + Frame.Width / 2 - 5, Frame.Y + Frame.Height / 2 + 5);
        }
    }
}
