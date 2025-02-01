using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    public struct CGRect
    {
        public CGPoint Origin;
        public CGSize Size;

        public CGRect(NFloat x, NFloat y, NFloat width, NFloat height) :
            this(new CGPoint(x, y), new CGSize(width, height))
        {
        }

        public CGRect(CGPoint origin, CGSize size)
        {
            this.Origin = origin;
            this.Size = size;
        }

        public static implicit operator Rect(CGRect rect) => new Rect(rect.Origin.X, rect.Origin.Y, rect.Size.Width, rect.Size.Height);
        public static implicit operator CGRect(Rect rect) => new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}