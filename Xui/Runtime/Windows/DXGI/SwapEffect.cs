namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public enum SwapEffect : uint
    {
        Discard	= 0,
        Sequential	= 1,
        FlipSequential	= 3,
        FlipDiscard	= 4,
    }
}