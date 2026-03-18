using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

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
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
        new Color(0xF5, 0xA6, 0x23, 0xFF), // Yellow
        new Color(0x9B, 0x59, 0xB6, 0xFF), // Purple
        new Color(0xF3, 0x9C, 0x12, 0xFF), // Orange
        new Color(0x1A, 0xBC, 0x9C, 0xFF), // Cyan
        new Color(0xE7, 0x4C, 0x3C, 0xFF), // Pink
        new Color(0x34, 0x98, 0xDB, 0xFF), // LightBlue
        new Color(0x2E, 0xCC, 0x71, 0xFF), // LightGreen
    ];

    public override int Count => 1;
    public override View this[int index] => rootFlex;

    public ResponsiveLayoutTest()
    {
        rootFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                CreateHeader(),
                CreateMainContent(),
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
        context.SetFill(new Color(0xF5, 0xF5, 0xF5, 0xFF));
        context.FillRect(Frame);
        base.RenderCore(context);
    }

    private View CreateHeader()
    {
        var headerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Center,
            Content =
            [
                Box("Logo", palette[0], 100, 50),
                CreateHeaderNav(),
                Box("User", palette[6], 50, 50),
            ]
        };
        return new FixedHeightContainer(headerFlex, 60);
    }

    private View CreateHeaderNav()
    {
        var navFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.Center,
            FlexAlignItems = AlignItems.Center,
            Content =
            [
                Box("Dashboard", palette[1], 90, 35),
                Box("Products", palette[2], 85, 35),
                Box("Analytics", palette[3], 85, 35),
                Box("Settings", palette[4], 80, 35),
            ]
        };
        navFlex[Grow] = 1;
        return navFlex;
    }

    private View CreateMainContent()
    {
        var mainFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                CreateSidebar(),
                CreateContentArea(),
            ]
        };
        mainFlex[Grow] = 1;
        return mainFlex;
    }

    private View CreateSidebar()
    {
        var sidebarFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                Box("Item 1", palette[7], 200, 40),
                Box("Item 2", palette[8], 200, 40),
                Box("Item 3", palette[7], 200, 40),
                Box("Item 4", palette[8], 200, 40),
                Box("Item 5", palette[7], 200, 40),
            ]
        };
        return sidebarFlex;
    }

    private View CreateContentArea()
    {
        var contentGrid = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexWrap = Wrap.Wrap,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.FlexStart,
            FlexAlignContent = AlignContent.FlexStart,
            Content =
            [
                Box("Card 1", palette[0], 180, 120),
                Box("Card 2", palette[1], 180, 120),
                Box("Card 3", palette[2], 180, 120),
                Box("Card 4", palette[3], 180, 120),
                Box("Card 5", palette[4], 180, 120),
                Box("Card 6", palette[5], 180, 120),
            ]
        };
        contentGrid[Grow] = 1;
        return contentGrid;
    }

    private View CreateFooter()
    {
        var footerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Center,
            Content =
            [
                Box("© 2024 Company", palette[7], 150, 35),
                CreateFooterLinks(),
            ]
        };
        return new FixedHeightContainer(footerFlex, 60);
    }

    private View CreateFooterLinks()
    {
        return new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.FlexEnd,
            FlexAlignItems = AlignItems.Center,
            Content =
            [
                Box("Privacy", palette[8], 70, 30),
                Box("Terms", palette[9], 70, 30),
                Box("Contact", palette[0], 70, 30),
            ]
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

    private class FixedHeightContainer : View
    {
        private readonly View child;
        private readonly NFloat height;

        public override int Count => 1;
        public override View this[int index] => child;

        public FixedHeightContainer(View child, NFloat height)
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
            var size = new Size(Width > 0 ? Width : availableSize.Width, Height > 0 ? Height : availableSize.Height);
            _content?.Measure(size, context);
            return size;
        }

        protected override void ArrangeCore(Rect rect, IMeasureContext context)
        {
            _content?.Arrange(rect, context);
        }
    }
}
