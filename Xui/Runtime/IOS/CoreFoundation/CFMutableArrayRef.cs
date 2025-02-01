using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreFoundation.CFArray;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public ref partial struct CFMutableArrayRef
    {
        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFArrayCreateMutable(nint allocator, nint capacity, nint callbacks);

        [LibraryImport(CoreFoundationLib)]
        public static partial void CFArrayAppendValue(nint cfMutableArrayRef, nint value);

        public delegate void ReleaseCallBack(nint allocator, nint value);

        public delegate void RetainCallBack(nint allocator, nint value);

        private static void Retain(nint allocatorPtr, nint valuePtr) => CFRetain(valuePtr);

        private static void Release(nint allocatorPtr, nint valuePtr) => CFRelease(valuePtr);

        public static readonly nint CallBacks = NativeLibrary.GetExport(Lib, "kCFTypeArrayCallBacks");

        public readonly nint Self;

        public CFMutableArrayRef()
        {
            this.Self = CFArrayCreateMutable(0, 0, CallBacks);

            if (this.Self == 0)
            {
                throw new ObjCException($"Failed to create {nameof(CFMutableArrayRef)}.");
            }
        }

        public void Add(nint value) => CFArrayAppendValue(this.Self, value);

        public void Dispose() => CFRelease(this.Self);

        public static implicit operator nint(CFMutableArrayRef arr) => arr.Self;
    }
}