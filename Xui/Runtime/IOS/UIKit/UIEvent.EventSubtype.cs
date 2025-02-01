namespace Xui.Runtime.IOS;

public static partial class UIKit
{
    public partial class UIEvent
    {
        public enum EventSubtype : int
        {
            None = 0,
            MotionShake = 1,
            RemoteControlPlay = 100,
            RemoteControlPause = 101,
            RemoteControlStop = 102,
            RemoteControlTogglePlayPause = 103,
            RemoteControlNextTrack = 104,
            RemoteControlPreviousTrack = 105,
            RemoteControlBeginSeekingBackward = 106,
            RemoteControlEndSeekingBackward = 107,
            RemoteControlBeginSeekingForward = 108,
            RemoteControlEndSeekingForward = 109,
        }
    }
}
