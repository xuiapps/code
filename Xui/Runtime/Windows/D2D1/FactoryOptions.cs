namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    /// <summary>https://learn.microsoft.com/en-us/windows/win32/api/d2d1/ns-d2d1-d2d1_factory_options</summary>
    public struct FactoryOptions
    {
        public DebugLevel DebugLevel;

        public FactoryOptions(DebugLevel debugLevel)
        {
            this.DebugLevel = debugLevel;
        }
    }
}
