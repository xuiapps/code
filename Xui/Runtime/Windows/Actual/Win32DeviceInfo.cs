using Xui.Core.DI;

namespace Xui.Runtime.Windows.Actual;

internal sealed class Win32DeviceInfo : IDeviceInfo
{
    public static readonly Win32DeviceInfo Instance = new();

    public DevicePlatform Platform => DevicePlatform.Windows;
    public DeviceFormFactor FormFactor => DeviceFormFactor.Desktop;
}
