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
/// Demonstrates flex-wrap property for multi-line layouts.
/// Items wrap onto new lines when they don't fit.
/// </summary>
public class WrapTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox flexBox;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5, Orange5, Cyan5, Pink5
    ];

    public override int Count => 1;
    public override View this[int index] => flexBox;

    public WrapTest()
    {
        flexBox = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexWrap = Wrap.Wrap,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.FlexStart,
            FlexAlignContent = AlignContent.FlexStart,
            ColumnGap = 10,
            RowGap = 10,
            Content =
            [
                Box("1", palette[0], 120, 60),
                Box("2", palette[1], 140, 60),
                Box("3", palette[2], 100, 60),
                Box("4", palette[3], 130, 60),
                Box("5", palette[4], 110, 60),
                Box("6", palette[5], 150, 60),
                Box("7", palette[6], 120, 60),
                Box("8", palette[7], 100, 60),
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
        context.SetFill(Gray10);
        context.FillRect(Frame);

        // Draw legend
        context.SetFill(White);
        context.SetFont(["Inter"], 12, FontWeight.Normal, FontSlant.Normal);
        context.FillText("Items wrap onto multiple lines with row-gap and column-gap", Frame.X + 10, Frame.Y + Frame.Height - 20);

        base.RenderCore(context);
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
