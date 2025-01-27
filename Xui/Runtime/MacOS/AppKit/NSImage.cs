using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSImage : NSControl
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSImage");

        private static readonly Sel InitWithContentsOfFileSel = new Sel("initWithContentsOfFile:");

        public NSImage(nint id) : base(id)
        {
        }

        public NSImage() : base(Class.New())
        {
        }

        public static NSImage LoadImageFile(string path)
        {
            var cfStringRef = new CFStringRef(path);
            return new NSImage(objc_msgSend_retIntPtr(Class.New(), InitWithContentsOfFileSel, cfStringRef));
        }

        public override NSImage Autorelease() => (NSImage)base.Autorelease();
    }
}