namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/objectivec/nsuinteger
    /// NSUInteger - treat as nuint
    /// </summary>
    public enum NSWindowStyleMask : uint
    {
        Borderless = 0,
        Titled = 1 << 0,
        Closable = 1 << 1,
        Miniaturizable = 1 << 2,
        Resizable = 1 << 3,
        UtilityWindow = 1 << 4,
        DocModalWindow = 1 << 6,
        NonactivatingPanel = 1 << 7,
        UnifiedTitleAndToolbar = 1 << 12,
        HUDWindow = 1 << 13,
        FullScreen = 1 << 14,
        FullSizeContentView = 1 << 15,
    }
}