using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontCollection : Unknown
    {
        private static new readonly Guid IID = new Guid("a84cee02-3eea-4eee-a827-87c1a02a0fcc");

        public FontCollection(void* ptr) : base(ptr)
        {
        }

        public uint Count => GetFontFamilyCount();

        public uint GetFontFamilyCount() => ((delegate* unmanaged[MemberFunction]<void*, uint>)this[4])(this);

        public FontFamily GetFontFamily(uint index)
        {
            void* fontFamily;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[5])(this, &fontFamily));
            return new FontFamily(fontFamily);
        }
    }
}