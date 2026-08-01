using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;

namespace Xui.Runtime.Software.Font;

/// <summary>Builds a compact TrueType sfnt containing the rendered simple glyphs and kerning pairs.</summary>
public static class TrueTypeFontSubset
{
    private const uint ChecksumMagic = 0xB1B0AFBA;

    /// <summary>
    /// Creates a deterministic TrueType subset. The initial format supports BMP characters and
    /// simple and composite glyf outlines; its callers must retain the original font for unsupported scripts.
    /// </summary>
    public static ReadOnlyMemory<byte> Create(TrueTypeFont font, FontUsage usage)
    {
        ArgumentNullException.ThrowIfNull(font);
        ArgumentNullException.ThrowIfNull(usage);
        RequireTables(font, "head", "maxp", "hhea", "hmtx", "loca", "glyf", "cmap");

        var characters = usage.Characters
            .Where(character => font.GetGlyphIndex(character) is not null)
            .OrderBy(character => character)
            .ToArray();
        var oldGlyphs = new SortedSet<int> { 0 };
        foreach (var character in characters)
            oldGlyphs.Add(font.GetGlyphIndex(character)!.Value);
        font.Glyf!.ExpandCompositeDependencies(oldGlyphs);

        var selectedGlyphs = oldGlyphs.ToArray();
        var glyphMap = new Dictionary<int, ushort>(selectedGlyphs.Length);
        ushort nextGlyph = 0;
        foreach (var glyph in selectedGlyphs)
            glyphMap.Add(glyph, nextGlyph++);

        var glyphTables = font.Glyf.CreateSubset(selectedGlyphs, glyphMap);
        var mappings = characters
            .Select(character => new CMapTable.GlyphMapping(character, glyphMap[font.GetGlyphIndex(character)!.Value]))
            .ToArray();

        var tables = new Dictionary<string, byte[]>(StringComparer.Ordinal)
        {
            ["glyf"] = glyphTables.Glyf,
            ["loca"] = glyphTables.Loca,
            ["hmtx"] = font.Hmtx!.CreateSubset(selectedGlyphs),
            ["cmap"] = CMapTable.CreateSubset(mappings),
            ["kern"] = BuildKern(font, usage.Pairs, glyphMap),
            ["head"] = HeaderTable.CreateSubset(Copy(font, "head")),
            ["maxp"] = MaxProfileTable.CreateSubset(Copy(font, "maxp"), (ushort)selectedGlyphs.Length),
            ["hhea"] = HorizontalHeaderTable.CreateSubset(Copy(font, "hhea"), (ushort)selectedGlyphs.Length),
        };

        if (font.Tables.ContainsKey("post"))
            tables.Add("post", PostTable.CreateSubset(Copy(font, "post")));

        // These tables do not refer to glyph indices and preserve names, metrics and hinting.
        foreach (var tag in new[] { "OS/2", "name", "cvt ", "fpgm", "prep", "gasp" })
            if (font.Tables.ContainsKey(tag))
                tables.Add(tag, Copy(font, tag));

        return WriteSfnt(font.Blob.Span.Slice(0, 4), tables);
    }

    private static byte[] BuildKern(TrueTypeFont font, IReadOnlySet<FontUsage.RunePair> pairs, IReadOnlyDictionary<int, ushort> glyphMap)
    {
        var entries = new List<KernTable.Pair>();
        foreach (var pair in pairs)
        {
            var left = font.GetGlyphIndex(pair.Left);
            var right = font.GetGlyphIndex(pair.Right);
            if (left is not int oldLeft || right is not int oldRight || !glyphMap.TryGetValue(oldLeft, out var newLeft) || !glyphMap.TryGetValue(oldRight, out var newRight))
                continue;
            var value = font.Kerning[pair.Left, pair.Right].XAdvance;
            if (value is not null and not 0)
                entries.Add(new KernTable.Pair(newLeft, newRight, (short)value.Value));
        }
        return KernTable.CreateSubset(entries);
    }

