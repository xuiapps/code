using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-wndclassexa
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WNDCLASSEXW
    {
        public uint cbSize;
        public WindowClassStyles styles;
        public nint lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public nint hInstance;
        public nint hIcon;
        public nint hCursor;
        public nint hbrBackground;
        public nint lpszMenuName;
        public nint lpszClassName;
        public nint hIconSm;
    }
}
