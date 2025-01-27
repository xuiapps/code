namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    /// <summary>
    /// https://developer.apple.com/documentation/appkit/nsvisualeffectstate?language=objc
    /// NSInteger - treat as nint
    /// </summary>
    public enum NSVisualEffectState : int
    {
        FollowsWindowActiveState = 0,
        Active = 1,
        Inactive = 2
    }
}