    private static ReadOnlyMemory<byte> WriteSfnt(ReadOnlySpan<byte> scalerType, IReadOnlyDictionary<string, byte[]> tables)
    {
        var ordered = tables.OrderBy(pair => pair.Key, StringComparer.Ordinal).ToArray();
        var result = new List<byte>(12 + ordered.Length * 16 + tables.Sum(pair => (pair.Value.Length + 3) & ~3));
        result.AddRange(scalerType.ToArray());
        WriteUInt16(result, (ushort)ordered.Length);
        ushort power = HighestPowerOfTwo((ushort)ordered.Length);
        WriteUInt16(result, (ushort)(power * 16)); WriteUInt16(result, Log2(power)); WriteUInt16(result, (ushort)(ordered.Length * 16 - power * 16));
        int offset = 12 + ordered.Length * 16;
        foreach (var (tag, data) in ordered)
        {
            result.AddRange(System.Text.Encoding.ASCII.GetBytes(tag));
            WriteUInt32(result, Checksum(data));
            WriteUInt32(result, (uint)offset); WriteUInt32(result, (uint)data.Length);
            offset += (data.Length + 3) & ~3;
        }
        foreach (var (_, data) in ordered)
        {
            result.AddRange(data);
            while ((result.Count & 3) != 0) result.Add(0);
        }

        var bytes = result.ToArray();
        int headOffset = FindTableOffset(bytes, "head");
        uint adjustment = unchecked(ChecksumMagic - Checksum(bytes));
        BinaryPrimitives.WriteUInt32BigEndian(bytes.AsSpan(headOffset + 8, 4), adjustment);
        return bytes;
    }

    private static byte[] Copy(TrueTypeFont font, string tag)
    {
        var record = font.Tables[tag];
        return font.Blob.Slice((int)record.Offset, (int)record.Length).ToArray();
    }

    private static void RequireTables(TrueTypeFont font, params string[] tags)
    {
        foreach (var tag in tags)
            if (!font.Tables.ContainsKey(tag))
                throw new InvalidOperationException($"Font is missing required '{tag}' table.");
    }

    private static int FindTableOffset(ReadOnlySpan<byte> sfnt, string tag)
    {
        ushort count = BinaryPrimitives.ReadUInt16BigEndian(sfnt.Slice(4, 2));
        for (int i = 0; i < count; i++)
        {
            var entry = sfnt.Slice(12 + i * 16, 16);
            if (entry.Slice(0, 4).SequenceEqual(System.Text.Encoding.ASCII.GetBytes(tag)))
                return (int)BinaryPrimitives.ReadUInt32BigEndian(entry.Slice(8, 4));
        }
        throw new InvalidOperationException($"Subset font is missing '{tag}' table.");
    }

    private static uint Checksum(ReadOnlySpan<byte> data)
    {
        uint checksum = 0;
        Span<byte> word = stackalloc byte[4];
        for (int offset = 0; offset < data.Length; offset += 4)
        {
            word.Clear();
            data.Slice(offset, Math.Min(4, data.Length - offset)).CopyTo(word);
            checksum = unchecked(checksum + BinaryPrimitives.ReadUInt32BigEndian(word));
        }
        return checksum;
    }

    private static ushort HighestPowerOfTwo(ushort value)
    {
        ushort result = 1;
        while (result * 2 <= value) result *= 2;
        return result;
    }

    private static ushort Log2(ushort value)
    {
        ushort result = 0;
        while (value > 1) { value /= 2; result++; }
        return result;
    }

    private static void WriteUInt16(List<byte> destination, ushort value) { destination.Add((byte)(value >> 8)); destination.Add((byte)value); }
    private static void WriteUInt32(List<byte> destination, uint value) { destination.Add((byte)(value >> 24)); destination.Add((byte)(value >> 16)); destination.Add((byte)(value >> 8)); destination.Add((byte)value); }
}
