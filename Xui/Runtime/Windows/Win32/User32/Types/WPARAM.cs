using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xui.Core.Math2D;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct WPARAM
    {
        public ulong value;

        public WPARAM(ulong value)
        {
            this.value = value;
        }

        public ulong Value => this.value;

        public short HiWord => (short)(this.value >> 16 & 0xFFFF);

        public short LoWord => (short)(this.value & 0xFFFF);

        public Vector WheelDelta => new (0, this.HiWord);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(WPARAM v) => v.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator WPARAM(ulong v) => new (v);
    }
}