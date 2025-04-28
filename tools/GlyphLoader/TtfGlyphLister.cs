using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class TtfGlyphLister
{
    private string[] glyphNames; // <- NEW: glyph names from post table
    private Dictionary<int, List<int>> glyphToUnicode; // <- map from cmap

    public void ListGlyphsAndPaths(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var reader = new BinaryReader(stream);

        uint scalerType = ReadUInt32BE(reader);
        ushort numTables = ReadUInt16BE(reader);
        reader.BaseStream.Seek(6, SeekOrigin.Current); // skip searchRange, entrySelector, rangeShift

        uint locaOffset = 0, glyfOffset = 0, maxpOffset = 0, headOffset = 0, cmapOffset = 0, postOffset = 0;
        ushort indexToLocFormat = 0;

        for (int i = 0; i < numTables; i++)
        {
            string tag = new string(reader.ReadChars(4));
            uint checkSum = ReadUInt32BE(reader);
            uint offset = ReadUInt32BE(reader);
            uint length = ReadUInt32BE(reader);

            if (tag == "loca") locaOffset = offset;
            else if (tag == "glyf") glyfOffset = offset;
            else if (tag == "maxp") maxpOffset = offset;
            else if (tag == "head") headOffset = offset;
            else if (tag == "cmap") cmapOffset = offset;
            else if (tag == "post") postOffset = offset;
        }

        if (locaOffset == 0 || glyfOffset == 0 || maxpOffset == 0 || cmapOffset == 0)
        {
            Console.WriteLine("Required tables not found.");
            return;
        }

        // Read post table (glyph names)
        if (postOffset != 0)
        {
            glyphNames = ReadPostTable(reader, postOffset);
        }

        // Read cmap
        glyphToUnicode = ReadCmapTable(reader, cmapOffset);

        // Read indexToLocFormat
        reader.BaseStream.Seek(headOffset + 50, SeekOrigin.Begin);
        indexToLocFormat = ReadUInt16BE(reader);

        // Read numGlyphs
        reader.BaseStream.Seek(maxpOffset + 4, SeekOrigin.Begin);
        ushort numGlyphs = ReadUInt16BE(reader);

        // Read loca table
        uint[] glyphOffsets = new uint[numGlyphs + 1];
        reader.BaseStream.Seek(locaOffset, SeekOrigin.Begin);

        if (indexToLocFormat == 0)
        {
            for (int i = 0; i <= numGlyphs; i++)
                glyphOffsets[i] = (uint)(ReadUInt16BE(reader) * 2);
        }
        else
        {
            for (int i = 0; i <= numGlyphs; i++)
                glyphOffsets[i] = ReadUInt32BE(reader);
        }

        // Read and list glyphs
        for (int i = 0; i < numGlyphs; i++)
        {
            uint start = glyphOffsets[i];
            uint end = glyphOffsets[i + 1];

            if (start == end)
            {
                Console.WriteLine($"Glyph {i}: EMPTY");
                continue;
            }

            if (glyphToUnicode != null && glyphToUnicode.TryGetValue(i, out var unicodeList))
            {
                unicodeList.Sort();
                Console.Write($"Glyph {i}: ");
                foreach (var code in unicodeList)
                {
                    char ch = (char)code;
                    if (char.IsControl(ch))
                        Console.Write($"U+{code:X4} ");
                    else
                        Console.Write($"'{ch}'(U+{code:X4}) ");
                }
                Console.WriteLine();
            }
            else if (glyphNames != null && i < glyphNames.Length && glyphNames[i] != null)
            {
                Console.WriteLine($"Glyph {i}: Name '{glyphNames[i]}'");
            }
            else
            {
                Console.WriteLine($"Glyph {i}: (no character mapping)");
            }

            reader.BaseStream.Seek(glyfOffset + start, SeekOrigin.Begin);
            short numberOfContours = ReadInt16BE(reader);
            short xMin = ReadInt16BE(reader);
            short yMin = ReadInt16BE(reader);
            short xMax = ReadInt16BE(reader);
            short yMax = ReadInt16BE(reader);

            if (numberOfContours >= 0)
            {
                ReadSimpleGlyph(reader, numberOfContours);
            }
            else
            {
                Console.WriteLine("  Composite glyph (not handled yet)");
            }
        }
    }

    private void ReadSimpleGlyph(BinaryReader reader, int numberOfContours)
    {
        ushort[] endPtsOfContours = new ushort[numberOfContours];
        for (int i = 0; i < numberOfContours; i++)
            endPtsOfContours[i] = ReadUInt16BE(reader);

        ushort instructionLength = ReadUInt16BE(reader);
        reader.BaseStream.Seek(instructionLength, SeekOrigin.Current); // skip instructions

        int numPoints = endPtsOfContours[^1] + 1;

        List<byte> flags = new List<byte>();
        while (flags.Count < numPoints)
        {
            byte flag = reader.ReadByte();
            flags.Add(flag);

            if ((flag & 0x08) != 0)
            {
                byte repeatCount = reader.ReadByte();
                for (int i = 0; i < repeatCount; i++)
                    flags.Add(flag);
            }
        }

        int[] xCoordinates = new int[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            if ((flags[i] & 0x02) != 0)
            {
                byte dx = reader.ReadByte();
                xCoordinates[i] = (flags[i] & 0x10) != 0 ? dx : -dx;
            }
            else
            {
                if ((flags[i] & 0x10) != 0)
                    xCoordinates[i] = 0;
                else
                    xCoordinates[i] = ReadInt16BE(reader);
            }
        }

        int[] yCoordinates = new int[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            if ((flags[i] & 0x04) != 0)
            {
                byte dy = reader.ReadByte();
                yCoordinates[i] = (flags[i] & 0x20) != 0 ? dy : -dy;
            }
            else
            {
                if ((flags[i] & 0x20) != 0)
                    yCoordinates[i] = 0;
                else
                    yCoordinates[i] = ReadInt16BE(reader);
            }
        }

        // Convert to absolute positions
        for (int i = 1; i < numPoints; i++)
        {
            xCoordinates[i] += xCoordinates[i - 1];
            yCoordinates[i] += yCoordinates[i - 1];
        }

        int pointIndex = 0;
        for (int c = 0; c < numberOfContours; c++)
        {
            int contourEnd = endPtsOfContours[c];
            StringBuilder path = new StringBuilder();
            bool started = false;

            while (pointIndex <= contourEnd)
            {
                int x = xCoordinates[pointIndex];
                int y = yCoordinates[pointIndex];
                bool onCurve = (flags[pointIndex] & 0x01) != 0;

                if (!started)
                {
                    path.Append($"M {x} {y} ");
                    started = true;
                    pointIndex++;
                    continue;
                }

                if (onCurve)
                {
                    path.Append($"L {x} {y} ");
                    pointIndex++;
                }
                else
                {
                    int cx = x;
                    int cy = y;
                    int nx, ny;
                    bool nextOnCurve = false;

                    if (pointIndex + 1 <= contourEnd)
                    {
                        nx = xCoordinates[pointIndex + 1];
                        ny = yCoordinates[pointIndex + 1];
                        nextOnCurve = (flags[pointIndex + 1] & 0x01) != 0;
                    }
                    else
                    {
                        nx = xCoordinates[0];
                        ny = yCoordinates[0];
                        nextOnCurve = (flags[0] & 0x01) != 0;
                    }

                    if (nextOnCurve)
                    {
                        path.Append($"Q {cx} {cy} {nx} {ny} ");
                        pointIndex += 2;
                    }
                    else
                    {
                        int mx = (cx + nx) / 2;
                        int my = (cy + ny) / 2;
                        path.Append($"Q {cx} {cy} {mx} {my} ");
                        pointIndex++;
                    }
                }
            }

            path.Append("Z");
            Console.WriteLine("  Path: " + path.ToString());
        }
    }

    private Dictionary<int, List<int>> ReadCmapTable(BinaryReader reader, uint cmapOffset)
    {
        var glyphToUnicode = new Dictionary<int, List<int>>();

        reader.BaseStream.Seek(cmapOffset, SeekOrigin.Begin);

        ushort version = ReadUInt16BE(reader);
        ushort numTables = ReadUInt16BE(reader);

        uint bestSubtableOffset = 0;
        bool isFormat12 = false;
        ushort bestPlatformID = 0;
        ushort bestEncodingID = 0;

        Console.WriteLine("Available cmap subtables:");
        List<(ushort platformID, ushort encodingID, ushort format, uint subtableOffset)> candidates = new();

        for (int i = 0; i < numTables; i++)
        {
            ushort platformID = ReadUInt16BE(reader);
            ushort encodingID = ReadUInt16BE(reader);
            uint subtableOffset = ReadUInt32BE(reader);

            long currentPos = reader.BaseStream.Position;
            reader.BaseStream.Seek(cmapOffset + subtableOffset, SeekOrigin.Begin);
            ushort format = ReadUInt16BE(reader);

            Console.WriteLine($"  Platform {platformID}, Encoding {encodingID}, Format {format}");

            candidates.Add((platformID, encodingID, format, cmapOffset + subtableOffset));

            reader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
        }

        // Prefer Platform 3, Encoding 1 first (Microsoft Unicode BMP)
        foreach (var c in candidates)
        {
            if (c.platformID == 3 && c.encodingID == 1 && (c.format == 4 || c.format == 12))
            {
                bestSubtableOffset = c.subtableOffset;
                isFormat12 = (c.format == 12);
                bestPlatformID = c.platformID;
                bestEncodingID = c.encodingID;
                break;
            }
        }

        if (bestSubtableOffset == 0)
        {
            foreach (var c in candidates)
            {
                if (c.platformID == 0 && (c.encodingID == 4 || c.encodingID == 3) && (c.format == 4 || c.format == 12))
                {
                    bestSubtableOffset = c.subtableOffset;
                    isFormat12 = (c.format == 12);
                    bestPlatformID = c.platformID;
                    bestEncodingID = c.encodingID;
                    break;
                }
            }
        }

        if (bestSubtableOffset == 0)
        {
            Console.WriteLine("⚠️ No usable Unicode cmap subtable found!");
            return glyphToUnicode;
        }

        Console.WriteLine($"Selected cmap subtable: Platform {bestPlatformID}, Encoding {bestEncodingID}, Format {(isFormat12 ? 12 : 4)}");

        reader.BaseStream.Seek(bestSubtableOffset, SeekOrigin.Begin);

        if (isFormat12)
        {
            reader.BaseStream.Seek(2 + 2 + 4 + 4, SeekOrigin.Current);
            uint nGroups = ReadUInt32BE(reader);

            for (uint i = 0; i < nGroups; i++)
            {
                uint startCharCode = ReadUInt32BE(reader);
                uint endCharCode = ReadUInt32BE(reader);
                uint startGlyphID = ReadUInt32BE(reader);

                for (uint code = startCharCode; code <= endCharCode; code++)
                {
                    int glyphId = (int)(startGlyphID + (code - startCharCode));
                    if (!glyphToUnicode.TryGetValue(glyphId, out var list))
                        glyphToUnicode[glyphId] = list = new List<int>();
                    list.Add((int)code);
                }
            }
        }
        else
        {
            ushort length = ReadUInt16BE(reader);
            ushort language = ReadUInt16BE(reader);
            ushort segCountX2 = ReadUInt16BE(reader);
            ushort segCount = (ushort)(segCountX2 / 2);

            reader.BaseStream.Seek(6, SeekOrigin.Current);

            ushort[] endCodes = new ushort[segCount];
            for (int i = 0; i < segCount; i++) endCodes[i] = ReadUInt16BE(reader);

            reader.BaseStream.Seek(2, SeekOrigin.Current);

            ushort[] startCodes = new ushort[segCount];
            for (int i = 0; i < segCount; i++) startCodes[i] = ReadUInt16BE(reader);

            short[] idDeltas = new short[segCount];
            for (int i = 0; i < segCount; i++) idDeltas[i] = ReadInt16BE(reader);

            ushort[] idRangeOffsets = new ushort[segCount];
            long idRangeOffsetStart = reader.BaseStream.Position;
            for (int i = 0; i < segCount; i++) idRangeOffsets[i] = ReadUInt16BE(reader);

            for (int i = 0; i < segCount; i++)
            {
                ushort start = startCodes[i];
                ushort end = endCodes[i];
                short delta = idDeltas[i];
                ushort rangeOffset = idRangeOffsets[i];

                for (int c = start; c <= end; c++)
                {
                    int glyphId;
                    if (rangeOffset == 0)
                    {
                        glyphId = (c + delta) & 0xFFFF;
                    }
                    else
                    {
                        long pos = idRangeOffsetStart + (i * 2) + rangeOffset + (c - start) * 2;
                        long savePos = reader.BaseStream.Position;
                        reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                        ushort glyphIndex = ReadUInt16BE(reader);
                        reader.BaseStream.Seek(savePos, SeekOrigin.Begin);

                        if (glyphIndex == 0)
                            continue;

                        glyphId = (glyphIndex + delta) & 0xFFFF;
                    }

                    if (!glyphToUnicode.TryGetValue(glyphId, out var list))
                        glyphToUnicode[glyphId] = list = new List<int>();
                    list.Add(c);
                }
            }
        }

        return glyphToUnicode;
    }

    private ushort ReadUInt16BE(BinaryReader reader)
    {
        var data = reader.ReadBytes(2);
        return (ushort)((data[0] << 8) | data[1]);
    }

    private short ReadInt16BE(BinaryReader reader)
    {
        var data = reader.ReadBytes(2);
        return (short)((data[0] << 8) | data[1]);
    }

    private uint ReadUInt32BE(BinaryReader reader)
    {
        var data = reader.ReadBytes(4);
        return (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
    }

    private string[] ReadPostTable(BinaryReader reader, uint postOffset)
    {
        reader.BaseStream.Seek(postOffset, SeekOrigin.Begin);

        uint version = ReadUInt32BE(reader);
        reader.BaseStream.Seek(28, SeekOrigin.Current); // skip italicAngle etc.

        if (version == 0x00010000)
        {
            Console.WriteLine("post table version 1.0: Standard Mac glyph names (not useful).");
            return null;
        }
        else if (version == 0x00020000)
        {
            ushort numGlyphs = ReadUInt16BE(reader);
            ushort[] nameIndices = new ushort[numGlyphs];

            for (int i = 0; i < numGlyphs; i++)
                nameIndices[i] = ReadUInt16BE(reader);

            List<string> customNames = new();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte len = reader.ReadByte();
                if (len == 0)
                    continue;
                var str = Encoding.ASCII.GetString(reader.ReadBytes(len));
                customNames.Add(str);
            }

            string[] glyphNames = new string[numGlyphs];
            for (int i = 0; i < numGlyphs; i++)
            {
                if (nameIndices[i] < 258)
                {
                    glyphNames[i] = StandardMacGlyphNames(nameIndices[i]);
                }
                else
                {
                    int customIndex = nameIndices[i] - 258;
                    if (customIndex >= 0 && customIndex < customNames.Count)
                        glyphNames[i] = customNames[customIndex];
                }
            }

            return glyphNames;
        }
        else if (version == 0x00030000)
        {
            Console.WriteLine("post table version 3.0: No glyph names.");
            return null;
        }
        else
        {
            Console.WriteLine($"Unknown post table version 0x{version:X8}");
            return null;
        }
    }

    private static readonly string[] MacGlyphNames = new string[]
    {
        ".notdef", "null", "CR", "space", "exclam", "quotedbl", "numbersign", "dollar",
        "percent", "ampersand", "quotesingle", "parenleft", "parenright", "asterisk",
        "plus", "comma", "hyphen", "period", "slash", "zero", "one", "two", "three",
        "four", "five", "six", "seven", "eight", "nine", "colon", "semicolon", "less",
        "equal", "greater", "question", "at", "A", "B", "C", "D", "E", "F", "G", "H",
        "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W",
        "X", "Y", "Z", "bracketleft", "backslash", "bracketright", "asciicircum",
        "underscore", "grave", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
        "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y",
        "z", "braceleft", "bar", "braceright", "asciitilde", "Adieresis", "Aring",
        "Ccedilla", "Eacute", "Ntilde", "Odieresis", "Udieresis", "aacute", "agrave",
        "acircumflex", "adieresis", "atilde", "aring", "ccedilla", "eacute", "egrave",
        "ecircumflex", "edieresis", "iacute", "igrave", "icircumflex", "idieresis",
        "ntilde", "oacute", "ograve", "ocircumflex", "odieresis", "otilde", "uacute",
        "ugrave", "ucircumflex", "udieresis", "dagger", "degree", "cent", "sterling",
        "section", "bullet", "paragraph", "germandbls", "registered", "copyright",
        "trademark", "acute", "dieresis", "notequal", "AE", "Oslash", "infinity",
        "plusminus", "lessequal", "greaterequal", "yen", "mu", "partialdiff",
        "summation", "product", "pi", "integral", "ordfeminine", "ordmasculine",
        "Omega", "ae", "oslash", "questiondown", "exclamdown", "logicalnot",
        "radical", "florin", "approxequal", "Delta", "guillemotleft",
        "guillemotright", "ellipsis", "nonbreakingspace", "Agrave", "Atilde",
        "Otilde", "OE", "oe", "endash", "emdash", "quotedblleft", "quotedblright",
        "quoteleft", "quoteright", "divide", "lozenge", "ydieresis", "Ydieresis",
        "fraction", "currency", "guilsinglleft", "guilsinglright", "fi", "fl",
        "daggerdbl", "periodcentered", "quotesinglbase", "quotedblbase",
        "perthousand", "Acircumflex", "Ecircumflex", "Aacute", "Edieresis",
        "Egrave", "Iacute", "Icircumflex", "Idieresis", "Igrave", "Oacute",
        "Ocircumflex", "apple", "Ograve", "Uacute", "Ucircumflex", "Ugrave",
        "dotlessi", "circumflex", "tilde", "macron", "breve", "dotaccent", "ring",
        "cedilla", "hungarumlaut", "ogonek", "caron"
    };

    private string StandardMacGlyphNames(ushort index)
    {
        if (index < MacGlyphNames.Length)
            return MacGlyphNames[index];
        return $"macGlyph{index}";
    }
}
