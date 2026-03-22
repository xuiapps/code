using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes the blend state.
    /// Mirrors <c>D3D11_BLEND_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct BlendDesc
    {
        public int AlphaToCoverageEnable;
        public int IndependentBlendEnable;
        public fixed byte RenderTarget[8 * 32]; // 8 RenderTargetBlendDesc
    }
}
