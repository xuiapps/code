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
/// Demonstrates mixing different sizing strategies: fixed-size items, flex-basis, flex-grow, and flex-shrink.
/// Shows how items with different flexibility interact in the same container.
/// </summary>
public class MixedSizingTest : View
{
    private readonly VerticalStack container;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5, Orange5, Cyan5, Pink5
    ];

    public override int Count => 1;
    public override View this[int index] => container;

    public MixedSizingTest()
    {
        container = new VerticalStack
        {
            Gap = 20,
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
        context.SetFill(Gray10);
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
                FlexDirection = Direction.Row,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = AlignItems.Center,
                ColumnGap = 10,
                Content = items
            }
        };
    }

    private View[] CreateFixedItems()
    {
        return
        [
            FixedBox("100px", palette[0], 100, 45),
            FixedBox("100px", palette[1], 100, 45),
            FixedBox("100px", palette[2], 100, 45),
        ];
    }

    private View[] CreateFixedPlusGrow()
    {
        return
        [
            FixedBox("80px", palette[0], 80, 45),
            FlexBox("grow=1", palette[1], 0, 45, 1, 0, 0),
            FixedBox("80px", palette[2], 80, 45),
        ];
    }

    private View[] CreateGrowRatios()
    {
        return
        [
            FlexBox("1", palette[3], 0, 45, 1, 0, 0),
            FlexBox("2", palette[4], 0, 45, 2, 0, 0),
            FlexBox("1", palette[5], 0, 45, 1, 0, 0),
        ];
    }

    private View[] CreateBasisPlusGrow()
    {
        return
        [
            FlexBox("basis:100\ngrow:1", palette[0], 100, 45, 1, 0, 100),
            FlexBox("basis:150\ngrow:2", palette[1], 150, 45, 2, 0, 150),
            FlexBox("basis:80\ngrow:1", palette[2], 80, 45, 1, 0, 80),
        ];
    }

    private View[] CreateMixedComplex()
    {
        return
        [
            FixedBox("60", palette[6], 60, 45),
            FlexBox("g:2", palette[7], 0, 45, 2, 0, 0),
            FixedBox("80", palette[0], 80, 45),
            FlexBox("g:1", palette[1], 0, 45, 1, 0, 0),
            FixedBox("50", palette[2], 50, 45),
        ];
    }

    private static View FixedBox(string text, Color color, nfloat width, nfloat height)
    {
        return new BoxView
        {
            Text = text,
            BackgroundColor = color,
            Width = width,
            Height = height,
            IsFixed = true
        };
    }

    private static View FlexBox(string text, Color color, nfloat width, nfloat height, 
        nfloat grow, nfloat shrink, nfloat basis)
    {
        return new FlexibleBoxView
        {
            Text = text,
            BackgroundColor = color,
            MinWidth = width,
            Height = height,
            FlexGrow = grow,
            FlexShrink = shrink,
            FlexBasis = basis
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
            context.SetFill(Gray9);
            context.FillRect(new Rect(Frame.X, Frame.Y, 135, Frame.Height));

            // Draw label text
            context.SetFill(White);
            context.SetFont(["Inter"], 10, FontWeight.Normal, FontSlant.Normal);
            context.FillText(Label, Frame.X + 5, Frame.Y + Frame.Height / 2 + 3);

            // Draw flex container background
            context.SetFill(Gray8);
            context.FillRect(new Rect(Frame.X + 140, Frame.Y, Frame.Width - 140, Frame.Height));

            base.RenderCore(context);
        }
    }

    private class BoxView : View
    {
        public string Text { get; set; } = "";
        public Color BackgroundColor { get; set; }
        public nfloat Width { get; set; }
        public nfloat Height { get; set; }
        public bool IsFixed { get; set; }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            return new Size(Width, Height);
        }

        protected override void RenderCore(IContext context)
        {
            context.SetFill(BackgroundColor);
            context.FillRect(Frame);

            // Draw border for fixed items
            if (IsFixed)
            {
                context.SetStroke(White);
                context.SetLineWidth(2);
                context.StrokeRect(Frame);
            }

            context.SetFill(White);
            context.SetFont(["Inter"], 11, FontWeight.Normal, FontSlant.Normal);
            
            var lines = Text.Split('\n');
            var startY = Frame.Y + Frame.Height / 2 - (lines.Length - 1) * 6;
            for (int i = 0; i < lines.Length; i++)
            {
                var textWidth = lines[i].Length * 5.5;
                context.FillText(lines[i], Frame.X + Frame.Width / 2 - textWidth / 2, startY + i * 14);
            }
        }
    }

    private class FlexibleBoxView : View
    {
        public string Text { get; set; } = "";
        public Color BackgroundColor { get; set; }
        public nfloat MinWidth { get; set; }
        public nfloat Height { get; set; }
        public nfloat FlexGrow { get; set; }
        public nfloat FlexShrink { get; set; }
        public nfloat FlexBasis { get; set; }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            var width = FlexBasis > 0 ? FlexBasis : (MinWidth > 0 ? MinWidth : 100);
            return new Size(width, Height);
        }

        protected override void RenderCore(IContext context)
        {
            context.SetFill(BackgroundColor);
            context.FillRect(Frame);

            // Draw dashed border for flex items
            context.SetStroke(White);
            context.SetLineWidth(1);
            var dashPattern = new nfloat[] { 4, 4 };
            context.SetLineDash(dashPattern);
            context.StrokeRect(Frame);
            context.SetLineDash([]); // Reset

            context.SetFill(White);
            context.SetFont(["Inter"], 10, FontWeight.Normal, FontSlant.Normal);
            
            var lines = Text.Split('\n');
            var startY = Frame.Y + Frame.Height / 2 - (lines.Length - 1) * 6;
            for (int i = 0; i < lines.Length; i++)
            {
                var textWidth = lines[i].Length * 5;
                context.FillText(lines[i], Frame.X + Frame.Width / 2 - textWidth / 2, startY + i * 12);
            }
        }
    }
}
