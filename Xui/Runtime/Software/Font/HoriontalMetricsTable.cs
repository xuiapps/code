using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public class HorizontalMetricsTable
{
    private readonly List<HorizontalMetric> _metrics;

    public HorizontalMetricsTable(ReadOnlySpan<byte> data, ushort numberOfHMetrics)
    {
        _metrics = new List<HorizontalMetric>(numberOfHMetrics);

        int offset = 0;
        for (int i = 0; i < numberOfHMetrics; i++)
        {
            ushort advanceWidth = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            short leftSideBearing = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset + 2, 2));
            _metrics.Add(new HorizontalMetric(advanceWidth, leftSideBearing));
            offset += 4;
        }

        // Any additional glyphs use the last advanceWidth, with their own leftSideBearings
        ushort lastAdvance = _metrics[^1].AdvanceWidth;

        while (offset + 2 <= data.Length)
        {
            short lsb = BinaryPrimitives.ReadInt16BigEndian(data.Slice(offset, 2));
            _metrics.Add(new HorizontalMetric(lastAdvance, lsb));
            offset += 2;
        }
    }

    public HorizontalMetric GetMetric(int glyphIndex)
    {
        if (glyphIndex < _metrics.Count)
            return _metrics[glyphIndex];

        return _metrics[^1];
    }

    public record struct HorizontalMetric(ushort AdvanceWidth, short LeftSideBearing);
}
