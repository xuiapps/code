using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class TransparentUnifiedStyle : TransparentStyle
    {
        public static new readonly TransparentUnifiedStyle Instance = new();

        private TransparentUnifiedStyle()
        {
        }

        protected override NSWindowToolbarStyle? ToolbarStyle => NSWindowToolbarStyle.Unified;
    }
}
