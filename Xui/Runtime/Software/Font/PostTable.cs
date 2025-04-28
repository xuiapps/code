using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace Xui.Runtime.Software.Font;

public class PostTable
{
    private static readonly string[] MacGlyphNames = new string[]
    {
        ".notdef", "null", "CR", "space", "exclam", "quotedbl", "numbersign", "dollar",
        "percent", "ampersand", "quotesingle", "parenleft", "parenright", "asterisk",
        "plus", "comma", "hyphen", "period", "slash", "zero", "one", "two", "three",
        "four", "five", "six", "seven", "eight", "nine", "colon", "semicolon", "less",
        "equal", "greater", "question", "at", "A", "B", "C", "D", "E", "F", "G", "H",
        "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W",
        "X", "Y", "Z", "bracketleft", "backslash", "bracketright", "asciicircum",
        "underscore", "grave", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k",
        "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "braceleft", "bar", "braceright", "asciitilde", "Adieresis", "Aring",
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
        "cedilla", "hungarumlaut", "ogonek", "caron",
        "Lslash", "lslash", "Scaron", "scaron", "Zcaron", "zcaron", "brokenbar",
        "Eth", "eth", "Yacute", "yacute", "Thorn", "thorn", "minus", "multiply",
        "onesuperior", "twosuperior", "threesuperior", "onehalf", "onequarter",
        "threequarters", "franc", "Gbreve", "gbreve", "Idotaccent", "Scedilla",
        "scedilla", "Cacute", "cacute", "Ccaron", "ccaron", "dcroat"
    };

    private string[]? _glyphNames;

    public PostTable(ReadOnlySpan<byte> data)
    {
        uint version = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(0, 4));
        if (version != 0x00020000) return;

        ushort numGlyphs = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(32, 2));
        var nameIndices = new ushort[numGlyphs];

        int offset = 34;
        for (int i = 0; i < numGlyphs; i++)
        {
            nameIndices[i] = BinaryPrimitives.ReadUInt16BigEndian(data.Slice(offset, 2));
            offset += 2;
        }

        var customNames = new List<string>();
        while (offset < data.Length)
        {
            byte len = data[offset++];
            if (len == 0) continue;
            if (offset + len > data.Length) break;
            customNames.Add(Encoding.ASCII.GetString(data.Slice(offset, len)));
            offset += len;
        }

        _glyphNames = new string[numGlyphs];
        for (int i = 0; i < numGlyphs; i++)
        {
            ushort index = nameIndices[i];
            if (index < 258)
                _glyphNames[i] = MacGlyphNames[index];
            else
            {
                int customIndex = index - 258;
                if (customIndex >= 0 && customIndex < customNames.Count)
                    _glyphNames[i] = customNames[customIndex];
            }
        }
    }

    public string? GetGlyphName(int glyphIndex)
    {
        if (_glyphNames != null && glyphIndex >= 0 && glyphIndex < _glyphNames.Length)
            return _glyphNames[glyphIndex];
        return null;
    }
}
