using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public struct RoundRect
    {
        public RectF Rect;
        public float RadiusX;
        public float RadiusY;

        public RoundRect(RectF rect, float radius) : this()
        {
            Rect = rect;
            this.RadiusX = this.RadiusY = radius;
        }
    }
}
