using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

public partial class TrueTypeFont
{
    public record struct ValueRecord(short? XPlacement, short? YPlacement, short? XAdvance, short? YAdvance)
    {
        public static ValueRecord Parse(ReadOnlySpan<byte> span, ushort format, out int size)
        {
            size = 0;
            short? xPlace = null, yPlace = null, xAdv = null, yAdv = null;

            if ((format & 0x0001) != 0) { xPlace = BinaryPrimitives.ReadInt16BigEndian(span.Slice(size, 2)); size += 2; }
            if ((format & 0x0002) != 0) { yPlace = BinaryPrimitives.ReadInt16BigEndian(span.Slice(size, 2)); size += 2; }
            if ((format & 0x0004) != 0) { xAdv   = BinaryPrimitives.ReadInt16BigEndian(span.Slice(size, 2)); size += 2; }
            if ((format & 0x0008) != 0) { yAdv   = BinaryPrimitives.ReadInt16BigEndian(span.Slice(size, 2)); size += 2; }

            // Note: Anchors (0x0010+) are ignored here

            return new ValueRecord(xPlace, yPlace, xAdv, yAdv);
        }
    }
}
