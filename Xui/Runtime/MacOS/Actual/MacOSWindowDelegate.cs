using Xui.Runtime.MacOS;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS.Actual;

public class MacOSWindowDelegate : NSObject
{
    public static unsafe new readonly Class Class =
        NSObject.Class
            .Extend("XUIMacOSWindowDelegate")
            .AddProtocol(new Protocol(AppKit.Lib, "NSWindowDelegate"))
            .AddMethod("windowShouldClose:", WindowShouldClose)
            .AddMethod("windowWillEnterFullScreen:", WindowWillEnterFullScreen)
            .AddMethod("windowDidExitFullScreen:", WindowDidExitFullScreen)
            .Register();

    protected static bool WindowShouldClose(nint self, nint sel, nint window) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowShouldClose();

    private bool WindowShouldClose()
        => this.window.Closing();

    protected static void WindowWillEnterFullScreen(nint self, nint sel, nint notification)
    {
    }

    protected static void WindowDidExitFullScreen(nint self, nint sel, nint notification)
    {
    }

    protected readonly MacOSWindow window;

    public MacOSWindowDelegate(MacOSWindow window) : base(Class.New())
    {
        this.window = window;
    }
}