using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Xui.Core.Canvas;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class CoreText
{
    public ref partial struct CTFontRef : IDisposable
    {
        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontCreateWithFontDescriptorAndOptions(nint ctFontDescriptorRefDescriptor, NFloat size, ref CGAffineTransform matrix, nint ctFontOptions);

        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontCreateWithFontDescriptorAndOptions(nint ctFontDescriptorRefDescriptor, NFloat size, nint matrixPtr, nint ctFontOptions);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetAscent(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetDescent(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetLeading(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial CGRect CTFontGetBoundingBox(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial uint CTFontGetUnitsPerEm(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetCapHeight(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetXHeight(nint font);

        [LibraryImport(CoreTextLib)]
        private static partial NFloat CTFontGetSize(nint font);

        public readonly nint Self;

        public CTFontRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CTFontRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CTFontRef(nint ctFontDescriptorRefDescriptor, NFloat size, ref CGAffineTransform matrix, nint ctFontOptions)
            : this(CTFontCreateWithFontDescriptorAndOptions(ctFontDescriptorRefDescriptor, size, ref matrix, ctFontOptions))
        {
        }

        public CTFontRef(nint ctFontDescriptorRefDescriptor, nint ctFontOptions)
            : this(CTFontCreateWithFontDescriptorAndOptions(ctFontDescriptorRefDescriptor, 0, 0, ctFontOptions))
        {
        }

        public NFloat Ascent => CTFontGetAscent(Self);
        public NFloat Descent => CTFontGetDescent(Self);
        public NFloat Leading => CTFontGetLeading(Self);
        public NFloat CapHeight => CTFontGetCapHeight(Self);
        public NFloat XHeight => CTFontGetXHeight(Self);
        public uint UnitsPerEm => CTFontGetUnitsPerEm(Self);
        public NFloat PointSize => CTFontGetSize(Self);
        public CGRect BoundingBox => CTFontGetBoundingBox(Self);

        public FontMetrics FontMetrics
        {
            get
            {
                var emHeight = TryGetEmAscentDescentFromHhea(out var emAscent, out var emDescent);
                // var fontBoundingBox = TryGetFontBoundingBox(out var yMin, out var yMax);
                var baselines = TryGetBaselineFromBslnTable(out var hangingBaseline, out var ideographicBaseline);

                var scale = this.PointSize / this.UnitsPerEm;
                var ascent = this.Ascent;
                var descent = this.Descent;

                return new FontMetrics(
                    // fontAscent: fontBoundingBox ? yMax * scale : ascent,
                    // fontDescent: fontBoundingBox ? -yMin * scale : descent,
                    fontAscent: emHeight ? emAscent * scale : ascent,
                    fontDescent: emHeight ? -emDescent * scale : descent,
                    emAscent: emHeight ? emAscent * scale : ascent,
                    emDescent: emHeight ? -emDescent * scale : descent,
                    alphabeticBaseline: 0,
                    hangingBaseline: baselines ? hangingBaseline * scale : -ascent,
                    ideographicBaseline: baselines ? ideographicBaseline * scale : descent
                );
            }
        }

        [LibraryImport("/System/Library/Frameworks/CoreText.framework/CoreText")]
        private static partial nint CTFontCopyTable(nint font, uint tableTag, uint ctFontTableOptions = 0);

        public bool TryGetBaselineFromBslnTable(out short hanging, out short ideographic)
        {
            nint bslnPtr = CTFontCopyTable(this, TableTagBsln, 0);
            if (bslnPtr == 0)
            {
                hanging = 0;
                ideographic = 0;
                return false;
            }

            using var bsln = new CFDataRef(bslnPtr);
            var span = bsln.AsSpan();
            if (span.Length < 68 || BinaryPrimitives.ReadUInt16BigEndian(span.Slice(0, 2)) != 0)
            {
                hanging = 0;
                ideographic = 0;
                return false;
            }

            hanging = BinaryPrimitives.ReadInt16BigEndian(span.Slice(4 + 2 * 2, 2)); // index 2
            ideographic = BinaryPrimitives.ReadInt16BigEndian(span.Slice(4 + 1 * 2, 2)); // index 1
            return true;
        }

        public bool TryGetFontBoundingBox(out short yMin, out short yMax)
        {
            nint headPtr = CTFontCopyTable(this, TableTagHead, 0);
            if (headPtr == 0)
            {
                yMin = 0;
                yMax = 0;
                return false;
            }

            using var head = new CFDataRef(headPtr);
            var span = head.AsSpan();

            if (span.Length < 44)
            {
                yMin = 0;
                yMax = 0;
                return false;
            }

            yMin = BinaryPrimitives.ReadInt16BigEndian(span.Slice(38, 2));
            yMax = BinaryPrimitives.ReadInt16BigEndian(span.Slice(42, 2));
            return true;
        }

        public bool TryGetEmAscentDescentFromHhea(out short ascent, out short descent)
        {
            using var hhea = CopyTable(TableTagHhea);
            var span = hhea.AsSpan();

            // hhea must be at least 8 bytes to access ascender (4) and descender (6)
            if (span.Length < 8)
            {
                ascent = 0;
                descent = 0;
                return false;
            }

            ascent = BinaryPrimitives.ReadInt16BigEndian(span.Slice(4, 2));   // ascender at offset 4
            descent = BinaryPrimitives.ReadInt16BigEndian(span.Slice(6, 2));  // descender at offset 6
            return true;
        }

        private CFDataRef CopyTable(uint table) => new CFDataRef(CTFontCopyTable(this, table, 0));

        private const uint TableTagBsln = 0x62736C6E;
        private const uint TableTagHead = 0x68656164;
        private const uint TableTagCmap = 0x636D6170;
        private const uint TableTagHhea = 0x68686561;

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        public static implicit operator nint(CTFontRef ctFontRef) => ctFontRef.Self;
    }
}