using System;
using System.Runtime.InteropServices;
using Xui.Core.Abstract;
using Xui.Core.Abstract.Events;
using Xui.Core.Canvas;
using Xui.Core.DI;
using Xui.Core.Math2D;
using Xui.Middleware.Emulator.Devices;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;

namespace Xui.Middleware.Emulator.Actual;

/// <summary>
/// Host window shown on desktop runtimes. It owns host chrome/input and links to a virtual
/// mobile window that drives the emulated app UI lifecycle.
/// </summary>
public partial class EmulatorWindow : Xui.Core.Middleware.IWindow, Xui.Core.Abstract.IWindow.IDesktopStyle
{
    private readonly LinkedEmulatorWindow emulator;
    private readonly EmulatorControlRootView controls = new();
    private readonly EmulatorDeviceInfo deviceInfo;
    private readonly EmulatorChromeRenderer chromeRenderer = new();
    private EmulatorGeometry lastGeometry = EmulatorGeometry.Create(new Rect(0, 0, 430, 940), DeviceCatalog.All[0]);
    private EmulatorStatusBarStyle? statusBarStyleOverride;
    public bool RenderTouchIndicators { get; set; } = true;

    public EmulatorWindow(Xui.Core.Abstract.IWindow appWindow, IServiceProvider nextServiceProvider)
    {
        this.emulator = new LinkedEmulatorWindow(appWindow);
        this.deviceInfo = new EmulatorDeviceInfo(this);
        this.NextServiceProvider = nextServiceProvider;
    }

    /// <summary>The linked emulated app window abstraction.</summary>
    internal LinkedEmulatorWindow Emulator => emulator;

    /// <summary>The wrapped app window abstraction.</summary>
    public Xui.Core.Abstract.IWindow AppWindow => emulator.AppWindow;

    Xui.Core.Abstract.IWindow Xui.Core.Actual.IWindow.Abstract => AppWindow;

    /// <summary>The actual window immediately downstream of this middleware layer.</summary>
    public Xui.Core.Actual.IWindow? Actual { get; private set; }

    public EmulatorStatusBarStyle? StatusBarStyleOverride
    {
        get => statusBarStyleOverride;
        set => statusBarStyleOverride = value;
    }

    public Rect DisplayArea { get => emulator.DisplayArea; set => emulator.DisplayArea = value; }

    public Rect SafeArea { get => emulator.SafeArea; set => emulator.SafeArea = value; }

    public NFloat ScreenCornerRadius { get => emulator.ScreenCornerRadius; set => emulator.ScreenCornerRadius = value; }

    internal void AttachActual(Xui.Core.Actual.IWindow actual)
    {
        if (this.Actual is not null)
            throw new InvalidOperationException("The emulator window is already attached to an actual window.");

        this.Actual = actual;
    }

    /// <summary>Application services reached after the complete actual-window chain.</summary>
    public IServiceProvider NextServiceProvider { get; }

    WindowBackdrop Xui.Core.Abstract.IWindow.IDesktopStyle.Backdrop => WindowBackdrop.Chromeless;

    DesktopWindowLevel Xui.Core.Abstract.IWindow.IDesktopStyle.Level => DesktopWindowLevel.Floating;

    Size? Xui.Core.Abstract.IWindow.IDesktopStyle.StartupSize =>
        (CurrentDevice.LogicalResolution.Width + 48, CurrentDevice.LogicalResolution.Height + 88);

#region Platform.IWindow
    string Xui.Core.Actual.IWindow.Title { get => Actual!.Title; set => Actual!.Title = value; }

    public bool RequireKeyboard { get => Actual?.RequireKeyboard ?? false; set { if (Actual != null) Actual.RequireKeyboard = value; } }

    public ITextMeasureContext? TextMeasureContext => Actual?.TextMeasureContext;

    void Xui.Core.Actual.IWindow.Invalidate() => Actual!.Invalidate();

    void Xui.Core.Actual.IWindow.Show() => Actual!.Show();

    void Xui.Core.Actual.IWindow.Close() => Actual?.Close();

    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IDeviceInfo))
            return deviceInfo;

        var platformService = Actual?.GetService(serviceType);
        if (platformService != null)
            return platformService;

        return NextServiceProvider.GetService(serviceType);
    }

    /// <summary>The device profile currently shown by the emulator.</summary>
    public DeviceProfile CurrentDevice { get; set; } = DeviceCatalog.All[0];
#endregion

