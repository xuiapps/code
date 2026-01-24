using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class FontFace : Unknown 
    {
        private static new readonly Guid IID = new Guid("5f49804d-7024-4d43-bfa9-d25984f53849");
        
        public FontFace(void* ptr) : base(ptr)
        {
        }

        public FontFaceType GetFaceType()
        {
            return (FontFaceType)((delegate* unmanaged[MemberFunction]<void*, int>)this[3])(this);
        }

        public ushort GetGlyphCount()
        {
            return ((delegate* unmanaged[MemberFunction]<void*, ushort>)this[9])(this);
        }
    }
}