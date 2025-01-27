using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSResponder : NSObject
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSResponder");

        public NSResponder(nint id) : base(id)
        {
        }

        public override NSResponder Autorelease() => (NSResponder)base.Autorelease();
    }
}