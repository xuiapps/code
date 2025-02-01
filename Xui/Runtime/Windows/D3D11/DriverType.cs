namespace Xui.Runtime.Windows;

public static partial class D3D11
{
    public enum DriverType : uint
    {
        Unknown = 0,
        Hardware = 1,
        Reference = 2,
        Null = 3,
        Software = 4,
        Warp = 5
    }
}