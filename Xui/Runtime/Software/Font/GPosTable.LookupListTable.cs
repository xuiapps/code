using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public sealed class LookupListTable : IReadOnlyList<LookupTable>
    {
        private readonly List<LookupTable> _lookups;

        public LookupListTable(ReadOnlySpan<byte> span)
        {
            // LookupList:
            //   0: uint16 lookupCount
            //   2: Offset16[lookupCount] (offsets from start of LookupList)
            //
            // Each LookupTable:
            //   0: uint16 lookupType
            //   2: uint16 lookupFlag
            //   4: uint16 subTableCount
            //   6: Offset16[] subTableOffsets

            ushort lookupCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            _lookups = new List<LookupTable>(lookupCount);

            for (int i = 0; i < lookupCount; i++)
            {
                ushort lookupOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2 + i * 2, 2));
                var table = new LookupTable(span.Slice(lookupOffset));
                _lookups.Add(table);
            }
        }

        public LookupTable this[int index] => _lookups[index];
        public int Count => _lookups.Count;
        public IEnumerator<LookupTable> GetEnumerator() => _lookups.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _lookups.GetEnumerator();
    }

    public sealed class LookupTable
    {
        public GPosLookupType LookupType { get; }
        public ushort LookupFlag { get; }

        public List<ushort> SubTableOffsets { get; } = new();

        public List<PairAdjustmentSubtable>? PairAdjustmentSubtables { get; private set; }

        public LookupTable(ReadOnlySpan<byte> span)
        {
            LookupType = (GPosLookupType)BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            LookupFlag = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
            ushort subTableCount = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));

            for (int i = 0; i < subTableCount; i++)
            {
                ushort offset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6 + i * 2, 2));
                SubTableOffsets.Add(offset);
            }

            // TODO: Probably make polymorphic and handle the rest
            if (LookupType == GPosLookupType.PairAdjustment)
            {
                PairAdjustmentSubtables = new List<PairAdjustmentSubtable>();

                foreach (var subOffset in SubTableOffsets)
                {
                    PairAdjustmentSubtables.Add(PairAdjustmentSubtable.Parse(span.Slice(subOffset)));
                }
            }
        }

        public enum GPosLookupType : ushort
        {
            // Adjust a single glyph's position
            SingleAdjustment = 1,
            // Adjust position between pairs (kerning!)
            PairAdjustment = 2,
            // Join cursive glyphs (Arabic-style)
            CursiveAttachment = 3,
            // Attach diacritics to base glyphs
            MarkToBase = 4,
            // Attach diacritics to ligatures
            MarkToLigature = 5,
            // Attach marks to other marks
            MarkToMark = 6,
            // Position glyphs based on context
            ContextPositioning = 7,
            // Context + chaining (more complex rules)
            ChainedContextPositioning = 8,
            // Wraps other types to extend limits
            ExtensionPositioning = 9
        }
    }
}
