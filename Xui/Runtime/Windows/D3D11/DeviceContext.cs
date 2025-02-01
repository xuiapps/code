
using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static unsafe partial class D3D11
{
    public unsafe class DeviceContext : DeviceChild
    {
        public static new readonly Guid IID = new Guid("c0bfa96c-e089-44fb-8eaf-26f8796190da");

        public DeviceContext(void* ptr) : base(ptr)
        {
        }

        public void ClearState() =>
            ((delegate* unmanaged[MemberFunction]<void*, void> )this[110])(this);
    }
}