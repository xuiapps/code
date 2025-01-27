namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// This is not 64 bit on 64 bit architecture, it is defined as int32_t.
    /// </summary>
    public enum CGLineJoin : int
    {
        Miter = 0,
        Round = 1,
        Bevel = 2
    }
}