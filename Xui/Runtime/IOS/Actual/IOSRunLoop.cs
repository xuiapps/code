using System;
using System.Threading;
using Xui.Core.Actual;
using Xui.Core.Debug;
using static Xui.Runtime.IOS.CoreFoundation;

namespace Xui.Runtime.IOS.Actual;

public class IOSRunLoop : IRunLoop, IDispatcher
{
    private SynchronizationContext synchronizationContext;

    protected Xui.Core.Abstract.Application Application { get; }

    public IRunLoopInstruments? Instruments { get; }

    public IOSRunLoop(Xui.Core.Abstract.Application application, IRunLoopInstruments? instruments)
    {
        this.synchronizationContext = new IOSSynchronizationContext(this);
        SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);
        this.Application = application;
        this.Instruments = instruments;
    }

    public void Post(Action callback)
    {
        throw new NotImplementedException();
    }

    public static bool ApplicationDidFinishLaunchingWithOptions(nint self, nint sel, nint app, nint opt)
    {
        return true;
    }

    public int Run()
    {
        Xui.Core.Actual.Runtime.CurrentRunLoop = this;
        IOSApplicationDelegate.ApplicationOnStack = this.Application;
        using var nsAppDelegateName = new CFStringRef(IOSApplicationDelegate.Class.Name);
        return Xui.Runtime.IOS.UIKit.UIApplicationMain(0, 0, 0, nsAppDelegateName);
    }
}