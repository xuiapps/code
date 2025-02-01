using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RECT
    {
        [FieldOffset(0)]
        public int Left;
        [FieldOffset(4)]
        public int Top;
        [FieldOffset(8)]
        public int Right;
        [FieldOffset(12)]
        public int Bottom;

        public RECT(Rect rect)
        {
            this.Left = (int)rect.X;
            this.Top = (int)rect.Y;
            this.Right = (int)(rect.X + rect.Width);
            this.Bottom = (int)(rect.Y + rect.Height);
        }
    }
}
