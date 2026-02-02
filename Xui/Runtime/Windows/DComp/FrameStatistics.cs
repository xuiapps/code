using System;
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

        public TimeSpan LastFrameTimeSpan =>
            TimeSpan.FromSeconds(TickToSeconds(this.LastFrameTime));

        public TimeSpan CurrentTimeSpan =>
            TimeSpan.FromSeconds(TickToSeconds(this.CurrentTime));

        public TimeSpan NextEstimatedFrameTimeSpan =>
            TimeSpan.FromSeconds(TickToSeconds(this.NextEstimatedFrameTime));

        public TimeSpan EstimatedFrameInterval =>
            this.CurrentCompositionRate.Denominator != 0
                ? TimeSpan.FromSeconds(
                    (double)this.CurrentCompositionRate.Denominator /
                    this.CurrentCompositionRate.Numerator)
                : default;
        
        private double TickToSeconds(long ticks) =>
            this.TimeFrequency > 0
                ? (double)ticks / this.TimeFrequency
                : 0.0;
    }
}