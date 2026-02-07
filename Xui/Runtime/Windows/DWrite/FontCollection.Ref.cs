using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontCollection : COM.Unknown
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

            public void FindFamilyName(string familyName, out uint index, out bool exists)
            {
                uint idx = 0;
                int ex = 0;

                fixed (void* familyNamePtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(familyName))
                {
                    Marshal.ThrowExceptionForHR(
                        ((delegate* unmanaged[MemberFunction]<void*, void*, uint*, int*, int>)this[5])(
                            this.ptr, familyNamePtr, &idx, &ex));
                }

                index = idx;
                exists = ex != 0;
            }

            public FontFamily.Ref GetFontFamily(uint index)
            {
                void* fontFamily;

                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, uint, void**, int>)this[4])(
                        this.ptr, index, &fontFamily));

                return new FontFamily.Ref(fontFamily);
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
