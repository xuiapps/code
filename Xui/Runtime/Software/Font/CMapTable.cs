using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xui.Runtime.Software.Font;

public class CMapTable
{
    private readonly Dictionary<int, int> _unicodeToGlyph = new();
    private readonly List<Format12Group> format12Groups = [];

    public CMapTable(ReadOnlySpan<byte> data)
    {
        ushort version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(0, 2));
        ushort numTables = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(2, 2));

        uint bestOffset = 0;
        int bestScore = -1;

        for (int i = 0; i < numTables; i++)
        {
            int entryOffset = 4 + i * 8;
            ushort platformID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(entryOffset + 0, 2));
            ushort encodingID = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(entryOffset + 2, 2));
            uint subtableOffset = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(entryOffset + 4, 4));

            var subtableF = data.Slice((int)subtableOffset);
            ushort formatF = BinaryPrimitives.ReadUInt16BigEndian(subtableF.Slice(0, 2));

            if (formatF is not 4 and not 12)
                continue;

            int score = platformID switch
            {
                3 when encodingID == 10 => 5,
                3 when encodingID == 1  => 4,
                0 when encodingID == 4  => 3,
                0 when encodingID == 3  => 2,
                1 when encodingID == 0  => 1,
                _ => 0
            };

            if (score > bestScore)
            {
                bestScore = score;
                bestOffset = subtableOffset;
            }
        }

        if (bestOffset == 0) return;

        var subtable = data.Slice((int)bestOffset);
        ushort format = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(0, 2));

        if (format == 12)
        {
            ParseFormat12(subtable);
            return;
        }
        if (format == 4)
            ParseFormat4(subtable);
    }

    private void ParseFormat4(ReadOnlySpan<byte> subtable)
    {
        ushort segCountX2 = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(6, 2));
        int segCount = segCountX2 / 2;

        int endCodeOffset = 14;
        int startCodeOffset = endCodeOffset + segCount * 2 + 2;
        int idDeltaOffset = startCodeOffset + segCount * 2;
        int idRangeOffsetOffset = idDeltaOffset + segCount * 2;
        int glyphIdArrayOffset = idRangeOffsetOffset + segCount * 2;

        for (int i = 0; i < segCount; i++)
        {
            ushort endCode = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(endCodeOffset + i * 2, 2));
            ushort startCode = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(startCodeOffset + i * 2, 2));
            short idDelta = BinaryPrimitives.ReadInt16BigEndian(subtable.Slice(idDeltaOffset + i * 2, 2));
            ushort idRangeOffset = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(idRangeOffsetOffset + i * 2, 2));

            for (int c = startCode; c <= endCode; c++)
            {
                int glyphId;

                if (idRangeOffset == 0)
                {
                    glyphId = (c + idDelta) & 0xFFFF;
                }
                else
                {
                    int offsetWithinIdRangeOffsetField = idRangeOffsetOffset + i * 2;
                    int glyphOffsetInBytes = offsetWithinIdRangeOffsetField + idRangeOffset + (c - startCode) * 2;

                    if (glyphOffsetInBytes + 1 >= subtable.Length)
                        continue;

                    ushort glyphIndex = BinaryPrimitives.ReadUInt16BigEndian(subtable.Slice(glyphOffsetInBytes, 2));
                    if (glyphIndex == 0) continue;

                    glyphId = (glyphIndex + idDelta) & 0xFFFF;
                }

                _unicodeToGlyph[c] = glyphId;
            }
        }
    }

    public int? GetGlyphIndex(int unicode)
    {
        if (_unicodeToGlyph.TryGetValue(unicode, out var gid))
            return gid;

        foreach (var group in format12Groups)
            if (unicode >= group.StartCharCode && unicode <= group.EndCharCode)
                return checked((int)(group.StartGlyphId + unicode - group.StartCharCode));

        return null;
    }

    public int? GetGlyphIndex(Rune rune) => GetGlyphIndex(rune.Value);

    /// <summary>Creates a Windows Unicode cmap for the retained Unicode scalar values.</summary>
    public static byte[] CreateSubset(IReadOnlyList<GlyphMapping> mappings)
    {
        if (mappings.Any(mapping => mapping.Character.Value > ushort.MaxValue))
            return CreateFormat12(mappings);

        const int cmapHeaderLength = 12;
        int segmentCount = mappings.Count + 1; // Includes required U+FFFF sentinel.
        int subtableLength = 16 + segmentCount * 8 + mappings.Count * 2;
        var result = new byte[cmapHeaderLength + subtableLength];
        var span = result.AsSpan();
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(2, 2), 1);
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(4, 2), 3); // Windows Unicode BMP
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(6, 2), 1);
        BinaryPrimitives.WriteUInt32BigEndian(span.Slice(8, 4), cmapHeaderLength);

        var table = span.Slice(cmapHeaderLength);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(0, 2), 4);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(2, 2), (ushort)subtableLength);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(6, 2), (ushort)(segmentCount * 2));
        ushort power = HighestPowerOfTwo((ushort)segmentCount);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(8, 2), (ushort)(power * 2));
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(10, 2), Log2(power));
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(12, 2), (ushort)(segmentCount * 2 - power * 2));

        int endCodes = 14;
        int startCodes = endCodes + segmentCount * 2 + 2;
        int deltas = startCodes + segmentCount * 2;
        int ranges = deltas + segmentCount * 2;
        int glyphArray = ranges + segmentCount * 2;
        for (int i = 0; i < mappings.Count; i++)
        {
            var mapping = mappings[i];
            ushort code = (ushort)mapping.Character.Value;
            BinaryPrimitives.WriteUInt16BigEndian(table.Slice(endCodes + i * 2, 2), code);
            BinaryPrimitives.WriteUInt16BigEndian(table.Slice(startCodes + i * 2, 2), code);
            BinaryPrimitives.WriteUInt16BigEndian(table.Slice(ranges + i * 2, 2), (ushort)(segmentCount * 2));
            BinaryPrimitives.WriteUInt16BigEndian(table.Slice(glyphArray + i * 2, 2), mapping.GlyphId);
        }
        int sentinel = mappings.Count;
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(endCodes + sentinel * 2, 2), ushort.MaxValue);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(startCodes + sentinel * 2, 2), ushort.MaxValue);
        BinaryPrimitives.WriteInt16BigEndian(table.Slice(deltas + sentinel * 2, 2), 1);
        return result;
    }

    private static byte[] CreateFormat12(IReadOnlyList<GlyphMapping> mappings)
    {
        var groups = new List<Format12Group>();
        foreach (var mapping in mappings)
        {
            uint character = (uint)mapping.Character.Value;
            if (groups.Count > 0 && groups[^1].EndCharCode + 1 == character && groups[^1].StartGlyphId + groups[^1].EndCharCode - groups[^1].StartCharCode + 1 == mapping.GlyphId)
            {
                groups[^1] = groups[^1] with { EndCharCode = character };
            }
            else
            {
                groups.Add(new Format12Group(character, character, mapping.GlyphId));
            }
        }

        const int cmapHeaderLength = 12;
        int subtableLength = 16 + groups.Count * 12;
        var result = new byte[cmapHeaderLength + subtableLength];
        var span = result.AsSpan();
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(2, 2), 1);
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(4, 2), 3); // Windows Unicode full repertoire
        BinaryPrimitives.WriteUInt16BigEndian(span.Slice(6, 2), 10);
        BinaryPrimitives.WriteUInt32BigEndian(span.Slice(8, 4), cmapHeaderLength);

        var table = span.Slice(cmapHeaderLength);
        BinaryPrimitives.WriteUInt16BigEndian(table.Slice(0, 2), 12);
        BinaryPrimitives.WriteUInt32BigEndian(table.Slice(4, 4), (uint)subtableLength);
        BinaryPrimitives.WriteUInt32BigEndian(table.Slice(12, 4), (uint)groups.Count);
        for (int i = 0; i < groups.Count; i++)
        {
            int offset = 16 + i * 12;
            var group = groups[i];
            BinaryPrimitives.WriteUInt32BigEndian(table.Slice(offset, 4), group.StartCharCode);
            BinaryPrimitives.WriteUInt32BigEndian(table.Slice(offset + 4, 4), group.EndCharCode);
            BinaryPrimitives.WriteUInt32BigEndian(table.Slice(offset + 8, 4), group.StartGlyphId);
        }
        return result;
    }

    public readonly record struct GlyphMapping(Rune Character, ushort GlyphId);

    private void ParseFormat12(ReadOnlySpan<byte> table)
    {
        uint groupCount = BinaryPrimitives.ReadUInt32BigEndian(table.Slice(12, 4));
        for (int i = 0; i < groupCount; i++)
        {
            int offset = 16 + i * 12;
            format12Groups.Add(new Format12Group(
                BinaryPrimitives.ReadUInt32BigEndian(table.Slice(offset, 4)),
                BinaryPrimitives.ReadUInt32BigEndian(table.Slice(offset + 4, 4)),
                BinaryPrimitives.ReadUInt32BigEndian(table.Slice(offset + 8, 4))));
        }
    }

    private readonly record struct Format12Group(uint StartCharCode, uint EndCharCode, uint StartGlyphId);

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
}
