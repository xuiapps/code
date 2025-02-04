using System;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;

namespace Xui.Runtime.Browser.Actual;

public partial class BrowserWindow : Xui.Core.Actual.IWindow
{
    [JSImport("Xui.Runtime.Browser.Actual.BrowserWindow.setTitle", "main.js")]
    internal static partial void SetTitle(string title);

    [JSExport]
    internal static void OnAnimationFrame(double width, double height, double timestamp) => Instance?.SendAnimationFrameEvent(width, height, timestamp);

    [JSExport]
    internal static void OnMouseMove(double x, double y) => Instance?.SendMouseMoveEvent(x, y);

    [JSExport]
    internal static void OnWheel(double x, double y, double deltaX, double deltaY) => Instance?.SendWheelEvent(x, y, deltaX, deltaY);

    public static BrowserWindow? Instance { get; internal set; }

    private Xui.Core.Abstract.IWindow Abstract;

    private string _title = "";
    private bool invalidated = false;

    public BrowserWindow(Xui.Core.Abstract.IWindow windowAbstract)
    {
        this.Abstract = windowAbstract;
    }

    public string Title
    {
        get => _title;
        set
        {
            if (this._title != value)
            {
                this._title = value;
                SetTitle(this._title);
            }
        }
    }

    public void Invalidate()
    {
        this.invalidated = true;
        // Call "draw" on next animation frame...
    }

    public void Show()
    {
    }

    private void SendAnimationFrameEvent(double width, double height, double timestamp)
    {
        var previous = TimeSpan.FromMilliseconds(timestamp);
        // TODO: Imagine 60fps in all browsers, probably find an API or track FPS.
        var next = previous + TimeSpan.FromSeconds(1.0 / 60.0);
        FrameEventRef frameEventRef = new FrameEventRef(previous, next);
        this.Abstract.OnAnimationFrame(ref frameEventRef);

        // If invalidated = draw...
        if (this.invalidated)
        {
            this.invalidated = false;

            Rect rect = new Rect(0, 0, (NFloat)width, (NFloat)height);
            RenderEventRef renderEventRef = new RenderEventRef(rect, frameEventRef);

            BrowserDrawingContext.CanvasReset();
            this.Abstract.Render(ref renderEventRef);
        }
    }

    private void SendMouseMoveEvent(double x, double y)
    {
        MouseMoveEventRef mouseMoveEventRef = new MouseMoveEventRef()
        {
            Position = new Core.Math2D.Point((NFloat)x, (NFloat)y)
        };
        this.Abstract.OnMouseMove(ref mouseMoveEventRef);
    }

    private void SendWheelEvent(double x, double y, double deltaX, double deltaY)
    {
        ScrollWheelEventRef scrollWheelEventRef = new ScrollWheelEventRef()
        {
            Delta = new Vector((NFloat)deltaX, (NFloat)deltaY)
        };
        this.Abstract.OnScrollWheel(ref scrollWheelEventRef);
    }
}
