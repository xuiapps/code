using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextLayout1 : TextLayout
    {
        // IDWriteTextLayout1 IID
        public static new readonly Guid IID = new Guid("9064D822-80A7-465C-A986-DF65F78B8FEB");

        public TextLayout1(void* ptr) : base(ptr)
        {
        }

        public static TextLayout1 FromTextLayout(TextLayout layout)
        {
            void* p = COM.Unknown.QueryInterface((void*)layout, IID);
            if (p == null)
            {
                throw new NotSupportedException("IDWriteTextLayout1 is not available on this system.");
            }

            return new TextLayout1(p);
        }
    }
}
