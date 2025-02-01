namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/appkit/nswindowtitlevisibility?language=objc
    /// NSInteger - treat as nint.
    /// </summary>
    public enum NSWindowToolbarStyle : int
    {
        Automatic = 0,
        Expanded = 1,
        Preference = 2,
        Unified = 3,
        UnifiedCompact = 4
    }
}