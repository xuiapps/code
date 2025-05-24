using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class Foundation
{
    public partial class NSAttributedString : NSObject
    {
        public static new readonly Class Class = new Class(Foundation.Lib, "NSAttributedString");

        public static readonly Sel InitWithStringAttributesSel = new Sel("initWithString:attributes:");

        public NSAttributedString(nint self) : base(self)
        {
        }

        public NSAttributedString() : base(Class.New())
        {
        }
    }
}