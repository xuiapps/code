using System.Runtime.InteropServices;
using Xui.Core.Canvas;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [StructLayout(LayoutKind.Explicit, Size=16)]
    public struct ColorF {
        [FieldOffset(0)]
        public float R;
        [FieldOffset(4)]
        public float G;
        [FieldOffset(8)]
        public float B;
        [FieldOffset(12)]
        public float A;

        public static implicit operator ColorF(Color color) => new ColorF()
        {
            R = (float)color.Red,
            G = (float)color.Green,
            B = (float)color.Blue,
            A = (float)color.Alpha
        };

        public static implicit operator ColorF(uint rgbaHex) => (ColorF)(Color)rgbaHex;
    }
}
