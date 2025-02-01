namespace Xui.Runtime.IOS;

public static partial class CoreGraphics
{
    /// <summary>
    /// This is not 64 bit on 64 bit architecture, it is defined as int32_t.
    /// </summary>
    public enum CGTextDrawingMode : int
    {
        Fill = 0,
        Stroke = 1,
        FillStroke = 2,
        Invisible = 3,
        FillClip = 4,
        StrokeClip = 5,
        FillStrokeClip = 6,
        Clip = 7,
    }
}
