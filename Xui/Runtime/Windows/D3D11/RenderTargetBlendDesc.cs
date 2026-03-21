using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes the blend state for a render target.
    /// Mirrors <c>D3D11_RENDER_TARGET_BLEND_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderTargetBlendDesc
    {
        public int BlendEnable;
        public uint SrcBlend;
        public uint DestBlend;
        public uint BlendOp;
        public uint SrcBlendAlpha;
        public uint DestBlendAlpha;
        public uint BlendOpAlpha;
        public byte RenderTargetWriteMask;
    }
}
