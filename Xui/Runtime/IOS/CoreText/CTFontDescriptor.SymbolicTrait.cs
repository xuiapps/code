using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.Foundation;

namespace Xui.Runtime.IOS;

public static partial class CoreText
{
    public partial class CTFontDescriptor
    {
        public enum SymbolicTrait : uint
        {
            Italic = (1 << 0),
            Bold = (1 << 1),
            Expanded = (1 << 5),
            Condensed = (1 << 6),
            MonoSpace = (1 << 10),
            Vertical = (1 << 11),
            UIOptimized = (1 << 12),
            ColorGlyphs = (1 << 13),
            Composite = (1 << 14),

            // ClassMask = (15U << kCTFontClassMaskShift)
        }
    }
}