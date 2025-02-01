using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public class NSMutableDictionary : NSDictionary
    {
        public static new readonly Class Class = new Class(Foundation.Lib, "NSMutableDictionary");

        public NSMutableDictionary(nint self) : base(self)
        {
        }

        public NSMutableDictionary() : base(Class.New())
        {
        }
    }
}