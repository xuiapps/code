using System;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe class Image : Resource
    {
        public static new readonly Guid IID = new Guid("65019f75-8da2-497c-b32c-dfa34e48ede6");

        public Image(void* ptr) : base(ptr)
        {
        }
    }
}