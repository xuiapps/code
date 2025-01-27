using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public enum Scaling : uint
    {
        Stretch	= 0,
        None = 1,
        AspectRatioStretch = 2
    }
}
