using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    public struct CGPoint
    {
        public NFloat X;
        public NFloat Y;

        public CGPoint(NFloat x, NFloat y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator CGPoint(Point point) => new CGPoint(point.X, point.Y);
        public static implicit operator Point(CGPoint point) => new Point(point.X, point.Y);
    }
}