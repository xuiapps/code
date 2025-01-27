using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreAnimation;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public partial class NSView : NSResponder
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSView");

        public static readonly Sel FrameSel = new Sel("frame");

        public static readonly Sel SetFrameSel = new Sel("setFrame:");

        public static readonly Sel AutoresizesSubviewsSel = new Sel("autoresizesSubviews");

        public static readonly Sel SetAutoresizesSubviewsSel = new Sel("setAutoresizesSubviews:");

        public static readonly Sel NeedsDisplaySel = new Sel("needsDisplay");

        public static readonly Sel SetNeedsDisplaySel = new Sel("setNeedsDisplay:");

        public static readonly Sel AutoresizingMaskSel = new Sel("autoresizingMask");

        public static readonly Sel SetAutoresizingMaskSel = new Sel("setAutoresizingMask:");

        public static readonly Sel TranslatesAutoresizingMaskIntoConstraintsSel = new Sel("translatesAutoresizingMaskIntoConstraints");

        public static readonly Sel SetTranslatesAutoresizingMaskIntoConstraintsSel = new Sel("setTranslatesAutoresizingMaskIntoConstraints:");

        public static readonly Sel AddSubviewSel = new Sel("addSubview:");

        public static readonly Sel FlippedSel = new Sel("flipped");

        public static readonly Sel SetFlippedSel = new Sel("setFlipped:");

        public static readonly Sel WantsLayerSel = new Sel("wantsLayer");

        public static readonly Sel SetWantsLayerSel = new Sel("setWantsLayer:");

        public static readonly Sel SizeThatFitsSel = new Sel("sizeThatFits:");

        public static readonly Sel SetNeedsDisplayInRectSel = new Sel("setNeedsDisplayInRect:");

        public static readonly Sel ConvertPointFromViewSel = new Sel("convertPoint:fromView:");

        public static readonly Sel LayerSel = new Sel("layer");
        public static readonly Sel SetLayerSel = new Sel("setLayer:");

        public NSView(nint id) : base(id)
        {
        }

        public NSView() : base(Class.New())
        {
        }

        public NSRect Frame
        {
            get => objc_msgSend_NSRectRet(this, FrameSel);
            set => objc_msgSend(this, SetFrameSel, value);
        }

        public bool AutoresizesSubviews
        {
            get => objc_msgSend_retBool(this, AutoresizesSubviewsSel);
            set => objc_msgSend(this, SetAutoresizesSubviewsSel, value);
        }

        public NSAutoresizingMaskOptions AutoresizingMask
        {
            get => (NSAutoresizingMaskOptions)objc_msgSend_retNUInt(this, AutoresizingMaskSel);
            set => objc_msgSend(this, SetAutoresizingMaskSel, (nuint)value);
        }

        public bool TranslatesAutoresizingMaskIntoConstraints
        {
            get => objc_msgSend_retBool(this, TranslatesAutoresizingMaskIntoConstraintsSel);
            set => objc_msgSend(this, SetTranslatesAutoresizingMaskIntoConstraintsSel, value);
        }

        public bool NeedsDisplay
        {
            get => objc_msgSend_retBool(this, NeedsDisplaySel);
            set => objc_msgSend(this, SetNeedsDisplaySel, value);
        }

        public bool Flipped
        {
            get => objc_msgSend_retBool(this, FlippedSel);
            set => objc_msgSend(this, SetFlippedSel, value);
        }

        public bool WantsLayer
        {
            get => objc_msgSend_retBool(this, WantsLayerSel);
            set => objc_msgSend(this, SetWantsLayerSel, value);
        }

        public CALayer Layer
        {
            // TODO: If this was created in C#, this may need to be captured somehow.
            get => new CALayer(objc_msgSend_retIntPtr(this, LayerSel));
            set => objc_msgSend(this, SetLayerSel, value);
        }

        public void AddSubview(NSView child) => objc_msgSend(this, AddSubviewSel, child);

        public NSSize SizeThatFits(NSSize size) => objc_msgSend_NSSizeRet(this, SizeThatFitsSel, size);

        public void SetNeedsDisplayInRect(NSRect rect) => objc_msgSend(this, SetNeedsDisplayInRectSel, rect);

        public NSPoint ConvertPointFromView(NSPoint point, NSView? view) => objc_msgSend_NSPointRet(this, ConvertPointFromViewSel, point, view == null ? 0 : view);

        public override NSView Autorelease() => (NSView)base.Autorelease();

        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial NSRect objc_msgSend_NSRectRet(nint obj, nint sel);

        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial NSSize objc_msgSend_NSSizeRet(nint obj, nint sel, NSSize size);

        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial NSPoint objc_msgSend_NSPointRet(nint obj, nint sel, NSPoint point, nint fromView);
    }
}