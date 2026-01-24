using System;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFace1 : FontFace 
    {
        public static new readonly Guid IID = new Guid("a71efdb4-9fdb-4838-ad90-cfc3be8c3daf");
        
        public FontFace1(void* ptr) : base(ptr)
        {
        }

        public static FontFace1 FromFontFace(FontFace face)
        {
            if (face == null)
            {
                throw new ArgumentNullException(nameof(face));
            }

            void* p = face.QueryInterface(in IID);
            return new FontFace1(p);
        }

        public void GetMetrics(out FontMetrics1 metrics)
        {
            metrics = default;
            fixed (FontMetrics1* p = &metrics)
            {
                // IDWriteFontFace1::GetMetrics(DWRITE_FONT_METRICS1*)
                ((delegate* unmanaged[MemberFunction]<void*, FontMetrics1*, void>)this[18])(this, p);
            }
        }

        public bool IsMonospacedFont()
        {
            return ((delegate* unmanaged[MemberFunction]<void*, Win32.Types.BOOL>)this[22])(this);
        }
    }
}