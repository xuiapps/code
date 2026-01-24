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

            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, uint, void**, int>)this[4])(
                    this, index, &fontFamily));

            return new FontFamily(fontFamily);
        }

        public void FindFamilyName(string familyName, out uint index, out bool exists)
        {
            uint idx = 0;
            int ex = 0;

            fixed (void* familyNamePtr = &global::System.Runtime.InteropServices.Marshalling.Utf16StringMarshaller.GetPinnableReference(familyName))
            {
                Marshal.ThrowExceptionForHR(
                    ((delegate* unmanaged[MemberFunction]<void*, void*, uint*, int*, int>)this[5])(
                        this, familyNamePtr, &idx, &ex));
            }

            index = idx;
            exists = ex != 0;
        }
    }
}