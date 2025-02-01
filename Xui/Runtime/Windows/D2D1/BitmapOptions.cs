namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public enum BitmapOptions : uint
    {
        None = 0x00000000,
        Target = 0x00000001,
        CannotDraw = 0x00000002,
        CpuRead = 0x00000004,
        GdiCompatible = 0x00000008,
    }
}
