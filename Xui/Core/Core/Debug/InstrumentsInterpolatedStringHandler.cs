using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Core.Debug;

[InterpolatedStringHandler]
public ref struct InstrumentsInterpolatedStringHandler
{
    private InlineBuffer256 inlineBuffer;
    private Span<char> buffer;
    private char[]? rented;
    private int pos;

    public InstrumentsInterpolatedStringHandler(
        int literalLength,
        int formattedCount,
        InstrumentsAccessor accessor,
        Scope scope,
        LevelOfDetail lod,
        out bool shouldAppend)
    {
        shouldAppend = accessor.Sink != null && accessor.Sink.IsEnabled(scope, lod);

        if (!shouldAppend)
        {
            this = default;
            return;
        }

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
        {
            Grow(s.Length);
        }

        s.AsSpan().CopyTo(buffer[pos..]);
        pos += s.Length;
    }

    public void AppendFormatted<T>(T value) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(buffer[pos..], out charsWritten, default, provider: null))
        {
            Grow(16);
        }

        pos += charsWritten;
    }

    public void AppendFormatted<T>(T value, ReadOnlySpan<char> format) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(buffer[pos..], out charsWritten, format, provider: null))
        {
            Grow(16);
        }

        pos += charsWritten;
    }

    public void AppendFormatted(ReadOnlySpan<char> value)
    {
        if (value.Length > buffer.Length - pos)
        {
            Grow(value.Length);
        }

        value.CopyTo(buffer[pos..]);
        pos += value.Length;
    }

    public void AppendFormatted(string? value) =>
        AppendLiteral(value ?? "");

    public void AppendFormatted(object? value) =>
        AppendLiteral(value?.ToString() ?? "");

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
        {
            ArrayPool<char>.Shared.Return(rented);
        }

        rented = newRented;
        buffer = rented;
    }

    [InlineArray(256)]
    private struct InlineBuffer256
    {
        private char _element;
    }
}
