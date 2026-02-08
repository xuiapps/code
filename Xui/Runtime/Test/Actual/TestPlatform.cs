using Xui.Core.Actual;
using Xui.Core.Canvas;

namespace Xui.Runtime.Test.Actual;

public class TestPlatform : IRuntime, IDispatcher
{
    internal readonly List<TestWindow> Windows = new();
    internal readonly Queue<Action> PostQueue = new();
    internal IContext? CurrentDrawingContext;

    public IContext DrawingContext =>
        CurrentDrawingContext ?? throw new InvalidOperationException(
            "DrawingContext is only available during a Render() call on TestApp.");

    public IDispatcher MainDispatcher => this;

    public IRunLoop CreateRunloop(Xui.Core.Abstract.Application applicationAbstract) =>
        new TestRunLoop(this, applicationAbstract);

    public IWindow CreateWindow(Xui.Core.Abstract.IWindow windowAbstract)
    {
        var window = new TestWindow(windowAbstract);
        Windows.Add(window);
        return window;
    }

    public void Post(Action callback)
    {
        if (callback == null) return;
        PostQueue.Enqueue(callback);
    }
}
