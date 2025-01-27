using Xui.Core.Abstract;
using Xui.Core.Actual;
using Xui.Core.Canvas;
using Xui.Emulator.Actual;

namespace Xui.Middleware.Emulator.Actual;

public class EmulatorPlatform : IRuntime
{
    private IRuntime BasePlatform;

    public IContext DrawingContext => this.BasePlatform.DrawingContext;

    public IDispatcher MainDispatcher => this.BasePlatform.MainDispatcher;

    public EmulatorPlatform(IRuntime basePlatform)
    {
        this.BasePlatform = basePlatform;
    }

    public IRunLoop CreateRunloop(Application applicationAbstract) => this.BasePlatform.CreateRunloop(applicationAbstract);

    public Xui.Core.Actual.IWindow CreateWindow(Xui.Core.Abstract.IWindow windowAbstract)
    {
        var middleware = new EmulatorWindow();
        middleware.Abstract = windowAbstract;
        var window = this.BasePlatform.CreateWindow(middleware);
        middleware.Platform = window;
        return middleware;
    }
}