using System;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    [Flags]
    public enum NSApplicationPresentationOptions : ulong
    {
        Default = 0,
        AutoHideToolbar = 1 << 11,
    }
}
