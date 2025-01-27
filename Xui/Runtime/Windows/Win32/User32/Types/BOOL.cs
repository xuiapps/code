using System.Diagnostics;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct BOOL
    {
        public int value;

        public BOOL(bool v)
        {
            this.value = v ? 1 : 0;
        }

        public bool Value => this.value != 0;

        public static implicit operator bool(BOOL v) => v.Value;
        public static implicit operator BOOL(bool v) => new (v);
    }
}
