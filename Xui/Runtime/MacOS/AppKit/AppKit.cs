using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public const string AppKitLib = "/System/Library/Frameworks/AppKit.framework/AppKit";

    public static readonly nint Lib;

    public static readonly Protocol NSApplicationDelegate = new Protocol(AppKit.Lib, "NSApplicationDelegate");

    static AppKit()
    {
        Lib = NativeLibrary.Load(AppKitLib);
    }

    [LibraryImport(AppKitLib)]
    public static partial nint NSSelectorFromString(nint selector);
}