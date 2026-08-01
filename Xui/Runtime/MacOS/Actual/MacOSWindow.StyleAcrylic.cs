using System;
using Xui.Core.Abstract;
using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class AcrylicStyle : Style
    {
        private static readonly AcrylicStyle titled = new(MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Default);
        private static readonly AcrylicStyle titledCompact = new(MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Medium);
        private static readonly AcrylicStyle titledUnified = new(MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Large);
        private static readonly AcrylicStyle untitled = new(MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Default);
        private static readonly AcrylicStyle untitledCompact = new(MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Medium);
        private static readonly AcrylicStyle untitledUnified = new(MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Large);

        private readonly MacOSWindowTitle title;
        private readonly MacOSWindowTitleHeight titleHeight;

        private AcrylicStyle(MacOSWindowTitle title, MacOSWindowTitleHeight titleHeight)
        {
            this.title = title;
            this.titleHeight = titleHeight;
        }

        public static AcrylicStyle Get(MacOSWindowTitle title, MacOSWindowTitleHeight titleHeight) => (title, titleHeight) switch
        {
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Default) => titled,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Medium) => titledCompact,
            (MacOSWindowTitle.Visible, MacOSWindowTitleHeight.Large) => titledUnified,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Default) => untitled,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Medium) => untitledCompact,
            (MacOSWindowTitle.Hidden, MacOSWindowTitleHeight.Large) => untitledUnified,
            _ => throw new ArgumentOutOfRangeException(nameof(titleHeight)),
        };

        private bool IsUntitled => this.title == MacOSWindowTitle.Hidden;

        public override void ConfigureInitialWindow(
            Xui.Core.Abstract.IWindow @abstract,
            ref NSWindowStyleMask mask,
            ref Rect rect)
        {
            if (this.IsUntitled)
                mask |= NSWindowStyleMask.FullSizeContentView;

            if (@abstract.GetDesktopStyle() is { StartupSize: { } startupSize })
                rect.Size = startupSize;
        }

        public override void ConfigureContentView(MacOSWindow window, Foundation.NSRect initialContentFrame)
        {
            using var transparent = new NSColorRef(0, 0, 0, 0);
            window.BackgroundColor = transparent;
            window.Opaque = false;

            var visualEffectView = new NSVisualEffectView
            {
                Material = NSVisualEffectMaterial.UnderWindowBackground,
                BlendingMode = NSVisualEffectBlendingMode.BehindWindow,
                State = NSVisualEffectState.Active,
            };
            NSView.SetFrame(visualEffectView, initialContentFrame);
            visualEffectView.AutoresizingMask =
                NSAutoresizingMaskOptions.WidthSizable |
                NSAutoresizingMaskOptions.HeightSizable;
            visualEffectView.AddSubview(window.rootView);
            window.ContentView = visualEffectView;
            if (this.IsUntitled)
            {
                window.StyleMask |= NSWindowStyleMask.FullSizeContentView;
                window.TitleVisibility = NSWindowTitleVisibility.Hidden;
                window.TitlebarAppearsTransparent = true;
            }

            if (this.titleHeight is MacOSWindowTitleHeight.Medium or MacOSWindowTitleHeight.Large)
            {
                window.Toolbar = new NSToolbar
                {
                    ShowsBaselineSeparator = false,
                    Visible = true,
                };
                window.ToolbarStyle = this.titleHeight == MacOSWindowTitleHeight.Medium
                    ? NSWindowToolbarStyle.UnifiedCompact
                    : NSWindowToolbarStyle.Unified;
            }
        }

        public override Rect GetLayoutArea(MacOSWindow window, Rect rootArea)
        {
            if (this.IsUntitled)
                return rootArea;

            var nativeLayout = window.ContentLayoutRect;
            var rootFrame = window.rootView.Frame;
            return new Rect(
                nativeLayout.Origin.x,
                rootFrame.Size.height - nativeLayout.Origin.y - nativeLayout.Size.height,
                nativeLayout.Size.width,
                nativeLayout.Size.height);
        }

        public override bool ShouldSoftwareTitleBarZoom(MacOSWindow window) => this.IsUntitled;

    }
}
