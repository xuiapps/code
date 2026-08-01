using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private class TransparentStyle : Style
    {
        public static readonly TransparentStyle Instance = new();

        protected virtual NSWindowToolbarStyle? ToolbarStyle => null;

        protected TransparentStyle()
        {
        }

        public override void ConfigureInitialWindow(
            Xui.Core.Abstract.IWindow @abstract,
            ref NSWindowStyleMask mask,
            ref Rect rect)
        {
            mask =
                NSWindowStyleMask.Titled |
                NSWindowStyleMask.Closable |
                NSWindowStyleMask.Miniaturizable |
                NSWindowStyleMask.Resizable |
                NSWindowStyleMask.FullSizeContentView;

            if (@abstract.GetDesktopStyle() is { StartupSize: { } startupSize })
                rect.Size = startupSize;
        }

        public override void ConfigureContentView(MacOSWindow window, Foundation.NSRect initialContentFrame)
        {
            using var transparent = new NSColorRef(0, 0, 0, 0);
            window.BackgroundColor = transparent;
            window.Opaque = false;
            window.StyleMask |= NSWindowStyleMask.FullSizeContentView;
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
            window.TitlebarAppearsTransparent = true;
            window.ContentView = window.rootView;

            if (this.ToolbarStyle is not { } toolbarStyle)
                return;

            window.Toolbar = new NSToolbar
            {
                ShowsBaselineSeparator = false,
                Visible = true,
            };
            window.ToolbarStyle = toolbarStyle;
        }

        public override Rect GetLayoutArea(MacOSWindow window, Rect rootArea) => rootArea;

        public override bool ShouldSoftwareTitleBarZoom(MacOSWindow window) => true;

    }
}
