using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public unsafe partial class TextLayout : TextFormat
    {
        private static new readonly Guid IID = new Guid("53737037-6d14-410b-9bfe-0b182bb70961");

        public TextLayout(void* ptr) : base(ptr)
        {
        }

        public TextMetrics GetTextMetrics()
        {
            TextMetrics textMetrics;
            Marshal.ThrowExceptionForHR(((delegate* unmanaged[MemberFunction]<void*, TextMetrics*, int>)this[60])(this, &textMetrics));
            return textMetrics;
        }
    }
}