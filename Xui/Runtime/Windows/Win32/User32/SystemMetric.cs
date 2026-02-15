using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    public enum SystemMetric : int
    {
        // Height of a caption area (title bar), in pixels.
        SM_CYCAPTION = 4,

        // Caption button width in pixels.
        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        SM_CXSIZE = 30,

        // caption button height
        SM_CYSIZE = 31,

        // resize frame thickness X
        SM_CXSIZEFRAME = 32,

        // Thickness of the sizing border around a resizable window, in pixels.
        // (Vertical border thickness.)
        SM_CYSIZEFRAME = 33,

        // extra padding
        SM_CXPADDEDBORDER = 92,  
    }
}
