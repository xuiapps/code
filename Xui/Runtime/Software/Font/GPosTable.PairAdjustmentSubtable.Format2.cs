using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class PairAdjustmentSubtable
    {
        /// <summary>
        /// Represents a GPOS LookupType 2 subtable: Pair Adjustment Format 2 (class-based kerning).
        /// </summary>
        public sealed class Format2 : PairAdjustmentSubtable
        {
            public ushort ValueFormat1 { get; }
            public ushort ValueFormat2 { get; }

            public ClassDefTable ClassDef1 { get; }
            public ClassDefTable ClassDef2 { get; }

            public PairValueRecord[,] Matrix { get; }

            public CoverageTable Coverage { get; }

            public ushort Class1Count { get; }
            public ushort Class2Count { get; }

            public Format2(ReadOnlySpan<byte> span)
            {
                // Format 2 layout:
                //   0: uint16 posFormat (=2)
                //   2: Offset16 coverageOffset
                //   4: uint16 valueFormat1
                //   6: uint16 valueFormat2
                //   8: Offset16 classDef1Offset
                //  10: Offset16 classDef2Offset
                //  12: uint16 class1Count
                //  14: uint16 class2Count
                //  16: Matrix[class1Count][class2Count] of (ValueRecord1, ValueRecord2)

                ushort posFormat = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
                if (posFormat != 2)
                    throw new NotSupportedException("Only Pair Adjustment Format 2 is supported here.");

                ushort coverageOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
                Coverage = CoverageTable.Parse(span.Slice(coverageOffset));

                ValueFormat1 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));
                ValueFormat2 = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6, 2));
                ushort classDef1Offset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(8, 2));
                ushort classDef2Offset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(10, 2));
                Class1Count = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(12, 2));
                Class2Count = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(14, 2));

                ClassDef1 = ClassDefTable.Parse(span.Slice(classDef1Offset));
                ClassDef2 = ClassDefTable.Parse(span.Slice(classDef2Offset));

                Matrix = new PairValueRecord[Class1Count, Class2Count];

                int offset = 16;
                for (int c1 = 0; c1 < Class1Count; c1++)
                {
                    for (int c2 = 0; c2 < Class2Count; c2++)
                    {
                        var v1 = TrueTypeFont.ValueRecord.Parse(span.Slice(offset), ValueFormat1, out int size1);
                        offset += size1;
                        var v2 = TrueTypeFont.ValueRecord.Parse(span.Slice(offset), ValueFormat2, out int size2);
                        offset += size2;

                        Matrix[c1, c2] = new PairValueRecord(v1, v2);
                    }
                }
            }

            public sealed record PairValueRecord(
                TrueTypeFont.ValueRecord Value1,
                TrueTypeFont.ValueRecord Value2
            );

            public override TrueTypeFont.ValueRecordTuple this[ushort leftGlyph, ushort rightGlyph]
            {
                get
                {
                    if (!Coverage.TryGetValue(leftGlyph, out int _))
                        return default;

                    var class1 = ClassDef1[leftGlyph];
                    var class2 = ClassDef2[rightGlyph];

                    if (class1 >= Class1Count || class2 >= Class2Count)
                        return default;

                    var record = Matrix[class1, class2];
                    return new TrueTypeFont.ValueRecordTuple(record.Value1, record.Value2);
                }
            }
        }
    }
}
