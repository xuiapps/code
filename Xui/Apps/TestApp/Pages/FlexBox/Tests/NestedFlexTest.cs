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
/// Demonstrates nested flexbox containers for complex layouts.
/// Shows how flex containers can be composed within each other to create sophisticated UI structures.
/// </summary>
public class NestedFlexTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox outerFlex;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5, Orange5, Cyan5, Pink5
    ];

    public override int Count => 1;
    public override View this[int index] => outerFlex;

    public NestedFlexTest()
    {
        // Outer vertical container
        outerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            RowGap = 15,
            Content =
            [
                // Header section
                CreateHeader(),
                
                // Content area with nested flex
                CreateContent(),
                
                // Footer section
                CreateFooter(),
            ]
        };
        AddProtectedChild(outerFlex);
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        outerFlex.Measure(availableSize, context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        outerFlex.Arrange(rect, context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(Gray10);
        context.FillRect(Frame);

        // Draw legend
        context.SetFill(White);
        context.SetFont(["Inter"], 12, FontWeight.Normal, FontSlant.Normal);
        context.FillText("Nested flex containers: header, content (with 3 columns), and footer", Frame.X + 10, Frame.Y + Frame.Height - 15);

        base.RenderCore(context);
    }

    private View CreateHeader()
    {
        var headerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 10,
            Content =
            [
                Box("Logo", palette[0], 80, 40),
                CreateNavigation(),
                Box("User", palette[1], 60, 40),
            ]
        };
        return new FixedHeightContainer(headerFlex, 40);
    }

    private View CreateNavigation()
    {
        return new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.Center,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 8,
            Content =
            [
                Box("Home", palette[2], 60, 30),
                Box("About", palette[3], 60, 30),
                Box("Contact", palette[4], 60, 30),
            ]
        };
    }

    private View CreateContent()
    {
        var contentFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Stretch,
            ColumnGap = 15,
            Content =
            [
                CreateColumn("Left", palette[5], 3),
                CreateColumn("Center", palette[6], 4),
                CreateColumn("Right", palette[7], 3),
            ]
        };
        return new FixedHeightContainer(contentFlex, 200);
    }

    private View CreateColumn(string title, Color color, int itemCount)
    {
        var items = new View[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            items[i] = Box($"{i + 1}", color, 0, 35);
        }

        return new FlexContainer
        {
            FlexGrow = 1,
            InnerFlex = new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = Direction.Column,
                FlexJustifyContent = JustifyContent.FlexStart,
                FlexAlignItems = AlignItems.Stretch,
                RowGap = 8,
                Content = items
            }
        };
    }

    private View CreateFooter()
    {
        return new FixedHeightContainer(
            Box("Footer Area", palette[0], 0, 50),
            50
        );
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

    private class FlexContainer : View
    {
        public nfloat FlexGrow { get; set; }
        public global::Xui.Core.UI.Layout.FlexBox? InnerFlex { get; set; }

        public override int Count => InnerFlex != null ? 1 : 0;
        public override View this[int index] => InnerFlex!;

        public FlexContainer()
        {
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            InnerFlex?.Measure(availableSize, context);
            return InnerFlex?.DesiredSize ?? Size.Zero;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            InnerFlex?.Arrange(rect, context);
        }

        protected override void RenderCore(IContext context)
        {
            base.RenderCore(context);
        }
    }

    private class FixedHeightContainer : View
    {
        private readonly View child;
        private readonly nfloat height;

        public override int Count => 1;
        public override View this[int index] => child;

        public FixedHeightContainer(View child, nfloat height)
        {
            this.child = child;
            this.height = height;
            AddProtectedChild(child);
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            child.Measure(new Size(availableSize.Width, height), context);
            return new Size(availableSize.Width, height);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            child.Arrange(new Rect(rect.X, rect.Y, rect.Width, height), context);
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
            if (Width > 0)
                return new Size(Width, Height);
            return new Size(availableSize.Width, Height);
        }

        protected override void RenderCore(IContext context)
        {
            context.SetFill(BackgroundColor);
            context.FillRect(Frame);

            context.SetFill(White);
            context.SetFont(["Inter"], 12, FontWeight.Normal, FontSlant.Normal);
            var textWidth = Text.Length * 6;
            context.FillText(Text, Frame.X + Frame.Width / 2 - textWidth / 2, Frame.Y + Frame.Height / 2 + 4);
        }
    }
}
