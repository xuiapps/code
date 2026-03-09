using Xui.Core.DI;

namespace Xui.Runtime.MacOS.Actual;

internal sealed class MacOSDeviceInfo : IDeviceInfo
{
    public static readonly MacOSDeviceInfo Instance = new();

    public DevicePlatform Platform => DevicePlatform.MacOS;
    public DeviceFormFactor FormFactor => DeviceFormFactor.Desktop;
}
