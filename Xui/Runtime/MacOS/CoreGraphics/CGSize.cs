using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    public struct CGSize
    {
        public NFloat Width;
        public NFloat Height;

        public CGSize(NFloat width, NFloat height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static implicit operator Vector(CGSize size) => new Vector(size.Width, size.Height);
    }
}