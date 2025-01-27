namespace Xui.Runtime.MacOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// This is not 64 bit on 64 bit architecture, it is defined as int32_t.
    /// </summary>
    public enum CGLineCap : int
    {
        Butt = 0,
        Round = 1,
        Square = 2
    }
}