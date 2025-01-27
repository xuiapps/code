using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Device : Unknown
    {
        public static new readonly Guid IID = new Guid("54ec77fa-1377-44e6-8c32-88fd5f44c84c");

        public Device(void* ptr) : base(ptr)
        {
        }
    }
}