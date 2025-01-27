using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xui.Core.Math2D;
using static Xui.Runtime.Windows.Win32.User32;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public partial struct HDC
    {
        [LibraryImport(User32Lib, EntryPoint="FillRect")]
        public static partial BOOL HDCFillRect(nint hDC, ref RECT lprc, nint hbr);

        [LibraryImport(User32Lib, EntryPoint="FrameRect")]
        public static partial BOOL HDCFrameRect(nint hDC, ref RECT lprc, nint hbr);

        public nint value;

        public HDC(nint value)
        {
            this.value = value;
        }

        public bool FillRect(Rect rect, nint brush)
        {
            var r = new RECT(rect);
            return HDCFillRect(this.value, ref r, brush);
        }

        public bool FrameRect(Rect rect, nint brush)
        {
            var r = new RECT(rect);
            return HDCFrameRect(this.value, ref r, brush);
        }

        public nint Value => this.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(HDC v) => v.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HDC(nint v) => new (v);
    }
}