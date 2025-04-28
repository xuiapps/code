using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Software;

/// <summary>
/// Represents a grayscale 16-bit stencil buffer for supersampled shape coverage.
/// </summary>
public class G16Stencil
{
    public uint Width { get; }
    public uint Height { get; }

    public readonly G16[] Alpha;

    public G16Stencil(uint width, uint height)
    {
        Width = width;
        Height = height;
        Alpha = new G16[width * height];
    }

    public G16 this[uint x, uint y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Alpha[y * Width + x];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Alpha[y * Width + x] = value;
    }

    public ReadOnlySpan<byte> AsBytes()
    {
        return MemoryMarshal.AsBytes(new ReadOnlySpan<G16>(Alpha));
    }

    public void Clear()
    {
        Array.Clear(Alpha, 0, Alpha.Length);
    }

    public void Clear(uint startX, uint startY, uint w, uint h)
    {
        for (uint y = startY; y < startY + h && y < Height; y++)
        {
            for (uint x = startX; x < startX + w && x < Width; x++)
            {
                Alpha[y * Width + x] = new G16(0);
            }
        }
    }

    /// <summary>
    /// Adds the specified coverage amount to a stencil pixel at (x,y), saturating at 65535.
    /// </summary>
    public void Add(uint x, uint y, ushort amount = 1)
    {
        if (x >= Width || y >= Height)
            return;

        uint total = (uint)Alpha[y * Width + x].Gray + amount;
        Alpha[y * Width + x] = new G16((ushort)(total > 65535 ? 65535 : total));
    }
}
