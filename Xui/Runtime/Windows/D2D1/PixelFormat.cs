using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PixelFormat
    {
        public DXGI.Format Format;
        public AlphaMode AlphaMode;
    }
}