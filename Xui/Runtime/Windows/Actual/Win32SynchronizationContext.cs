using System;
using System.Diagnostics;
using System.Threading;

namespace Xui.Runtime.Windows.Actual;

public class Win32SynchronizationContext : SynchronizationContext
{
    private int operations;

    private Win32RunLoop win32RunLoop;

    public Win32SynchronizationContext(Win32RunLoop win32RunLoop)
    {
        this.win32RunLoop = win32RunLoop;
    }

    public override void OperationStarted()
    {
        var ops = Interlocked.Increment(ref this.operations);
        Debug.WriteLine($"SynchronizationContext OperationStarted... ({ops} running)");
        base.OperationStarted();
    }

    public override void OperationCompleted()
    {
        var ops = Interlocked.Decrement(ref this.operations);
        Debug.WriteLine($"SynchronizationContext OperationCompleted. ({ops} running)");
        base.OperationCompleted();
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        Debug.WriteLine($"SynchronizationContext Post {d} {state}");
        base.Post(d, state);
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        Debug.WriteLine($"SynchronizationContext Send {d} {state}");
        base.Send(d, state);
    }

    public override int Wait(nint[] waitHandles, bool waitAll, int millisecondsTimeout)
    {
        Debug.WriteLine($"SynchronizationContext Wait");
        return base.Wait(waitHandles, waitAll, millisecondsTimeout);
    }

    public override System.Threading.SynchronizationContext CreateCopy()
    {
        throw new NotSupportedException();
        // return base.CreateCopy();
    }
}
