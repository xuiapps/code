using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes depth-stencil state.
    /// Mirrors <c>D3D11_DEPTH_STENCIL_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DepthStencilDesc
    {
        public int DepthEnable;
        public uint DepthWriteMask;   // 0 = ZERO, 1 = ALL
        public uint DepthFunc;        // 4 = LESS
        public int StencilEnable;
        public byte StencilReadMask;
        public byte StencilWriteMask;
        public DepthStencilOpDesc FrontFace;
        public DepthStencilOpDesc BackFace;
    }
}
