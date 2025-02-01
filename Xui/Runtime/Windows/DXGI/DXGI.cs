using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    public const string DXGILib = "dxgi.dll";

    public static readonly nint Lib = NativeLibrary.Load(DXGILib);
}