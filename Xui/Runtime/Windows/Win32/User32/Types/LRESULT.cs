using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct LRESULT
    {
        public int value;

        public LRESULT(int value)
        {
            this.value = value;
        }

        public int Value => this.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(LRESULT v) => v.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator LRESULT(int v) => new (v);
    }
}