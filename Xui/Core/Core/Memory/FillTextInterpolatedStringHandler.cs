using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Core.Memory;

[InterpolatedStringHandler]
public ref struct FillTextInterpolatedStringHandler
{
    private InlineBuffer256 inlineBuffer;
    private Span<char> buffer;
    private char[]? rented;
    private int pos;

    public FillTextInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        int initialCapacity = literalLength + formattedCount * 16;
        inlineBuffer = default;
        rented = null;
        pos = 0;

        if (initialCapacity <= 256)
        {
            buffer = MemoryMarshal.CreateSpan(ref Unsafe.As<InlineBuffer256, char>(ref inlineBuffer), 256);
        }
        else
        {
            rented = ArrayPool<char>.Shared.Rent(initialCapacity);
            buffer = rented;
        }
    }

    public void AppendLiteral(string s)
    {
        if (s.Length > buffer.Length - pos)
            Grow(s.Length);

        s.AsSpan().CopyTo(buffer[pos..]);
        pos += s.Length;
    }

    public void AppendFormatted<T>(T value)
    {
        if (value is ISpanFormattable spanFormattable)
        {
            int charsWritten;
            while (!spanFormattable.TryFormat(buffer[pos..], out charsWritten, default, null))
                Grow(16);
            pos += charsWritten;
        }
        else
        {
            AppendLiteral(value?.ToString() ?? "");
        }
    }

    public void AppendFormatted<T>(T value, ReadOnlySpan<char> format) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(buffer[pos..], out charsWritten, format, null))
            Grow(16);
        pos += charsWritten;
    }

    public void AppendFormatted<T>(T value, int alignment) where T : ISpanFormattable
    {
        int start = pos;
        AppendFormatted(value);
        ApplyAlignment(start, alignment);
    }

    public void AppendFormatted<T>(T value, int alignment, ReadOnlySpan<char> format) where T : ISpanFormattable
    {
        int start = pos;
        AppendFormatted(value, format);
        ApplyAlignment(start, alignment);
    }

    public void AppendFormatted(ReadOnlySpan<char> value)
    {
        if (value.Length > buffer.Length - pos)
            Grow(value.Length);

        value.CopyTo(buffer[pos..]);
        pos += value.Length;
    }

    public void AppendFormatted(string? value) =>
        AppendLiteral(value ?? "");

    public readonly ReadOnlySpan<char> AsSpan() => buffer[..pos];

    public void Dispose()
    {
        if (rented != null)
        {
            ArrayPool<char>.Shared.Return(rented);
            rented = null;
        }
    }

    private void Grow(int additionalRequired)
    {
        int needed = pos + additionalRequired;
        int newSize = Math.Max(buffer.Length * 2, needed);
        char[] newRented = ArrayPool<char>.Shared.Rent(newSize);
        buffer[..pos].CopyTo(newRented);

        if (rented != null)
            ArrayPool<char>.Shared.Return(rented);

        rented = newRented;
        buffer = rented;
    }

    private void ApplyAlignment(int start, int alignment)
    {
        int written = pos - start;
        int padding = Math.Abs(alignment) - written;
        if (padding <= 0) return;

        if (padding > buffer.Length - pos)
            Grow(padding);

        if (alignment > 0)
        {
            buffer[start..pos].CopyTo(buffer[(start + padding)..]);
            buffer.Slice(start, padding).Fill(' ');
        }
        else
        {
            buffer.Slice(pos, padding).Fill(' ');
        }

        pos += padding;
    }

    [InlineArray(256)]
    private struct InlineBuffer256
    {
        private char _element;
    }
}
