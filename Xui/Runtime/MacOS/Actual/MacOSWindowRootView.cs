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
        .Register();
    
    public static void DrawRect(nint self, nint sel, NSRect rect) =>
        Marshalling.Get<MacOSWindowRootView>(self).DrawRect(rect);

    private readonly MacOSWindow window;
    
    public MacOSWindowRootView(MacOSWindow window) : base(Class.New())
    {
        this.window = window;
        this.Flipped = true;
    }

    private void DrawRect(NSRect rect) => this.window.Render(rect);
}