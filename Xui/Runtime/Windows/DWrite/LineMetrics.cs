using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public struct LineMetrics
    {
        public uint Length;
        public uint TrailingWhitespaceLength;
        public uint NewlineLength;
        public float Height;
        public float Baseline;
        public BOOL IsTrimmed;
    }
}