using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DComp
{
    public unsafe class Target : Unknown
    {
        public static new readonly Guid IID = new Guid("eacdd04c-117e-4e17-88f4-d1b12b0e3d89");

        public Target(void* ptr) : base(ptr)
        {
        }

        public void SetRoot(Visual visual) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, int> )this[3])(this, visual));
    }
}
