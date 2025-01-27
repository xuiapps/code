using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.Foundation;

namespace Xui.Runtime.MacOS;

public static partial class CoreText
{
    public partial class CTFontDescriptor
    {
        public class FontTrait : NSString
        {
            /// <summary>
            /// CFNumberRef.
            /// </summary>
            public static readonly FontTrait Symbolic = GetKeyConstant("kCTFontSymbolicTrait");

            /// <summary>
            /// Normalized font weight. CFNumberRef, from -1 to 1, where 0 is normal.
            /// </summary>
            public static readonly FontTrait Weight = GetKeyConstant("kCTFontWeightTrait");

            /// <summary>
            /// CFNumberRef spacing, from -1.0 to 1.0 where -1 is condensed, 0 is regular.
            /// </summary>
            public static readonly FontTrait Width = GetKeyConstant("kCTFontWidthTrait");

            /// <summary>
            /// CFNumberRef spacing, from -1.0 to 1.0 where -1 is condensed, 0 is regular. (like oblique)
            /// </summary>
            public static readonly FontTrait Slant = GetKeyConstant("kCTFontSlantTrait");

            private static FontTrait GetKeyConstant(string name) => new FontTrait(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreText.Lib, name)));

            private FontTrait(nint id) : base(id)
            {
            }
        }
    }
}