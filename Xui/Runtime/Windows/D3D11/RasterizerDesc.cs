using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes rasterizer state.
    /// Mirrors <c>D3D11_RASTERIZER_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RasterizerDesc
    {
        public uint FillMode;   // 3 = SOLID
        public uint CullMode;   // 1 = NONE, 2 = FRONT, 3 = BACK
        public int FrontCounterClockwise;
        public int DepthBias;
        public float DepthBiasClamp;
        public float SlopeScaledDepthBias;
        public int DepthClipEnable;
        public int ScissorEnable;
        public int MultisampleEnable;
        public int AntialiasedLineEnable;
    }
}
