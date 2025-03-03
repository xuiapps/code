using System.Diagnostics;
using static Xui.Runtime.IOS.CoreGraphics;
using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit;
using Xui.Core.Math2D;

namespace Xui.Runtime.IOS.Actual;

public class IOSWindowRootView : UIView
{
    private static readonly Sel SafeAreaInsetsDidChangeSel = new Sel("safeAreaInsetsDidChange");

    public static new readonly Class Class = UIView.Class
        .Extend("XUIIOSWindowRootView")
        .AddMethod("drawRect:", DrawRect)
        .AddMethod("safeAreaInsetsDidChange", SafeAreaInsetsDidChange)
        .Register();

    public static void DrawRect(nint self, nint sel, CGRect rect) =>
        Marshalling.Get<IOSWindowRootView>(self).DrawRect(rect);
    
    public static void SafeAreaInsetsDidChange(nint self, nint sel) =>
        Marshalling.Get<IOSWindowRootView>(self).SafeAreaInsetsDidChange(sel);
    
    public IOSWindowRootView(IOSWindow window) : base(Class.New())
    {
        this.Window = window;
    }
    
    protected internal IOSWindow Window { get; }

    private void DrawRect(CGRect rect) => this.Window.Render(rect);

    protected void SafeAreaInsetsDidChange(nint sel)
    {
        Super super = new Super(this, UIView.Class);
        objc_msgSendSuper(ref super, sel);

        var frame = this.Frame;
        var safeInsets = this.SafeAreaInsets;

        this.Window.DisplayArea = frame;
        this.Window.SafeArea = new Rect(
            frame.Origin.X + safeInsets.Left,
            frame.Origin.Y + safeInsets.Top,
            frame.Size.Width - safeInsets.Left - safeInsets.Right,
            frame.Size.Height - safeInsets.Top - safeInsets.Bottom);
    }
}