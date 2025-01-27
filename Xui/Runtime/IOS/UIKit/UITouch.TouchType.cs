using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UITouch : NSObject
    {
        public enum TouchType : int
        {
            Direct = 0,
            Indirect = 1,
            Pencil = 2,
            IndirectPointer = 3,
        }
    }
}
