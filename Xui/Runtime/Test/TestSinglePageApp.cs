using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Core.UI;
using Xui.Runtime.Software.Actual;
using Xui.Runtime.Test.Actual;

namespace Xui.Runtime.Test;

/// <summary>
/// A unit-test platform harness that boots a real <see cref="Application"/> subclass
/// and exposes synchronous methods to drive input, animation, and rendering.
/// <para>
/// Like the Browser platform, <c>Run()</c> calls <c>Start()</c> and returns immediately.
/// The test then drives events manually — no OS event loop is involved.
/// </para>
/// </summary>
public class TestSinglePageApp : IDisposable
{
    private readonly TestPlatform platform;
    private bool disposed;

    /// <summary>
    /// The first (and typically only) window created by the application.
    /// </summary>
    public Window Window { get; }

    /// <summary>
    /// The window size used for rendering.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Creates a test harness that boots the given application with a window of the specified size.
    /// </summary>
    public TestSinglePageApp(Application application, Size windowSize)
    {
        this.Size = windowSize;
        this.platform = new TestPlatform();
        Xui.Core.Actual.Runtime.Current = this.platform;

        application.Run();

        this.Window = Window.OpenWindows[Window.OpenWindows.Count - 1];
        this.Window.DisplayArea = new Rect(0, 0, windowSize.Width, windowSize.Height);
        this.Window.SafeArea = this.Window.DisplayArea;
    }

    // ── Input ────────────────────────────────────────────────────

    public void MouseMove(Point position)
    {
        var e = new MouseMoveEventRef { Position = position };
        this.Window.OnMouseMove(ref e);
    }

    public void MouseDown(Point position, MouseButton button = MouseButton.Left)
    {
        var e = new MouseDownEventRef { Position = position, Button = button };
        this.Window.OnMouseDown(ref e);
    }

    public void MouseUp(Point position, MouseButton button = MouseButton.Left)
    {
        var e = new MouseUpEventRef { Position = position, Button = button };
        this.Window.OnMouseUp(ref e);
    }

    /// <summary>
    /// Sends a mouse-move event targeting the center of the given view's <see cref="View.Frame"/>.
    /// </summary>
    public void MouseMove(View view) => MouseMove(view.Frame.Center);

    /// <summary>
    /// Sends a mouse-down event targeting the center of the given view's <see cref="View.Frame"/>.
    /// </summary>
    public void MouseDown(View view, MouseButton button = MouseButton.Left) => MouseDown(view.Frame.Center, button);

    /// <summary>
    /// Sends a mouse-up event targeting the center of the given view's <see cref="View.Frame"/>.
    /// </summary>
    public void MouseUp(View view, MouseButton button = MouseButton.Left) => MouseUp(view.Frame.Center, button);

    // ── Ticks ────────────────────────────────────────────────────

    /// <summary>
    /// Drains all pending callbacks from the dispatcher post queue.
    /// </summary>
    public void HandlePostActions()
    {
        while (this.platform.PostQueue.TryDequeue(out var action))
        {
            action();
        }
    }

    /// <summary>
    /// Sends a <see cref="FrameEventRef"/> to the window, driving <c>AnimateCore</c>
    /// on all views that requested an animation frame.
    /// </summary>
    public void AnimationFrame(TimeSpan previous, TimeSpan next)
    {
        var frame = new FrameEventRef(previous, next);
        ((Xui.Core.Abstract.IWindow)this.Window).OnAnimationFrame(ref frame);
    }

    /// <summary>
    /// Renders the window to SVG and returns the SVG string.
    /// Creates a fresh <see cref="SvgDrawingContext"/> backed by a <see cref="MemoryStream"/>,
    /// drives <c>Window.Render</c> through the standard platform path, and returns the result.
    /// </summary>
    public string Render()
    {
        using var stream = new MemoryStream();

        var context = new SvgDrawingContext(
            this.Size, stream, Xui.Core.Fonts.Inter.URIs, keepOpen: true);
        this.platform.CurrentDrawingContext = context;

        var frame = new FrameEventRef(TimeSpan.Zero, TimeSpan.Zero);
        var rect = new Rect(0, 0, this.Size.Width, this.Size.Height);
        var render = new RenderEventRef(rect, frame);
        ((Xui.Core.Abstract.IWindow)this.Window).Render(ref render);

        this.platform.CurrentDrawingContext = null;

        stream.Position = 0;
        return new StreamReader(stream).ReadToEnd();
    }

    // ── Cleanup ──────────────────────────────────────────────────

    /// <summary>
    /// Drains remaining post actions and closes all windows.
    /// </summary>
    public void Quit()
    {
        HandlePostActions();
        this.Window.Closed();
    }

    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            Quit();
        }
    }
}
