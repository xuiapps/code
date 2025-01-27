using System.Runtime.InteropServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct Matrix3X2F
    {
        [FieldOffset(0)]
        public float _11;
        [FieldOffset(4)]
        public float _12;
        [FieldOffset(8)]
        public float _21;
        [FieldOffset(12)]
        public float _22;
        [FieldOffset(16)]
        public float _31;
        [FieldOffset(20)]
        public float _32;

        public static readonly Matrix3X2F Identity = new Matrix3X2F() { _11 = 1, _12 = 0, _21 = 0, _22 = 1, _31 = 0, _32 = 0 };

        public static implicit operator AffineTransform(Matrix3X2F m) => new AffineTransform(m._11, m._12, m._21, m._22, m._31, m._32);
        public static implicit operator Matrix3X2F(AffineTransform t) => new Matrix3X2F() { _11 = (float)t.A, _12 = (float)t.B, _21 = (float)t.C, _22 = (float)t.D, _31 = (float)t.Tx, _32 = (float)t.Ty };
    }
}
