using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Middleware.DevTools.IO;
using Xui.Runtime.Software.Actual;

namespace Xui.Middleware.DevTools.Actual;

/// <summary>
/// A middleware window inserted between the abstract application window and the real platform window.
/// Implements both <see cref="Xui.Core.Abstract.IWindow"/> and <see cref="Xui.Core.Actual.IWindow"/>,
/// delegating in both directions while adding DevTools capabilities:
/// SVG screenshot capture, UI-tree inspection, and synthetic input injection.
/// </summary>
internal sealed class DevToolsWindow : Xui.Core.Abstract.IWindow, Xui.Core.Actual.IWindow
{
    private readonly DevToolsPlatform platform;

    // Screenshot state — written and read exclusively on the UI thread.
    private TaskCompletionSource<string>? pendingScreenshot;
    private MemoryStream? svgStream;
    private SvgDrawingContext? pendingSvgContext;
    private Rect pendingRect;

    // Overlay context captured from GetService so Render() can dispose it after Abstract.Render.
    private IContext? pendingOverlayCtx;

    // Overlay state: last interaction point and identity label, shown on every frame until replaced.
    private (Point pos, bool isTouch)? lastInputOverlay;
    private string? overlayLabel;

    /// <summary>The abstract application window (app layer).</summary>
    public Xui.Core.Abstract.IWindow? Abstract { get; set; }

    /// <summary>The underlying real platform window.</summary>
    public Xui.Core.Actual.IWindow? Platform { get; set; }

    public DevToolsWindow(DevToolsPlatform platform) => this.platform = platform;

    string Xui.Core.Actual.IWindow.Title
    {
        get => Platform!.Title;
        set => Platform!.Title = value;
    }

    void Xui.Core.Actual.IWindow.Show() => Platform!.Show();
    void Xui.Core.Actual.IWindow.Invalidate() => Platform!.Invalidate();
    void Xui.Core.Actual.IWindow.Close() => Platform!.Close();

    bool Xui.Core.Actual.IWindow.RequireKeyboard
    {
        get => Platform!.RequireKeyboard;
        set => Platform!.RequireKeyboard = value;
    }

    Xui.Core.Canvas.ITextMeasureContext? Xui.Core.Actual.IWindow.TextMeasureContext
        => Platform!.TextMeasureContext;

    /// <summary>
    /// Returns a <see cref="SplicingContext"/> when a screenshot is pending so the next
    /// render pass writes simultaneously to the real display and an SVG capture stream.
    /// </summary>
    object? IServiceProvider.GetService(Type t)
    {
        // During construction, Platform is not yet wired — fall back to the abstract window's services.
        if (Platform == null)
            return (Abstract as IServiceProvider)?.GetService(t);

        if (t != typeof(IContext))
            return Platform.GetService(t);

        IContext ctx;
        if (pendingScreenshot != null && svgStream != null)
        {
            var realCtx = (Platform!.GetService(t) as IContext)!;
            var svgCtx = new SvgDrawingContext(
                new Size(pendingRect.Width, pendingRect.Height),
                svgStream,
                keepOpen: true);
            pendingSvgContext = svgCtx;
            ctx = new SplicingContext(realCtx, svgCtx);
        }
        else
        {
            ctx = (Platform!.GetService(t) as IContext)!;
        }

        // Wrap with OverlayContext when there's an interaction point or a client label to show.
        // Store it so Render() can call Dispose() after Abstract.Render to trigger the drawing.
        if (lastInputOverlay is { } overlay)
        {
            pendingOverlayCtx = new OverlayContext(ctx, overlay.pos, overlay.isTouch, overlayLabel);
            return pendingOverlayCtx;
        }

        if (overlayLabel != null)
        {
            pendingOverlayCtx = new OverlayContext(ctx, null, false, overlayLabel);
            return pendingOverlayCtx;
        }

        return ctx;
    }

    Rect Xui.Core.Abstract.IWindow.DisplayArea
    {
        get => Abstract!.DisplayArea;
        set => Abstract!.DisplayArea = value;
    }

