using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSText : NSView
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSText");

        public NSText(nint id) : base(id)
        {
        }

        public NSText() : base(Class.New())
        {
        }

        public override NSText Autorelease() => (NSText)base.Autorelease();
    }
}