using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public class NSMutableAttributedString : NSAttributedString
    {
        public static new readonly Class Class = new Class(Foundation.Lib, "NSMutableAttributedString");

        public NSMutableAttributedString(nint self) : base(self)
        {
        }

        public NSMutableAttributedString() : base(Class.New())
        {
        }
    }
}