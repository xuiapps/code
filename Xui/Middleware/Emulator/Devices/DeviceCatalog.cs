using System.Collections.Generic;
using Xui.Core.Math2D;

namespace Xui.Middleware.Emulator.Devices;

public static class DeviceCatalog
{
    public static readonly List<DeviceProfile> All = new()
    {
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Phone,
            Model = "iPhone 15 Pro",
            LogicalResolution = new Size(393, 852),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 59, 0, 34),
            SafeAreaInsetsLandscape = new Frame(59, 0, 59, 21),
            NotchType = NotchType.DynamicIsland,
            NotchFrame = new Rect(151.5f, 12, 90, 26),
            ScreenCornerRadius = 44
        },
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Phone,
            Model = "iPhone 14",
            LogicalResolution = new Size(390, 844),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 47, 0, 34),
            SafeAreaInsetsLandscape = new Frame(47, 0, 47, 21),
            NotchType = NotchType.Notch,
            NotchFrame = new Rect(132, 0, 126, 32),
            ScreenCornerRadius = 44
        },
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Phone,
            Model = "iPhone SE (3rd gen)",
            LogicalResolution = new Size(375, 667),
            ScaleFactor = 2,
            SafeAreaInsetsPortrait = new Frame(0, 0, 0, 0),
            SafeAreaInsetsLandscape = new Frame(0, 0, 0, 0),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 16
        },
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Tablet,
            Model = "iPad Pro 12.9\" (6th gen)",
            LogicalResolution = new Size(1024, 1366),
            ScaleFactor = 2,
            SafeAreaInsetsPortrait = new Frame(0, 24, 0, 20),
            SafeAreaInsetsLandscape = new Frame(24, 0, 24, 20),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 18
        },
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Tablet,
            Model = "iPad Air (5th gen)",
            LogicalResolution = new Size(820, 1180),
            ScaleFactor = 2,
            SafeAreaInsetsPortrait = new Frame(0, 24, 0, 20),
            SafeAreaInsetsLandscape = new Frame(24, 0, 24, 20),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 18
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Phone,
            Model = "Pixel 8 Pro",
            LogicalResolution = new Size(412, 915),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 40, 0, 34),
            SafeAreaInsetsLandscape = new Frame(40, 0, 40, 20),
            NotchType = NotchType.PinHole,
            NotchFrame = new Rect(190, 12, 32, 32),
            ScreenCornerRadius = 28
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Phone,
            Model = "Galaxy S24 Ultra",
            LogicalResolution = new Size(440, 960),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 38, 0, 32),
            SafeAreaInsetsLandscape = new Frame(38, 0, 38, 20),
            NotchType = NotchType.PinHole,
            NotchFrame = new Rect(204, 38, 32, 32),
            ScreenCornerRadius = 12
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Tablet,
            Model = "Galaxy Tab S9+",
            LogicalResolution = new Size(1170, 1870),
            ScaleFactor = 2.4f,
            SafeAreaInsetsPortrait = new Frame(0, 0, 0, 0),
            SafeAreaInsetsLandscape = new Frame(0, 0, 0, 0),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 12
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Tablet,
            Model = "Pixel Tablet",
            LogicalResolution = new Size(1280, 800),
            ScaleFactor = 2,
            SafeAreaInsetsPortrait = new Frame(0, 0, 0, 0),
            SafeAreaInsetsLandscape = new Frame(0, 0, 0, 0),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 12
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Phone,
            Model = "Galaxy A54",
            LogicalResolution = new Size(393, 873),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 34, 0, 30),
            SafeAreaInsetsLandscape = new Frame(34, 0, 34, 18),
            NotchType = NotchType.Notch,
            NotchFrame = new Rect(152, 0, 88, 24),
            ScreenCornerRadius = 22
        },
        new DeviceProfile
        {
            Brand = Brand.Android,
            DeviceType = DeviceType.Phone,
            Model = "RedMagic 9 Pro",
            LogicalResolution = new Size(430, 960),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 28, 0, 24),
            SafeAreaInsetsLandscape = new Frame(28, 0, 28, 18),
            NotchType = NotchType.None,
            NotchFrame = new Rect(0, 0, 0, 0),
            ScreenCornerRadius = 18
        },
        new DeviceProfile
        {
            Brand = Brand.Apple,
            DeviceType = DeviceType.Phone,
            Model = "iPhone 13 mini",
            LogicalResolution = new Size(375, 812),
            ScaleFactor = 3,
            SafeAreaInsetsPortrait = new Frame(0, 50, 0, 34),
            SafeAreaInsetsLandscape = new Frame(50, 0, 50, 21),
            NotchType = NotchType.Notch,
            NotchFrame = new Rect(120, 50, 134, 30),
            ScreenCornerRadius = 44
        }
    };
}