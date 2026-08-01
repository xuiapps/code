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
            .AddMethod("window:willUseFullScreenPresentationOptions:", WindowWillUseFullScreenPresentationOptions)
            .AddMethod("windowWillEnterFullScreen:", WindowWillEnterFullScreen)
            .AddMethod("windowDidResize:", WindowDidResize)
            .Register();

    protected static bool WindowShouldClose(nint self, nint sel, nint window) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowShouldClose();

    private bool WindowShouldClose()
        => this.window.Closing();

    protected static nuint WindowWillUseFullScreenPresentationOptions(nint self, nint sel, nint window, nuint proposedOptions) =>
        proposedOptions | (nuint)AppKit.NSApplicationPresentationOptions.AutoHideToolbar;

    protected static void WindowWillEnterFullScreen(nint self, nint sel, nint notification)
    {
    }

    protected static void WindowDidResize(nint self, nint sel, nint notification) =>
        Marshalling.Get<MacOSWindowDelegate>(self).WindowDidResize();

    private void WindowDidResize()
    {
        this.window.OnWindowDidResize();
        var area = this.window.LayoutArea;
        this.window.Abstract.DisplayArea = area;
        this.window.Abstract.SafeArea = area;
        this.window.Invalidate();
    }

    protected readonly MacOSWindow window;

    public MacOSWindowDelegate(MacOSWindow window) : base(Class.New())
    {
        this.window = window;
    }
}
