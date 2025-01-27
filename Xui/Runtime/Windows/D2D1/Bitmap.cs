using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Bitmap : Image
    {
        public static new readonly Guid IID = new Guid("a2296057-ea42-4099-983b-539fb6505426");

        public Bitmap(void* ptr) : base(ptr)
        {
        }
    }
}