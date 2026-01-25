namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    /// <summary>https://learn.microsoft.com/en-us/windows/win32/winmsg/about-messages-and-message-queues#system-defined-messages</summary>
    public enum WindowMessage : uint
    {
        // Window Notifications
        // https://learn.microsoft.com/en-us/windows/win32/winmsg/window-notifications

        WM_NULL = 0x0000,
        WM_CREATE = 0x0001,
        WM_DESTROY = 0x0002,
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,

        WM_ERASEBKGND = 0x0014,

        WM_SETCURSOR = 0x0020,

        WM_ACTIVATEAPP = 0x001C,
        WM_CANCELMODE = 0x001F,
        WM_CHILDACTIVATE = 0x0022,
        WM_CLOSE = 0x0010,
        WM_COMPACTING = 0x0041,
        WM_ENABLE = 0x000A,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_GETICON = 0x007F,
        WM_GETMINMAXINFO = 0x0024,
        WM_INPUTLANGCHANGE = 0x0051,
        WM_INPUTLANGCHANGEREQUEST = 0x0050,
        WM_MOVING = 0x0216,
        WM_NCACTIVATE = 0x0086,
        WM_NCCALCSIZE = 0x0083,
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_QUERYDRAGICON = 0x0037,
        WM_QUERYOPEN = 0x0013,
        WM_QUIT = 0x0012,
        WM_SHOWWINDOW = 0x0018,
        WM_SIZING = 0x0214,
        WM_STYLECHANGED = 0x007D,
        WM_STYLECHANGING = 0x007C,
        WM_THEMECHANGED = 0x031A,
        WM_USERCHANGED = 0x0054,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_WINDOWPOSCHANGING = 0x0046,

        // Mouse Input Notifications
        // https://learn.microsoft.com/en-us/windows/win32/inputdev/mouse-input-notifications

        WM_CAPTURECHANGED = 0x0215,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MOUSEACTIVATE = 0x0021,
        WM_MOUSEHOVER = 0x02A1,
        WM_MOUSEHWHEEL = 0x020E,
        WM_MOUSELEAVE = 0x02A3,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_NCHITTEST = 0x0084,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCMBUTTONDBLCLK = 0x00A9,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMOUSEHOVER = 0x02A0,
        WM_NCMOUSELEAVE = 0x02A2,
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,

        // Keyboard Input Notifications
        // https://learn.microsoft.com/en-us/windows/win32/inputdev/keyboard-input-notifications

        WM_ACTIVATE = 0x0006,
        WM_APPCOMMAND = 0x0319,
        WM_CHAR = 0x0102,
        WM_DEADCHAR = 0x0103,
        WM_HOTKEY = 0x0312,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_KILLFOCUS = 0x0008,
        WM_SETFOCUS = 0x0007,
        WM_SYSDEADCHAR = 0x0107,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_UNICHAR = 0x0109,

        // Painting and Drawing
        // https://learn.microsoft.com/en-us/windows/win32/gdi/wm-paint

        WM_DISPLAYCHANGE = 0x7E,
        WM_NCPAINT = 0x85,
        WM_PAINT  = 0xF,
        WM_PRINT = 0x317,
        WM_PRINTCLIENT = 0x318,
        WM_SETREDRAW = 	0xB,
        WM_SYNCPAINT = 	0x88,

        // Keyboard Accelerator Notifications
        // https://learn.microsoft.com/en-us/windows/win32/menurc/keyboard-accelerator-notifications

        WM_INITMENUPOPUP = 0x0117,
        WM_MENUCHAR = 0x0120,
        WM_MENUSELECT = 0x011F,
        WM_SYSCHAR = 0x0106,
        WM_SYSCOMMAND = 0x0112,

        // Timer
        WM_TIMER = 0x0113,

        WM_DPICHANGED = 0x02E0,
    }
}
