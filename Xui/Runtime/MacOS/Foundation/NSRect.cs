using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NSRect
    {
        public NSPoint Origin;
        public NSSize Size;

        public NSRect(NFloat x, NFloat y, NFloat width, NFloat height)
        {
            this.Origin.x = x;
            this.Origin.y = y;
            this.Size.width = width;
            this.Size.height = height;
        }

        public static implicit operator Rect(NSRect rect) => new Rect(rect.Origin.x, rect.Origin.y, rect.Size.width, rect.Size.height);
        public static implicit operator NSRect(Rect rect) => new NSRect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}