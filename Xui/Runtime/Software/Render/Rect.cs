using System;
using System.Runtime.CompilerServices;

namespace Xui.Runtime.Software.Raster;

public struct Rect
{
    public uint Left;
    public uint Top;
    public uint Right;
    public uint Bottom;

    public static readonly Rect Empty = new()
    {
        Left = ushort.MaxValue,
        Top = ushort.MaxValue,
        Right = ushort.MinValue,
        Bottom = ushort.MinValue
    };

    public bool IsEmpty => Left > Right || Top > Bottom;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnionWith(in Rect other)
    {
        if (other.IsEmpty) return;
        if (IsEmpty)
        {
            this = other;
            return;
        }

        Left = Math.Min(Left, other.Left);
        Top = Math.Min(Top, other.Top);
        Right = Math.Max(Right, other.Right);
        Bottom = Math.Max(Bottom, other.Bottom);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void IntersectWith(in Rect other)
    {
        if (IsEmpty || other.IsEmpty)
        {
            this = Empty;
            return;
        }

        Left = Math.Max(Left, other.Left);
        Top = Math.Max(Top, other.Top);
        Right = Math.Min(Right, other.Right);
        Bottom = Math.Min(Bottom, other.Bottom);

        if (IsEmpty)
            this = Empty;
    }

    /// <summary>
    /// Returns a new rectangle expanded outward and aligned to a SIMD boundary.
    /// This is used for conservative region initialization where vectorization is preferred.
    /// </summary>
    /// <param name="simdWidth">The number of pixels per vector (e.g., Vector&lt;T&gt;.Count).</param>
    public static Rect AlignOutward(Rect rect, int simdWidth)
    {
        if (rect.IsEmpty) return Empty;

        uint left = rect.Left > 1 ? rect.Left - 1 : 0;
        uint top = rect.Top > 1 ? rect.Top - 1 : 0;
        uint right = rect.Right + 1;
        uint bottom = rect.Bottom + 1;

        uint alignedLeft = left & ~(uint)(simdWidth - 1);
        uint alignedRight = (right + (uint)(simdWidth - 1)) & ~(uint)(simdWidth - 1);

        return new Rect
        {
            Left = alignedLeft,
            Top = top,
            Right = alignedRight,
            Bottom = bottom
        };
    }
}
