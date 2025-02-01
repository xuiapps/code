using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows;

public static partial class DComp
{
    public const string DCompLib = "dcomp.dll";

    public static readonly nint Lib = NativeLibrary.Load(DCompLib);

    [LibraryImport(DCompLib)]
    public static partial uint DCompositionWaitForCompositorClock(uint handleCount, nint handlesArrPtr, uint timeoutInMs);

    [LibraryImport(DCompLib)]
    public static unsafe partial HRESULT DCompositionCreateDevice(void* dxgiDevice, in Guid iid, out void* dcompositionDevice);
}