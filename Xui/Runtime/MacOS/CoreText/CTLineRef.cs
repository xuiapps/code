using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;

namespace Xui.Runtime.MacOS;

public static partial class CoreText
{
    public ref partial struct CTLineRef : IDisposable
    {
        [LibraryImport(CoreTextLib)]
        private static partial nint CTLineCreateWithAttributedString(nint attributedString);

        [LibraryImport(CoreTextLib)]
        private static partial double CTLineGetTypographicBounds(nint line, out double ascent, out double descent, out double leading);

        [LibraryImport(CoreTextLib)]
        private static partial CGRect CTLineGetBoundsWithOptions(nint line, nuint options);

        public readonly nint Self;

        public CTLineRef(nint self)
        {
            if (self == 0)
                throw new ObjCException($"{nameof(CTLineRef)} instantiated with nil self.");
            this.Self = self;
        }

        public static CTLineRef Create(nint attributedString)
        {
            return new CTLineRef(CTLineCreateWithAttributedString(attributedString));
        }

        public void Dispose()
        {
            if (Self != 0)
                CFRelease(Self);
        }

        public static implicit operator nint(CTLineRef r) => r.Self;

        public (NFloat Ascent, NFloat Descent, NFloat Leading) GetVerticalMetrics()
        {
            CTLineGetTypographicBounds(Self, out var ascent, out var descent, out var leading);
            return ((NFloat)ascent, (NFloat)descent, (NFloat)leading);
        }

        public CGRect GetBounds(BoundsOptions options = BoundsOptions.UseOpticalBounds) =>
            CTLineGetBoundsWithOptions(Self, (nuint)options);

        public NFloat GetWidth()
        {
            var width = CTLineGetTypographicBounds(Self, out _, out _, out _);
            return (NFloat)width;
        }

        /// <summary>
        /// Platform-safe flags controlling how CTLine bounds are calculated.
        /// Matches CTLineBoundsOptions from CoreText.
        /// (nuint)
        /// </summary>
        [Flags]
        public enum BoundsOptions : uint
        {
            ExcludeTypographicLeading     = 1 << 0,
            ExcludeTypographicShifts      = 1 << 1,
            UseHangingPunctuation         = 1 << 2,
            UseGlyphPathBounds            = 1 << 3,
            UseOpticalBounds              = 1 << 4,
            IncludeLanguageExtents        = 1 << 5
        }
    }
}