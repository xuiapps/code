using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.User32;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32.Types;
using System;
using System.Collections.Generic;
using Xui.Core.Abstract.Events;
using Xui.Core.Math2D;
using static Xui.Core.Abstract.IWindow.IDesktopStyle;

namespace Xui.Runtime.Windows.Actual;

public partial class Win32Window : Xui.Core.Actual.IWindow
{
    public const uint WM_ANIMATION_FRAME_MSG = 0x0401;

    [ThreadStatic]
    private static Win32Window? constructedInstanceOnStack;

    private static Dictionary<HWND, Win32Window> HwndToWindow = new Dictionary<HWND, Win32Window>();

    private TimeSpan previous;
    private TimeSpan next;

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
        var msg = (WindowMessage)uMsg;

        if (this.Renderer.HandleOnMessage(hWnd, uMsg, wParam, lParam, out var result))
        {
            return result;
        }

        switch (msg)
        {
            case WindowMessage.WM_CREATE:
                break;

            case WindowMessage.WM_DESTROY:
                break;

            case WindowMessage.WM_NCHITTEST:
                POINT win32ClientPoint = new POINT() { X = lParam.LoWord, Y = lParam.HiWord };
                this.Hwnd.ScreenToClient(ref win32ClientPoint);
                Point point = (win32ClientPoint.X, win32ClientPoint.Y);

                hWnd.GetClientRect(out var rc);
                Rect rect = new Rect(0, 0, rc.Right - rc.Left, rc.Bottom - rc.Top);

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

            case WindowMessage.WM_PAINT:
                return 0;

            case WindowMessage.WM_MOUSEMOVE:
                MouseMoveEventRef mouseMoveEventRef = new MouseMoveEventRef()
                {
                    Position = lParam.MousePosition
                };

                this.OnMouseMove(mouseMoveEventRef);
                return 0;

            case WindowMessage.WM_MOUSEWHEEL:
                ScrollWheelEventRef scrollWheelEventRef = new ScrollWheelEventRef()
                {
                    Delta = wParam.WheelDelta
                };

                this.OnScrollWheel(scrollWheelEventRef);
                return 0;
        }

        return this.Hwnd.DefWindowProc(uMsg, wParam, lParam);
    }

    private void OnMouseMove(MouseMoveEventRef mouseMoveEventRef) =>
        this.Abstract.OnMouseMove(ref mouseMoveEventRef);

    protected virtual void OnScrollWheel(ScrollWheelEventRef scrollWheelEventRef) =>
        this.Abstract.OnScrollWheel(ref scrollWheelEventRef);

    public void Show()
    {
        this.Hwnd.ShowWindow();
        this.Hwnd.UpdateWindow();
    }

    public virtual void Invalidate() => this.Hwnd.Invalidate();

    public void OnCompositionFrame() => this.Renderer.CheckForCompositionFrame();

    internal void Render(RenderEventRef render) => this.Abstract.Render(ref render);

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
}
