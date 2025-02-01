using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Resource : Unknown
    {
        public static new readonly Guid IID = new Guid("2cd90691-12e2-11dc-9fed-001143a055f9");

        public Resource(void* ptr) : base(ptr)
        {
        }
    }
}