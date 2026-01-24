using System;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [Flags]
    public enum TrackMouseEventFlags : uint
    {
        TME_HOVER  = 0x00000001,
        TME_LEAVE  = 0x00000002,
        TME_CANCEL = 0x80000000
    }
}