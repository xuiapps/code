using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public class LocaTable
{
    private readonly int[] _offsets;

    public LocaTable(ReadOnlySpan<byte> data, int numGlyphs, ushort indexToLocFormat)
    {
        _offsets = new int[numGlyphs + 1]; // One more than glyph count

        if (indexToLocFormat == 0)
        {
            // Format 0: ushort entries, multiplied by 2
            for (int i = 0; i <= numGlyphs; i++)
            {
                int raw = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(i * 2, 2));
                _offsets[i] = raw * 2;
            }
        }
        else
        {
            // Format 1: uint entries
            for (int i = 0; i <= numGlyphs; i++)
            {
                _offsets[i] = (int)BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i * 4, 4));
            }
        }
    }

    public (int Offset, int Length) GetGlyphBounds(int glyphIndex)
    {
        if (glyphIndex < 0 || glyphIndex >= _offsets.Length - 1)
            return (0, 0);

        int start = _offsets[glyphIndex];
        int end = _offsets[glyphIndex + 1];
        return (start, end - start);
    }
}