    Rect Xui.Core.Abstract.IWindow.SafeArea
    {
        get => Abstract!.SafeArea;
        set => Abstract!.SafeArea = value;
    }

    nfloat Xui.Core.Abstract.IWindow.ScreenCornerRadius
    {
        get => Abstract!.ScreenCornerRadius;
        set => Abstract!.ScreenCornerRadius = value;
    }

    void Xui.Core.Abstract.IWindow.Closed() => Abstract!.Closed();
    bool Xui.Core.Abstract.IWindow.Closing() => Abstract!.Closing();

    void Xui.Core.Abstract.IWindow.OnAnimationFrame(ref FrameEventRef e) => Abstract!.OnAnimationFrame(ref e);
    void Xui.Core.Abstract.IWindow.OnMouseDown(ref MouseDownEventRef e) => Abstract!.OnMouseDown(ref e);
    void Xui.Core.Abstract.IWindow.OnMouseMove(ref MouseMoveEventRef e) => Abstract!.OnMouseMove(ref e);
    void Xui.Core.Abstract.IWindow.OnMouseUp(ref MouseUpEventRef e) => Abstract!.OnMouseUp(ref e);
    void Xui.Core.Abstract.IWindow.OnScrollWheel(ref ScrollWheelEventRef e) => Abstract!.OnScrollWheel(ref e);
    void Xui.Core.Abstract.IWindow.OnTouch(ref TouchEventRef e) => Abstract!.OnTouch(ref e);
    void Xui.Core.Abstract.IWindow.WindowHitTest(ref WindowHitTestEventRef e) => Abstract!.WindowHitTest(ref e);
    void Xui.Core.Abstract.IWindow.OnKeyDown(ref KeyEventRef e) => Abstract!.OnKeyDown(ref e);
    void Xui.Core.Abstract.IWindow.OnChar(ref KeyEventRef e) => Abstract!.OnChar(ref e);

    /// <summary>
    /// Intercepts the render call: if a screenshot is pending, allocates the capture stream
    /// and rect before delegating to the abstract window, then reads the completed SVG after.
    /// </summary>
    void Xui.Core.Abstract.IWindow.Render(ref RenderEventRef render)
    {
        if (pendingScreenshot != null)
        {
            svgStream = new MemoryStream();
            pendingRect = render.Rect;
        }

        pendingOverlayCtx = null;
        Abstract!.Render(ref render);

        // Draw the overlay on top of the app content (and into the SVG stream if screenshotting).
        // Dispose() triggers the drawing; inner.Dispose() is NOT called by OverlayContext so the
        // underlying context (real or SplicingContext) remains valid for the screenshot read below.
        pendingOverlayCtx?.Dispose();
        pendingOverlayCtx = null;

        // Window.Render does not dispose the IContext, so we must dispose the
        // SvgDrawingContext ourselves to flush the closing SVG tags to the stream.
        if (pendingSvgContext != null)
        {
            pendingSvgContext.Dispose();
            pendingSvgContext = null;
        }

        if (pendingScreenshot != null && svgStream != null)
        {
            var tcs = pendingScreenshot;
            pendingScreenshot = null;
            var svg = System.Text.Encoding.UTF8.GetString(svgStream.ToArray());
            svgStream = null;
            tcs.SetResult(svg);
        }
    }

    internal Task<InspectResult> HandleInspect()
    {
        var tcs = new TaskCompletionSource<InspectResult>();
        platform.MainDispatcher.Post(() =>
        {
            try
            {
                var root = Abstract is Xui.Core.Abstract.Window w
                    ? WalkView(w.RootView)
                    : new ViewNode("window", 0, 0, 0, 0, 0, 0, true, null, null, []);
                tcs.SetResult(new InspectResult(root));
            }
            catch (Exception ex) { tcs.SetException(ex); }
        });
        return tcs.Task;
    }

    internal Task<ScreenshotResult> HandleScreenshot()
    {
        var tcs = new TaskCompletionSource<string>();
        platform.MainDispatcher.Post(() =>
        {
            pendingScreenshot = tcs;
            Platform!.Invalidate();
        });
        return tcs.Task.ContinueWith(
            t => new ScreenshotResult(t.Result),
            TaskContinuationOptions.ExecuteSynchronously);
    }

