using Xui.Runtime.Software.Font;

namespace GlyphLoader;

public static class FontDebugLogger
{
    public static void DumpGPosDebug(TrueTypeFont font)
    {
        var gpos = font.GPos;
        if (gpos is null)
        {
            Console.WriteLine("GPOS table not present.");
            return;
        }

        int? gA = font.Cmap?.GetGlyphIndex('A');
        int? gV = font.Cmap?.GetGlyphIndex('V');

        Console.WriteLine($"GlyphID['A'] = {gA}, GlyphID['V'] = {gV}");

        if (gpos.LookupList is null)
        {
            Console.WriteLine("No LookupList in GPOS.");
            return;
        }

        foreach (var lookup in gpos.LookupList)
        {
            Console.WriteLine($"LookupType = {lookup.LookupType}");

            // Dump direct PairAdjustment subtables
            if (lookup.PairAdjustmentSubtables is { } pairSubs)
            {
                foreach (var sub in pairSubs)
                {
                    DumpPairAdjustmentSubtable(sub);
                }
            }

            // Dump wrapped ExtensionPositioning subtables
            if (lookup.ExtensionPositioningSubtables is { } extSubs)
            {
                foreach (var ext in extSubs)
                {
                    Console.WriteLine("  ExtensionPositioning:");
                    Console.WriteLine($"    Wrapped LookupType = {ext.ExtensionLookupType}");

                    if (ext.ExtensionLookupType == GPosTable.LookupTable.GPosLookupType.PairAdjustment)
                    {
                        DumpPairAdjustmentSubtable(ext.WrappedPairSubtable);
                    }
                    else
                    {
                        Console.WriteLine("    (Wrapped lookup type not yet supported in debugger)");
                    }
                }
            }
        }
    }

    private static void DumpPairAdjustmentSubtable(GPosTable.PairAdjustmentSubtable sub)
    {
        switch (sub)
        {
            case GPosTable.PairAdjustmentSubtable.Format1 fmt1:
                Console.WriteLine("  Format1:");
                foreach (var kv in fmt1.Coverage)
                    Console.WriteLine($"    Coverage: glyphId = {kv.Key}, index = {kv.Value}");

                for (int i = 0; i < fmt1.PairSets.Count; i++)
                {
                    Console.WriteLine($"    PairSet[{i}]");
                    foreach (var pair in fmt1.PairSets[i].Pairs)
                    {
                        Console.WriteLine($"      secondGlyph = {pair.SecondGlyphID}, xAdvance = {pair.Value1.XAdvance}");
                    }
                }
                break;

            case GPosTable.PairAdjustmentSubtable.Format2 fmt2:
                Console.WriteLine("  Format2:");
                Console.WriteLine($"     Class1Count = {fmt2.Class1Count}, Class2Count = {fmt2.Class2Count}");
                Console.WriteLine("    Coverage:");

                foreach (var kv in fmt2.Coverage)
                    Console.WriteLine($"      glyphId = {kv.Key}, index = {kv.Value}");

                Console.WriteLine("    Matrix:");
                for (int c1 = 0; c1 < fmt2.Class1Count; c1++)
                {
                    for (int c2 = 0; c2 < fmt2.Class2Count; c2++)
                    {
                        var val = fmt2.Matrix[c1, c2];
                        if (val.Value1.XAdvance != null || val.Value1.XPlacement != null)
                            Console.WriteLine($"      [{c1},{c2}] = xAdvance: {val.Value1.XAdvance}");
                    }
                }
                break;
        }
    }
}