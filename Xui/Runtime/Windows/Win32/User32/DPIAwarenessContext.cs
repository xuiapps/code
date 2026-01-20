using System;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [Flags]
    public enum DPIAwarenessContext : long
    {
        DPI_AWARENESS_CONTEXT_UNAWARE = -1,
        DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2,
        DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3,
        DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4,
        DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED = -5
    }
}
