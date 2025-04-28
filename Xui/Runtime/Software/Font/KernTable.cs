using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace Xui.Runtime.Software.Font;

public sealed class KernTable
{
    public readonly Dictionary<(ushort Left, ushort Right), short> Pairs;

    public CMapTable CMap { get; }

    public KernTable(ReadOnlySpan<byte> data, CMapTable cmap)
    {
        this.CMap = cmap;

        Pairs = new();

        if (data.Length < 4) return;

        ushort version = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(0, 2));
        ushort nTables = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(2, 2));
        int offset = 4;

        for (int i = 0; i < nTables; i++)
        {
            if (data.Length < offset + 6) break;

            ushort subVersion = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            ushort length = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset + 2, 2));
            ushort coverage = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset + 4, 2));

            byte format = (byte)(coverage >> 8);
            int subtableOffset = offset + 6;

            if (format == 0 && data.Length >= subtableOffset + 8)
            {
                ushort nPairs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(subtableOffset + 2, 2));
                int pairOffset = subtableOffset + 6;

                for (int p = 0; p < nPairs && pairOffset + 6 <= data.Length; p++, pairOffset += 6)
                {
                    ushort left = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(pairOffset, 2));
                    ushort right = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(pairOffset + 2, 2));
                    short value = BinaryPrimitives.ReadInt16BigEndian(data.Slice(pairOffset + 4, 2));

                    Pairs[(left, right)] = value;
                }
            }

            offset += length;
        }
    }

    public short GetKerningAdjustment(ushort left, ushort right)
        => Pairs.TryGetValue((left, right), out var value) ? value : (short)0;

    public TrueTypeFont.ValueRecord this[char left, char right]
    {
        get
        {
            int? g1 = CMap.GetGlyphIndex(left);
            int? g2 = CMap.GetGlyphIndex(right);
            if (g1 is not int l || g2 is not int r)
                return default;

            if (Pairs.TryGetValue(((ushort)l, (ushort)r), out short dx))
                return new TrueTypeFont.ValueRecord(dx, null, null, null);

            return default;
        }
    }
}
