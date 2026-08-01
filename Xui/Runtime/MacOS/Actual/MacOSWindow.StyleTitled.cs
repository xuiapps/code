using Xui.Core.Math2D;
using Xui.Core.Middleware;
using Foundation = Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class TitledStyle : Style
    {
        public static readonly TitledStyle Instance = new();

        private TitledStyle()
        {
        }

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
