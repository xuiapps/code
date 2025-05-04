using System.Runtime.InteropServices;

namespace Xui.Runtime.Software;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct _G16
{
    public ushort Gray;

    public _G16(ushort gray)
    {
        Gray = gray;
    }
}