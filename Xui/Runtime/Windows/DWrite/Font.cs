using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class Font : Unknown
    {
        private static new readonly Guid IID = new Guid("acd16696-8c14-4f5d-877e-fe3fc1d32737");

        public Font(void* ptr) : base(ptr)
        {
        }

        public FontFace CreateFontFace()
        {
            void* fontFace;
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[13])(
                    this, &fontFace));
            return new FontFace(fontFace);
        }
    }
}