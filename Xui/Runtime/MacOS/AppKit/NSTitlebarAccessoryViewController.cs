using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSTitlebarAccessoryViewController : NSViewController
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSTitlebarAccessoryViewController");

        public static readonly Prop.NInt LayoutAttributeProp = new Prop.NInt("layoutAttribute", "setLayoutAttribute:");

        public NSTitlebarAccessoryViewController(nint id) : base(id)
        {
        }

        public NSTitlebarAccessoryViewController() : base(Class.New())
        {
        }

        public NSLayoutAttribute LayoutAttribute
        {
            get => (NSLayoutAttribute)LayoutAttributeProp.Get(this);
            set => LayoutAttributeProp.Set(this, (nint)value);
        }
    }
}