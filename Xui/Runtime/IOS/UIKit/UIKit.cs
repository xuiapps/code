using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public const string UIKitLib = "/System/Library/Frameworks/UIKit.framework/UIKit";

    public static readonly nint Lib;

    public static readonly Protocol UIApplicationDelegate = new Protocol("UIApplicationDelegate");

    static UIKit()
    {
        Lib = NativeLibrary.Load(UIKitLib);
    }

    [LibraryImport(UIKitLib)]
    public static partial int UIApplicationMain(int argc, nint argv, nint principalClassName, nint delegateClassName);

    [LibraryImport(UIKitLib)]
    public static partial nint NSSelectorFromString(nint selector);

    [LibraryImport(UIKitLib, EntryPoint="UIGraphicsGetCurrentContext")]
    public static partial nint UIGraphicsGetCurrentContext();
}