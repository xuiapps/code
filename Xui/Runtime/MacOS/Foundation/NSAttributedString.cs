using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public partial class NSAttributedString : NSObject
    {
        public static new readonly Class Class = new Class(Foundation.Lib, "NSAttributedString");

        public NSAttributedString(nint self) : base(self)
        {
        }

        public NSAttributedString() : base(Class.New())
        {
        }
    }
}