namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public struct FrameStatistics
    {
        public uint PresentCount;
        public uint PresentRefreshCount;
        public uint SyncRefreshCount;
        public long SyncQPCTime;
        public long SyncGPUTime;
    }
}