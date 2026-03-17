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
/// Demonstrates align-content for multi-line wrapping containers.
/// Controls how multiple lines are distributed along the cross-axis when there's extra space.
/// Only applies when FlexWrap is set to Wrap and there are multiple lines.
/// </summary>
public class AlignContentTest : View
{
    private readonly VerticalStack container;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5, Orange5
    ];

    public override int Count => 1;
    public override View this[int index] => container;

    public AlignContentTest()
    {
        container = new VerticalStack
        {
            Gap = 20,
            Content =
            [
                FlexRow("flex-start", AlignContent.FlexStart),
                FlexRow("flex-end", AlignContent.FlexEnd),
                FlexRow("center", AlignContent.Center),
                FlexRow("space-between", AlignContent.SpaceBetween),
                FlexRow("space-around", AlignContent.SpaceAround),
                FlexRow("space-evenly", AlignContent.SpaceEvenly),
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

    private View FlexRow(string label, AlignContent align)
    {
        return new LabeledFlexBox
        {
            Label = label,
            FlexBox = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = Direction.Row,
                FlexWrap = Wrap.Wrap,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = AlignItems.FlexStart,
                FlexAlignContent = align,
                ColumnGap = 8,
                RowGap = 8,
                Content =
                [
                    Box("1", palette[0], 70, 30),
                    Box("2", palette[1], 70, 30),
                    Box("3", palette[2], 70, 30),
                    Box("4", palette[3], 70, 30),
                    Box("5", palette[4], 70, 30),
                    Box("6", palette[5], 70, 30),
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
            FlexBox?.Measure(new Size(availableSize.Width - 120, 120), context);
            return new Size(availableSize.Width, 120);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            FlexBox?.Arrange(new Rect(rect.X + 120, rect.Y, rect.Width - 120, 120), context);
        }

        protected override void RenderCore(IContext context)
        {
            // Draw label background
            context.SetFill(Gray9);
            context.FillRect(new Rect(Frame.X, Frame.Y, 115, Frame.Height));

            // Draw label text
            context.SetFill(White);
            context.SetFont(["Inter"], 11, FontWeight.Normal, FontSlant.Normal);
            context.FillText(Label, Frame.X + 5, Frame.Y + Frame.Height / 2 + 4);

            // Draw flex container background
            context.SetFill(Gray8);
            context.FillRect(new Rect(Frame.X + 120, Frame.Y, Frame.Width - 120, Frame.Height));

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
