using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextFormat : Unknown
    {
        private static new readonly Guid IID = new Guid("9c906818-31d7-4fd3-a151-7c5e225db55a");

        public TextFormat(void* ptr) : base(ptr)
        {
        }

        public void SetTextAlignment(TextAlignment alignment)
        {
            Marshal.ThrowExceptionForHR(
                ((delegate* unmanaged[MemberFunction]<void*, TextAlignment, int>)this[3])(
                    this,
                    alignment));
        }
    }
}