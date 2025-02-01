using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit;

namespace Xui.Runtime.IOS.Actual;

public class IOSWindowRootView : UIView
{
    public static new readonly Class Class = UIView.Class
        .Extend("XUIIOSWindowRootView")
        .AddMethod("drawRect:", DrawRect)
        .Register();

    public static void DrawRect(nint self, nint sel, CGRect rect) =>
        Marshalling.Get<IOSWindowRootView>(self).DrawRect(rect);
    
    private void DrawRect(CGRect rect) => this.Window.Render(rect);

    public IOSWindowRootView(IOSWindow window) : base(Class.New())
    {
        this.Window = window;
    }
    
    protected internal IOSWindow Window { get; }
}