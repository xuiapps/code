using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Xui.Runtime.Windows.Win32.Types;

namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    public static partial class Types
    {
        [DebuggerDisplay("{Value}")]
        public partial struct HWND
        {
            [LibraryImport(User32Lib)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool ShowWindow(HWND hWnd, int nCmdShow);

            [LibraryImport(User32Lib)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool UpdateWindow(HWND hWnd);

            [LibraryImport(User32Lib, EntryPoint="SetWindowTextW")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetWindowText(HWND hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpString);

            [LibraryImport(User32Lib, EntryPoint="GetWindowTextW")]
            public static partial int GetWindowText(HWND hWnd, [MarshalAs(UnmanagedType.LPWStr)] out string lpString, int maxLength);

            [LibraryImport(User32Lib, EntryPoint="GetClientRect")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool GetClientRect(HWND hWnd, out RECT rc);

            [LibraryImport(User32Lib, EntryPoint="DefWindowProcW")] 
            public static partial LRESULT DefWindowProc(HWND hWnd, WindowMessage uMsg, WPARAM wParam, LPARAM lParam);

            [LibraryImport(User32Lib)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool InvalidateRect(HWND hWnd, ref RECT rect, [MarshalAs(UnmanagedType.Bool)] bool erase);

            [LibraryImport(User32Lib)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool InvalidateRect(HWND hWnd, nint rectPtr, [MarshalAs(UnmanagedType.Bool)] bool erase);

            [LibraryImport(User32Lib)]
            public static partial HDC BeginPaint(HWND hWnd, ref PAINTSTRUCT lpPaint);

            [LibraryImport(User32Lib)]
            public static partial BOOL EndPaint(HWND hWnd, ref PAINTSTRUCT lpPaint);

            [LibraryImport(User32Lib)]
            public static partial BOOL ScreenToClient(HWND hWnd, ref POINT lpPoint);

            public nint value;

            public HWND(nint value)
            {
                this.value = value;
            }

            public bool SetWindowText(string text) => SetWindowText(this, text);

            public int GetWindowText(out string text, int maxLength) => GetWindowText(this, out text, maxLength);

            public bool GetClientRect(out RECT rect) => GetClientRect(this, out rect);

            public void ShowWindow(int nCmdShow = 1) => ShowWindow(this, nCmdShow);

            public bool UpdateWindow() => UpdateWindow(this);

            public void Invalidate() => InvalidateRect(this, 0, false);

            public bool ScreenToClient(ref POINT point) => HWND.ScreenToClient(this, ref point);

            public LRESULT DefWindowProc(WindowMessage uMsg, WPARAM wParam, LPARAM lParam) => DefWindowProc(this, uMsg, wParam, lParam);

            public HDC BeginPaint(ref PAINTSTRUCT lpPaint) =>
                BeginPaint(this, ref lpPaint);

            public bool EndPaint(ref PAINTSTRUCT lpPaint) =>
                EndPaint(this, ref lpPaint);

            public nint Value => this.value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator nint(HWND v) => v.value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator HWND(nint v) => new (v);
        }
    }
}