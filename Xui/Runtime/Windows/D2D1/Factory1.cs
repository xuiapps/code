using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Factory1 : Factory
    {
        public static new readonly Guid IID = new Guid("bb12d362-daee-4b9a-aa1d-14ba401cfa1f");

        public Factory1() : base(CreateFactory(IID))
        {
        }

        public Factory1(void* ptr) : base(ptr)
        {
        }
    }
}