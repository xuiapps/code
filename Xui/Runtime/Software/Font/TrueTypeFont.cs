using System;
using System.Collections.Generic;
using System.Buffers.Binary;
using System.Text;

namespace Xui.Runtime.Software.Font;

public partial class TrueTypeFont
{
    private readonly ReadOnlyMemory<byte> _blob;
    private readonly Dictionary<string, TableRecord> _tables = new();

    public IReadOnlyDictionary<string, TableRecord> Tables => _tables;

    public HeaderTable? Head { get; private set; }
    public MaxProfileTable? Maxp { get; private set; }
    public CMapTable? Cmap { get; private set; }
    public PostTable? Post { get; private set; }
    public LocaTable? Loca { get; private set; }
    public GlyfTable? Glyf { get; private set; }
    public OS2Table? OS2 { get; private set; }
    public NameTable? Name { get; private set; }
    public HorizontalHeaderTable? Hhea { get; private set; }
    public HorizontalMetricsTable? Hmtx { get; private set; }
    public KernTable? Kern { get; private set; }
    public GPosTable? GPos { get; private set; }

    public KerningQuery Kerning => new(this);

    public TrueTypeFont(ReadOnlyMemory<byte> blob)
    {
        _blob = blob;

        var span = blob.Span;

        uint scalerType = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(0, 4));
        ushort numTables = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));
        // Skip: searchRange, entrySelector, rangeShift

        for (int i = 0; i < numTables; i++)
        {
            int offset = 12 + i * 16;
            string tag = Encoding.ASCII.GetString(span.Slice(offset, 4));
            uint checksum = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(offset + 4, 4));
            uint tableOffset = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(offset + 8, 4));
            uint length = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(offset + 12, 4));

            _tables[tag] = new TableRecord(tag, tableOffset, length, checksum);
        }

        // You can optionally parse critical tables here or defer
        if (_tables.TryGetValue("head", out var headRec))
            Head = new HeaderTable(GetTableSpan(headRec));

        if (_tables.TryGetValue("maxp", out var maxpRec))
            Maxp = new MaxProfileTable(GetTableSpan(maxpRec));

        if (_tables.TryGetValue("hhea", out var hheaRec))
            Hhea = new HorizontalHeaderTable(GetTableSpan(hheaRec));

        if (_tables.TryGetValue("OS/2", out var os2Rec))
            OS2 = new OS2Table(GetTableSpan(os2Rec));

        if (_tables.TryGetValue("post", out var postRec))
            Post = new PostTable(GetTableSpan(postRec));

        if (_tables.TryGetValue("cmap", out var cmapRec))
            Cmap = new CMapTable(GetTableSpan(cmapRec));

        if (_tables.TryGetValue("name", out var nameRec))
            Name = new NameTable(GetTableSpan(nameRec));

        if (_tables.TryGetValue("loca", out var locaRec) && Maxp is not null && Head is not null)
            Loca = new LocaTable(GetTableSpan(locaRec), Maxp.NumGlyphs, Head.IndexToLocFormat);

        if (_tables.TryGetValue("glyf", out var glyfRec) && Loca is not null)
            Glyf = new GlyfTable(GetTableMemory(glyfRec), Loca);

        if (_tables.TryGetValue("hmtx", out var hmtxRec) && Hhea is not null)
            Hmtx = new HorizontalMetricsTable(GetTableSpan(hmtxRec), Hhea.NumberOfHMetrics);

        if (_tables.TryGetValue("kern", out var kernRec) && Cmap is not null)
            Kern = new KernTable(GetTableSpan(kernRec), Cmap);

        if (_tables.TryGetValue("GPOS", out var gPosRec) && Cmap is not null)
            GPos = new GPosTable(GetTableSpan(gPosRec), Cmap);
    }

    private ReadOnlySpan<byte> GetTableSpan(TableRecord record)
    {
        return _blob.Span.Slice((int)record.Offset, (int)record.Length);
    }

    private ReadOnlyMemory<byte> GetTableMemory(TableRecord record)
    {
        return _blob.Slice((int)record.Offset, (int)record.Length);
    }

    public bool TryGetGlyph(int glyphId, out GlyphShape shape)
    {
        if (Glyf != null)
            return Glyf.TryGetGlyph(glyphId, out shape);

        shape = default;
        return false;
    }

    public int? GetGlyphIndexFromChar(int unicode) => Cmap?.GetGlyphIndex(unicode);

    public string? GetGlyphName(int glyphId) => Post?.GetGlyphName(glyphId);

    public readonly record struct TableRecord(string Tag, uint Offset, uint Length, uint Checksum);

    public readonly struct KerningQuery
    {
        private readonly TrueTypeFont _font;

        public KerningQuery(TrueTypeFont font) => _font = font;

        public ValueRecord this[char left, char right]
        {
            get
            {
                if (_font.GPos is { } gpos)
                    return gpos[left, right];

                if (_font.Kern is { } kern)
                    return kern[left, right];

                return default;
            }
        }
    }
}
