using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;

namespace Xui.Runtime.MacOS;

public static partial class ImageIO
{
    public ref partial struct CGImageSourceRef : IDisposable
    {
        [LibraryImport(ImageIOLib)]
        private static partial nint CGImageSourceCreateWithURL(nint url, nint options);

        [LibraryImport(ImageIOLib)]
        private static partial nint CGImageSourceCreateImageAtIndex(nint source, nuint index, nint options);

        public readonly nint Self;

        public CGImageSourceRef(nint self)
        {
            if (self == 0)
                throw new ObjCException($"{nameof(CGImageSourceRef)} instantiated with nil self.");
            this.Self = self;
        }

        /// <summary>
        /// Creates a CGImageSource from a CFURL. The source is retained; caller must dispose.
        /// </summary>
        public static CGImageSourceRef CreateWithURL(nint cfUrlRef)
        {
            var result = CGImageSourceCreateWithURL(cfUrlRef, 0);
            return new CGImageSourceRef(result);
        }

        /// <summary>
        /// Decodes the image at <paramref name="index"/> (0 for the primary frame).
        /// Returns a new CGImageRef that the caller owns and must dispose.
        /// </summary>
        public CGImageRef CreateImageAtIndex(nuint index = 0)
        {
            var result = CGImageSourceCreateImageAtIndex(Self, index, 0);
            return new CGImageRef(result);
        }

        public void Dispose()
        {
            if (Self != 0)
                CFRelease(Self);
        }

        public static implicit operator nint(CGImageSourceRef r) => r.Self;
    }
}
