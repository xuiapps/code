using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    /// <summary>https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG {
        public nint hWnd;
        public WindowMessage message;
#pragma warning disable CS0169
        public WPARAM wParam;
        public LPARAM lParam;
        public DWORD time;
        public POINT pt;
        public DWORD lPrivate;
#pragma warning restore
    }
}
