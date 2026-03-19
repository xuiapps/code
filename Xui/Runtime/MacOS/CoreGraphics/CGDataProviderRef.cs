using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    public ref partial struct CGDataProviderRef : IDisposable
    {
        [LibraryImport(CoreGraphicsLib)]
        private static partial nint CGDataProviderCreateWithData(
            nint info,
            nint data,
            nuint size,
            nint releaseCallback);

        [LibraryImport(CoreGraphicsLib)]
        private static partial void CGDataProviderRelease(nint provider);

        public readonly nint Self;

        public CGDataProviderRef(nint self)
        {
            if (self == 0)
                throw new ObjCException($"{nameof(CGDataProviderRef)} instantiated with nil self.");
            this.Self = self;
        }

        /// <summary>
        /// Creates a data provider from a raw memory buffer.
        /// The buffer must remain valid for the lifetime of the provider.
        /// Use releaseCallback to free the buffer when the provider is destroyed.
        /// </summary>
        public static CGDataProviderRef CreateWithData(nint data, nuint size, nint releaseCallback = 0)
        {
            var result = CGDataProviderCreateWithData(0, data, size, releaseCallback);
            return new CGDataProviderRef(result);
        }

        public void Dispose()
        {
            if (Self != 0)
                CGDataProviderRelease(Self);
        }

        public static implicit operator nint(CGDataProviderRef r) => r.Self;
    }
}
