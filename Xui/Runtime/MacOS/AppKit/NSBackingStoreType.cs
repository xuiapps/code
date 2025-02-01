namespace Xui.Runtime.MacOS;

public partial class AppKit
{
    public enum NSBackingStoreType : uint
    {
        Retained = 0,
        Nonretained = 1,
        Buffered = 2
    }
}