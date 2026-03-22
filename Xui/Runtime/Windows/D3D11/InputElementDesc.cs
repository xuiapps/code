using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Describes a single element for the input-assembler stage.
    /// Mirrors <c>D3D11_INPUT_ELEMENT_DESC</c>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct InputElementDesc
    {
        public byte* SemanticName;
        public uint SemanticIndex;
        public DXGI.Format Format;
        public uint InputSlot;
        public uint AlignedByteOffset;
        public uint InputSlotClass;  // 0 = PER_VERTEX_DATA
        public uint InstanceDataStepRate;
    }
}
