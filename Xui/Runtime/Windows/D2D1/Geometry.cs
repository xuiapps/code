using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Geometry : Resource
    {
        public static new readonly Guid IID = new Guid("2cd906a1-12e2-11dc-9fed-001143a055f9");

        public Geometry(void* ptr) : base(ptr)
        {
        }
    }
}