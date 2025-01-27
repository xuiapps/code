namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public struct StrokeStyleProperties
    {
        public CapStyle StartCap;
        public CapStyle EndCap;
        public CapStyle DashCap;
        public LineJoin LineJoin;
        public float MiterLimit;
        public DashStyle DashStyle;
        public float DashOffset;
    }
}