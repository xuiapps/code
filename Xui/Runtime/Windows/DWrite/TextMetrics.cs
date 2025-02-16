namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public struct TextMetrics
    {
        public float Left;
        public float Top;
        public float Width;
        public float WidthIncludingTrailingWhitespace;
        public float Height;
        public float LayoutWidth;
        public float LayoutHeight;
        public int MaxBidiReorderingDepth;
        public int LineCount;
    }
}