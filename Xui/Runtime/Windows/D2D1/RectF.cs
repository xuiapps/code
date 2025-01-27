using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RectF
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public static readonly RectF Inifinity = new RectF(float.NegativeInfinity, float.NegativeInfinity, float.PositiveInfinity, float.PositiveInfinity);
    
        public RectF(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public static implicit operator RectF(Rect rect) => new RectF()
        {
            Left = (float)rect.X,
            Top = (float)rect.Y,
            Right = (float)(rect.X + rect.Width),
            Bottom = (float)(rect.Y + rect.Height),
        };
    }
}
