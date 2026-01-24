using System;
using static Xui.Runtime.Windows.COM;

namespace Xui.Runtime.Windows;

public static partial class DWrite
{
    public enum FontFaceType : int
    {
        Cff = 0,
        Truetype = 1,
        OpenTypeCollection = 2,
        Type1 = 3,
        Vector = 4,
        Bitmap = 5,
        Unknown = 6,
        RawCff = 7,
    }
}
