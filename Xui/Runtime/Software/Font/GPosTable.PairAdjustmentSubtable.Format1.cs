using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class PairAdjustmentSubtable
    {
        public class Format1 : PairAdjustmentSubtable
        {
            public ushort ValueFormat1 { get; }
            public ushort ValueFormat2 { get; }

            public List<PairSet> PairSets { get; } = new();

            public CoverageTable Coverage { get; }

            public Format1(ReadOnlySpan<byte> span)
            {
                // Format 1 layout:
                //   0: uint16 posFormat (=1)
                //   2: Offset16 coverageOffset
                //   4: uint16 valueFormat1
                //   6: uint16 valueFormat2
                //   8: uint16 pairSetCount
                //  10: Offset16 pairSetOffsets[pairSetCount]
                //
                // Each PairSet:
                //   0: uint16 pairValueCount
                //   2: pairValueCount × {
                //         uint16 secondGlyphID,
                //         ValueRecord1,
                //         ValueRecord2
                //      }

                ushort posFormat = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
                if (posFormat != 1)
                    throw new NotSupportedException("Only Pair Adjustment Format 1 is supported here.");

                ushort coverageOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
                Coverage = CoverageTable.Parse(span.Slice(coverageOffset));

                ValueFormat1 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));
                ValueFormat2 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6, 2));
                ushort pairSetCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(8, 2));

                for (int i = 0; i < pairSetCount; i++)
                {
                    ushort pairSetOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(10 + i * 2, 2));
                    PairSets.Add(ParsePairSet(span.Slice(pairSetOffset), ValueFormat1, ValueFormat2));
                }
            }

            private static PairSet ParsePairSet(ReadOnlySpan<byte> span, ushort fmt1, ushort fmt2)
            {
                ushort pairValueCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
                var pairs = new List<PairValueRecord>(pairValueCount);

                int offset = 2;
                for (int i = 0; i < pairValueCount; i++)
                {
                    ushort secondGlyphID = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(offset, 2));
                    offset += 2;

                    var v1 = TrueTypeFont.ValueRecord.Parse(span.Slice(offset), fmt1, out int v1Size);
                    offset += v1Size;

                    var v2 = TrueTypeFont.ValueRecord.Parse(span.Slice(offset), fmt2, out int v2Size);
                    offset += v2Size;

                    pairs.Add(new PairValueRecord(secondGlyphID, v1, v2));
                }

                return new PairSet(pairs);
            }

            public sealed record PairSet(List<PairValueRecord> Pairs);
            public sealed record PairValueRecord(ushort SecondGlyphID, TrueTypeFont.ValueRecord Value1, TrueTypeFont.ValueRecord Value2);

            public override TrueTypeFont.ValueRecordTuple this[ushort leftGlyph, ushort rightGlyph]
            {
                get
                {
                    if (!Coverage.TryGetValue(leftGlyph, out int pairSetIndex))
                        return default;

                    if (pairSetIndex < 0 || pairSetIndex >= PairSets.Count)
                        return default;

                    var pairSet = PairSets[pairSetIndex];
                    foreach (var pair in pairSet.Pairs)
                    {
                        if (pair.SecondGlyphID == rightGlyph)
                            return new TrueTypeFont.ValueRecordTuple(pair.Value1, pair.Value2);
                    }

                    return default;
                }
            }
        }
    }
}
