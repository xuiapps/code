using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class D2D1
{
    public const string D2D1Lib = "d2d1.dll";

    public static readonly nint Lib = NativeLibrary.Load(D2D1Lib);
}