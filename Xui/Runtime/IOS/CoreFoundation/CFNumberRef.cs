using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public ref partial struct CFNumberRef
    {
        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFNumberCreate(nint allocator, CFNumberType theType, uint valuePtr);

        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFNumberCreate(nint allocator, CFNumberType theType, ref int valuePtr);

        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFNumberCreate(nint allocator, CFNumberType theType, ref NFloat valuePtr);

        public readonly nint Self;

        public CFNumberRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CFNumberRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public CFNumberRef(NFloat value) : this(CFNumberCreate(0, CFNumberType.CGFloat, ref value))
        {
        }

        public CFNumberRef(int value) : this(CFNumberCreate(0, CFNumberType.Int, ref value))
        {
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        public static implicit operator nint(CFNumberRef cfColorRef) => cfColorRef.Self;
    }
}