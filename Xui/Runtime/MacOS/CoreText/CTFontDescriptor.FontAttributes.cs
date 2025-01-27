using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.Foundation;

namespace Xui.Runtime.MacOS;

public static partial class CoreText
{
    public partial class CTFontDescriptor
    {
        public class FontAttributes : NSString
        {
            public static readonly FontAttributes URL = GetKeyConstant("kCTFontURLAttribute");
            public static readonly FontAttributes Name = GetKeyConstant("kCTFontNameAttribute");
            public static readonly FontAttributes DisplayName = GetKeyConstant("kCTFontDisplayNameAttribute");
            public static readonly FontAttributes FamilyName = GetKeyConstant("kCTFontFamilyNameAttribute");
            public static readonly FontAttributes StyleName = GetKeyConstant("kCTFontStyleNameAttribute");
            public static readonly FontAttributes Traits = GetKeyConstant("kCTFontTraitsAttribute");
            public static readonly FontAttributes Variant = GetKeyConstant("kCTFontVariationAttribute");
            public static readonly FontAttributes Size = GetKeyConstant("kCTFontSizeAttribute");
            public static readonly FontAttributes Matrix = GetKeyConstant("kCTFontMatrixAttribute");
            public static readonly FontAttributes CascadeList = GetKeyConstant("kCTFontCascadeListAttribute");
            public static readonly FontAttributes CharacterSet = GetKeyConstant("kCTFontCharacterSetAttribute");
            public static readonly FontAttributes Languages = GetKeyConstant("kCTFontLanguagesAttribute");
            public static readonly FontAttributes BaselineAdjust = GetKeyConstant("kCTFontBaselineAdjustAttribute");
            public static readonly FontAttributes MacintoshEncodings = GetKeyConstant("kCTFontMacintoshEncodingsAttribute");
            public static readonly FontAttributes Features = GetKeyConstant("kCTFontFeaturesAttribute");
            public static readonly FontAttributes FeatureSettings = GetKeyConstant("kCTFontFeatureSettingsAttribute");
            public static readonly FontAttributes FixedAdvance = GetKeyConstant("kCTFontFixedAdvanceAttribute");
            public static readonly FontAttributes FontOrientation = GetKeyConstant("kCTFontOrientationAttribute");
            public static readonly FontAttributes FontFormat = GetKeyConstant("kCTFontFormatAttribute");
            public static readonly FontAttributes RegistrationScope = GetKeyConstant("kCTFontRegistrationScopeAttribute");
            public static readonly FontAttributes Priority = GetKeyConstant("kCTFontPriorityAttribute");
            public static readonly FontAttributes Enabled = GetKeyConstant("kCTFontEnabledAttribute");
            public static readonly FontAttributes Downloadable = GetKeyConstant("kCTFontDownloadableAttribute");
            public static readonly FontAttributes Downloaded = GetKeyConstant("kCTFontDownloadedAttribute");

            private static FontAttributes GetKeyConstant(string name) => new FontAttributes(Marshal.ReadIntPtr(NativeLibrary.GetExport(CoreText.Lib, name)));

            private FontAttributes(nint id) : base(id)
            {
            }
        }
    }
}