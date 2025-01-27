using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

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