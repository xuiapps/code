using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }
}
