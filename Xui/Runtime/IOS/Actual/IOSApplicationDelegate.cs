using System;
using Xui.Core.Abstract;
using Xui.Runtime.IOS;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS.Actual;

internal class IOSApplicationDelegate : NSObject
{
    [ThreadStatic]
    public static Application? ApplicationOnStack;

    public static unsafe new readonly Class Class = NSObject.Class
        .Extend("XUIIOSApplicationDelegate")
        .AddProtocol(Xui.Runtime.IOS.UIKit.UIApplicationDelegate)
        .AddMethod("init", Init)
        .AddMethod("application:willFinishLaunchingWithOptions:", ApplicationWillFinishLaunchingWithOptions)
        .AddMethod("application:didFinishLaunchingWithOptions:", ApplicationDidFinishLaunchingWithOptions)
        .Register();

    protected Xui.Core.Abstract.Application Application { get; }

    public IOSApplicationDelegate(nint self) : base(self)
    {
        this.Application = ApplicationOnStack!;
    }

    public IOSApplicationDelegate() : base(Class.New())
    {
        this.Application = ApplicationOnStack!;
    }

    public IOSApplicationDelegate(Xui.Core.Abstract.Application application) : base(Class.New())
    {
        this.Application = application;
    }

    public static nint Init(nint self, nint sel)
    {
        // TRICKY: Entry from UIApplicationMain

        new IOSApplicationDelegate(self);
        Super super = new Super(self, NSObject.Class);
        ObjC.objc_msgSendSuper(ref super, sel);
        return self;
    }

    public static bool ApplicationWillFinishLaunchingWithOptions(nint self, nint sel, nint app, nint options) =>
        Marshalling.Get<IOSApplicationDelegate>(self)
            .ApplicationWillFinishLaunchingWithOptions();

    public static bool ApplicationDidFinishLaunchingWithOptions(nint self, nint sel, nint app, nint options) =>
        Marshalling.Get<IOSApplicationDelegate>(self)
            .ApplicationDidFinishLaunchingWithOptions();

    private bool ApplicationWillFinishLaunchingWithOptions()
    {
        return true;
    }

    private bool ApplicationDidFinishLaunchingWithOptions()
    {
        this.Application.Start();
        return true;
    }
}
