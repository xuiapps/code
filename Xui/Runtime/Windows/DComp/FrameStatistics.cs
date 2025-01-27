using static Xui.Runtime.Windows.DXGI;

namespace Xui.Runtime.Windows;

public static partial class DComp
{
    public struct FrameStatistics
    {
        public long LastFrameTime;
        public Rational CurrentCompositionRate;
        public long CurrentTime;
        public long TimeFrequency;
        public long NextEstimatedFrameTime;
    }
}