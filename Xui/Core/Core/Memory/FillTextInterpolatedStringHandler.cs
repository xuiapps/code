using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xui.Core.Memory;

/// <summary>
/// Low-allocation interpolated string handler for building text in a stack-first inline buffer,
/// spilling to a rented array only when the text exceeds 256 characters.
/// </summary>
[InterpolatedStringHandler]
public ref struct FillTextInterpolatedStringHandler
{
    private InlineBuffer256 inlineBuffer;
    private Span<char> buffer;
    private char[]? rented;
    private int pos;

    /// <summary>Initializes the handler and allocates or rents a buffer of the appropriate size.</summary>
    /// <param name="literalLength">Total character count of all literal string segments.</param>
    /// <param name="formattedCount">Number of interpolated holes in the string.</param>
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

    // Zero-alloc path for "any span-formattable" value types and structs.
    // This is the important one: no boxing, no interface-cast allocations.
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

    /// <summary>Appends a span-formattable value with the specified alignment width.</summary>
    public void AppendFormatted<T>(T value, int alignment) where T : ISpanFormattable
    {
        int start = pos;
        AppendFormatted(value);
        ApplyAlignment(start, alignment);
    }

    /// <summary>Appends a span-formattable value with the specified format and alignment width.</summary>
    public void AppendFormatted<T>(T value, int alignment, ReadOnlySpan<char> format) where T : ISpanFormattable
    {
        int start = pos;
        AppendFormatted(value, format);
        ApplyAlignment(start, alignment);
    }

    // Common fast-paths the compiler will pick for these interpolations.
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

    // Explicitly "allocating fallback" for non-span-formattable objects.
    // Keeping it explicit avoids accidental boxing allocations in a generic method.
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

    private void ApplyAlignment(int start, int alignment)
    {
        int written = pos - start;
        int padding = Math.Abs(alignment) - written;
        if (padding <= 0)
        {
            return;
        }

        if (padding > buffer.Length - pos)
        {
            Grow(padding);
        }

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
