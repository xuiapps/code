using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public struct SwapChainDesc1
    {
        public uint Width;
        public uint Height;
        public Format Format;
        public BOOL Stereo;
        public SampleDesc SampleDesc;
        public Usage BufferUsage;
        public uint BufferCount;
        public Scaling Scaling;
        public SwapEffect SwapEffect;
        public AlphaMode AlphaMode;
        public uint Flags;
    }
}
