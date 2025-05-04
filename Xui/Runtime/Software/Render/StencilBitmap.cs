using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Runtime.Software.Raster;

public unsafe sealed class StencilBitmap : IDisposable
{
    public readonly int Width;
    public readonly int Height;
    public readonly int Stride; // In pixels (SIMD-aligned)

    private readonly IntPtr _rawMemory;
    private readonly IntPtr _alignedPixels;
    private readonly int _lengthInPixels;
    private bool _disposed;

    public G16* Pixels => (G16*)_alignedPixels;

    public Span<G16> Span => new Span<G16>((void*)_alignedPixels, _lengthInPixels);

    Span<Vector<ushort>> VectorSpan => MemoryMarshal.Cast<G16, Vector<ushort>>(Span);

    public StencilBitmap(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException();

        Width = width;
        Height = height;

        int simdWidth = Vector<NFloat>.Count;
        Stride = (width + simdWidth - 1) & ~(simdWidth - 1); // Round up to SIMD multiple
        _lengthInPixels = Stride * Height;

        int byteLength = _lengthInPixels * sizeof(G16);
        _rawMemory = Marshal.AllocHGlobal(byteLength + simdWidth * sizeof(NFloat)); // Extra for alignment

        long addr = (long)_rawMemory;
        long aligned = (addr + (simdWidth * sizeof(NFloat) - 1)) & ~(long)(simdWidth * sizeof(NFloat) - 1);
        _alignedPixels = (IntPtr)aligned;

        Unsafe.InitBlockUnaligned((void*)_alignedPixels, 0, (uint)byteLength);
    }

    public G16 this[int x, int y]
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

    public void Fill(G16 value = 0)
    {
        int count = _lengthInPixels;
        int vectorSize = Vector<ushort>.Count;

        var vec = new Vector<ushort>(value);
        var span = Span;
        int i = 0;

        // SIMD fill
        for (; i <= count - vectorSize; i += vectorSize)
            vec.CopyTo(span.Slice(i, vectorSize));

        // Tail fill
        for (; i < count; i++)
            span[i] = value;
    }

    public void FillRect(in Rect rect, G16 value)
    {
        if (rect.IsEmpty) return;

        var vec = new Vector<ushort>(value);
        var span = Span;

        for (uint y = rect.Top; y < rect.Bottom; y++)
        {
            int row = (int)(y * Stride + rect.Left);
            int count = (int)(rect.Right - rect.Left);
            int i = 0;
            for (; i <= count - Vector<ushort>.Count; i += Vector<ushort>.Count)
                vec.CopyTo(span.Slice(row + i, Vector<ushort>.Count));

            for (; i < count; i++)
                span[row + i] = value;
        }
    }

    /// <summary>
    /// Initializes a rectangular region of the stencil buffer using SIMD, aligned outward to the nearest vector boundary.
    /// This method may overdraw slightly beyond the specified rectangle for performance.
    /// Suitable for fast clearing of bounds when preparing coverage masks.
    /// </summary>
    /// <param name="rect">The region to initialize.</param>
    /// <param name="value">The fill value (default is 0, meaning no coverage).</param>
    public void InitRect(in Rect rect, G16 value = 0)
    {
        if (rect.IsEmpty) return;

        int simdSize = Vector<ushort>.Count;
        var aligned = Rect.AlignOutward(rect, simdSize);
        var vec = new Vector<ushort>(value);

        for (uint y = aligned.Top; y < aligned.Bottom; y++)
        {
            int rowStart = (int)(y * Stride + aligned.Left);
            Span<Vector<ushort>> rowVec = MemoryMarshal.Cast<G16, Vector<ushort>>(Span.Slice(rowStart, (int)(aligned.Right - aligned.Left)));

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

    ~StencilBitmap()
    {
        Debug.WriteLine($"StencilBitmap instance ({Width}x{Height}) was finalized without being disposed.");
        Dispose();
    }
}
