using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Output : DXGI.Object
    {
        public static new readonly Guid IID = new Guid("ae02eedb-c735-4690-8d52-5a8dc20213aa");

        public Output(void* ptr) : base(ptr)
        {
        }
    }
}