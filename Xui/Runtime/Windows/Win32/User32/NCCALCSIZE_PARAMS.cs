namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    public unsafe struct NCCALCSIZE_PARAMS
    {
        public RECT r1;
        public RECT r2;
        public RECT r3;

        // Incomplete ... PWINDOWPOS lppos;
    }
}
