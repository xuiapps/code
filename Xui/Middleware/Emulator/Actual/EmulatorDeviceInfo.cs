using System.Runtime.InteropServices;
using Xui.Core.DI;
using Xui.Middleware.Emulator.Devices;

namespace Xui.Middleware.Emulator.Actual;

/// <summary>Device characteristics of the logical device currently shown by an emulator window.</summary>
internal sealed class EmulatorDeviceInfo(EmulatorWindow window) : IDeviceInfo
{
    private DeviceProfile Device => window.CurrentDevice;

    public DevicePlatform Platform => Device.Brand switch
    {
        Brand.Apple => DevicePlatform.iOS,
        Brand.Android => DevicePlatform.Android,
        _ => DevicePlatform.Android,
    };

    public DeviceFormFactor FormFactor => Device.DeviceType switch
    {
        DeviceType.Tablet => DeviceFormFactor.Tablet,
        _ => DeviceFormFactor.Mobile,
    };

    public PointerModel PointerModel => PointerModel.Touch;
    public NFloat AccessibilityFontScale => 1;
    public bool PrefersReducedMotion => false;
    public bool PrefersHighContrast => false;
    public ColorScheme ColorScheme => ColorScheme.Light;
}
