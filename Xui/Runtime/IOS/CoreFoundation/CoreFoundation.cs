using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public const string CoreFoundationLib = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

    public static readonly nint Lib;

    static CoreFoundation()
    {
        Lib = NativeLibrary.Load(CoreFoundationLib);
    }

    public static readonly Sel UTF8StringSel = new Sel("UTF8String");

    [LibraryImport(CoreFoundationLib)]
    public static partial void CFRelease(nint obj);

    [LibraryImport(CoreFoundationLib)]
    public static partial nint CFGetRetainCount(nint obj);

    [LibraryImport(CoreFoundationLib)]
    public static partial nuint CFHash(nint cf);

    [LibraryImport(CoreFoundationLib)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static partial bool CFEqual(nint cf1, nint cf2);

    [LibraryImport(CoreFoundationLib)]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string CFStringGetCStringPtr(nint obj);

    [LibraryImport(CoreFoundationLib)]
    public static partial nint CFStringGetCString(nint obj);

    [LibraryImport(CoreFoundationLib)]
    public static partial nint CFRetain(nint obj);

    /// <summary>
    /// Creates a CFStringRef from C# string.
    /// You need to manually release after use.
    /// <exception cref="ObjCException"></exception>
    public static nint CFString(string? str)
    {
        if (str == null)
        {
            return 0;
        }

        nint cfStr = CFStringCreateWithCString(nint.Zero, str, kCFStringEncodingUTF8);
        if (cfStr == 0)
        {
            throw new ObjCException("Failed to create new CFString.");
        }

        return cfStr;
    }

    public static string? ToString(nint cfStringRef)
    {
        return Marshal.PtrToStringUTF8(objc_msgSend_retIntPtr(cfStringRef, UTF8StringSel));
    }

    [LibraryImport(CoreFoundationLib)]
    private static partial nint CFStringCreateWithCString(nint allocator, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, uint encoding);

    private const uint kCFStringEncodingUTF8 = 0x08000100;
}