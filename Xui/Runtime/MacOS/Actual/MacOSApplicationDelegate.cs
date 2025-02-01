using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS.Actual;

internal class MacOSApplicationDelegate : NSObject
{
    public static unsafe new readonly Class Class = NSObject.Class
        .Extend("XUIMacOSApplicationDelegate")
        .AddProtocol(AppKit.NSApplicationDelegate)
        .AddMethod("applicationShouldTerminate:", ApplicationShouldTerminate)
        .AddMethod("applicationWillFinishLaunching:", ApplicationWillFinishLaunching)
        .AddMethod("applicationDidFinishLaunching:", ApplicationDidFinishLaunching)
        .Register();

    protected Xui.Core.Abstract.Application Application { get; }

    public MacOSApplicationDelegate(Xui.Core.Abstract.Application application) : base(Class.New())
    {
        this.Application = application;
    }

    public static bool ApplicationShouldTerminate(nint self, nint sel, nint sender)
    {
        return true;
    }

    public static void ApplicationWillFinishLaunching(nint self, nint sel) =>
        Marshalling.Get<MacOSApplicationDelegate>(self)
            .ApplicationWillFinishLaunching();

    public static void ApplicationDidFinishLaunching(nint self, nint sel) =>
        Marshalling.Get<MacOSApplicationDelegate>(self)
            .ApplicationDidFinishLaunching();

    private void ApplicationWillFinishLaunching() =>
        this.Application.Start();

    private void ApplicationDidFinishLaunching() {}
}
