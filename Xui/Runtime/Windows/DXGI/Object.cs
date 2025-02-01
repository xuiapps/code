using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public unsafe class Object : Unknown
    {
        public static new readonly Guid IID = new Guid("aec22fb8-76f3-4639-9be0-28eb43a67a2e");

        public Object(void* ptr) : base(ptr)
        {
        }
    }
}