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
}
