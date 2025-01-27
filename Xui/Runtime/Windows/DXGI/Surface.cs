using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Surface : Unknown
    {
        public static new readonly Guid IID = new Guid("cafcb56c-6ac3-4889-bf47-9e23bbd260ec");

        public Surface(void* ptr) : base(ptr)
        {
        }
    }
}