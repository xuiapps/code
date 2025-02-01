using System;
using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class CoreFoundation
{
    public ref partial struct CFSetRef : IDisposable
    {
        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFSetGetCount(nint cfSetRef);

        [LibraryImport(CoreFoundationLib)]
        public static partial nint CFSetGetValues(CFSetRef theSet, nint arrayOfValuePtrs);

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

        public static implicit operator nint(CFSetRef cfSetRef) => cfSetRef.Self;
    }
}
