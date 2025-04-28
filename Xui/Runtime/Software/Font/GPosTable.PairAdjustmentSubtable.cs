using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public sealed partial class GPosTable
{
    public abstract partial class PairAdjustmentSubtable
    {
        public static PairAdjustmentSubtable Parse(ReadOnlySpan<byte> span)
        {
            ushort format = BinaryPrimitives.ReadUInt16BigEndian(span);
            return format switch
            {
                1 => new Format1(span),
                2 => new Format2(span),
                _ => throw new NotSupportedException($"Unsupported PairAdjustment format {format}")
            };
        }

        public abstract TrueTypeFont.ValueRecordTuple this[ushort leftGlyph, ushort rightGlyph] { get; }
    }
}