using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public partial class Block
{
    [StructLayout(LayoutKind.Sequential)]
    public ref struct BlockRef
    {
        private BlockLiteral block;

        public BlockRef(Action action)
        {
            this.block = new BlockLiteral()
            {
                Isa = Block.NSConcreteStackBlock,
                Flags = (int)BlockFlags.HasCopyDispose,
                Reserved = 0,
                FunctionPtr = Block.InvokeFPtr,
                BlockDescriptor = Block.StackBlockDescriptor,
                Handler = GCHandle.ToIntPtr(GCHandle.Alloc(action))
            };
        }
    }
}