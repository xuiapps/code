using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [Flags]
    public enum PathSegment : uint
    {
        None = 0x00000000,
        ForceUnstroked = 0x00000001,
        ForceRoundLineJoin = 0x00000002,
    }
}
