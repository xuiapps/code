using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    public ref partial struct CGImageRef : IDisposable
    {
        [LibraryImport(CoreGraphicsLib)]
        public static partial void CGImageRelease(nint image);

        [LibraryImport(CoreGraphicsLib)]
        private static partial nuint CGImageGetWidth(nint image);

        [LibraryImport(CoreGraphicsLib)]
        private static partial nuint CGImageGetHeight(nint image);

        [LibraryImport(CoreGraphicsLib)]
        private static partial nint CGImageCreateWithImageInRect(nint image, CGRect rect);

        [LibraryImport(CoreGraphicsLib)]
        private static partial nint CGImageCreate(
            nuint width,
            nuint height,
            nuint bitsPerComponent,
            nuint bitsPerPixel,
            nuint bytesPerRow,
            nint colorspace,
            CGBitmapInfo bitmapInfo,
            nint provider,
            nint decode,
            [MarshalAs(UnmanagedType.I1)] bool shouldInterpolate,
            int renderingIntent);

        public readonly nint Self;

        public CGImageRef(nint self)
        {
            if (self == 0)
                throw new ObjCException($"{nameof(CGImageRef)} instantiated with nil self.");
            this.Self = self;
        }

        public nuint Width => CGImageGetWidth(Self);
        public nuint Height => CGImageGetHeight(Self);

        /// <summary>
        /// Creates a new CGImage cropped to <paramref name="rect"/> (in image pixel coordinates).
        /// The caller owns the returned image and must dispose it.
        /// </summary>
        public static CGImageRef CreateWithImageInRect(nint image, CGRect rect)
        {
            var result = CGImageCreateWithImageInRect(image, rect);
            return new CGImageRef(result);
        }

        /// <summary>
        /// Creates a new CGImage from raw pixel data provided by a data provider.
        /// The caller owns the returned image and must dispose it.
        /// </summary>
        public static CGImageRef Create(
            nuint width,
            nuint height,
            nuint bitsPerComponent,
            nuint bitsPerPixel,
            nuint bytesPerRow,
            nint colorspace,
            CGBitmapInfo bitmapInfo,
            nint provider,
            bool shouldInterpolate = true)
        {
            var result = CGImageCreate(
                width,
                height,
                bitsPerComponent,
                bitsPerPixel,
                bytesPerRow,
                colorspace,
                bitmapInfo,
                provider,
                0, // decode array (null)
                shouldInterpolate,
                0  // rendering intent (default)
            );
            return new CGImageRef(result);
        }

        public void Dispose()
        {
            if (Self != 0)
                CGImageRelease(Self);
        }

        public static implicit operator nint(CGImageRef r) => r.Self;
    }
}
