using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;d3d11.h&gt; in the d3d11.dll lib.
/// </summary>
public static partial class D3D11
{
    public unsafe class Device : Unknown
    {
        public static new readonly Guid IID = new Guid("db6f6ddb-ac77-4e88-8253-819df9bbf140");

        public Device(void* ptr) : base(ptr)
        {
        }
    }
}
