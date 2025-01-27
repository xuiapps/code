using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Runtime.MacOS.Actual;

public class MacOSPlatform : Xui.Core.Actual.IRuntime
{
    public static readonly MacOSPlatform Instance = new MacOSPlatform();

    private MacOSDrawingContext macOSDrawingContext = new MacOSDrawingContext();
    private MacOSRunLoop? macOSRunLoop;

    private MacOSPlatform()
    {
    }

    public IContext DrawingContext => macOSDrawingContext.Bind();

    public IDispatcher MainDispatcher => macOSRunLoop!;

    public IRunLoop CreateRunloop(Xui.Core.Abstract.Application application) => this.macOSRunLoop = new MacOSRunLoop(application);

    public IWindow CreateWindow(Xui.Core.Abstract.IWindow window) => new MacOSWindow(window);
}
