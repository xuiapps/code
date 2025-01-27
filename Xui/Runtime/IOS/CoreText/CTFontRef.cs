using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.CoreGraphics;

namespace Xui.Runtime.IOS;

public static partial class CoreText
{
    public ref partial struct CTFontRef : IDisposable
    {
        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontCreateWithFontDescriptorAndOptions(nint ctFontDescriptorRefDescriptor, NFloat size, ref CGAffineTransform matrix, nint ctFontOptions);

        [LibraryImport(CoreTextLib)]
        public static partial nint CTFontCreateWithFontDescriptorAndOptions(nint ctFontDescriptorRefDescriptor, NFloat size, nint matrixPtr, nint ctFontOptions);

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