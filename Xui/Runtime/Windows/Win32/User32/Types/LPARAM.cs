using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct LPARAM
    {
        public ulong value;

        public LPARAM(ulong value)
        {
            this.value = value;
        }

        public ulong Value => this.value;

        public short LoWord => (short)(this.value &0xFFFF);

        public short HiWord => (short)(this.value >> 16 &0xFFFF);

        public Point MousePosition => new Point(this.LoWord, this.HiWord);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(LPARAM v) => v.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator LPARAM(ulong v) => new (v);
    }
}