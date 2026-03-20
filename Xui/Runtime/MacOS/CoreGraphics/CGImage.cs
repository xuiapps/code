using System;
using static Xui.Runtime.MacOS.CoreFoundation;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// Long-lived CGImage wrapper for images that persist across multiple frames.
    /// Wraps a native CGImageRef pointer and handles CoreFoundation reference counting.
    /// This is the class equivalent to D2D1.Bitmap1 in the Windows runtime.
    /// </summary>
    public class CGImage : IDisposable
    {
        public readonly nint Ptr;

        public CGImage(nint ptr)
        {
            if (ptr == 0)
                throw new ObjCException($"{nameof(CGImage)} instantiated with nil pointer.");
            this.Ptr = ptr;
        }

        public uint Width => (uint)CGImageRef.CGImageGetWidth(Ptr);
        public uint Height => (uint)CGImageRef.CGImageGetHeight(Ptr);

        public static implicit operator nint(CGImage? image) => image?.Ptr ?? 0;

        public void Dispose()
        {
            if (Ptr != 0)
            {
                CGImageRef.CGImageRelease(Ptr);
            }
            GC.SuppressFinalize(this);
        }

        ~CGImage()
        {
            if (Ptr != 0)
            {
                CGImageRef.CGImageRelease(Ptr);
            }
        }
    }
}
