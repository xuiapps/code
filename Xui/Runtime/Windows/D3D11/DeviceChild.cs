
using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static unsafe partial class D3D11
{
    public unsafe class DeviceChild : Unknown  
    {
        public static new readonly Guid IID = new Guid("1841e5c8-16b0-489b-bcc8-44cfb0d5deae");

        public DeviceChild(void* ptr) : base(ptr)
        {
        }
    }
}