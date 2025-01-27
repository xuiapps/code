namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public struct SizeF
    {
        public float Width;
        public float Height;

        public SizeF(float radius)
        {
            this.Width = this.Height = radius;
        }

        public SizeF(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
