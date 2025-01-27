using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public partial class NSAttributedString
    {
        public class Key : NSString
        {
            // Rendering attribute keys

            public static readonly Key BackgroundColor = GetKeyConstant("NSBackgroundColorAttributeName");

            public static readonly Key BaselineOffset = GetKeyConstant("NSBaselineOffsetAttributeName");

            public static readonly Key Font = GetKeyConstant("NSFontAttributeName");
            
            public static readonly Key ForegroundColor = GetKeyConstant("NSForegroundColorAttributeName");

            public static readonly Key GlyphInfo = GetKeyConstant("NSGlyphInfoAttributeName");

            public static readonly Key Kern = GetKeyConstant("NSKernAttributeName");

            public static readonly Key Ligature = GetKeyConstant("NSLigatureAttributeName");

            public static readonly Key ParagraphStyle = GetKeyConstant("NSParagraphStyleAttributeName");

            public static readonly Key StrikethroughColor = GetKeyConstant("NSStrikethroughColorAttributeName");

            public static readonly Key StrikethroughStyle = GetKeyConstant("NSStrikethroughStyleAttributeName");

            public static readonly Key StrokeColor = GetKeyConstant("NSStrokeColorAttributeName");

            public static readonly Key StrokeWidth = GetKeyConstant("NSStrokeWidthAttributeName");

            public static readonly Key Superscript = GetKeyConstant("NSSuperscriptAttributeName");

            public static readonly Key Tracking = GetKeyConstant("NSTrackingAttributeName");

            public static readonly Key UnderlineColor = GetKeyConstant("NSUnderlineColorAttributeName");

            public static readonly Key UnderlineStyle = GetKeyConstant("NSUnderlineStyleAttributeName");

            public static readonly Key WritingDirection = GetKeyConstant("NSWritingDirectionAttributeName");

            // Text attribute keys
            // Attachment attribute keys
            // Accessibility attribute keys
            // Markdown attribute keys
            // Translation-related attribute keys

            private static Key GetKeyConstant(string name) => new Key(Marshal.ReadIntPtr(NativeLibrary.GetExport(AppKit.Lib, name)));

            private Key(nint id) : base(id)
            {
            }
        }
    }
}