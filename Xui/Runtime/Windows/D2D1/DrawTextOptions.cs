using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [Flags]
    public enum DrawTextOptions
    {
        None = 0x00000000,
        NoSnap = 0x00000001,
        Clip = 0x00000002,
        EnableColorFont = 0x00000004,
        DisableColorBitmapSnapping = 0x00000008,
    }
}
