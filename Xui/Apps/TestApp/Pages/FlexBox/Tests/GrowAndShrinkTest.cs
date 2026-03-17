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
/// Demonstrates flex-grow and flex-shrink properties.
/// Items grow or shrink to fill available space.
/// </summary>
public class GrowAndShrinkTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox flexBox;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5
    ];

    public override int Count => 1;
    public override View this[int index] => flexBox;

    public GrowAndShrinkTest()
    {
        flexBox = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            ColumnGap = 10,
            Content =
            [
                Box("Fixed\n80px", palette[0], 80, 60),
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
        context.SetFill(Gray10);
        context.FillRect(Frame);

        // Draw legend
        context.SetFill(White);
        context.SetFont(["Inter"], 12, FontWeight.Normal, FontSlant.Normal);
        context.FillText("Fixed item, then three growing items (grow: 1, 2, 1)", Frame.X + 10, Frame.Y + Frame.Height - 20);

        base.RenderCore(context);
    }

    private static View Box(string text, Color color, nfloat width, nfloat height, nfloat grow = default)
    {
        var box = new BoxView
        {
            Text = text,
            BackgroundColor = color,
            Width = width,
            Height = height
        };
        if (grow > 0)
            box[Grow] = grow;
        return box;
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
            context.SetFont(["Inter"], 12, FontWeight.Normal, FontSlant.Normal);
            
            var lines = Text.Split('\n');
            var y = Frame.Y + (Frame.Height - lines.Length * 16) / 2 + 12;
            foreach (var line in lines)
            {
                context.FillText(line, Frame.X + 8, y);
                y += 16;
            }
        }
    }
}
