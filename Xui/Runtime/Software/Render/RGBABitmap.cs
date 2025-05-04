using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Software.Raster;

public unsafe sealed class RGBABitmap : IDisposable
{
    public readonly int Width;
    public readonly int Height;
    public readonly int Stride; // In pixels (SIMD-aligned)

    private readonly IntPtr _rawMemory;
    private readonly IntPtr _alignedPixels;
    private readonly int _lengthInPixels;
    private bool _disposed;

    public RGBA* Pixels => (RGBA*)_alignedPixels;

    public Span<RGBA> Span => new Span<RGBA>((void*)_alignedPixels, _lengthInPixels);

    public Span<Vector<uint>> VectorSpan => MemoryMarshal.Cast<RGBA, Vector<uint>>(Span);

    public RGBABitmap(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException();

        Width = width;
        Height = height;

        int simdWidth = Vector<uint>.Count;
        Stride = (width + simdWidth - 1) & ~(simdWidth - 1); // Round up to SIMD multiple
        _lengthInPixels = Stride * Height;

        int byteLength = _lengthInPixels * sizeof(RGBA);
        _rawMemory = Marshal.AllocHGlobal(byteLength + simdWidth * sizeof(uint)); // Extra for alignment

        long addr = (long)_rawMemory;
        long aligned = (addr + (simdWidth * sizeof(uint) - 1)) & ~(long)(simdWidth * sizeof(uint) - 1);
        _alignedPixels = (IntPtr)aligned;

        Unsafe.InitBlockUnaligned((void*)_alignedPixels, 0, (uint)byteLength);
    }

    public RGBA this[int x, int y]
    {
        get
        {
            if ((uint)x >= Width || (uint)y >= Height)
                throw new ArgumentOutOfRangeException();
            return Pixels[y * Stride + x];
        }
        set
        {
            if ((uint)x >= Width || (uint)y >= Height)
                throw new ArgumentOutOfRangeException();
            Pixels[y * Stride + x] = value;
        }
    }

    public void Fill(RGBA value = 0)
    {
        int count = _lengthInPixels;
        int vectorSize = Vector<uint>.Count;

        var vec = new Vector<uint>(value);
        var span = Span;
        int i = 0;

        // SIMD fill
        for (; i <= count - vectorSize; i += vectorSize)
            vec.CopyTo(span.Slice(i, vectorSize));

        // Tail fill
        for (; i < count; i++)
            span[i] = value;
    }

    public void FillRect(in Rect rect, RGBA value)
    {
        if (rect.IsEmpty) return;

        var vec = new Vector<uint>(value);
        var span = Span;

        for (uint y = rect.Top; y < rect.Bottom; y++)
        {
            int row = (int)(y * Stride + rect.Left);
            int count = (int)(rect.Right - rect.Left);
            int i = 0;
            for (; i <= count - Vector<uint>.Count; i += Vector<uint>.Count)
                vec.CopyTo(span.Slice(row + i, Vector<uint>.Count));

            for (; i < count; i++)
                span[row + i] = value;
        }
    }

    /// <summary>
    /// Initializes a rectangular region of the bitmap using SIMD, aligned outward to the nearest vector boundary.
    /// This method may overdraw slightly beyond the specified rectangle for performance.
    /// Suitable for fast clearing of bounds when rendering shapes or tiles.
    /// </summary>
    /// <param name="rect">The region to initialize.</param>
    /// <param name="value">The fill color (default is transparent black).</param>
    public void InitRect(in Rect rect, RGBA value = 0)
    {
        if (rect.IsEmpty) return;

        int simdSize = Vector<uint>.Count;
        var aligned = Rect.AlignOutward(rect, simdSize);
        var vec = new Vector<uint>(value);

        for (uint y = aligned.Top; y < aligned.Bottom; y++)
        {
            int rowStart = (int)(y * Stride + aligned.Left);
            Span<Vector<uint>> rowVec = MemoryMarshal.Cast<RGBA, Vector<uint>>(Span.Slice(rowStart, (int)(aligned.Right - aligned.Left)));

            for (int i = 0; i < rowVec.Length; i++)
                rowVec[i] = vec;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        Marshal.FreeHGlobal(_rawMemory);
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    ~RGBABitmap()
    {
        Debug.WriteLine($"RGBABitmap instance ({Width}x{Height}) was finalized without being disposed.");
        Dispose();
    }
}
