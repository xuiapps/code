using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class SolidColorBrush : Brush
    {
        public static new readonly Guid IID = new Guid("2cd906a9-12e2-11dc-9fed-001143a055f9");

        public SolidColorBrush(void* ptr) : base(ptr)
        {
        }

        public ColorF Color
        {
            get
            {
                ColorF colorf;
                ((delegate* unmanaged[MemberFunction]<void*, ColorF*, void> )this[8])(this, &colorf);
                return colorf;
            }

            set
            {
                ColorF colorf = value;
                ((delegate* unmanaged[MemberFunction]<void*, ColorF*, void> )this[8])(this, &colorf);
            }
        }
    }
}