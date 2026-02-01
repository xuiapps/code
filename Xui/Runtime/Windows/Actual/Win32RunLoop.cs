using System;
using System.Threading;
using Xui.Core.Actual;
using Xui.Core.Debug;
using Xui.Runtime.Windows.Win32;
using static Xui.Runtime.Windows.Win32.User32;

namespace Xui.Runtime.Windows.Actual;

public class Win32RunLoop : IRunLoop, IDispatcher
{
    public const uint WM_DISPATCH_POST_MSG = 0x0400;

    private SynchronizationContext synchronizationContext;

    protected Xui.Core.Abstract.Application Application { get; }

    public IRunLoopInstruments? Instruments { get; }

    public Win32RunLoop(Xui.Core.Abstract.Application application, IRunLoopInstruments? instruments)
    {
        this.synchronizationContext = new Win32SynchronizationContext(this);
        SynchronizationContext.SetSynchronizationContext(this.synchronizationContext);
        this.Application = application;
        this.Instruments = instruments;
    }

    public void Post(Action callback)
    {
        throw new NotImplementedException();
    }

    public int Run()
    {
        Xui.Core.Actual.Runtime.CurrentRunLoop = this;
        using var runTrace = this.Instruments.NullableTrace(LevelOfDetail.Essential, Aspect.ApplicationOS, $"Win32RunLoop.Run()");
        Application.Start();

        if (Environment.OSVersion.Version.Major <= 10)
        {
            nuint timerId = User32.SetTimer(0, 0, 16, 0);

            MSG msg = new MSG();
            do
            {
                while(GetMessage(ref msg, 0, 0, 0))
                {
                    if (msg.message == WindowMessage.WM_TIMER)
                    {
                        if (msg.wParam == timerId)
                        {
                            foreach(var w in Xui.Core.Abstract.Window.OpenWindows)
                            {
                                if (w.Actual is Win32Window win32window)
                                {
                                    win32window.OnCompositionFrame();
                                }
                            }
                        }
                    }
                    else
                    {
                        using var msgTrace = this.Instruments.NullableTrace(LevelOfDetail.Diagnostic, Aspect.ApplicationOS, $"Message {msg.message}");
                        var transalted = TranslateMessage(ref msg);
                        nint result = DispatchMessage(ref msg);
                    }
                }
            }
            while((uint)msg.message != (uint)WindowMessage.WM_QUIT);
        }
        else
        {
            MSG msg = new MSG();
            do
            {
                while (PeekMessage(ref msg, 0, 0, 0, 1) && (uint)msg.message != (uint)WindowMessage.WM_QUIT)
                {
                    using var msgTrace = this.Instruments.NullableTrace(LevelOfDetail.Diagnostic, Aspect.ApplicationOS, $"Message {msg.message}");
                    var transalted = TranslateMessage(ref msg);
                    nint result = DispatchMessage(ref msg);
                }

                DComp.DCompositionWaitForCompositorClock(0, 0, 16);

                foreach(var w in Xui.Core.Abstract.Window.OpenWindows)
                {
                    if (w.Actual is Win32Window win32window)
                    {
                        win32window.OnCompositionFrame();
                    }
                }
            }
            while((uint)msg.message != (uint)WindowMessage.WM_QUIT);
        }

        return 0;
    }
}
