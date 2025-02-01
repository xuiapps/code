using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DComp
{
    public unsafe class Visual : Unknown
    {
        public static new readonly Guid IID = new Guid("4d93059d-097b-4651-9a60-f0f25116e2f3");

        public Visual(void* ptr) : base(ptr)
        {
        }

        public void SetContent(Unknown content) =>
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, void*, int> )this[15])(this, content));
    }
}
