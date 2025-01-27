namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UITouch
    {
        public enum Phase : int
        {
            Began = 0,
            Moved = 1,
            Stationary = 2,
            Ended = 3,
            Cancelled = 4,
            RegionEntered = 5,
            RegionMoved = 6,
            RegionExited = 7,
        }
    }
}
