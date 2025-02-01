using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public ref partial struct CFSetRef : IDisposable
    {
        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFSetGetCount(nint cfSetRef);

        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFSetGetValues(CFSetRef theSet, ref nint arrayOfValuePtrs);

        public readonly nint Self;

        public CFSetRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(CFSetRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public nint Count => CFSetGetCount(this);

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CFRelease(this.Self);
            }
        }

        /// <summary>
        /// https://developer.apple.com/documentation/corefoundation/1520437-cfsetgetvalues?language=objc
        /// Follows the creation rule, so you need to release the objects.
        /// </summary>
        public void GetValues(ref nint nintArrPtr) => CFSetGetValues(this, ref nintArrPtr);

        public static implicit operator nint(CFSetRef cfSetRef) => cfSetRef.Self;
    }
}
