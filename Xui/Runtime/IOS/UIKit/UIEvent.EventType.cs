using static Xui.Runtime.IOS.ObjC;

namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIEvent
    {
        public enum EventType : int
        {
            Touches = 0,
            Motion = 1,
            RemoteControl = 2,
            Presses = 3,
            Scroll = 10,
            Hover = 11,
            Transform = 14,
        }
    }
}
