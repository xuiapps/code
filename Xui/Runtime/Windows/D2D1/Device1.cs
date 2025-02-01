using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Device1 : Device
    {
        public static new readonly Guid IID = new Guid("d21768e1-23a4-4823-a14b-7c3eba85d658");

        public Device1(void* ptr) : base(ptr)
        {
        }
    }
}