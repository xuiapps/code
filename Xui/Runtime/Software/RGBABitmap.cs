using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Software;

public class RGBABitmap
{
    public uint Width { get; }
    public uint Height { get; }

    public readonly RGBA[] Pixels;

    public RGBABitmap(uint width, uint height)
    {
        Width = width;
        Height = height;
        Pixels = new RGBA[width * height];
    }

    public RGBA this[uint x, uint y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Pixels[y * Width + x];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Pixels[y * Width + x] = value;
    }

    public ReadOnlySpan<byte> AsBytes()
    {
        return MemoryMarshal.AsBytes(new ReadOnlySpan<RGBA>(Pixels));
    }

    public void Blend(uint x, uint y, RGBA src)
    {
        if (x >= Width || y >= Height)
            return;

        ref var dst = ref Pixels[y * Width + x];

        byte srcAlpha = src.Alpha;
        if (srcAlpha == 0)
            return; // Fully transparent, skip

        if (srcAlpha == 255)
        {
            dst = src; // Fully opaque, overwrite
            return;
        }

        // Blend src over dst
        // result = src.rgb * src.a + dst.rgb * (1 - src.a)

        byte invAlpha = (byte)(255 - srcAlpha);

        dst.Red = (byte)((src.Red * srcAlpha + dst.Red * invAlpha) / 255);
        dst.Green = (byte)((src.Green * srcAlpha + dst.Green * invAlpha) / 255);
        dst.Blue = (byte)((src.Blue * srcAlpha + dst.Blue * invAlpha) / 255);
        dst.Alpha = (byte)(Math.Min(255, srcAlpha + (dst.Alpha * invAlpha) / 255)); // Premultiplied model
    }
}
