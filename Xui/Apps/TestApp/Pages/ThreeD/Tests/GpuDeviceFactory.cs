using Xui.GPU.Hardware;

namespace Xui.Apps.TestApp.Pages.ThreeD.Tests;

/// <summary>
/// Factory for creating a GPU hardware device based on the current platform.
/// Returns null if no hardware GPU backend is available for this platform.
/// </summary>
public static class GpuDeviceFactory
{
    /// <summary>
    /// Tries to create a hardware GPU device appropriate for the current platform.
    /// Returns null if hardware GPU is not available.
    /// </summary>
    public static IGpuDevice? TryCreate()
    {
        try
        {
#if WINDOWS
            return new Xui.GPU.Hardware.D3D11.D3D11GpuDevice();
#elif MACOS || IOS
            return new Xui.GPU.Hardware.Metal.MetalGpuDevice();
#else
            return null;
#endif
        }
        catch
        {
            // Hardware GPU initialization failed (e.g., missing driver, unsupported hardware).
            // Return null to indicate the hardware backend is unavailable; callers should fall back
            // to software rendering. The exception is intentionally suppressed here because GPU
            // availability is treated as a capability discovery, not an error condition.
            return null;
        }
    }
}
