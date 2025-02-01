using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    /// <summary>https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-paintstruct</summary>
    [StructLayout(LayoutKind.Explicit, Size=72)]
    public struct PAINTSTRUCT
    {
        [FieldOffset(0)]
        public uint hdc;

        [FieldOffset(8)]
        public BOOL fErase;

        [FieldOffset(12)]
        public RECT rcPaint;

        [FieldOffset(28)]
        public BOOL fRestore;

        [FieldOffset(32)]
        public BOOL fIncUpdate;

        [FieldOffset(36)]
        public UInt64 rgbReserved1, rgbReserved2, rgbReserved3, rgbReserved4;
    }
}
