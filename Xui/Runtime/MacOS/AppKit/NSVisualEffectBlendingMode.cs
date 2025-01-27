namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/objectivec/nsuinteger
    /// NSUInteger - treat as nint
    /// </summary>
    public enum NSVisualEffectBlendingMode : int
    {
        BehindWindow = 0,
        WithinWindow = 1
    }
}