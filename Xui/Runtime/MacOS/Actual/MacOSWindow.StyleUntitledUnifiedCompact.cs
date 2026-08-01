using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class UntitledUnifiedCompactStyle : UntitledUnifiedStyle
    {
        public static new readonly UntitledUnifiedCompactStyle Instance = new();
        private UntitledUnifiedCompactStyle() { }
        protected override NSWindowToolbarStyle ToolbarStyle => NSWindowToolbarStyle.UnifiedCompact;
    }
}
