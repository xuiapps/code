using System.Runtime.InteropServices;

namespace Xui.Runtime.MacOS;

public static partial class ImageIO
{
    public const string ImageIOLib = "/System/Library/Frameworks/ImageIO.framework/ImageIO";

    public static readonly nint Lib;

    static ImageIO()
    {
        Lib = NativeLibrary.Load(ImageIOLib);
    }
}
