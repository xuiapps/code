using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Factory1 : Factory
    {
        public static new readonly Guid IID = new Guid("770aae78-f26f-4dba-a829-253c83d1b387");

        public Factory1(void* ptr) : base(ptr)
        {
        }
    }
}