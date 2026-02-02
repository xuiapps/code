using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using Xui.Runtime.Windows.Win32;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows.Actual;

public partial class Win32Window : Xui.Core.Actual.IWindow
{
    public const uint WM_ANIMATION_FRAME_MSG = 0x0401;

    [ThreadStatic]
    private static Win32Window? constructedInstanceOnStack;

    private static Dictionary<HWND, Win32Window> HwndToWindow = new Dictionary<HWND, Win32Window>();

    private volatile bool invalid = true;

    public bool NeedsFrame => this.invalid;

    private TimeSpan previous;
    private TimeSpan next;

    private NFloat dpiScale = 1.0f;

    private NFloat invDpiScale = 1.0f;

    private bool trackingMouseLeave;

    public nint CompositionFrameHandle
    {
        get
        {
            if (this.Renderer is D2DComp d2dComp)
            {
                return d2dComp.FrameLatencyHandle;
            }

            return 0;
        }
    }

    private static int OnMessageStatic(HWND hWnd, WindowMessage uMsg, WPARAM wParam, LPARAM lParam)
    {
        Win32Window window;
        if (HwndToWindow.TryGetValue(hWnd, out var w))
        {
            window = w;
        }
        else if (constructedInstanceOnStack != null)
        {
            window = constructedInstanceOnStack;
            HwndToWindow[hWnd] = constructedInstanceOnStack;
            constructedInstanceOnStack.Hwnd = hWnd;
            constructedInstanceOnStack = null;
        }
        else
        {
            throw new Win32Exception("Unknown window for hWnd.");
        }

        return window.OnMessage(hWnd, uMsg, wParam, lParam);
    }

    public Win32Window(Xui.Core.Abstract.IWindow @abstract)
    {
        this.Abstract = @abstract;
        this.Title = "";

        nint hbrBackground = GetSysColorBrush((int)WindowColor.COLOR_3DFACE);

        WNDPROC wndProcDelegate = OnMessageStatic;
        GCHandle.Alloc(wndProcDelegate);
        nint wndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate);

        nint lpszClassNamePtr = Marshal.StringToHGlobalUni("XuiWindow");
        var w = new WNDCLASSEXW
        {
            cbSize = (uint)Marshal.SizeOf<WNDCLASSEXW>(),
            styles = WindowClassStyles.CS_HREDRAW | WindowClassStyles.CS_VREDRAW,
            lpfnWndProc = wndProc,
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = 0,
            hIcon = 0,
            hCursor = 0,
            hbrBackground = 0, // hbrBackground,
            lpszMenuName = 0,
            lpszClassName = lpszClassNamePtr,
            hIconSm = 0
        };
        Marshal.FreeHGlobal(lpszClassNamePtr);

        // this.Renderer = new D2D(this);
        this.Renderer = new D2DComp(this);

        ushort classAtom = RegisterClassEx(w);

        uint dwExStyle;
        uint dwStyle;

        int x = 100;
        int y = 100;
        int width = 800;
        int height = 600;

        DesktopWindowLevel level = DesktopWindowLevel.Normal;

        if (this.Abstract is Xui.Core.Abstract.IWindow.IDesktopStyle desktopStyle)
        {
            level = desktopStyle.Level;

            if (desktopStyle.StartupSize.HasValue)
            {
                width = (int)desktopStyle.StartupSize.Value.Width;
                height = (int)desktopStyle.StartupSize.Value.Height;
            }

            if (desktopStyle.Chromeless)
            {
                dwExStyle = (uint)ExtendedWindowStyles.WS_EX_NOREDIRECTIONBITMAP;
                dwStyle = (uint)WindowStyles.WS_POPUP;
            }
            else
            {
                dwExStyle = 0;
                dwStyle = (uint)WindowStyles.WS_TILEDWINDOW;
            }
        }
        else
        {
            dwExStyle = 0;
            dwStyle = (uint)WindowStyles.WS_TILEDWINDOW;
        }

        // This sets a property for the whole process.
        // That's better be in its own platform abstraction like PlatformUIProcess.
        SetProcessDpiAwarenessContext((nint)DPIAwarenessContext.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);

        width = (int)(width * PrimaryMonitorDPIScale);
        height = (int)(height * PrimaryMonitorDPIScale);

