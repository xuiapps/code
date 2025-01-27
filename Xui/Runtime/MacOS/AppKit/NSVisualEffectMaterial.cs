namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/objectivec/nsuinteger
    /// NSUInteger - treat as nint
    /// </summary>
    public enum NSVisualEffectMaterial : int
    {
        Titlebar = 3,
        Selection = 4,
        Menu = 5,
        Popover = 6,
        Sidebar = 7,
        HeaderView = 10,
        Sheet = 11,
        WindowBackground = 12,
        HUDWindow = 13,
        FullScreenUI = 15,
        ToolTip = 17,
        ContentBackground = 18,
        UnderWindowBackground = 21,
        UnderPageBackground = 22
    }
}