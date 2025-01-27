using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Factory : DXGI.Object
    {
        public static new readonly Guid IID = new Guid("7b7166ec-21c7-44ae-b21a-c9ae321ae369");

        public Factory(void* ptr) : base(ptr)
        {
        }
    }
}