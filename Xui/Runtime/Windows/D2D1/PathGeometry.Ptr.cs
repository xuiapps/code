using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class PathGeometry : Geometry
    {
        public struct Ptr : IDisposable
        {
            private void* ptr;

            public Ptr(void* ptr)
            {
                this.ptr = ptr;
            }

            private void* this[uint slot] => *(*(void***)this.ptr + slot);

            public static implicit operator void*(Ptr p) => p.ptr;

            public bool IsNull => ptr == null;

            public GeometrySink.Ptr Open()
            {
                void* sink;
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[17])(this.ptr, &sink));
                return new GeometrySink.Ptr(sink);
            }

            public void Dispose()
            {
                if (ptr != null)
                {
                    COM.Unknown.Release(ptr);
                    ptr = null;
                }
            }
        }
    }
}
