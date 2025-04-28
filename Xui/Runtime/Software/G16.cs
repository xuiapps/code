using System.Runtime.InteropServices;

namespace Xui.Runtime.Software;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct G16
{
    public ushort Gray;

    public G16(ushort gray)
    {
        Gray = gray;
    }
}