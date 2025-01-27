namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    /// <summary>https://learn.microsoft.com/en-us/windows/win32/api/d2d1/ne-d2d1-d2d1_factory_type</summary>
    public enum FactoryType : uint
    {
        SingleThreaded = 0,
        MultiThreaded = 1,
        ForceDword = 0xffffffff
    }
}
