using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSTextView : NSText
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSTextView");

        public NSTextView(nint id) : base(id)
        {
        }

        public NSTextView() : base(Class.New())
        {
        }

        public override NSTextView Autorelease() => (NSTextView)base.Autorelease();
    }
}