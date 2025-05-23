using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class Foundation
{
    public const string FoundationLib = "/System/Library/Frameworks/Foundation.framework/Foundation";

    public static readonly nint Lib;

    static Foundation()
    {
        Lib = NativeLibrary.Load(FoundationLib);
    }

    [LibraryImport(FoundationLib)]
    public static partial nint NSStringFromClass(nint cls);

    [LibraryImport(FoundationLib)]
    public static partial nint NSClassFromString(nint name);

    // [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
    // public static partial void objc_msgSend(nint obj, nint sel, NSRect rect);

    // [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
    // public static partial void objc_msgSend(nint obj, nint sel, NSSize rect);
}