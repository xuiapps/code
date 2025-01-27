namespace Xui.Runtime.Windows.Win32;

public static partial class Types
{
    public partial struct COLORREF
    {
        public uint __BBGGRR;

        public COLORREF(uint v)
        {
            this.__BBGGRR = v;
        }
    }
}