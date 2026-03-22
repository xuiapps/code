using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Provides access to subresource data.
    /// Mirrors <c>D3D11_MAPPED_SUBRESOURCE</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MappedSubresource
    {
        public void* pData;
        public uint RowPitch;
        public uint DepthPitch;
    }
}
