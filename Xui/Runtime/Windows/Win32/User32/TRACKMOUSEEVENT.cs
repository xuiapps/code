using System;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;
using static Xui.Runtime.Windows.Win32.User32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-trackmouseevent
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TRACKMOUSEEVENT
    {
        // DWORD cbSize;
        [FieldOffset(0)]
        public uint cbSize;

        // DWORD dwFlags;
        [FieldOffset(4)]
        public TrackMouseEventFlags dwFlags;

        // HWND hwndTrack;
        [FieldOffset(8)]
        public HWND hwndTrack;

        // DWORD dwHoverTime;
        [FieldOffset(16)]
        public uint dwHoverTime;
    }
}
