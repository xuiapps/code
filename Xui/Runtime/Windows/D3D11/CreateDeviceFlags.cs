using System;

namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    [Flags]
    public enum CreteDeviceFlags : uint
    {
        SingleThreaded = 0x1,
        Debug = 0x2,
        SwitchToRef = 0x4,
        PreventInternalThreadingOptimizations = 0x8,
        BGRASupport = 0x20,
        Debuggable = 0x40,
        PreventAlteringLayerSettingsFromRegistry = 0x80,
        DisableGpuTimeout = 0x100,
        VideoSupport = 0x800
    }
}