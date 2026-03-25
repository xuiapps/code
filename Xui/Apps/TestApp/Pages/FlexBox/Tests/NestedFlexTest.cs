using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Core.UI.Layout;
using static Xui.Core.Canvas.Colors;
using static Xui.Core.UI.Layout.FlexBox;

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
        new Color(0x4A, 0x90, 0xD9, 0xFF), // Blue
        new Color(0x5C, 0xC8, 0x5A, 0xFF), // Green
        new Color(0xE8, 0x5D, 0x5D, 0xFF), // Red
        new Color(0xF5, 0xA6, 0x23, 0xFF), // Yellow
        new Color(0x9B, 0x59, 0xB6, 0xFF), // Purple
        new Color(0xF3, 0x9C, 0x12, 0xFF), // Orange
        new Color(0x1A, 0xBC, 0x9C, 0xFF), // Cyan
        new Color(0xE7, 0x4C, 0x3C, 0xFF), // Pink
    ];

    public override int Count => 1;
    public override View this[int index] => outerFlex;

    public NestedFlexTest()
    {
        // Outer vertical container
        outerFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
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
                Box("Logo", palette[0], 80, 40),
                CreateNavigation(),
                Box("User", palette[1], 60, 40),
            ]
        };
        return new FixedHeightContainer(headerFlex, 40);
    }

    private View CreateNavigation()
    {
        var navFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.Center,
            FlexAlignItems = AlignItems.Center,
            Content =
            [
                Box("Home", palette[2], 60, 30),
                Box("About", palette[3], 60, 30),
                Box("Contact", palette[4], 60, 30),
            ]
        };
        navFlex[Grow] = 1;
        return navFlex;
    }

    private View CreateContent()
    {
        var contentFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Row,
            FlexJustifyContent = JustifyContent.SpaceBetween,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                CreateColumn("Left", palette[5]),
                CreateColumn("Center", palette[6]),
                CreateColumn("Right", palette[7]),
            ]
        };
        contentFlex[Grow] = 1;
        return contentFlex;
    }

    private View CreateColumn(string title, Color color)
    {
        var columnFlex = new global::Xui.Core.UI.Layout.FlexBox
        {
            FlexDirection = global::Xui.Core.UI.Layout.FlexBox.Direction.Column,
            FlexJustifyContent = JustifyContent.FlexStart,
            FlexAlignItems = AlignItems.Stretch,
            Content =
            [
                Box(title, color, 0, 30),
                Box("Item 1", color, 0, 35),
                Box("Item 2", color, 0, 35),
                Box("Item 3", color, 0, 35),
            ]
        };
        columnFlex[Grow] = 1;
        return columnFlex;
    }

    private View CreateFooter()
    {
        return Box("Footer Area", palette[0], 0, 50);
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
