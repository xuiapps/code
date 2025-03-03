using static Xui.Runtime.IOS.ObjC;
using Xui.Core.Math2D;
using System.Runtime.InteropServices;
using static Xui.Runtime.IOS.CoreGraphics;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIView : UIResponder
    {
        public static new readonly Class Class = new Class(Lib, "UIView");

        public static readonly Sel SetNeedsDisplaySel = new Sel(Lib, "setNeedsDisplay");

        public static readonly Sel SafeAreaInsetsSel = new Sel(Lib, "safeAreaInsets");

        public static readonly Sel FrameSel = new Sel(Lib, "frame");
        public static readonly Sel SetFrameSel = new Sel(Lib, "setFrame:");

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        public static unsafe partial UIEdgeInsets objc_msgSend_retUIEdgeInsets(nint obj, nint sel);

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        public static unsafe partial CGRect objc_msgSend_retCGRect(nint obj, nint sel);

        [LibraryImport(LibObjCLib, EntryPoint = "objc_msgSend")]
        public static unsafe partial CGRect objc_msgSend(nint obj, nint sel, CGRect rect);

        public UIView() : base(Class.New())
        {
        }

        public UIView(nint id) : base(id)
        {
        }

        public void SetNeedsDisplay() => ObjC.objc_msgSend(this, SetNeedsDisplaySel);

        public UIEdgeInsets SafeAreaInsets => objc_msgSend_retUIEdgeInsets(this, SafeAreaInsetsSel);

        public CGRect Frame
        {
            get => objc_msgSend_retCGRect(this, FrameSel);
            set => objc_msgSend(this, SetFrameSel, value);
        }
    }
}