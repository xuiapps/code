using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes a buffer resource.
    /// Mirrors <c>D3D11_BUFFER_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BufferDesc
    {
        public uint ByteWidth;
        public Usage Usage;
        public uint BindFlags;
        public uint CPUAccessFlags;
        public uint MiscFlags;
        public uint StructureByteStride;
    }
}
