using System;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFace1 : FontFace
    {
        public new ref struct Ref : IDisposable
        {
            private void* ptr;

            public Ref(void* ptr)
            {
                this.ptr = ptr;
            }

            private void* this[uint slot] => *(*(void***)this.ptr + slot);

            public static implicit operator void*(Ref r) => r.ptr;

            public static FontFace1.Ref FromFontFace(FontFace.Ref face)
            {
                void* p = face.QueryInterface(in IID);
                if (p == null)
                {
                    throw new NotSupportedException("IDWriteFontFace1 is not available on this system.");
                }

                return new FontFace1.Ref(p);
            }

            public void GetMetrics(out FontMetrics1 metrics)
            {
                metrics = default;
                fixed (FontMetrics1* p = &metrics)
                {
                    ((delegate* unmanaged[MemberFunction]<void*, FontMetrics1*, void>)this[18])(this.ptr, p);
                }
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
