using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFamily : COM.Unknown
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

            public Font.Ref GetFirstMatchingFont(FontWeight fontWeight, FontStretch fontStretch, FontStyle fontStyle)
            {
                void* font;

                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, FontWeight, FontStretch, FontStyle, void**, int>)this[7])(
                        this.ptr, fontWeight, fontStretch, fontStyle, &font));

                return new Font.Ref(font);
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
