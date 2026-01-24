using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class COM
{
    public unsafe class Unknown : IDisposable
    {
        public static uint AddRef(void* ptr) => ((delegate* unmanaged<void*, uint>)(*(*(void***)ptr + 1)))(ptr);

        public static uint Release(void* ptr) => ((delegate* unmanaged<void*, uint>)(*(*(void***)ptr + 2)))(ptr);

        public static readonly Guid IID = new Guid("00000000-0000-0000-C000-000000000046");

        public readonly void* Ptr;

        public Unknown(void* ptr)
        {
            this.Ptr = ptr;
        }

        public void* QueryInterface(in Guid refId)
        {
            void* ppv;
            fixed (Guid* pIID = &refId)
            {
                Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, Guid*, void**, int>)this[0])(this, pIID, &ppv));
            }
            return ppv;
        }

        public static void* QueryInterface(void* comPtr, in Guid refId)
        {
            if (comPtr == null)
            {
                return null;
            }

            void* ppv;
            fixed (Guid* pIID = &refId)
            {
                // vtbl[0] is QueryInterface
                var qi = (delegate* unmanaged[MemberFunction]<void*, Guid*, void**, int>)(*(*(void***)comPtr + 0));
                Marshal.ThrowExceptionForHR(qi(comPtr, pIID, &ppv));
            }
            return ppv;
        }

        public uint AddRef() => ((delegate* unmanaged[MemberFunction]<void*, uint>)this[1])(this);
        public uint Release() => ((delegate* unmanaged[MemberFunction]<void*, uint>)this[2])(this);

        public static implicit operator void*(Unknown? unknown) => unknown == null ? null : unknown.Ptr;
        public void* this[uint slot] => *(*(void***)this.Ptr + slot);

        public void Dispose()
        {
            this.Release();
            GC.SuppressFinalize(this);
        }

        ~Unknown()
        {
            Debug.WriteLine($"Reached to finalizer for {this.GetType().FullName}. Treat as resource and call Dispose.");
            this.Release();
        }
    }
}