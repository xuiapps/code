using System;

namespace Xui.Runtime.Software.Font;

public class GlyfTable
{
    private readonly ReadOnlyMemory<byte> _data;
    private readonly LocaTable _loca;

    public GlyfTable(ReadOnlyMemory<byte> data, LocaTable loca)
    {
        _data = data;
        _loca = loca;
    }

    /// <summary>
    /// Returns a glyph shape by slicing the raw memory using loca table offsets.
    /// </summary>
    public bool TryGetGlyph(int glyphIndex, out GlyphShape shape)
    {
        var (offset, length) = _loca.GetGlyphBounds(glyphIndex);

        if (length <= 0 || offset + length > _data.Length)
        {
            shape = default;
            return false;
        }

        shape = new GlyphShape(_data.Span.Slice(offset, length));
        return true;
    }
}
