using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows;

/// <summary>
/// Code from &lt;dwrite.h&gt; in the dwrite.dll lib.
/// </summary>
public static partial class DWrite
{
    public const string DWriteLib = "dwrite.dll";

    public static readonly nint Lib = NativeLibrary.Load(DWriteLib);
}