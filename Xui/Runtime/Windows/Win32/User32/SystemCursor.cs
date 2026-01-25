namespace Xui.Runtime.Windows.Win32;

public static partial class User32
{
    public enum SystemCursor : int
    {
        /// <summary>Standard arrow cursor (IDC_ARROW)</summary>
        Arrow = 32512,

        /// <summary>I-beam text cursor (IDC_IBEAM)</summary>
        IBeam = 32513,

        /// <summary>Hourglass / wait (legacy) (IDC_WAIT)</summary>
        Wait = 32514,

        /// <summary>Crosshair (IDC_CROSS)</summary>
        Cross = 32515,

        /// <summary>Up arrow (rarely used) (IDC_UPARROW)</summary>
        UpArrow = 32516,

        /// <summary>Resize NW-SE / diagonal (IDC_SIZENWSE)</summary>
        SizeNWSE = 32642,

        /// <summary>Resize NE-SW / diagonal (IDC_SIZENESW)</summary>
        SizeNESW = 32643,

        /// <summary>Resize horizontal (IDC_SIZEWE)</summary>
        SizeWE = 32644,

        /// <summary>Resize vertical (IDC_SIZENS)</summary>
        SizeNS = 32645,

        /// <summary>Resize all directions (IDC_SIZEALL)</summary>
        SizeAll = 32646,

        /// <summary>Slashed circle (not allowed) (IDC_NO)</summary>
        No = 32648,

        /// <summary>Hand / link cursor (IDC_HAND)</summary>
        Hand = 32649,

        /// <summary>Application starting (arrow + wait) (IDC_APPSTARTING)</summary>
        AppStarting = 32650,

        /// <summary>Help cursor (arrow + question) (IDC_HELP)</summary>
        Help = 32651,
    }
}
