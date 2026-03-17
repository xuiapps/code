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
/// Demonstrates a typical responsive web layout using flexbox.
/// Shows a header, sidebar, main content area, and footer with realistic proportions and spacing.
/// </summary>
public class ResponsiveLayoutTest : View
{
    private readonly global::Xui.Core.UI.Layout.FlexBox rootFlex;

    private static readonly Color[] palette =
    [
        Blue5, Green5, Red5, Yellow5, Purple5, Orange5, Cyan5, Pink5, Blue6, Green6
    ];

    public override int Count => 1;
    public override View this[int index] => rootFlex;

    public ResponsiveLayoutTest()
    {
        rootFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            RowGap = 0,
            Content =
            [
                // Header
                CreateHeader(),
                
                // Main content area (sidebar + content)
                CreateMainContent(),
                
                // Footer
                CreateFooter(),
            ]
        };
        AddProtectedChild(rootFlex);
    }

    protected override Size MeasureCore(Size availableSize, IMeasureContext context)
    {
        rootFlex.Measure(availableSize, context);
        return availableSize;
    }

    protected override void ArrangeCore(Rect rect, IMeasureContext context)
    {
        rootFlex.Arrange(rect, context);
    }

    protected override void RenderCore(IContext context)
    {
        context.SetFill(Gray10);
        context.FillRect(Frame);
        base.RenderCore(context);
    }

    private View CreateHeader()
    {
        var headerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 20,
            Content =
            [
                Box("Logo", palette[0], 100, 50),
                CreateHeaderNav(),
                CreateHeaderActions(),
            ]
        };
        return new PaddedContainer(headerFlex, 60, 15);
    }

    private View CreateHeaderNav()
    {
        return new FlexGrowContainer(
            new global::Xui.Core.UI.Layout.FlexBox
            {
                FlexDirection = Direction.Row,
                FlexJustifyContent = JustifyContent.Center,
                FlexAlignItems = AlignItems.Center,
                ColumnGap = 15,
                Content =
                [
                    Box("Dashboard", palette[1], 90, 35),
                    Box("Products", palette[2], 85, 35),
                    Box("Analytics", palette[3], 85, 35),
                    Box("Settings", palette[4], 80, 35),
                ]
            },
            1
        );
    }

    private View CreateHeaderActions()
    {
        return new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.FlexEnd,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 10,
            Content =
            [
                Box("🔔", palette[5], 40, 40),
                Box("👤", palette[6], 40, 40),
            ]
        };
    }

    private View CreateMainContent()
    {
        var mainFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            ColumnGap = 0,
            Content =
            [
                CreateSidebar(),
                CreateContentArea(),
            ]
        };
        return new FlexGrowContainer(mainFlex, 1);
    }

    private View CreateSidebar()
    {
        var sidebarItems = new View[8];
        for (int i = 0; i < 8; i++)
        {
            sidebarItems[i] = Box($"Item {i + 1}", i % 2 == 0 ? palette[7] : palette[8], 0, 40);
        }

        var sidebarFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            RowGap = 2,
            Content = sidebarItems
        };

        return new PaddedContainer(sidebarFlex, 200, 15);
    }

    private View CreateContentArea()
    {
        var contentGrid = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexWrap = Wrap.Wrap,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.FlexStart,
            FlexAlignContent = AlignContent.FlexStart,
            ColumnGap = 15,
            RowGap = 15,
            Content =
            [
                CreateCard("Card 1", palette[0]),
                CreateCard("Card 2", palette[1]),
                CreateCard("Card 3", palette[2]),
                CreateCard("Card 4", palette[3]),
                CreateCard("Card 5", palette[4]),
                CreateCard("Card 6", palette[5]),
            ]
        };

        return new FlexGrowContainer(new PaddedContainer(contentGrid, 0, 20), 1);
    }

    private View CreateCard(string title, Color color)
    {
        var cardFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Column,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Stretch,
            RowGap = 8,
            Content =
            [
                Box(title, color, 0, 30),
                Box("Content", palette[9], 0, 80),
                Box("Action", palette[6], 0, 25),
            ]
        };

        return new FixedSizeContainer(cardFlex, 180, 150);
    }

    private View CreateFooter()
    {
        var footerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 20,
            Content =
            [
                Box("© 2024 Company", palette[7], 150, 35),
                CreateFooterLinks(),
            ]
        };
        return new PaddedContainer(footerFlex, 60, 15);
    }

    private View CreateFooterLinks()
    {
        return new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = Direction.Row,
            FlexJustifyContent = JustifyContent.FlexEnd,
            FlexAlignItems = AlignItems.Center,
            ColumnGap = 15,
            Content =
            [
                Box("Privacy", palette[8], 70, 30),
                Box("Terms", palette[9], 70, 30),
                Box("Contact", palette[0], 70, 30),
            ]
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

    private class FlexGrowContainer : View
    {
        private readonly View child;
        private readonly nfloat flexGrow;

        public override int Count => 1;
        public override View this[int index] => child;

        public FlexGrowContainer(View child, nfloat flexGrow)
        {
            this.child = child;
            this.flexGrow = flexGrow;
            AddProtectedChild(child);
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            child.Measure(availableSize, context);
            return child.DesiredSize;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            child.Arrange(rect, context);
        }
    }

    private class PaddedContainer : View
    {
        private readonly View child;
        private readonly nfloat width;
        private readonly nfloat padding;

        public override int Count => 1;
        public override View this[int index] => child;

        public PaddedContainer(View child, nfloat width, nfloat padding)
        {
            this.child = child;
            this.width = width;
            this.padding = padding;
            AddProtectedChild(child);
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            var innerWidth = width > 0 ? width - padding * 2 : availableSize.Width - padding * 2;
            child.Measure(new Size(innerWidth, availableSize.Height - padding * 2), context);
            var desiredWidth = width > 0 ? width : availableSize.Width;
            return new Size(desiredWidth, child.DesiredSize.Height + padding * 2);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            child.Arrange(new Rect(rect.X + padding, rect.Y + padding, 
                rect.Width - padding * 2, rect.Height - padding * 2), context);
        }
    }

    private class FixedSizeContainer : View
    {
        private readonly View child;
        private readonly nfloat width;
        private readonly nfloat height;

        public override int Count => 1;
        public override View this[int index] => child;

        public FixedSizeContainer(View child, nfloat width, nfloat height)
        {
            this.child = child;
            this.width = width;
            this.height = height;
            AddProtectedChild(child);
        }

        protected override Size MeasureCore(Size availableSize, IMeasureContext context)
        {
            child.Measure(new Size(width, height), context);
            return new Size(width, height);
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            child.Arrange(new Rect(rect.X, rect.Y, width, height), context);
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
            context.SetFont(["Inter"], 11, FontWeight.Normal, FontSlant.Normal);
            var textWidth = Text.Length * 5.5;
            context.FillText(Text, Frame.X + Frame.Width / 2 - textWidth / 2, Frame.Y + Frame.Height / 2 + 4);
        }
    }
}
