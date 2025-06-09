using static Xui.Runtime.MacOS.Foundation;
using static Xui.Runtime.MacOS.ObjC;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private static readonly Sel StandardWindowButtonSel = new Sel("standardWindowButton:");
    private static readonly Sel SetHiddenSel = new Sel("setHidden:");

    public void HideTitleButtons()
    {
        for (int i = 0; i <= 2; i++) // Close, Minimize, Zoom
        {
            nint button = objc_msgSend_retIntPtr(this, StandardWindowButtonSel, (nint)i);
            if (button != 0)
            {
                objc_msgSend(button, SetHiddenSel, true);
            }
        }
    }
}
