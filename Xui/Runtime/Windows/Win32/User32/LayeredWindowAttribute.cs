using System;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [Flags]
    public enum LayeredWindowAttribute : long
    {
        LWA_COLORKEY = 0x00000001,
        LWA_ALPHA = 0x00000002,
    }
}
