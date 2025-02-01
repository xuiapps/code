using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [StructLayout(LayoutKind.Explicit, Size=32)]
    public struct BitmapProperties1
    {
        [FieldOffset(0)]
        public PixelFormat PixelFormat;
        [FieldOffset(8)]
        public float DpiX;
        [FieldOffset(12)]
        public float DpiY;
        [FieldOffset(16)]
        public BitmapOptions BitmapOptions;

        [FieldOffset(16)]
        public nint ColorContext;

        [FieldOffset(24)]
        public nint __TODO;
    }
}
