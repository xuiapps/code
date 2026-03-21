using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes stencil operations that can be performed based on the results of stencil test.
    /// Mirrors <c>D3D11_DEPTH_STENCILOP_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DepthStencilOpDesc
    {
        public uint StencilFailOp;      // 1 = KEEP
        public uint StencilDepthFailOp; // 1 = KEEP
        public uint StencilPassOp;      // 1 = KEEP
        public uint StencilFunc;        // 8 = ALWAYS
    }
}
