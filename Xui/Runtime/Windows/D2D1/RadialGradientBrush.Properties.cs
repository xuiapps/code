namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public unsafe partial class RadialGradientBrush
    {
        public struct Properties
        {
            public Point2F Center;
            public Point2F GradientOriginOffset;
            public Point2F Radius;
            public float RadiusX;
            public float RadiusY;
        }
    }
}