using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    // Matches DWRITE_FONT_METRICS1 from dwrite_1.h
    // Total size: 48 bytes
    [StructLayout(LayoutKind.Explicit, Pack = 2, Size = 48)]
    public struct FontMetrics1
    {
        // ---- DWRITE_FONT_METRICS (base) ---- (20 bytes)

        [FieldOffset(0)]  public ushort DesignUnitsPerEm;          // UINT16
        [FieldOffset(2)]  public ushort Ascent;                    // UINT16
        [FieldOffset(4)]  public ushort Descent;                   // UINT16
        [FieldOffset(6)]  public short  LineGap;                   // INT16
        [FieldOffset(8)]  public ushort CapHeight;                 // UINT16
        [FieldOffset(10)] public ushort XHeight;                   // UINT16
        [FieldOffset(12)] public short  UnderlinePosition;         // INT16
        [FieldOffset(14)] public ushort UnderlineThickness;        // UINT16
        [FieldOffset(16)] public short  StrikethroughPosition;     // INT16
        [FieldOffset(18)] public ushort StrikethroughThickness;    // UINT16

        // ---- DWRITE_FONT_METRICS1 additions ---- (28 bytes)

        [FieldOffset(20)] public short GlyphBoxLeft;               // INT16
        [FieldOffset(22)] public short GlyphBoxTop;                // INT16
        [FieldOffset(24)] public short GlyphBoxRight;              // INT16
        [FieldOffset(26)] public short GlyphBoxBottom;             // INT16

        [FieldOffset(28)] public short SubscriptPositionX;         // INT16
        [FieldOffset(30)] public short SubscriptPositionY;         // INT16
        [FieldOffset(32)] public short SubscriptSizeX;             // INT16
        [FieldOffset(34)] public short SubscriptSizeY;             // INT16

        [FieldOffset(36)] public short SuperscriptPositionX;       // INT16
        [FieldOffset(38)] public short SuperscriptPositionY;       // INT16
        [FieldOffset(40)] public short SuperscriptSizeX;           // INT16
        [FieldOffset(42)] public short SuperscriptSizeY;           // INT16

        // Win32 BOOL is 4 bytes (int)
        [FieldOffset(44)] public int HasTypographicMetrics;        // BOOL
    }
}
