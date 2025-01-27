using System.Runtime.InteropServices;
using static Xui.Runtime.MacOS.CoreFoundation;
using static Xui.Runtime.MacOS.CoreGraphics;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public partial class NSRunLoop : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "NSRunLoop");

        public static readonly Prop.NInt MainRunLoopProp = new Prop.NInt("mainRunLoop", "mainRunLoop:");

        static NSRunLoop()
        {
            Marshalling.SetClassWrapper(Class, (nint self) => new NSRunLoop(self));
        }

        public NSRunLoop(nint id) : base(id)
        {
        }

        public static NSRunLoop MainRunLoop => Marshalling.ObjCToCSharp<NSRunLoop>(MainRunLoopProp.Get(Class));
    }
}