        constructedInstanceOnStack = this;
        this.Hwnd = CreateWindowEx(
            dwExStyle,
            classAtom,
            this.Title,
            dwStyle,
            x, y, width, height,
            hWndParent: 0,
            hMenu: 0,
            hInstance: 0,
            lpParam: 0);

        if (constructedInstanceOnStack == this)
        {
            constructedInstanceOnStack = null;
        }

        HwndToWindow[this.Hwnd] = this;

        SetLevel(this.Hwnd, level);

        // Make black color in layered window transparent
        SetLayeredWindowAttributes(this.Hwnd, new COLORREF(0), 255, LayeredWindowAttribute.LWA_COLORKEY);
    }

    public HWND Hwnd { get; private set; }

    protected internal Xui.Core.Abstract.IWindow Abstract { get; }

    public RenderTarget Renderer { get; }

    public string Title
    {
        get
        {
            this.Hwnd.GetWindowText(out var str, 2048);
            return str;
        }
        set => this.Hwnd.SetWindowText(value);
    }

    public bool RequireKeyboard { get; set; }

    public int OnMessage(HWND hWnd, WindowMessage uMsg, WPARAM wParam, LPARAM lParam)
    {
        // Console.WriteLine("WndProc " + hWnd + " " + uMsg);
        var msg = (WindowMessage)uMsg;

        // if (this.Renderer.HandleOnMessage(hWnd, uMsg, wParam, lParam, out var result))
        // {
        //     return result;
        // }

        switch (msg)
        {
            case WindowMessage.WM_DPICHANGED:
            {
                this.UpdateDpiScale();

                // lParam points to a RECT in *physical pixels* (suggested new bounds)
                unsafe
                {
                    RECT* suggested = (RECT*)lParam.Value;
                    HWND.SetWindowPos(
                        this.Hwnd,
                        0,
                        suggested->Left,
                        suggested->Top,
                        suggested->Right - suggested->Left,
                        suggested->Bottom - suggested->Top,
                        SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE);
                }

                var res = this.Hwnd.DefWindowProc(uMsg, wParam, lParam);
                ((D2DComp)this.Renderer).ResizeBuffers(hWnd);
                this.invalid = true;
                this.Render();
                return res;
            }
            case WindowMessage.WM_SIZE:
            {
                var res = this.Hwnd.DefWindowProc(uMsg, wParam, lParam);
                ((D2DComp)this.Renderer).ResizeBuffers(hWnd);
                this.invalid = true;
                this.Render();
                this.invalid = true;
                return res;
            }

            case WindowMessage.WM_PAINT:
            {
                // Validate the update region; do not render here.
                // Rendering should be driven by compositor pacing (frame latency handle / run loop),
                // not by WM_PAINT.
                PAINTSTRUCT ps = new();
                hWnd.BeginPaint(ref ps);
                hWnd.EndPaint(ref ps);

                return 0;
            }

            case WindowMessage.WM_CREATE:
                this.UpdateDpiScale();
                break;

            case WindowMessage.WM_DESTROY:
                Win32Platform.Instance.RemoveWindow(this);
                break;

            case WindowMessage.WM_NCHITTEST:
                POINT win32ClientPoint = new POINT() { X = lParam.LoWord, Y = lParam.HiWord };
                this.Hwnd.ScreenToClient(ref win32ClientPoint);

                Point point = this.ToDip(new Point(win32ClientPoint.X, win32ClientPoint.Y));

                hWnd.GetClientRect(out var rc);
                Rect rect = this.ToDip(new Rect(0, 0, rc.Right - rc.Left, rc.Bottom - rc.Top));

                WindowHitTestEventRef eventRef = new WindowHitTestEventRef(point, rect);
                this.Abstract.WindowHitTest(ref eventRef);

                var area = eventRef.Area;
                switch (area)
                {
                    case WindowHitTestEventRef.WindowArea.Title: return (int)HitTest.HTCAPTION;

                    case WindowHitTestEventRef.WindowArea.Transparent: return (int)HitTest.HTTRANSPARENT;
                    case WindowHitTestEventRef.WindowArea.Client: return (int)HitTest.HTCLIENT;

                    case WindowHitTestEventRef.WindowArea.BorderTopLeft: return (int)HitTest.HTTOPLEFT;
                    case WindowHitTestEventRef.WindowArea.BorderTop: return (int)HitTest.HTTOP;
                    case WindowHitTestEventRef.WindowArea.BorderTopRight: return (int)HitTest.HTTOPRIGHT;
                    case WindowHitTestEventRef.WindowArea.BorderRight: return (int)HitTest.HTRIGHT;
                    case WindowHitTestEventRef.WindowArea.BorderBottomRight: return (int)HitTest.HTBOTTOMRIGHT;
                    case WindowHitTestEventRef.WindowArea.BorderBottom: return (int)HitTest.HTBOTTOM;
                    case WindowHitTestEventRef.WindowArea.BorderBottomLeft: return (int)HitTest.HTBOTTOMLEFT;
                    case WindowHitTestEventRef.WindowArea.BorderLeft: return (int)HitTest.HTLEFT;

                    case WindowHitTestEventRef.WindowArea.Default:
                    default:
                        break;
                }

                break;

            case WindowMessage.WM_MOUSEMOVE:
            {
                // Ensure we get WM_MOUSELEAVE so hover can clear when leaving the window.
                if (!this.trackingMouseLeave)
                {
                    TRACKMOUSEEVENT tme = new TRACKMOUSEEVENT
                    {
                        cbSize = (uint)Marshal.SizeOf<TRACKMOUSEEVENT>(),
                        dwFlags = TrackMouseEventFlags.TME_LEAVE,
                        hwndTrack = this.Hwnd,
                        dwHoverTime = 0
                    };

                    TrackMouseEvent(ref tme);
                    this.trackingMouseLeave = true;
                }

                var pos = this.GetMousePosDip(lParam);

                MouseMoveEventRef mouseMoveEventRef = new MouseMoveEventRef()
                {
                    Position = pos
                };

                this.OnMouseMove(mouseMoveEventRef);
                return 0;
            }

            case WindowMessage.WM_MOUSELEAVE:
            {
                this.trackingMouseLeave = false;
                return 0;
            }

            case WindowMessage.WM_LBUTTONDOWN:
            {
                // Capture so we continue to get mouse up even if the cursor leaves the window while pressed.
                this.Hwnd.CaptureMouse();

                var pos = this.GetMousePosDip(lParam);

                // If you want parity with macOS: do a hit test first.
                var hit = new WindowHitTestEventRef(pos, this.GetClientRectDip(hWnd));
                this.Abstract.WindowHitTest(ref hit);

                // For now: always forward to Xui unless you're doing custom chrome behaviors.
                this.RaiseMouseDown(MouseButton.Left, pos);
                return 0;
            }

            case WindowMessage.WM_LBUTTONUP:
            {
                HWND.ReleaseCapture();

                var pos = this.GetMousePosDip(lParam);

                var hit = new WindowHitTestEventRef(pos, this.GetClientRectDip(hWnd));
                this.Abstract.WindowHitTest(ref hit);

                this.RaiseMouseUp(MouseButton.Left, pos);
                return 0;
            }

            case WindowMessage.WM_RBUTTONDOWN:
            {
                this.Hwnd.CaptureMouse();

                var pos = this.GetMousePosDip(lParam);
                this.RaiseMouseDown(MouseButton.Right, pos);
                return 0;
            }

            case WindowMessage.WM_RBUTTONUP:
            {
                HWND.ReleaseCapture();

                var pos = this.GetMousePosDip(lParam);
                this.RaiseMouseUp(MouseButton.Right, pos);
                return 0;
            }

            case WindowMessage.WM_MBUTTONDOWN:
            {
                this.Hwnd.CaptureMouse();

                var pos = this.GetMousePosDip(lParam);
                this.RaiseMouseDown(MouseButton.Other, pos);
                return 0;
            }

            case WindowMessage.WM_MBUTTONUP:
            {
                HWND.ReleaseCapture();

                var pos = this.GetMousePosDip(lParam);
                this.RaiseMouseUp(MouseButton.Other, pos);
                return 0;
            }

            case WindowMessage.WM_MOUSEWHEEL:
                ScrollWheelEventRef scrollWheelEventRef = new ScrollWheelEventRef()
                {
                    Delta = wParam.WheelDelta
                };

                this.OnScrollWheel(scrollWheelEventRef);
                return 0;

            case WindowMessage.WM_SETCURSOR:
            {
                // LOWORD(lParam) == hit-test code
                int hitTest = (short)lParam.LoWord;

                if (hitTest == (int)HitTest.HTCLIENT)
                {
                    var arrow = Win32.User32.LoadCursor(0, (int)Win32.User32.SystemCursor.Arrow);
                    Win32.User32.SetCursor(arrow);
                    return 1;
                }

                // Let Windows handle resize / caption / etc
                return this.Hwnd.DefWindowProc(uMsg, wParam, lParam);
            }
        }

        return this.Hwnd.DefWindowProc(uMsg, wParam, lParam);
    }

    private void OnMouseMove(MouseMoveEventRef mouseMoveEventRef) =>
        this.Abstract.OnMouseMove(ref mouseMoveEventRef);

    protected virtual void OnScrollWheel(ScrollWheelEventRef scrollWheelEventRef) =>
        this.Abstract.OnScrollWheel(ref scrollWheelEventRef);

    private void EnsureRendererInitialized()
    {
        if (this.Renderer is D2DComp d2dComp)
        {
            d2dComp.EnsureInitialized(this.Hwnd);
        }
    }

    public void Show()
    {
        this.Hwnd.ShowWindow();
        this.Hwnd.UpdateWindow();

        // Initialize swapchain/DComp outside WM_PAINT so the run loop can
        // use the frame latency handle to pace frames.
        this.EnsureRendererInitialized();
    }

    public virtual void Invalidate() => this.invalid = true;

    public void OnAnimationFrame(ref FrameEventRef @event) =>
        this.Abstract.OnAnimationFrame(ref @event);

    public void Render()
    {
        if (this.invalid)
        {
            this.invalid = false;
            this.Renderer.Render();
        }
    }

    internal void Render(RenderEventRef render)
    {
        // Console.WriteLine("Render()");
        this.Abstract.Render(ref render);
    }

    internal void OnAnimationFrame(FrameEventRef animationFrame)
    {
        this.previous = animationFrame.Previous;
        this.next = animationFrame.Next;
        this.Abstract.OnAnimationFrame(ref animationFrame);
    }

    private static void SetLevel(HWND hwnd, DesktopWindowLevel level)
    {
        HWND insertAfter = level == DesktopWindowLevel.Normal
            ? HWND.HWND_NOTOPMOST
            : HWND.HWND_TOPMOST;

        HWND.SetWindowPos(
            hwnd,
            insertAfter,
            0, 0, 0, 0,
            SetWindowPosFlags.SWP_NOMOVE |
            SetWindowPosFlags.SWP_NOSIZE |
            SetWindowPosFlags.SWP_NOACTIVATE);
    }

    public static float PrimaryMonitorDPIScale => GetDpiForSystem() / 96f;

    private void UpdateDpiScale()
    {
        this.dpiScale = (NFloat)this.Hwnd.DPIScale;
        this.invDpiScale = (NFloat)1.0 / this.dpiScale;
    }

    private Point ToDip(Point p) => new Point(p.X * this.invDpiScale, p.Y * this.invDpiScale);

    private Rect ToDip(Rect r) => new Rect(
        r.X * this.invDpiScale,
        r.Y * this.invDpiScale,
        r.Width * this.invDpiScale,
        r.Height * this.invDpiScale);

    private Point GetMousePosDip(LPARAM lParam) =>
        lParam.MousePosition * this.invDpiScale;

    private Rect GetClientRectDip(HWND hWnd)
    {
        hWnd.GetClientRect(out var rc);
        return this.ToDip(new Rect(0, 0, rc.Right - rc.Left, rc.Bottom - rc.Top));
    }

    private void RaiseMouseDown(MouseButton button, Point pos)
    {
        var evRef = new MouseDownEventRef()
        {
            Position = pos,
            Button = button
        };
        this.Abstract.OnMouseDown(ref evRef);
    }

    private void RaiseMouseUp(MouseButton button, Point pos)
    {
        var evRef = new MouseUpEventRef()
        {
            Position = pos,
            Button = button
        };
        this.Abstract.OnMouseUp(ref evRef);
    }
}
