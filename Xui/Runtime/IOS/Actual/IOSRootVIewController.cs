using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit;

namespace Xui.Runtime.IOS.Actual;

public class IOSRootViewController : UIViewController
{
    public static new readonly Class Class = UIViewController.Class
        .Extend("XUIIOSRootViewController")
        .Register();

    public IOSRootViewController(IOSWindow window) : base(Class.New())
    {
        this.Window = window;
    }
    
    protected internal IOSWindow Window { get; }
}