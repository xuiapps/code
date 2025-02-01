using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    [Flags]
    public enum Usage : uint
    {
        ShaderInput = 0x00000010U,
        RenderTargetOutput = 0x00000020U,
        BackBuffer = 0x00000040U,
        Shared = 0x00000080U,
        ReadOnly = 0x00000100U,
        DiscardOnPresent = 0x00000200U,
        UnorderedAccess = 0x00000400U      
    }
}