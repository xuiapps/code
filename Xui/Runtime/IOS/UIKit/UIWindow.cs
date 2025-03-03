using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIWindow : UIView
    {
        public static new readonly Class Class = new Class(Lib, "UIWindow");

        public static readonly Sel MakeKeyAndVisibleSel = new Sel("makeKeyAndVisible");

        public static readonly Prop.NInt RootViewControllerProp = new Prop.NInt("rootViewController", "setRootViewController:");

        public static readonly Prop.NInt BackgroundColorProp = new Prop.NInt("backgroundColor", "setBackgroundColor:");

        public UIWindow() : base(Class.New())
        {
        }

        public UIWindow(nint id) : base(id)
        {
        }

        public UIViewController RootViewController
        {
            get => Marshalling.Get<UIViewController>(RootViewControllerProp.Get(this));
            set => RootViewControllerProp.Set(this, value);
        }

        public void MakeKeyAndVisible() => ObjC.objc_msgSend(this, MakeKeyAndVisibleSel);
    }
}