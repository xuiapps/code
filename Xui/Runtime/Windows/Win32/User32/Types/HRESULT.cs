using System.Diagnostics;

namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    [DebuggerDisplay("{Value}")]
    public struct HRESULT
    {
        public int value;

        public HRESULT(int value)
        {
            this.value = value;
        }

        public int Value => this.value;

        public bool Failed => this.value < 0;
        public bool Succeeded => this.value >= 0;

        public static implicit operator bool(HRESULT res) => res.Succeeded;

        public static implicit operator int(HRESULT res) => res.value;
    }
}