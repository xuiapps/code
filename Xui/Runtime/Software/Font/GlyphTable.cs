using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public class GlyfTable
{
    private const ushort Arg1And2AreWords = 0x0001;
    private const ushort WeHaveAScale = 0x0008;
    private const ushort MoreComponents = 0x0020;
    private const ushort WeHaveAnXAndYScale = 0x0040;
    private const ushort WeHaveATwoByTwo = 0x0080;

    private readonly ReadOnlyMemory<byte> _data;
    private readonly LocaTable _loca;

    public GlyfTable(ReadOnlyMemory<byte> data, LocaTable loca)
    {
        _data = data;
        _loca = loca;
    }

    /// <summary>
    /// Returns a glyph shape by slicing the raw memory using loca table offsets.
    /// </summary>
    public bool TryGetGlyph(int glyphIndex, out GlyphShape shape)
    {
        var (offset, length) = _loca.GetGlyphBounds(glyphIndex);

        if (length <= 0 || offset + length > _data.Length)
        {
            shape = default;
            return false;
        }

        shape = new GlyphShape(_data.Span.Slice(offset, length));
        return true;
    }

    /// <summary>Adds every component needed by retained composite glyphs.</summary>
    public void ExpandCompositeDependencies(SortedSet<int> glyphs)
    {
        var pending = new Queue<int>(glyphs);
        while (pending.TryDequeue(out var glyph))
        {
            if (!TryGetGlyphData(glyph, out var data) || BinaryPrimitives.ReadInt16BigEndian(data) >= 0)
                continue;

            int cursor = 10;
            ushort flags;
            do
            {
                flags = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(cursor, 2));
                int component = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(cursor + 2, 2));
                if (glyphs.Add(component))
                    pending.Enqueue(component);
                cursor = NextCompositeComponent(data, cursor, flags);
            }
            while ((flags & MoreComponents) != 0);
        }
    }

    /// <summary>Creates compact glyf and long-format loca tables for retained glyphs.</summary>
    public Subset CreateSubset(IReadOnlyList<int> glyphs, IReadOnlyDictionary<int, ushort> glyphMap)
    {
        var result = new List<byte>();
        var offsets = new List<int>(glyphs.Count + 1);
        foreach (var glyph in glyphs)
        {
            offsets.Add(result.Count);
            if (TryGetGlyphData(glyph, out var data))
                result.AddRange(CopyGlyph(data, glyphMap));
            while ((result.Count & 3) != 0)
                result.Add(0);
        }
        offsets.Add(result.Count);
        return new Subset(result.ToArray(), LocaTable.CreateLong(offsets));
    }

    public readonly record struct Subset(byte[] Glyf, byte[] Loca);

    private bool TryGetGlyphData(int glyphIndex, out ReadOnlySpan<byte> data)
    {
        var (offset, length) = _loca.GetGlyphBounds(glyphIndex);
        if (length <= 0 || offset + length > _data.Length)
        {
            data = default;
            return false;
        }
        data = _data.Span.Slice(offset, length);
        return true;
    }

    private static byte[] CopyGlyph(ReadOnlySpan<byte> source, IReadOnlyDictionary<int, ushort> glyphMap)
    {
        var result = source.ToArray();
        if (BinaryPrimitives.ReadInt16BigEndian(source) >= 0)
            return result;

        int cursor = 10;
        ushort flags;
        do
        {
            flags = BinaryPrimitives.ReadUInt16BigEndian(source.Slice(cursor, 2));
            int oldComponent = BinaryPrimitives.ReadUInt16BigEndian(source.Slice(cursor + 2, 2));
            if (!glyphMap.TryGetValue(oldComponent, out var newComponent))
                throw new InvalidOperationException($"Composite glyph component {oldComponent} was not included in the subset.");
            BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(cursor + 2, 2), newComponent);
            cursor = NextCompositeComponent(source, cursor, flags);
        }
        while ((flags & MoreComponents) != 0);
        return result;
    }

    private static int NextCompositeComponent(ReadOnlySpan<byte> glyph, int cursor, ushort flags)
    {
        cursor += 4;
        cursor += (flags & Arg1And2AreWords) != 0 ? 4 : 2;
        if ((flags & WeHaveAScale) != 0) cursor += 2;
        else if ((flags & WeHaveAnXAndYScale) != 0) cursor += 4;
        else if ((flags & WeHaveATwoByTwo) != 0) cursor += 8;
        if (cursor > glyph.Length)
            throw new InvalidOperationException("Composite glyph component exceeds its glyph data.");
        return cursor;
    }
}
