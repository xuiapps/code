using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private class TitledUnifiedCompactStyle : Style
    {
        public static readonly TitledUnifiedCompactStyle Instance = new();

        protected TitledUnifiedCompactStyle()
        {
        }

        protected virtual NSWindowToolbarStyle ToolbarStyle => NSWindowToolbarStyle.UnifiedCompact;

        public override void ConfigureInitialWindow(
            Xui.Core.Abstract.IWindow @abstract,
            ref NSWindowStyleMask mask,
            ref Rect rect)
        {
            if (@abstract.GetDesktopStyle() is { StartupSize: { } startupSize })
                rect.Size = startupSize;
        }

        public override void ConfigureContentView(MacOSWindow window, Foundation.NSRect initialContentFrame)
        {
            window.ContentView = window.rootView;
            window.Toolbar = new NSToolbar
            {
                ShowsBaselineSeparator = false,
                Visible = true,
            };
            window.ToolbarStyle = this.ToolbarStyle;
        }

        public override Rect GetLayoutArea(MacOSWindow window, Rect rootArea)
        {
            var nativeLayout = window.ContentLayoutRect;
            var rootFrame = window.rootView.Frame;
            return new Rect(
                nativeLayout.Origin.x,
                rootFrame.Size.height - nativeLayout.Origin.y - nativeLayout.Size.height,
                nativeLayout.Size.width,
                nativeLayout.Size.height);
        }

        public override bool ShouldSoftwareTitleBarZoom(MacOSWindow window) => false;

    }
}
