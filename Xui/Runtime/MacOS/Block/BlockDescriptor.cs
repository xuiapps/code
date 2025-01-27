using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public partial class Block
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BlockDescriptor
    {
        [FieldOffset(0)]
        public uint Reserved;
        [FieldOffset(8)]
        public uint Size;

        [FieldOffset(16)]
        public nint CopyFPtr;
        [FieldOffset(24)]
        public nint DisposeFPtr;
        [FieldOffset(32)]
        public nint Signature;
    }
}