using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSSize
    {
        public NFloat width;
        public NFloat height;

        public NSSize(NFloat width, NFloat height)
        {
            this.width = width;
            this.height = height;
        }
    }
}