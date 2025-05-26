using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreFoundation
{
    public partial class CFDataRef : IDisposable
    {
        [LibraryImport(CoreFoundationLib)]
        private static partial IntPtr CFDataGetBytePtr(nint cfData);

        [LibraryImport(CoreFoundationLib)]
        private static partial nint CFDataGetLength(nint cfData);

        public readonly nint Self;

        public CFDataRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CFDataRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CFDataRef(string? str) : this(CFString(str))
        {
        }

        public unsafe ReadOnlySpan<byte> AsSpan()
        {
            var ptr = (byte*)CFDataGetBytePtr(Self);
            var len = (int)CFDataGetLength(Self);
            return new ReadOnlySpan<byte>(ptr, len);
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        public static implicit operator nint(CFDataRef cfDataRef) => cfDataRef.Self;
    }
}