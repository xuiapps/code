namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public enum ModeScanlineOrder : uint
    {
        Unspecified = 0,
        Progressive = 1,
        UpperFieldFirst = 2,
        LowerFieldFirst = 3,
    }
}