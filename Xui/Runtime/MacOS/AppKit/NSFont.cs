using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSFont : NSObject
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSFont");

        public NSFont(nint id) : base(id)
        {
        }

        public NSFont() : base(Class.New())
        {
        }

        public override NSFont Autorelease() => (NSFont)base.Autorelease();
    }
}