using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

public static partial class Foundation
{
    public class NSDictionary : NSObject
    {
        public static new readonly Class Class = new Class(Foundation.Lib, "NSDictionary");

        public NSDictionary(nint self) : base(self)
        {
        }

        public NSDictionary() : base(Class.New())
        {
        }
    }
}