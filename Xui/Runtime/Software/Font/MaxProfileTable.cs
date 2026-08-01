using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public class MaxProfileTable
{
    public ushort NumGlyphs { get; }

    public MaxProfileTable(ReadOnlySpan<byte> data)
    {
        // Version is 4 bytes (skip it), then NumGlyphs is at offset 4
        NumGlyphs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(4, 2));
    }

    /// <summary>Copies a maxp table with the compact glyph count.</summary>
    public static byte[] CreateSubset(ReadOnlySpan<byte> source, ushort numGlyphs)
    {
        var result = source.ToArray();
        BinaryPrimitives.WriteUInt16BigEndian(result.AsSpan(4, 2), numGlyphs);
        return result;
    }
}
