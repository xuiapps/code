using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class CoreFoundation
{
    public partial class CFArray : NSObject
    {
        public static new readonly Class Class = new Class(Lib, "CFArray");

        public CFArray(nint id) : base(id)
        {
        }
    }
}