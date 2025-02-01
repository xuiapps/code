

using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class GradientStopCollection : Resource
    {
        public static new readonly Guid IID = new Guid("2cd906a7-12e2-11dc-9fed-001143a055f9");

        public GradientStopCollection(void* ptr) : base(ptr)
        {
        }
    }
}