using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class DrawingStateBlock : Resource
    {
        public static new readonly Guid IID = new Guid("28506e39-ebf6-46a1-bb47-fd85565ab957");

        public DrawingStateBlock(void* ptr) : base(ptr)
        {
        }
    }
}