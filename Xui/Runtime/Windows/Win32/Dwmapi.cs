using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class Dwmapi
{
    public const string DwmapiLib = "dwmapi.dll";

    public const uint DWMWA_CAPTION_COLOR = 35;
    public const uint DWMWA_TEXT_COLOR = 36;
    public const uint DWMWA_SYSTEMBACKDROP_TYPE = 38;

    /// <summary>COLORREF value that indicates "no color" / transparent for DWM attributes.</summary>
    public const int DWMWA_COLOR_NONE = unchecked((int)0xFFFFFFFE);

    /// <summary>Mica backdrop (Windows 11 22H2+).</summary>
    public const int DWMSBT_MAINWINDOW = 2;

    /// <summary>Acrylic backdrop (Windows 11 22H2+).</summary>
    public const int DWMSBT_TRANSIENTWINDOW = 3;

    /// <summary>Mica Alt / Tabbed backdrop (Windows 11 22H2+).</summary>
    public const int DWMSBT_TABBEDWINDOW = 4;

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }

    [LibraryImport(DwmapiLib)]
    public static partial int DwmExtendFrameIntoClientArea(nint hwnd, ref MARGINS pMarInset);

    [LibraryImport(DwmapiLib)]
    public static partial int DwmSetWindowAttribute(nint hwnd, uint dwAttribute, ref int pvAttribute, int cbAttribute);

    [LibraryImport(DwmapiLib)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool DwmDefWindowProc(
        nint hwnd,
        User32.WindowMessage msg,
        WPARAM wParam,
        LPARAM lParam,
        out LRESULT plResult);

    [DllImport("dwmapi.dll")]
    public static extern int DwmFlush();
}
