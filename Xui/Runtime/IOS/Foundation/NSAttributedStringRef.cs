using System;
using static Xui.Runtime.IOS.CoreFoundation;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class Foundation
{
    public ref struct NSAttributedStringRef : IDisposable
    {
        public readonly nint Self;

        public NSAttributedStringRef(CFStringRef text, CFMutableDictionaryRef attributes)
        {
            Self = NSAttributedString.Class.Alloc();
            if (Self == 0) throw new ObjCException("Failed to alloc NSAttributedString");

            Self = ObjC.objc_msgSend_retIntPtr(Self, NSAttributedString.InitWithStringAttributesSel, text, attributes);
            if (Self == 0) throw new ObjCException("Failed to init NSAttributedString");
        }

        public void Dispose()
        {
            if (Self != 0)
                CFRelease(Self);
        }

        public static implicit operator nint(NSAttributedStringRef str) => str.Self;
    }
}
