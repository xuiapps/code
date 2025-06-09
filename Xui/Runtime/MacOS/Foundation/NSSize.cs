using System.Runtime.InteropServices;
using Xui.Core.Math2D;

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

        public static implicit operator NSSize(Size size) => new NSSize(size.Width, size.Height);
    }
}