    internal Task HandleClick(ClickParams p)
    {
        platform.MainDispatcher.Post(() =>
        {
            var pos = new Point(p.X, p.Y);
            lastInputOverlay = (pos, isTouch: false);
            var down = new MouseDownEventRef { Position = pos, Button = MouseButton.Left };
            Abstract!.OnMouseDown(ref down);
            var up = new MouseUpEventRef { Position = pos, Button = MouseButton.Left };
            Abstract!.OnMouseUp(ref up);
            Platform!.Invalidate();
        });
        return Task.CompletedTask;
    }

    internal Task HandleTap(TapParams p)
    {
        platform.MainDispatcher.Post(() =>
        {
            var pos = new Point(p.X, p.Y);
            lastInputOverlay = (pos, isTouch: true);
            var touch = new Touch { Index = 0, Phase = TouchPhase.Start, Position = pos, Radius = 0.5f };
            var te = new TouchEventRef([touch]);
            Abstract!.OnTouch(ref te);

            touch.Phase = TouchPhase.End;
            te = new TouchEventRef([touch]);
            Abstract!.OnTouch(ref te);
            Platform!.Invalidate();
        });
        return Task.CompletedTask;
    }

    internal Task HandlePointer(PointerParams p)
    {
        platform.MainDispatcher.Post(() =>
        {
            var phase = p.Phase switch
            {
                "start" => TouchPhase.Start,
                "move" => TouchPhase.Move,
                "end" => TouchPhase.End,
                _ => TouchPhase.End,
            };
            var pos = new Point(p.X, p.Y);
            lastInputOverlay = (pos, isTouch: true);
            var touch = new Touch { Index = p.Index, Phase = phase, Position = pos, Radius = 0.5f };
            var te = new TouchEventRef([touch]);
            Abstract!.OnTouch(ref te);
            Platform!.Invalidate();
        });
        return Task.CompletedTask;
    }

    internal Task HandleMouseMove(MouseMoveParams p)
    {
        platform.MainDispatcher.Post(() =>
        {
            var pos = new Point(p.X, p.Y);
            lastInputOverlay = (pos, isTouch: false);
            var move = new MouseMoveEventRef { Position = pos };
            Abstract!.OnMouseMove(ref move);
            Platform!.Invalidate();
        });
        return Task.CompletedTask;
    }

    internal Task HandleInvalidate()
    {
        platform.MainDispatcher.Post(() => Platform!.Invalidate());
        return Task.CompletedTask;
    }

    internal Task HandleIdentify(IdentifyParams p)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        platform.MainDispatcher.Post(() =>
        {
            overlayLabel = string.IsNullOrWhiteSpace(p.Label) ? null : p.Label;
            Platform!.Invalidate();
            tcs.SetResult();
        });
        return tcs.Task;
    }

    private static ViewNode WalkView(Xui.Core.UI.View view)
    {
        var f = view.Frame;
        var children = new ViewNode[view.Count];
        for (int i = 0; i < view.Count; i++)
            children[i] = WalkView(view[i]);
        
        // Calculate center point for clicking
        var centerX = (float)(f.X + f.Width / 2);
        var centerY = (float)(f.Y + f.Height / 2);
        
        // Get ClassName as comma-separated string or null
        var className = view.ClassName.Count > 0 
            ? string.Join(" ", view.ClassName) 
            : null;

        // Strip generic arity suffix (e.g. "ViewCollection`1" -> "ViewCollection") so the
        // type name can be used as a valid XML element name by the MCP InspectUi tool.
        var typeName = view.GetType().Name;
        var backtickIdx = typeName.IndexOf('`');
        if (backtickIdx >= 0)
            typeName = typeName[..backtickIdx];

        return new ViewNode(
            typeName,
            (float)f.X, (float)f.Y,
            (float)f.Width, (float)f.Height,
            centerX, centerY,
            true,
            view.Id,
            className,
            children);
    }
}
