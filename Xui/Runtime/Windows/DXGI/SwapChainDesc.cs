using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public struct SwapChainDesc
    {
        public ModeDesc BufferDesc;
        public SampleDesc SampleDesc;
        public Usage BufferUsage;
        public uint BufferCount;
        public nint OutputWindow;
        public BOOL Windowed;
        public SwapEffect SwapEffect;
        public SwapChainFlags Flags;
    }
}