#region Abstract.IWindow
    void Xui.Core.Abstract.IWindow.Closed() => emulator.Closed();

    bool Xui.Core.Abstract.IWindow.Closing() => emulator.Closing();

    void Xui.Core.Abstract.IWindow.OnAnimationFrame(ref FrameEventRef animationFrame)
    {
        emulator.OnAnimationFrame(ref animationFrame);
    }

    public Point MapEmulatorToHost(Point emulatorPoint) =>
        lastGeometry.MapEmulatorToHost(emulatorPoint);

    public Point MapEmulatorToHost(Point emulatorPoint, Size hostSize) =>
        UpdateHostGeometry(hostSize).MapEmulatorToHost(emulatorPoint);

    public Point MapEmulatorToSnapshot(Point emulatorPoint) =>
        EmulatorGeometry.CreateSnapshot(CurrentDevice).MapEmulatorToHost(emulatorPoint);

    public Size SnapshotSize =>
        new Size(
            CurrentDevice.LogicalResolution.Width + 16f,
            CurrentDevice.LogicalResolution.Height + 16f);

    private Point MapHostToEmulator(Point hostPoint) =>
        lastGeometry.MapHostToEmulator(hostPoint);

    void Xui.Core.Abstract.IWindow.OnMouseDown(ref MouseDownEventRef evRef)
    {
        if (lastGeometry.HasHostTitle && lastGeometry.TitleRect.Contains(evRef.Position))
        {
            controls.OnMouseDown(ref evRef);
            return;
        }

        if (!lastGeometry.TryMapHostToEmulator(evRef.Position, out var mapped))
            return;

        MouseDownEventRef evMobile = new MouseDownEventRef()
        {
            Position = mapped,
            Button = evRef.Button
        };
        emulator.OnMouseDown(ref evMobile);
    }

    void Xui.Core.Abstract.IWindow.OnMouseMove(ref MouseMoveEventRef evRef)
    {
        if (!emulator.HasActiveTouch)
            controls.OnMouseMove(ref evRef);

        var mapped = MapHostToEmulator(evRef.Position);
        if (!lastGeometry.EmulatorRect.Contains(evRef.Position) && !emulator.HasActiveTouch)
            return;

        MouseMoveEventRef evMobile = new MouseMoveEventRef()
        {
            Position = mapped
        };
        emulator.OnMouseMove(ref evMobile);
    }

    void Xui.Core.Abstract.IWindow.OnMouseUp(ref MouseUpEventRef evRef)
    {
        if (!emulator.HasActiveTouch)
            controls.OnMouseUp(ref evRef);

        var mapped = MapHostToEmulator(evRef.Position);
        if (!lastGeometry.EmulatorRect.Contains(evRef.Position) && !emulator.HasActiveTouch)
            return;

        MouseUpEventRef evMobile = new MouseUpEventRef()
        {
            Position = mapped,
            Button = evRef.Button
        };
        emulator.OnMouseUp(ref evMobile);
    }

    void Xui.Core.Abstract.IWindow.OnScrollWheel(ref ScrollWheelEventRef evRef)
    {
        emulator.OnScrollWheel(ref evRef);
    }

    void Xui.Core.Abstract.IWindow.OnTouch(ref TouchEventRef touchEventRef)
    {
        emulator.OnTouch(ref touchEventRef);
    }

    void Xui.Core.Abstract.IWindow.Render(ref RenderEventRef render)
    {
        lastGeometry = EmulatorGeometry.Create(render.Rect, CurrentDevice);
        var ctx = render.Context;

        ctx.Save();
        ctx.BeginPath();
        ctx.RoundRect(lastGeometry.EmulatorRect, lastGeometry.ScreenCornerRadius);
        ctx.Clip();

        ctx.Translate(lastGeometry.EmulatorRect.TopLeft);

        RenderEventRef emulatorRender = new RenderEventRef(
            rect: new Rect(0, 0, lastGeometry.EmulatorRect.Width, lastGeometry.EmulatorRect.Height),
            frame: render.Frame,
            context: ctx
        );

        ctx.SetFill(Colors.White);
        ctx.FillRect(emulatorRender.Rect);

        emulator.DisplayArea = emulatorRender.Rect;
        emulator.SafeArea = emulatorRender.Rect - CurrentDevice.SafeAreaInsetsPortrait;
        emulator.ScreenCornerRadius = CurrentDevice.ScreenCornerRadius;
        emulator.Render(ref emulatorRender);
        if (RenderTouchIndicators)
            emulator.RenderTouchIndicator(ctx);

        ctx.Restore();

        chromeRenderer.Render(ctx, in lastGeometry, render.Rect, CurrentDevice, ResolveStatusBarStyle(), ResolveClock());
        controls.UpdateLayout(lastGeometry.TitleRect);
        controls.Render(ctx);
    }

    public void RenderSnapshot(ref RenderEventRef render)
    {
        var geometry = EmulatorGeometry.CreateSnapshot(CurrentDevice);
        var ctx = render.Context;

        ctx.Save();
        ctx.BeginPath();
        ctx.RoundRect(geometry.EmulatorRect, geometry.ScreenCornerRadius);
        ctx.Clip();
        ctx.Translate(geometry.EmulatorRect.TopLeft);

        RenderEventRef emulatorRender = new RenderEventRef(
            rect: new Rect(0, 0, geometry.EmulatorRect.Width, geometry.EmulatorRect.Height),
            frame: render.Frame,
            context: ctx);

        ctx.SetFill(Colors.White);
        ctx.FillRect(emulatorRender.Rect);

        emulator.DisplayArea = emulatorRender.Rect;
        emulator.SafeArea = emulatorRender.Rect - CurrentDevice.SafeAreaInsetsPortrait;
        emulator.ScreenCornerRadius = CurrentDevice.ScreenCornerRadius;
        emulator.Render(ref emulatorRender);
        if (RenderTouchIndicators)
            emulator.RenderTouchIndicator(ctx);

        ctx.Restore();

        chromeRenderer.Render(ctx, in geometry, render.Rect, CurrentDevice, ResolveStatusBarStyle(), ResolveClock());
    }

    void Xui.Core.Abstract.IWindow.WindowHitTest(ref WindowHitTestEventRef evRef)
    {
        var point = evRef.Point;

        if (point.Y < lastGeometry.TitleHeight)
        {
            if (controls.IsInteractive(point))
            {
                evRef.Area = WindowHitTestEventRef.WindowArea.Client;
                return;
            }

            evRef.Area = WindowHitTestEventRef.WindowArea.Title;
            return;
        }

        var emulatorRect = lastGeometry.EmulatorRect;
        if (!emulatorRect.Contains(point))
            return;

        NFloat signedBorderDistance;
        var radius = lastGeometry.ScreenCornerRadius;

        var topLeftCenter = emulatorRect.TopLeft + (radius, radius);
        var topRightCenter = emulatorRect.TopRight + (-radius, radius);
        var bottomLeftCenter = emulatorRect.BottomLeft + (radius, -radius);
        var bottomRightCenter = emulatorRect.BottomRight + (-radius, -radius);

        if (point.X < topLeftCenter.X && point.Y < topLeftCenter.Y)
            signedBorderDistance = (point - topLeftCenter).Magnitude - radius;
        else if (point.X > topRightCenter.X && point.Y < topRightCenter.Y)
            signedBorderDistance = (point - topRightCenter).Magnitude - radius;
        else if (point.X < bottomLeftCenter.X && point.Y > bottomLeftCenter.Y)
            signedBorderDistance = (point - bottomLeftCenter).Magnitude - radius;
        else if (point.X > bottomRightCenter.X && point.Y > bottomRightCenter.Y)
            signedBorderDistance = (point - bottomRightCenter).Magnitude - radius;
        else
            signedBorderDistance = NFloat.Max(
                NFloat.Max(emulatorRect.Left - point.X, point.X - emulatorRect.Right),
                NFloat.Max(emulatorRect.Top - point.Y, point.Y - emulatorRect.Bottom)
            );

        if (-lastGeometry.BorderWidth <= signedBorderDistance && signedBorderDistance <= 0)
        {
            NFloat resizeRect = 48f;

            if (point.X <= emulatorRect.Left + resizeRect && point.Y <= emulatorRect.Top + resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderTopLeft;
            else if (point.X >= emulatorRect.Right - resizeRect && point.Y <= emulatorRect.Top + resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderTopRight;
            else if (point.X >= emulatorRect.Right - resizeRect && point.Y >= emulatorRect.Bottom - resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottomRight;
            else if (point.X <= emulatorRect.Left + resizeRect && point.Y >= emulatorRect.Bottom - resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottomLeft;
            else if (point.Y <= emulatorRect.Top + resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderTop;
            else if (point.Y >= emulatorRect.Bottom - resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderBottom;
            else if (point.X <= emulatorRect.Left + resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderLeft;
            else if (point.X >= emulatorRect.Right - resizeRect)
                evRef.Area = WindowHitTestEventRef.WindowArea.BorderRight;
            else
                evRef.Area = WindowHitTestEventRef.WindowArea.Client;
        }
        else if (signedBorderDistance <= 0)
        {
            evRef.Area = WindowHitTestEventRef.WindowArea.Client;
        }
        else
        {
            evRef.Area = WindowHitTestEventRef.WindowArea.Transparent;
        }
    }

    public void OnKeyDown(ref KeyEventRef e) =>
        emulator.OnKeyDown(ref e);

    public void OnChar(ref KeyEventRef e) =>
        emulator.OnChar(ref e);

    /// <summary>
    /// Exposes runtime services from the wrapped app window for tests that need app root access.
    /// </summary>
    public object? GetLinkedService(Type serviceType) =>
        (AppWindow as IServiceProvider)?.GetService(serviceType);

    private EmulatorStatusBarStyle ResolveStatusBarStyle() =>
        statusBarStyleOverride ?? default;

    private IClock ResolveClock() =>
        NextServiceProvider.GetService(typeof(IClock)) as IClock ?? SystemClock.Default;

    private EmulatorGeometry UpdateHostGeometry(Size hostSize)
    {
        lastGeometry = EmulatorGeometry.Create(new Rect(0, 0, hostSize.Width, hostSize.Height), CurrentDevice);
        controls.UpdateLayout(lastGeometry.TitleRect);
        return lastGeometry;
    }

#endregion
}
