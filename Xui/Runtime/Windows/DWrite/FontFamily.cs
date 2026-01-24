using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFamily : Unknown
    {
        private static new readonly Guid IID = new Guid("da20d8ef-812a-4c43-9802-62ec4abd7add");

        public FontFamily(void* ptr) : base(ptr)
        {
        }

        public Font GetFirstMatchingFont(FontWeight fontWeight, FontStretch fontStretch, FontStyle fontStyle)
        {
            void* font;

            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, FontWeight, FontStretch, FontStyle, void**, int>)this[7])(
                    this, fontWeight, fontStretch, fontStyle, &font));

            return new Font(font);
        }
    }
}