using Xui.Core.Math2D;
using Xui.Runtime.MacOS;
using static Xui.Runtime.MacOS.Foundation;
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
            .AddMethod("windowWillExitFullScreen:", WindowWillExitFullScreen)
            .AddMethod("windowDidExitFullScreen:", WindowDidExitFullScreen)
            .AddMethod("windowDidResize:", WindowDidResize)
            .Register();

    protected static bool WindowShouldClose(nint self, nint sel, nint window) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowShouldClose();

    private bool WindowShouldClose()
        => this.window.Closing();

    protected static void WindowWillEnterFullScreen(nint self, nint sel, nint notification)
    {
    }

    protected static void WindowWillExitFullScreen(nint self, nint sel, nint notification) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowWillExitFullScreen();

    private void WindowWillExitFullScreen()
    {
        this.window.IsExitingFullScreen = true;
        this.window.PositionSystemButtons();
    }

    protected static void WindowDidExitFullScreen(nint self, nint sel, nint notification) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowDidExitFullScreen();

    private void WindowDidExitFullScreen()
    {
        this.window.IsExitingFullScreen = false;
        this.window.PositionSystemButtons();
    }

    protected static void WindowDidResize(nint self, nint sel, nint notification) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowDidResize();

    private void WindowDidResize()
    {
        var contentFrame = this.window.ContentView!.Frame;
        var area = new Rect(0, 0, contentFrame.Size.width, contentFrame.Size.height);
        this.window.Abstract.DisplayArea = area;
        this.window.Abstract.SafeArea = area;
        this.window.PositionSystemButtons();
        this.window.Invalidate();
    }

    protected readonly MacOSWindow window;

    public MacOSWindowDelegate(MacOSWindow window) : base(Class.New())
    {
        this.window = window;
    }
}
