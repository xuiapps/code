using System.Runtime.InteropServices;
using Xui.Core.Canvas;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public ref partial struct UIColorRef
    {
        public static readonly Class Class = new Class(UIKit.Lib, "UIColor");

        private static Sel InitWithRedGreenBlueAlpha = new Sel("initWithRed:green:blue:alpha:");

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial nint objc_msgSend_retIntPtr(nint obj, nint sel, NFloat red, NFloat green, NFloat blue, NFloat alpha);

        public readonly nint Self;

        public UIColorRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(UIColorRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public UIColorRef(Color color)
            : this(objc_msgSend_retIntPtr(Class.Alloc(), InitWithRedGreenBlueAlpha, color.Red, color.Green, color.Blue, color.Alpha))
        {
        }

        public UIColorRef(NFloat red, NFloat green, NFloat blue, NFloat alpha)
            : this(objc_msgSend_retIntPtr(Class.Alloc(), InitWithRedGreenBlueAlpha, red, green, blue, alpha))
        {
        }

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CoreFoundation.CFRelease(this.Self);
            }
        }

        public static implicit operator nint(UIColorRef nsColorRef) => nsColorRef.Self;
    }
}