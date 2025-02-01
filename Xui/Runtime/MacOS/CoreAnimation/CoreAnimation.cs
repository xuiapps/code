using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreAnimation
{
    public const string CoreAnimationLib = "/System/Library/Frameworks/QuartzCore.framework/QuartzCore";

    public static readonly nint Lib;

    static CoreAnimation()
    {
        Lib = NativeLibrary.Load(CoreAnimationLib);
    }
}