using System;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public struct Point2F
    {
        public float X;
        public float Y;

        public static implicit operator Point2F(Point point) => new() { X = (float)point.X, Y = (float)point.Y };
        public static implicit operator Point2F(Vector vector) => new() { X = (float)vector.X, Y = (float)vector.Y };

        public static implicit operator Point2F(ValueTuple<float, float> tuple) => new () { X = tuple.Item1, Y = tuple.Item2 };
    }
}