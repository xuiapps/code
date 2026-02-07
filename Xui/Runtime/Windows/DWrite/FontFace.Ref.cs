using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFace : COM.Unknown
    {
        public ref struct Ref : IDisposable
        {
            private void* ptr;

            public Ref(void* ptr)
            {
                this.ptr = ptr;
            }

            private void* this[uint slot] => *(*(void***)this.ptr + slot);

            public static implicit operator void*(Ref r) => r.ptr;

            public void* QueryInterface(in Guid refId)
            {
                void* ppv;
                fixed (Guid* pIID = &refId)
                {
                    Marshal.ThrowExceptionForHR(
                        ((delegate* unmanaged[MemberFunction]<void*, Guid*, void**, int>)this[0])(
                            this.ptr, pIID, &ppv));
                }
                return ppv;
            }

            public void Dispose()
            {
                if (this.ptr != null)
                {
                    COM.Unknown.Release(this.ptr);
                    this.ptr = null;
                }
            }
        }
    }
}
