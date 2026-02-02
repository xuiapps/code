using System.Runtime.InteropServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class Kernel32
{
    public const string Kernel32Lib = "kernel32.dll";

    /// <summary>https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthreadid</summary>
    [LibraryImport(Kernel32Lib)]
    public static partial uint GetCurrentThreadId();
}
