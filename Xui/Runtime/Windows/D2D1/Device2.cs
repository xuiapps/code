using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Device2 : Device1
    {
        public static new readonly Guid IID = new Guid("a44472e1-8dfb-4e60-8492-6e2861c9ca8b");

        public Device2(void* ptr) : base(ptr)
        {
        }
    }
}