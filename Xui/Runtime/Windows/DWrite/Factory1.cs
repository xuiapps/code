using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;dwrite.h&gt; in the dwrite.dll lib.
/// </summary>
public static partial class DWrite
{
    public unsafe partial class Factory1 : Factory
    {
        private static new readonly Guid IID = new Guid("30572F99-DAC6-41DB-A16E-0486307E606A");

        private static void* CreateFactory()
        {
            Marshal.ThrowExceptionForHR(DWriteCreateFactory(FactoryType.Isolated, in IID, out var ppIFactory));
            return ppIFactory;
        }

        public Factory1() : base(CreateFactory())
        {
        }

        public Factory1(void* ptr) : base(ptr)
        {
        }
    }
}