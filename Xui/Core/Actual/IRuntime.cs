using Xui.Core.Canvas;

namespace Xui.Core.Actual;

public interface IRuntime
{
    IContext DrawingContext { get; }
    IDispatcher MainDispatcher { get; }

    Actual.IWindow CreateWindow(Abstract.IWindow windowAbstract);
    IRunLoop CreateRunloop(Abstract.Application applicationAbstract);
}
