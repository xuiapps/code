namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public struct ModeDesc
    {
        public uint Width;
        public uint Height;
        public Rational RefreshRate;
        public Format Format;
        public ModeScanlineOrder ScanlineOrdering;
        public ModeScaling Scaling;
    }
}
