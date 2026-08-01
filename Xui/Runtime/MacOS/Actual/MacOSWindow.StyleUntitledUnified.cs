using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private class UntitledUnifiedStyle : Style
    {
        public static readonly UntitledUnifiedStyle Instance = new();
        protected virtual NSWindowToolbarStyle ToolbarStyle => NSWindowToolbarStyle.Unified;
        protected UntitledUnifiedStyle() { }

        public override void ConfigureInitialWindow(Xui.Core.Abstract.IWindow @abstract, ref NSWindowStyleMask mask, ref Rect rect)
        {
            mask |= NSWindowStyleMask.FullSizeContentView;
            if (@abstract.GetDesktopStyle() is { StartupSize: { } startupSize })
                rect.Size = startupSize;
        }

        public override void ConfigureContentView(MacOSWindow window, Foundation.NSRect initialContentFrame)
        {
            window.StyleMask |= NSWindowStyleMask.FullSizeContentView;
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
            window.TitlebarAppearsTransparent = true;
            window.ContentView = window.rootView;
            window.Toolbar = new NSToolbar { ShowsBaselineSeparator = false, Visible = true };
            window.ToolbarStyle = this.ToolbarStyle;
        }

        public override Rect GetLayoutArea(MacOSWindow window, Rect rootArea) => rootArea;
        public override bool ShouldSoftwareTitleBarZoom(MacOSWindow window) => true;
    }
}
