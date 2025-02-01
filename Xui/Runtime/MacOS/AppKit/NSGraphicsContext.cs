
using static Xui.Runtime.MacOS.CoreGraphics;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class AppKit
{
    public class NSGraphicsContext : NSObject
    {
        public static new readonly Class Class = new Class(AppKit.Lib, "NSGraphicsContext");

        protected static readonly Prop.NInt CurrentContextProp = new Prop.NInt("currentContext", "setCurrentContext:");

        protected static readonly Sel CGContextSel = new Sel("CGContext");

        private NSGraphicsContext(nint id) : base(id)
        {
        }

        /// <summary>
        /// The underlying implementation is rarely crated by you,
        /// going through wrap and unwrap will raise questions about ownership.
        /// 
        /// Use the short-lived stack based <see cref="CurrentCGContext"/> instead.
        /// </summary>
        public static NSGraphicsContext? CurrentContext
        {
            get
            {
                var self = CurrentContextProp.Get(Class);
                return self == nint.Zero ? null : Marshalling.ObjCToCSharp<NSGraphicsContext>(self);
            }
            set => CurrentContextProp.Set(Class, value ?? nint.Zero);
        }

        public static CGContextRef CurrentCGContext
        {
            get
            {
                var self = CurrentContextProp.Get(Class);
                var cg = objc_msgSend_retIntPtr(self, CGContextSel);
                return new CGContextRef(cg);
            }
        }
    }
}
