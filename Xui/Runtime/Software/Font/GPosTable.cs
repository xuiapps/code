using System;
using System.Buffers.Binary;

namespace Xui.Runtime.Software.Font;

/// <summary>
/// Parses the 'GPOS' (Glyph Positioning) table from an OpenType font.
/// This table handles kerning, mark positioning, and other glyph adjustments.
/// </summary>
public sealed partial class GPosTable
{
    public ushort MajorVersion { get; }
    public ushort MinorVersion { get; }
    public ushort ScriptListOffset { get; }
    public ushort FeatureListOffset { get; }
    public ushort LookupListOffset { get; }
    public uint? FeatureVariationsOffset { get; }

    public ScriptListTable? ScriptList { get; }
    public FeatureListTable? FeatureList { get; }
    public LookupListTable? LookupList { get; }

    public CMapTable CMap { get; }

    public GPosTable(ReadOnlySpan<byte> span, CMapTable cMap)
    {
        this.CMap = cMap;

        // Table layout (Offset: Size)
        //
        // GPOS Header:
        //   0:  uint16 majorVersion
        //   2:  uint16 minorVersion
        //   4:  Offset16 scriptListOffset
        //   6:  Offset16 featureListOffset
        //   8:  Offset16 lookupListOffset
        //
        // If version == 1.1 or 1.2:
        //  10:  Offset32 featureVariationsOffset (optional, usually 0)
        //
        // ScriptList:
        //   -> List of scripts (Latin, Arabic, etc), each with language systems
        //
        // FeatureList:
        //   -> List of features (e.g. 'kern', 'mark', 'liga')
        //
        // LookupList:
        //   -> List of LookupTables
        //      Each LookupTable has:
        //          - uint16 lookupType
        //          - uint16 lookupFlag
        //          - uint16 subTableCount
        //          - Offset16[] subTableOffsets
        //
        // LookupType 2 = Pair Adjustment (kerning)
        //
        // Pair Adjustment Subtables:
        // Format 1:
        //   - Coverage Table (which glyphs are affected)
        //   - PairSetCount
        //   - PairSetOffsets[]
        //   - Each PairSet has:
        //       - PairValueCount
        //       - [secondGlyphId, value1, value2, ...]
        //
        // Format 2:
        //   - Class-based pair adjustment (less common, more compact)

        MajorVersion = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2));
        MinorVersion = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(2, 2));
        ScriptListOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(4, 2));
        FeatureListOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(6, 2));
        LookupListOffset = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(8, 2));

        if (MajorVersion == 1 && MinorVersion >= 1 && span.Length >= 12)
            FeatureVariationsOffset = BinaryPrimitives.ReadUInt32BigEndian(span.Slice(10, 4));

        if (ScriptListOffset != 0 && span.Length > ScriptListOffset)
            ScriptList = new ScriptListTable(span.Slice(ScriptListOffset));

        if (FeatureListOffset != 0 && span.Length > FeatureListOffset)
            FeatureList = new FeatureListTable(span.Slice(FeatureListOffset));

        if (LookupListOffset != 0 && span.Length > LookupListOffset)
            LookupList = new LookupListTable(span.Slice(LookupListOffset));
    }

    /// <summary>
    /// Gets the kerning adjustment between two characters using GPOS LookupType 2.
    /// Returns a default ValueRecord if no adjustment is defined.
    /// </summary>
    public TrueTypeFont.ValueRecord this[char left, char right]
    {
        get
        {
            int? g1 = CMap.GetGlyphIndex(left);
            int? g2 = CMap.GetGlyphIndex(right);
            if (g1 is not int leftGlyph || g2 is not int rightGlyph || LookupList is null)
                return default;
            
            // PairAdjustmentSubtables work with ushort indices.
            if (g1 > ushort.MaxValue || g2 > ushort.MaxValue)
                return default;

            foreach (var lookup in LookupList)
            {
                if (lookup.LookupType != LookupTable.GPosLookupType.PairAdjustment || lookup.PairAdjustmentSubtables == null)
                    continue;

                foreach (var sub in lookup.PairAdjustmentSubtables)
                {
                    var (leftAdj, rightAdj) = sub[(ushort)leftGlyph, (ushort)rightGlyph];
                    if (leftAdj.XAdvance is not null || leftAdj.XPlacement is not null ||
                        rightAdj.XAdvance is not null || rightAdj.XPlacement is not null)
                    {
                        return leftAdj;
                    }
                }
            }

            return default;
        }
    }
}
