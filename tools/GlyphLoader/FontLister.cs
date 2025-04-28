using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using Xui.Core.Math2D;
using Xui.Runtime.Software.Font;

namespace GlyphLoader;

public static class FontLister
{
    public static void ListFont(TrueTypeFont font)
    {
        Console.WriteLine("Font listing:\n");

        // Table info
        if (font.Tables.Count > 0)
        {
            Console.WriteLine("Tables:");
            foreach (var table in font.Tables.Values)
            {
                Console.WriteLine($"  {table.Tag,-4}  Offset: {table.Offset,8}  Length: {table.Length,8}  Checksum: 0x{table.Checksum:X8}");
            }
        }
        else
        {
            Console.WriteLine("⚠ No tables found.");
        }

        Console.WriteLine();

        // Header
        if (font.Head != null)
        {
            var head = font.Head;
            Console.WriteLine("Head Table:");
            Console.WriteLine($"  Font Revision: {head.FontRevisionMajor}.{head.FontRevisionMinor:D3}");
            Console.WriteLine($"  UnitsPerEm: {head.UnitsPerEm}");
            Console.WriteLine($"  Created: {head.Created:u}");
            Console.WriteLine($"  Modified: {head.Modified:u}");
            Console.WriteLine($"  xMin: {head.XMin}, yMin: {head.YMin}, xMax: {head.XMax}, yMax: {head.YMax}");
            Console.WriteLine($"  MacStyle: 0x{head.MacStyle:X4}");
            Console.WriteLine($"    Bold: {head.IsBold}, Italic: {head.IsItalic}, Underline: {head.IsUnderline}, Outline: {head.IsOutline}");
            Console.WriteLine($"    Shadow: {head.IsShadow}, Condensed: {head.IsCondensed}, Extended: {head.IsExtended}");
            Console.WriteLine($"  LowestRecPPEM: {head.LowestRecPPEM}");
            Console.WriteLine($"  IndexToLocFormat: {head.IndexToLocFormat}");
        }

        Console.WriteLine();

        if (font.Maxp != null)
            Console.WriteLine($"Maxp Table:\n  Number of glyphs: {font.Maxp.NumGlyphs}\n");
        else
            Console.WriteLine("⚠ No maxp table found.\n");

        if (font.Cmap != null) Console.WriteLine("Cmap table loaded.");
        if (font.Post != null) Console.WriteLine("Post table loaded.");
        Console.WriteLine();

        if (font.Glyf != null && font.Maxp != null)
        {
            Console.WriteLine("Glyphs:");

            for (int glyphId = 0; glyphId < font.Maxp.NumGlyphs; glyphId++)
            {
                if (!font.TryGetGlyph(glyphId, out var shape))
                {
                    Console.WriteLine($"Glyph {glyphId}: EMPTY");
                    continue;
                }

                string name = font.GetGlyphName(glyphId) ?? "(unnamed)";
                Console.WriteLine($"Glyph {glyphId}: Name '{name}'");

                var builder = new SvgPathBuilder();
                shape.Visit(builder);

                if (builder.HasCommands)
                    Console.WriteLine($"  <path d=\"{builder.Path}\" />");
                else
                    Console.WriteLine("  No path data.");
            }
        }
        else
        {
            Console.WriteLine("⚠ No glyf table loaded.");
        }
    }
}
