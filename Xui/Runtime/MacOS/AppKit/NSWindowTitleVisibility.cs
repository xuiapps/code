using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/appkit/nswindowtitlevisibility?language=objc
    /// NSInteger - treat as nint.
    /// </summary>
    public enum NSWindowTitleVisibility : uint
    {
        Visible = 0,
        Hidden = 1
    }
}