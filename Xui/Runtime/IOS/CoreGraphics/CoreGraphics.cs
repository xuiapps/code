using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    public const string CoreGraphicsLib = "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";

    public static readonly nint Lib;

    static CoreGraphics()
    {
        Lib = NativeLibrary.Load(CoreGraphicsLib);
    }


    [LibraryImport(CoreGraphicsLib)]
    private static partial void CGPathRelease(nint path);
}
