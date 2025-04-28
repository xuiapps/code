using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public class OS2Table
{
    public ushort WeightClass { get; }
    public ushort WidthClass { get; }
    public short TypoAscender { get; }
    public short TypoDescender { get; }
    public short TypoLineGap { get; }

    public OS2Table(ReadOnlySpan<byte> data)
    {
        WeightClass = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(4, 2));
        WidthClass = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(6, 2));

        TypoAscender = BinaryPrimitives.ReadInt16BigEndian(data.Slice(68, 2));
        TypoDescender = BinaryPrimitives.ReadInt16BigEndian(data.Slice(70, 2));
        TypoLineGap = BinaryPrimitives.ReadInt16BigEndian(data.Slice(72, 2));
    }
}
