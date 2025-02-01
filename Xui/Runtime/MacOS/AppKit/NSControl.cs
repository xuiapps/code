using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSControl : NSView
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSControl");

        public NSControl(nint id) : base(id)
        {
        }

        public NSControl() : base(Class.New())
        {
        }

        public override NSControl Autorelease() => (NSControl)base.Autorelease();
    }
}