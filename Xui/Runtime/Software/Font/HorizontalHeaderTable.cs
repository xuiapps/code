using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public class HorizontalHeaderTable
{
    public short Ascender { get; }
    public short Descender { get; }
    public short LineGap { get; }

    public ushort AdvanceWidthMax { get; }
    public short MinLeftSideBearing { get; }
    public short MinRightSideBearing { get; }
    public short XMaxExtent { get; }
    public short CaretSlopeRise { get; }
    public short CaretSlopeRun { get; }
    public short CaretOffset { get; }

    public short MetricDataFormat { get; }
    public ushort NumberOfHMetrics { get; }

    public HorizontalHeaderTable(ReadOnlySpan<byte> data)
    {
        Ascender = BinaryPrimitives.ReadInt16BigEndian(data.Slice(4, 2));
        Descender = BinaryPrimitives.ReadInt16BigEndian(data.Slice(6, 2));
        LineGap = BinaryPrimitives.ReadInt16BigEndian(data.Slice(8, 2));

        AdvanceWidthMax = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(10, 2));
        MinLeftSideBearing = BinaryPrimitives.ReadInt16BigEndian(data.Slice(12, 2));
        MinRightSideBearing = BinaryPrimitives.ReadInt16BigEndian(data.Slice(14, 2));
        XMaxExtent = BinaryPrimitives.ReadInt16BigEndian(data.Slice(16, 2));

        CaretSlopeRise = BinaryPrimitives.ReadInt16BigEndian(data.Slice(18, 2));
        CaretSlopeRun = BinaryPrimitives.ReadInt16BigEndian(data.Slice(20, 2));
        CaretOffset = BinaryPrimitives.ReadInt16BigEndian(data.Slice(22, 2));
        // skip 5 reserved int16 values from offset 24 to 34

        MetricDataFormat = BinaryPrimitives.ReadInt16BigEndian(data.Slice(32, 2));
        NumberOfHMetrics = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(34, 2));
    }
}
