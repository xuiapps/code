using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Defines the dimensions of a viewport.
    /// Mirrors <c>D3D11_VIEWPORT</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Viewport
    {
        public float TopLeftX;
        public float TopLeftY;
        public float Width;
        public float Height;
        public float MinDepth;
        public float MaxDepth;
    }
}
