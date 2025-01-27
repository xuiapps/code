using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct DWORD
    {
        public uint value;

        public DWORD(uint value)
        {
            this.value = value;
        }

        public uint Value => this.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator uint(DWORD v) => v.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator DWORD(uint v) => new (v);
    }
}