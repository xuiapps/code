using static Xui.Runtime.IOS.ObjC;
using static Xui.Runtime.IOS.UIKit;
using static Xui.Runtime.IOS.CoreFoundation;

namespace Xui.Runtime.IOS.Actual;

public class IOSDefaultResponder : UIResponder
{
    public static new readonly Class Class = UIResponder.Class
        .Extend("XUIIOSDefaultResponder")
        .AddMethod("canBecomeFirstResponder", CanBecomeFirstResponderStatic)
        .AddMethod("nextResponder", NextResponderStatic)
        .Register();
    
    private static bool CanBecomeFirstResponderStatic(nint self, nint sel) =>
        Marshalling.Get<IOSDefaultResponder>(self).CanBecomeFirstResponder;

    private static nint NextResponderStatic(nint self, nint sel) =>
        Marshalling.Get<IOSDefaultResponder>(self).NextResponder;
    
    public IOSDefaultResponder(IOSWindow window) : base(Class.New())
    {
        this.Window = window;
    }

    public bool CanBecomeFirstResponder => true;

    // TODO: Use some sort of ref-counting gere...
    public nint NextResponder { get; set; }
    
    protected internal IOSWindow Window { get; }
}