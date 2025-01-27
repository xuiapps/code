using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSPoint
    {
        public NFloat x;

        public NFloat y;

        public NSPoint(NFloat x, NFloat y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Point(NSPoint nsPoint) => new Point(nsPoint.x, nsPoint.y);
        public static implicit operator Vector(NSPoint nsPoint) => new Vector(nsPoint.x, nsPoint.y);
    }
}