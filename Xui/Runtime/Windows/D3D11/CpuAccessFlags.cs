using System;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    /// <summary>
    /// Specifies the types of CPU access allowed for a resource.
    /// Mirrors <c>D3D11_CPU_ACCESS_FLAG</c>.
    /// </summary>
    [Flags]
    public enum CpuAccessFlags : uint
    {
        None = 0,
        Write = 0x10000,
        Read = 0x20000,
    }
}
