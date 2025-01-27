using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Factory3 : Factory2
    {
        public static new readonly Guid IID = new Guid("0869759f-4f00-413f-b03e-2bda45404d0f");

        public Factory3() : base(CreateFactory(IID))
        {
        }

        public Factory3(void* ptr) : base(ptr)
        {
        }

        public D2D1.Device2 CreateDevice(DXGI.Device device)
        {
            void* ppv;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, void**, int>)(this[31]))(this, device, &ppv));
            return new Device2(ppv);
        }
    }
}