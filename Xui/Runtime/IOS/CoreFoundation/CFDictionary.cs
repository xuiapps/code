using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public partial class CFDictionary : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "CFDictionary");

        public CFDictionary(nint id) : base(id)
        {
        }
    }
}