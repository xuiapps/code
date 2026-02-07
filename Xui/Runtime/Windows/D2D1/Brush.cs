using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class Brush : Resource
    {
        public static new readonly Guid IID = new Guid("2cd906a8-12e2-11dc-9fed-001143a055f9");

        public Brush(void* ptr) : base(ptr)
        {
        }
    }
}