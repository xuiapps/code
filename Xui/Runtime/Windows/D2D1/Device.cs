using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Device : Unknown
    {
        public static new readonly Guid IID = new Guid("47dd575d-ac05-4cdd-8049-9b02cd16f44c");

        public Device(void* ptr) : base(ptr)
        {
        }

        public unsafe DeviceContext CreateDeviceContext(DeviceContextOptions deviceContextOptions)
        {
            void* ppv;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, DeviceContextOptions, void**, int>)(this[4]))(this, deviceContextOptions, &ppv));
            return new DeviceContext(ppv);
        }
    }
}