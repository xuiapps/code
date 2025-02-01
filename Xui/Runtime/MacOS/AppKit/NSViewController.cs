using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public partial class NSViewController : NSResponder
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSViewController");

        private static readonly Prop.NInt ViewProp = new Prop.NInt("view", "setView:");

        private static readonly Prop PreferredContentSizeProp = new Prop("preferredContentSize", "setPreferredContentSize:");

        public NSViewController(nint id) : base(id)
        {
        }

        public NSViewController() : base(Class.New())
        {
        }

        public NSView View
        {
            get => throw new NotImplementedException();
            set => ViewProp.Set(this, value);
        }

        public NSSize PreferredContentSize
        {
            get => objc_msgSend_NSSizeRet(this, PreferredContentSizeProp.GetSel);
            set => objc_msgSend(this, PreferredContentSizeProp.SetSel, value);
        }
        
        [LibraryImport(ObjC.LibObjCLib, EntryPoint = "objc_msgSend")]
        private static partial NSSize objc_msgSend_NSSizeRet(nint obj, nint sel);
    }
}