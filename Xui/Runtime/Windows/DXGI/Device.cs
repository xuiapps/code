using System;
using System.Runtime.InteropServices;
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

        public DXGI.Adapter GetAdapter()
        {
            void* adapter;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[7])(this, &adapter));
            return new DXGI.Adapter(adapter);
        }
    }
}