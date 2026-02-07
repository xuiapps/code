using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class PathGeometry : Geometry
    {
        public static new readonly Guid IID = new Guid("2cd906a5-12e2-11dc-9fed-001143a055f9");

        public PathGeometry(void* ptr) : base(ptr)
        {
        }

        public GeometrySink Open()
        {
            void* sink;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void**, int>)this[17])(this, &sink));
            return new GeometrySink(sink);
        }
    }
}