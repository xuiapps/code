using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Core.Debug;

/// <summary>
/// Low-allocation interpolated string handler used by <see cref="InstrumentsAccessor.Log"/> and
/// <see cref="InstrumentsAccessor.Trace"/>. The compiler selects this handler automatically
/// when the sink is disabled, skipping all formatting entirely.
/// </summary>
[InterpolatedStringHandler]
public ref struct InstrumentsInterpolatedStringHandler
{
    private InlineBuffer256 inlineBuffer;
    private Span<char> buffer;
    private char[]? rented;
    private int pos;

    /// <summary>
    /// Called by the compiler to initialize the handler and decide whether formatting is needed.
    /// </summary>
    /// <param name="literalLength">Total character count of all literal string segments.</param>
    /// <param name="formattedCount">Number of interpolated holes in the string.</param>
    /// <param name="accessor">The accessor used to check whether the sink is enabled.</param>
    /// <param name="scope">The instrumentation scope.</param>
    /// <param name="lod">The level of detail.</param>
    /// <param name="shouldAppend">Set to <c>true</c> if formatting should proceed; <c>false</c> to skip.</param>
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

    /// <summary>Appends a literal string segment to the buffer.</summary>
    public void AppendLiteral(string s)
    {
        if (s.Length > buffer.Length - pos)
        {
            Grow(s.Length);
        }

        s.AsSpan().CopyTo(buffer[pos..]);
        pos += s.Length;
    }

    /// <summary>Appends a span-formattable value using its default format.</summary>
    public void AppendFormatted<T>(T value) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(buffer[pos..], out charsWritten, default, provider: null))
        {
            Grow(16);
        }

        pos += charsWritten;
    }

    /// <summary>Appends a span-formattable value using the specified format string.</summary>
    public void AppendFormatted<T>(T value, ReadOnlySpan<char> format) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(buffer[pos..], out charsWritten, format, provider: null))
        {
            Grow(16);
        }

        pos += charsWritten;
    }

    /// <summary>Appends a <see cref="ReadOnlySpan{T}"/> of characters directly.</summary>
    public void AppendFormatted(ReadOnlySpan<char> value)
    {
        if (value.Length > buffer.Length - pos)
        {
            Grow(value.Length);
        }

        value.CopyTo(buffer[pos..]);
        pos += value.Length;
    }

    /// <summary>Appends a string value.</summary>
    public void AppendFormatted(string? value) =>
        AppendLiteral(value ?? "");

    /// <summary>Appends any object by calling <c>ToString()</c>.</summary>
    public void AppendFormatted(object? value) =>
        AppendLiteral(value?.ToString() ?? "");

    /// <summary>Returns the currently accumulated characters as a read-only span.</summary>
    public readonly ReadOnlySpan<char> AsSpan() => buffer[..pos];

    /// <summary>Returns any rented array buffer back to the pool.</summary>
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
