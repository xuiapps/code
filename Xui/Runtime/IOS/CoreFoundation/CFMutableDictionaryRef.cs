using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation.CFDictionary;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public ref partial struct CFMutableDictionaryRef
    {
        [LibraryImport(CoreFoundationLib)]
        private static partial nint CFDictionaryCreateMutable(nint allocator, long capacity, nint keyCallBacks, nint valueCallBacks);

        [LibraryImport(CoreFoundationLib)]
        private static partial void CFDictionarySetValue(nint cfMutableDictionaryRef, nint key, nint value);

        public static readonly nint KeyCallBacks = NativeLibrary.GetExport(Lib, "kCFTypeDictionaryKeyCallBacks");

        public static readonly nint ValueCallBacks = NativeLibrary.GetExport(Lib, "kCFTypeDictionaryValueCallBacks");

        public readonly nint Self;

        public CFMutableDictionaryRef()
        {
            this.Self = CFDictionaryCreateMutable(0, 0, KeyCallBacks, ValueCallBacks);

            if (this.Self == 0)
            {
                throw new ObjCException($"Failed to create {nameof(CFMutableDictionaryRef)}.");
            }
        }

        public void SetValue(nint key, nint value) => CFDictionarySetValue(this.Self, key, value);

        public void Dispose() => CFRelease(this.Self);


        public static implicit operator nint(CFMutableDictionaryRef arr) => arr.Self;
    }
}