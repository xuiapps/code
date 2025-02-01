using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/appkit/nsautoresizingmaskoptions?language=objc
    /// NSUInteger - treat as nuint.
    /// </summary>
    [Flags]
    public enum NSAutoresizingMaskOptions : uint
    {
        NotSizable = 0,
        MinXMargin = 1,
        WidthSizable = 2,
        MaxXMargin = 4,
        MinYMargin = 8,
        HeightSizable = 16,
        MaxYMargin = 32
    }
}