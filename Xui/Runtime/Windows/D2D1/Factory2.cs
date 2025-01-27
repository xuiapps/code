using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Factory2 : Factory1
    {
        public static new readonly Guid IID = new Guid("94f81a73-9212-4376-9c58-b16a3a0d3992");

        public Factory2() : base(CreateFactory(IID))
        {
        }

        public Factory2(void* ptr) : base(ptr)
        {
        }

        internal D2D1.Device1 CreateDevice(DXGI.Device dxgiDevice)
        {
            void* d2dDevice1;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, void**, int>)this[27])(this, dxgiDevice, &d2dDevice1));
            return new D2D1.Device1(d2dDevice1);
        }
    }
}