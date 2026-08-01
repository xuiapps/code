using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class TransparentUnifiedCompactStyle : TransparentStyle
    {
        public static new readonly TransparentUnifiedCompactStyle Instance = new();

        private TransparentUnifiedCompactStyle()
        {
        }

        protected override NSWindowToolbarStyle? ToolbarStyle => NSWindowToolbarStyle.UnifiedCompact;
    }
}
