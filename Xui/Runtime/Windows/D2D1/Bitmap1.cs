using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Bitmap1 : Bitmap
    {
        public static new readonly Guid IID = new Guid("a898a84c-3873-4588-b08b-ebbf978df041");

        public Bitmap1(void* ptr) : base(ptr)
        {
        }
    }
}