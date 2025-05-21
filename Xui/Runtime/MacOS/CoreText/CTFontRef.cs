using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;

namespace Xui.Runtime.MacOS;

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