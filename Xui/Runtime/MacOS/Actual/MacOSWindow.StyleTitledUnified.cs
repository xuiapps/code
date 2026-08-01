using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    private sealed class TitledUnifiedStyle : TitledUnifiedCompactStyle
    {
        public static new readonly TitledUnifiedStyle Instance = new();

        private TitledUnifiedStyle()
        {
        }

        protected override NSWindowToolbarStyle ToolbarStyle => NSWindowToolbarStyle.Unified;
    }
}
