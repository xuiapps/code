using Xui.Core.Canvas;

namespace Xui.Runtime.IOS.Actual;

public class IOSPlatform : Xui.Core.Actual.IRuntime
{
    public static readonly IOSPlatform Instance = new IOSPlatform();

    private IOSDrawingContext iOSDrawingContext = new IOSDrawingContext();
    private IOSRunLoop? iOSRunLoop;

    private IOSPlatform()
    {
    }
    
    public IContext DrawingContext => iOSDrawingContext.Bind();

    public Xui.Core.Actual.IDispatcher MainDispatcher => iOSRunLoop!;

    public Xui.Core.Actual.IRunLoop CreateRunloop(Xui.Core.Abstract.Application applicationAbstract) => this.iOSRunLoop = new IOSRunLoop(applicationAbstract);

    public Xui.Core.Actual.IWindow CreateWindow(Xui.Core.Abstract.IWindow windowAbstract) => new IOSWindow(windowAbstract);
}