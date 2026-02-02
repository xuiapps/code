using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Adapter : Unknown
    {
        public static new readonly Guid IID = new Guid("2411e7e1-12ac-4ccf-bd14-9798e8534dc0");

        public Adapter(void* ptr) : base(ptr)
        {
        }

        public DXGI.Factory2 GetParentFactory2()
        {
            void* factory = null;
            var iid = DXGI.Factory2.IID;

            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, Guid*, void**, int>)this[6])(
                    this, &iid, &factory));

            return new DXGI.Factory2(factory);
        }
    }
}