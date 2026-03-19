using System;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// Bitmap info flags for CGBitmapContext and CGImage creation.
    /// </summary>
    [Flags]
    public enum CGBitmapInfo : uint
    {
        None = 0,

        // Alpha info (lowest 5 bits)
        AlphaInfoMask = 0x1F,
        AlphaNone = 0,
        AlphaPremultipliedLast = 1,
        AlphaPremultipliedFirst = 2,
        AlphaLast = 3,
        AlphaFirst = 4,
        AlphaNoneSkipLast = 5,
        AlphaNoneSkipFirst = 6,
        AlphaOnly = 7,

        // Byte order (bits 12-13)
        ByteOrderMask = 0x7000,
        ByteOrderDefault = 0 << 12,
        ByteOrder16Little = 1 << 12,
        ByteOrder32Little = 2 << 12,
        ByteOrder16Big = 3 << 12,
        ByteOrder32Big = 4 << 12,

        // Float components (bit 8)
        FloatComponents = 1 << 8,
        FloatInfoMask = 0xF00,
    }
}
