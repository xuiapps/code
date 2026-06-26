using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xui.Runtime.Software.Font;

public partial class TrueTypeFont
{
    /// <summary>
    /// Produces a trimmed font blob that retains only tables needed by Xui text measurement and SVG text presentation.
    /// </summary>
    public ReadOnlyMemory<byte> TrimToTouchedTablesForSvg()
    {
        var requiredTags = new HashSet<string>(StringComparer.Ordinal);

        if (Head is not null) requiredTags.Add("head");
        if (Maxp is not null) requiredTags.Add("maxp");
        if (Cmap is not null) requiredTags.Add("cmap");
        if (Post is not null) requiredTags.Add("post");
        if (Loca is not null) requiredTags.Add("loca");
        if (Glyf is not null) requiredTags.Add("glyf");
        if (OS2 is not null) requiredTags.Add("OS/2");
        if (Name is not null) requiredTags.Add("name");
        if (Hhea is not null) requiredTags.Add("hhea");
        if (Hmtx is not null) requiredTags.Add("hmtx");
        if (Kern is not null) requiredTags.Add("kern");
        if (GPos is not null) requiredTags.Add("GPOS");

        // Ensure core dependencies for TrueType outlines remain consistent.
        if (requiredTags.Contains("glyf"))
        {
            requiredTags.Add("loca");
            requiredTags.Add("maxp");
            requiredTags.Add("head");
        }

        if (requiredTags.Contains("hmtx"))
        {
            requiredTags.Add("hhea");
            requiredTags.Add("maxp");
            requiredTags.Add("head");
        }

        if (requiredTags.Count == 0)
            return Blob;

        var selected = Tables
            .Where(x => requiredTags.Contains(x.Key))
            .Select(x => x.Value)
            .OrderBy(x => x.Tag, StringComparer.Ordinal)
            .ToArray();

        if (selected.Length == 0 || selected.Length == Tables.Count)
            return Blob;

        return BuildFontBlob(selected);
    }

    private ReadOnlyMemory<byte> BuildFontBlob(IReadOnlyList<TableRecord> tables)
    {
        if (Blob.Length < 12)
            return Blob;

        uint sfntVersion = BinaryPrimitives.ReadUInt32BigEndian(Blob.Span.Slice(0, 4));
        int tableCount = tables.Count;
        if (tableCount == 0)
            return Blob;

        int dataOffset = 12 + (tableCount * 16);
        int totalSize = dataOffset;

        for (int i = 0; i < tables.Count; i++)
            totalSize += Align4((int)tables[i].Length);

        var output = new byte[totalSize];
        var span = output.AsSpan();

        WriteUInt32(span, 0, sfntVersion);
        WriteUInt16(span, 4, (ushort)tableCount);

        ushort maxPow2 = HighestPowerOfTwoNotGreaterThan((ushort)tableCount);
        ushort searchRange = (ushort)(maxPow2 * 16);
        ushort entrySelector = Log2(maxPow2);
        ushort rangeShift = (ushort)(tableCount * 16 - searchRange);

        WriteUInt16(span, 6, searchRange);
        WriteUInt16(span, 8, entrySelector);
        WriteUInt16(span, 10, rangeShift);

        int tableWriteOffset = dataOffset;
        int headTableOffset = -1;
        int headTableLength = 0;
        int headRecordChecksumOffset = -1;

        for (int i = 0; i < tables.Count; i++)
        {
            var table = tables[i];
            int recordOffset = 12 + (i * 16);
            int paddedLength = Align4((int)table.Length);

            WriteTag(span, recordOffset, table.Tag);
            WriteUInt32(span, recordOffset + 8, (uint)tableWriteOffset);
            WriteUInt32(span, recordOffset + 12, table.Length);

            var src = Blob.Span.Slice((int)table.Offset, (int)table.Length);
            src.CopyTo(span.Slice(tableWriteOffset, (int)table.Length));

            if (table.Tag == "head" && table.Length >= 12)
            {
                headTableOffset = tableWriteOffset;
                headTableLength = (int)table.Length;
                headRecordChecksumOffset = recordOffset + 4;
                span.Slice(headTableOffset + 8, 4).Clear();
            }

            uint checksum = ComputeChecksum(span.Slice(tableWriteOffset, paddedLength));
            WriteUInt32(span, recordOffset + 4, checksum);

            tableWriteOffset += paddedLength;
        }

        if (headTableOffset >= 0)
        {
            uint fullChecksum = ComputeChecksum(span);
            uint adjustment = 0xB1B0AFBAu - fullChecksum;
            WriteUInt32(span, headTableOffset + 8, adjustment);

            uint headChecksum = ComputeChecksum(span.Slice(headTableOffset, Align4(headTableLength)));
            WriteUInt32(span, headRecordChecksumOffset, headChecksum);
        }

        return output;
    }

    private static int Align4(int value) => (value + 3) & ~3;

    private static ushort HighestPowerOfTwoNotGreaterThan(ushort value)
    {
        if (value == 0)
            return 0;

        ushort result = 1;
        while ((result << 1) <= value)
            result <<= 1;

        return result;
    }

    private static ushort Log2(ushort value)
    {
        ushort result = 0;
        while (value > 1)
        {
            value >>= 1;
            result++;
        }

        return result;
    }

    private static void WriteTag(Span<byte> destination, int offset, string tag)
    {
        if (tag.Length < 4)
            throw new ArgumentException("Table tag must be exactly 4 bytes.", nameof(tag));

        Encoding.ASCII.GetBytes(tag.AsSpan(0, 4), destination.Slice(offset, 4));
    }

    private static void WriteUInt16(Span<byte> destination, int offset, ushort value)
        => BinaryPrimitives.WriteUInt16BigEndian(destination.Slice(offset, 2), value);

    private static void WriteUInt32(Span<byte> destination, int offset, uint value)
        => BinaryPrimitives.WriteUInt32BigEndian(destination.Slice(offset, 4), value);

    private static uint ComputeChecksum(ReadOnlySpan<byte> data)
    {
        uint sum = 0;
        int paddedLength = Align4(data.Length);

        for (int i = 0; i < paddedLength; i += 4)
        {
            uint value;
            if (i + 4 <= data.Length)
            {
                value = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(i, 4));
            }
            else
            {
                Span<byte> tail = stackalloc byte[4];
                int remaining = data.Length - i;
                if (remaining > 0)
                    data.Slice(i, remaining).CopyTo(tail);
                value = BinaryPrimitives.ReadUInt32BigEndian(tail);
            }

            sum += value;
        }

        return sum;
    }
}
