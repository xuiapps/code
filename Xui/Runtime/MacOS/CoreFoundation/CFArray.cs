using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS;

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