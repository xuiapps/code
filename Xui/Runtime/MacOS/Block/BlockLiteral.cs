using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public partial class Block
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BlockLiteral
    {
        public nint Isa;
        public int Flags;
        public int Reserved;
        public nint FunctionPtr;
        public nint BlockDescriptor;

        /// <summary>
        /// Extending the Objecitve-C literal with our custom field,
        /// to hold reference to our C# instance that will perform.
        /// </summary>
        public nint Handler;
    }
}