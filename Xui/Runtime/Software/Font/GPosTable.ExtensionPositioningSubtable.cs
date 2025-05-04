using System;
using System.Buffers.Binary;
using static Xui.Runtime.Software.Font.GPosTable.LookupTable;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public sealed class ExtensionPositioningSubtable
    {
        public GPosLookupType ExtensionLookupType { get; }
        public PairAdjustmentSubtable? WrappedPairSubtable { get; }

        public ExtensionPositioningSubtable(ReadOnlySpan<byte> span)
        {
            // Format: ExtensionPosFormat1
            // 0: uint16 posFormat (=1)
            // 2: uint16 extensionLookupType
            // 4: Offset32 extensionOffset (relative to this table)

            ushort posFormat = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
            if (posFormat != 1)
                throw new NotSupportedException("Only ExtensionPositioning Format 1 is supported.");

            ushort rawType = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
            var extensionType = (GPosLookupType)rawType;
            ExtensionLookupType = extensionType;

            uint extensionOffset = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(4, 4));

            // Absolute slice for the wrapped table
            ReadOnlySpan<byte> wrappedSpan = span.Slice((int)extensionOffset);

            switch (extensionType)
            {
                case GPosLookupType.PairAdjustment:
                    WrappedPairSubtable = PairAdjustmentSubtable.Parse(wrappedSpan);
                    break;
                default:
                    // Other extension types are not yet supported.
                    break;
            }
        }

        public TrueTypeFont.ValueRecordTuple this[ushort leftGlyph, ushort rightGlyph]
        {
            get
            {
                if (ExtensionLookupType == GPosLookupType.PairAdjustment && WrappedPairSubtable is not null)
                {
                    var valueRecordTuple = WrappedPairSubtable[leftGlyph, rightGlyph];
                    return valueRecordTuple;
                }

                return default;
            }
        }
    }
}
