using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class SwapChain1 : SwapChain
    {
        public static new readonly Guid IID = new Guid("790a45f7-0d42-4876-983a-0a55cfe6f4aa");

        public SwapChain1(void* ptr) : base(ptr)
        {
        }
    }
}