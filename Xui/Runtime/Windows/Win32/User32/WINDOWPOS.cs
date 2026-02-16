using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public nint hwnd;             // window being moved/resized
        public nint hwndInsertAfter;  // z-order
        public int x;                 // new window X (screen coords)
        public int y;                 // new window Y
        public int cx;                // new width
        public int cy;                // new height
        public uint flags;            // SWP_* flags
    }
}
