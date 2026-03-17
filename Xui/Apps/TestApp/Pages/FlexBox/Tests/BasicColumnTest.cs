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
/// Demonstrates a basic vertical flexbox layout with fixed-size items.
/// </summary>
public class BasicColumnTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox flexBox;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5
    ];

    public override int Count => 1;
    public override View this[int index] => flexBox;

    public BasicColumnTest()
    {
        flexBox = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.FlexStart,
            RowGap = 10,
            Content =
            [
                Box("Item 1", palette[0], 150, 50),
                Box("Item 2", palette[1], 200, 60),
                Box("Item 3", palette[2], 180, 55),
                Box("Item 4", palette[3], 170, 65),
            ]
        };
        AddProtectedChild(flexBox);
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        flexBox.Measure(availableSize, context);
        return flexBox.DesiredSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        flexBox.Arrange(rect, context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(Gray10);
        context.FillRect(Frame);
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
            context.FillText(Text, Frame.X + 8, Frame.Y + Frame.Height / 2 + 5);
        }
    }
}
