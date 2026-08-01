using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                // The format-0 body starts immediately after the six-byte subtable header:
                // nPairs, searchRange, entrySelector, rangeShift, then pair records.
                ushort nPairs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(subtableOffset, 2));
                int pairOffset = subtableOffset + 8;

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

    public TrueTypeFont.ValueRecord this[Rune left, Rune right]
    {
        get
        {
            int? g1 = CMap.GetGlyphIndex(left);
            int? g2 = CMap.GetGlyphIndex(right);
            if (g1 is not int l || g2 is not int r)
                return default;

            if (Pairs.TryGetValue(((ushort)l, (ushort)r), out short dx))
                return new TrueTypeFont.ValueRecord(null, null, dx, null);

            return default;
        }
    }

    /// <summary>Creates a horizontal format-0 kern table from the retained pairs.</summary>
    public static byte[] CreateSubset(IEnumerable<Pair> pairs)
    {
        var ordered = pairs.OrderBy(pair => pair.Left).ThenBy(pair => pair.Right).ToArray();
        var result = new List<byte>();
        WriteUInt16(result, 0); WriteUInt16(result, 1);
        WriteUInt16(result, 0); WriteUInt16(result, (ushort)(14 + ordered.Length * 6)); WriteUInt16(result, 1);
        WriteUInt16(result, (ushort)ordered.Length);
        ushort power = HighestPowerOfTwo((ushort)Math.Max(ordered.Length, 1));
        WriteUInt16(result, (ushort)(power * 6)); WriteUInt16(result, Log2(power)); WriteUInt16(result, (ushort)(ordered.Length * 6 - power * 6));
        foreach (var pair in ordered)
        {
            WriteUInt16(result, pair.Left); WriteUInt16(result, pair.Right); WriteInt16(result, pair.Value);
        }
        return result.ToArray();
    }

    public readonly record struct Pair(ushort Left, ushort Right, short Value);

    private static ushort HighestPowerOfTwo(ushort value)
    {
        ushort result = 1;
        while (result * 2 <= value) result *= 2;
        return result;
    }

    private static ushort Log2(ushort value)
    {
        ushort result = 0;
        while (value > 1) { value /= 2; result++; }
        return result;
    }

    private static void WriteUInt16(List<byte> destination, ushort value)
    {
        destination.Add((byte)(value >> 8));
        destination.Add((byte)value);
    }

    private static void WriteInt16(List<byte> destination, short value) =>
        WriteUInt16(destination, unchecked((ushort)value));
}
