using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public class CMapTable
{
    private readonly Dictionary<int, int> _unicodeToGlyph = new();

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

            // Only Format 4 is supported for now...
            if (formatF != 4)
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

        if (format != 4) return;

        // Format 4 header:
        // 2: length
        // 4: language
        // 6: segCountX2
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
        => _unicodeToGlyph.TryGetValue(unicode, out var gid) ? gid : null;
}
