using UIKit;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIResponder : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "UIResponder");

        public UIResponder() : base(Class.New())
        {
        }

        public UIResponder(nint id) : base(id)
        {
        }
    }
}