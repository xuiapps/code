using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace Xui.Runtime.Software.Font;

public class NameTable
{
    public IReadOnlyList<NameRecord> Names { get; }

    public NameTable(ReadOnlySpan<byte> data)
    {
        ushort format = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(0, 2));
        ushort count = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(2, 2));
        ushort stringStorageOffset = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(4, 2));

        var names = new List<NameRecord>(count);

        for (int i = 0; i < count; i++)
        {
            int recordOffset = 6 + i * 12;

            ushort platformID   = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 0, 2));
            ushort encodingID   = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 2, 2));
            ushort languageID   = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 4, 2));
            ushort nameID       = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 6, 2));
            ushort length       = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 8, 2));
            ushort offsetInStorage = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(recordOffset + 10, 2));

            int stringOffset = stringStorageOffset + offsetInStorage;

            if (stringOffset + length > data.Length)
                continue;

            var stringData = data.Slice(stringOffset, length).ToArray(); // required for Encoding

            string value = DecodeNameString(platformID, encodingID, stringData);

            names.Add(new NameRecord(nameID, value));
        }

        Names = names;
    }

    private static string DecodeNameString(ushort platformID, ushort encodingID, byte[] data)
    {
        try
        {
            return (platformID, encodingID) switch
            {
                // Unicode Platform (ISO 10646)
                (0, _) => Encoding.BigEndianUnicode.GetString(data), // UTF-16BE

                // Macintosh Platform
                (1, 0) => Encoding.GetEncoding(10000).GetString(data), // MacRoman
                (1, 1) => Encoding.GetEncoding(10001).GetString(data), // Japanese (MacJapanese)
                (1, 2) => Encoding.GetEncoding(10002).GetString(data), // Traditional Chinese (Big5)
                (1, 3) => Encoding.GetEncoding(10003).GetString(data), // Korean
                (1, 4) => Encoding.GetEncoding(10004).GetString(data), // Arabic
                (1, 5) => Encoding.GetEncoding(10005).GetString(data), // Hebrew
                (1, 6) => Encoding.GetEncoding(10006).GetString(data), // Greek
                (1, 7) => Encoding.GetEncoding(10007).GetString(data), // Cyrillic
                (1, 8) => Encoding.GetEncoding(10008).GetString(data), // Simplified Chinese (GB2312)
                (1, 9) => Encoding.GetEncoding(10009).GetString(data), // Romanian
                (1, 10) => Encoding.GetEncoding(10010).GetString(data), // Ukrainian
                (1, 11) => Encoding.GetEncoding(10017).GetString(data), // Thai
                (1, 12) => Encoding.GetEncoding(10021).GetString(data), // Indic
                (1, 13) => Encoding.GetEncoding(10029).GetString(data), // Central Europe
                (1, 14) => Encoding.GetEncoding(10079).GetString(data), // Icelandic
                (1, 15) => Encoding.GetEncoding(10081).GetString(data), // Turkish
                (1, 16) => Encoding.GetEncoding(10082).GetString(data), // Croatian

                // Windows Platform
                (3, 0) => Encoding.BigEndianUnicode.GetString(data), // Symbol — usually UTF-16
                (3, 1) => Encoding.BigEndianUnicode.GetString(data), // Unicode BMP — UTF-16
                (3, 10) => Encoding.BigEndianUnicode.GetString(data), // Unicode full — UTF-16

                // Fallback
                _ => Encoding.UTF8.GetString(data), // attempt UTF-8 if nothing else matched
            };
        }
        catch
        {
            return $"NameTable missing encoding for platform {platformID}, encoding {encodingID}.";
        }
    }

    public string? GetNameById(int id)
    {
        foreach (var name in Names)
        {
            if (name.NameID == id)
                return name.Value;
        }
        return null;
    }

    public record struct NameRecord(ushort NameID, string Value);
}
