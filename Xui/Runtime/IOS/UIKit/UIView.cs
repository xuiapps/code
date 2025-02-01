using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIView : UIResponder
    {
        public static new readonly Class Class = new Class(Lib, "UIView");

        public static readonly Sel SetNeedsDisplaySel = new Sel(Lib, "setNeedsDisplay");

        public UIView() : base(Class.New())
        {
        }

        public UIView(nint id) : base(id)
        {
        }

        public void SetNeedsDisplay() => objc_msgSend(this, SetNeedsDisplaySel);
    }
}