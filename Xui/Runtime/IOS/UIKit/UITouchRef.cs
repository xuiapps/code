using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreGraphics;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public ref partial struct UITouchRef
    {
        public readonly nint Self;

        public UITouchRef(nint self)
        {
            if (self == 0)
            {
                throw new ObjCException($"{nameof(UITouchRef)} instantiated with nil self.");
            }

            this.Self = self;
        }

        public NFloat Radius => ObjC.objc_msgSend_retNFloat(this, UITouch.MajorRadiusSel);

        public UITouch.Phase Phase => (UITouch.Phase)UITouch.PhaseProp.Get(this);

        public CGPoint LocationInView(UIView? view) => UITouch.objc_msgSend_retCGPoint(this, UITouch.LocationInViewSel, view == null ? 0 : view);

        public CGPoint PreviousLocationInView(UIView? view) => UITouch.objc_msgSend_retCGPoint(this, UITouch.PreviousLocationInViewSel, view == null ? 0 : view);

        public void Dispose()
        {
            if (this.Self != 0)
            {
                CoreFoundation.CFRelease(this.Self);
            }
        }

        public static implicit operator nint(UITouchRef uiEventRef) => uiEventRef.Self;
    }
}