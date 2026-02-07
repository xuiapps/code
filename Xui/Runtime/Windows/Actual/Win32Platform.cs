using System.Collections.Generic;
using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Runtime.Windows.Actual;

public class Win32Platform : IRuntime
{
    public static readonly Win32Platform Instance = new Win32Platform();

    private Win32RunLoop? win32RunLoop;
    private readonly List<Win32Window> windows = new();

    private Win32Platform()
    {
    }

    internal IReadOnlyList<Win32Window> Windows => this.windows;
    
    public IContext DrawingContext => DisplayContextStack.Peek();

    public IDispatcher MainDispatcher => win32RunLoop!;

    // NOTE: This will have to be thread static, if we want to render in multiple threads.
    public static Stack<IContext> DisplayContextStack { get; } = new Stack<IContext>();

    public IRunLoop CreateRunloop(Xui.Core.Abstract.Application applicationAbstract) => this.win32RunLoop = new Win32RunLoop(applicationAbstract);

    public Xui.Core.Actual.IWindow CreateWindow(Xui.Core.Abstract.IWindow windowAbstract)
    {
        var window = new Win32Window(windowAbstract);
        this.windows.Add(window);
        return window;
    }

    internal void RemoveWindow(Win32Window window)
    {
        this.windows.Remove(window);
    }
}