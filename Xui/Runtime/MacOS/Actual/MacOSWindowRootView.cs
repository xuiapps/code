using System.Runtime.InteropServices;
using Xui.Runtime.MacOS;
using static Xui.Runtime.MacOS.AppKit;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS.Actual;

public class MacOSWindowRootView : NSView
{
    protected static unsafe new readonly Class Class = NSView.Class
        .Extend("XUIMacOSWindowRootView")
        .AddMethod("drawRect:", DrawRect)
        .AddMethod("cornerConfiguration", CornerConfiguration)
        .Register();
    
    public static void DrawRect(nint self, nint sel, NSRect rect) =>
        Marshalling.Get<MacOSWindowRootView>(self).DrawRect(rect);

    public static nint CornerConfiguration(nint self, nint sel) =>
        Marshalling.Get<MacOSWindowRootView>(self).GetCornerConfiguration();

    private readonly MacOSWindow window;
    
    public MacOSWindowRootView(MacOSWindow window) : base(Class.New())
    {
        this.window = window;
        this.Flipped = true;
    }

    private void DrawRect(NSRect rect) => this.window.Render(rect);

    private nint GetCornerConfiguration()
    {
        if (!this.window.UsesGlassCornerConfiguration || !NSViewCornerConfiguration.IsAvailable)
            return 0;

        return NSViewCornerConfiguration.CreateContainerConcentric(12);
    }
}
