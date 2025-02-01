using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    [Flags]
    public enum SwapChainFlags : uint
    {
        NonInterpolated = 1,
        AllowModeSwitch = 2,
        GdiCompatible = 4,
        RestrictedContent = 8,
        RestrictSharedResourceDriver = 16,
        DisplayOnly = 32,
        FrameLatencyWaitableObject = 64,
        ForegroundLayet = 128,
        FullscreenVideo = 256,
        YUVVideo = 512,
        HWProtected = 1024,
        AllowTearing = 2048,
        RestrictedToAllHolographicDisplays = 4096
    }
}