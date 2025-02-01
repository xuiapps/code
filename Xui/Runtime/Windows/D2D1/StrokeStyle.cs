using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class StrokeStyle : Resource
    {
        public static new readonly Guid IID = new Guid("2cd9069d-12e2-11dc-9fed-001143a055f9");

        public StrokeStyle(void* ptr) : base(ptr)
        {
        }
    }
}