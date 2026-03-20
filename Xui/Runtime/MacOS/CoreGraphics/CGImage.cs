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
        private nint ptr;

        public CGImage(nint ptr)
        {
            if (ptr == 0)
                throw new ObjCException($"{nameof(CGImage)} instantiated with nil pointer.");
            this.ptr = ptr;
        }

        public uint Width => (uint)CGImageRef.CGImageGetWidth(ptr);
        public uint Height => (uint)CGImageRef.CGImageGetHeight(ptr);

        public static implicit operator nint(CGImage? image) => image?.ptr ?? 0;

        public void Dispose()
        {
            Release();
            GC.SuppressFinalize(this);
        }

        ~CGImage()
        {
            Release();
        }

        private void Release()
        {
            if (ptr != 0)
            {
                CGImageRef.CGImageRelease(ptr);
                ptr = 0;
            }
        }
    }
}
