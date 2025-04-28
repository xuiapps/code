using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public class HeaderTable
{
    public ushort FontRevisionMajor { get; }
    public ushort FontRevisionMinor { get; }
    public ushort UnitsPerEm { get; }
    public DateTime Created { get; }
    public DateTime Modified { get; }
    public short XMin { get; }
    public short YMin { get; }
    public short XMax { get; }
    public short YMax { get; }
    public ushort MacStyle { get; }
    public ushort LowestRecPPEM { get; }
    public ushort IndexToLocFormat { get; }

    public bool IsBold => (MacStyle & 0x0001) != 0;
    public bool IsItalic => (MacStyle & 0x0002) != 0;
    public bool IsUnderline => (MacStyle & 0x0004) != 0;
    public bool IsOutline => (MacStyle & 0x0008) != 0;
    public bool IsShadow => (MacStyle & 0x0010) != 0;
    public bool IsCondensed => (MacStyle & 0x0020) != 0;
    public bool IsExtended => (MacStyle & 0x0040) != 0;

    public HeaderTable(ReadOnlySpan<byte> data)
    {
        uint fontRevisionFixed = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4, 4));
        FontRevisionMajor = (ushort)(fontRevisionFixed >> 16);
        FontRevisionMinor = (ushort)(fontRevisionFixed & 0xFFFF);

        UnitsPerEm = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(18, 2));

        ulong createdSeconds = BinaryPrimitives.ReadUInt64BigEndian(data.Slice(20, 8));
        Created = MacEpochToDateTime(createdSeconds);

        ulong modifiedSeconds = BinaryPrimitives.ReadUInt64BigEndian(data.Slice(28, 8));
        Modified = MacEpochToDateTime(modifiedSeconds);

        XMin = BinaryPrimitives.ReadInt16BigEndian(data.Slice(36, 2));
        YMin = BinaryPrimitives.ReadInt16BigEndian(data.Slice(38, 2));
        XMax = BinaryPrimitives.ReadInt16BigEndian(data.Slice(40, 2));
        YMax = BinaryPrimitives.ReadInt16BigEndian(data.Slice(42, 2));

        MacStyle = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(44, 2));
        LowestRecPPEM = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(46, 2));
        IndexToLocFormat = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(50, 2));
    }

    private static DateTime MacEpochToDateTime(ulong secondsSince1904)
    {
        var macEpoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        try
        {
            return macEpoch.AddSeconds(secondsSince1904);
        }
        catch
        {
            return macEpoch;
        }
    }
}
