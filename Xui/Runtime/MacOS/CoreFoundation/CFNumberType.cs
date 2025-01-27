using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreFoundation
{
    public enum CFNumberType : long
    {
        SInt8 = 1,
        SInt16 = 2,
        SInt32 = 3,
        SInt64 = 4,
        Float32 = 5,
        Float64 = 6,
        Char = 7,
        Short = 8,
        Int = 9,
        Long = 10,
        LongLong = 11,
        Float = 12,
        Double = 13,
        CFIndex = 14,
        NSInteger = 15,
        CGFloat = 16,
        Max = 16
    }
}