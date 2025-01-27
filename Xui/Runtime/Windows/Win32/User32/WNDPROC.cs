using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int WNDPROC(HWND hwnd, WindowMessage uMsg, WPARAM wParam, LPARAM lParam);
}
