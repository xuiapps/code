using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UITouch : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "UITouch");

        public static readonly Sel LocationInViewSel = new Sel("locationInView:");

        public static readonly Sel PreviousLocationInViewSel = new Sel("previousLocationInView:");

        public static readonly Prop.NInt PhaseProp = new Prop.NInt("phase", "setPhase:");

        public static readonly Sel MajorRadiusSel = new Sel("majorRadius");

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        public static partial CGPoint objc_msgSend_retCGPoint(nint obj, nint sel, nint view);

        public UITouch(nint id) : base(id)
        {
        }

        public UITouch() : base(Class.New())
        {
        }

        public NFloat Radius => objc_msgSend_retNFloat(this, MajorRadiusSel);

        public CGPoint LocationInView(UIView? view) => objc_msgSend_retCGPoint(this, LocationInViewSel, view == null ? 0 : view);

        public CGPoint PreviousLocationInView(UIView? view) => objc_msgSend_retCGPoint(this, PreviousLocationInViewSel, view == null ? 0 : view);

        public Phase GetPhase() => (Phase)PhaseProp.Get(this);
    }
}
