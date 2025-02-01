using System.Collections.Generic;
using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Runtime.Windows.Actual;

public class Win32Platform : IRuntime
{
    public static readonly Win32Platform Instance = new Win32Platform();

    private Win32RunLoop? win32RunLoop;

    private Win32Platform()
    {
    }
    
    public IContext DrawingContext => DisplayContextStack.Peek();

    public IDispatcher MainDispatcher => win32RunLoop!;

    // NOTE: This will have to be thread static, if we want to render in multiple threads.
    public static Stack<IContext> DisplayContextStack { get; } = new Stack<IContext>();

    public IRunLoop CreateRunloop(Xui.Core.Abstract.Application applicationAbstract) => this.win32RunLoop = new Win32RunLoop(applicationAbstract);

    public Xui.Core.Actual.IWindow CreateWindow(Xui.Core.Abstract.IWindow windowAbstract) => new Win32Window(windowAbstract);
}