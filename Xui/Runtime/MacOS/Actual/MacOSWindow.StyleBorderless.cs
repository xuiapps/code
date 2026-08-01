using Xui.Core.Abstract;
using static Xui.Runtime.MacOS.AppKit;

namespace Xui.Runtime.MacOS.Actual;

public partial class MacOSWindow
{
    /// <summary>
    /// Uses the transparent native surface but reserves interaction handling for
    /// application-defined title and resize regions.
    /// </summary>
    private sealed class BorderlessStyle : TransparentStyle
    {
        private static readonly BorderlessStyle regular = new(null);
        private static readonly BorderlessStyle medium = new(NSWindowToolbarStyle.UnifiedCompact);
        private static readonly BorderlessStyle large = new(NSWindowToolbarStyle.Unified);

        private readonly NSWindowToolbarStyle? toolbarStyle;

        private BorderlessStyle(NSWindowToolbarStyle? toolbarStyle)
        {
            this.toolbarStyle = toolbarStyle;
        }

        protected override NSWindowToolbarStyle? ToolbarStyle => this.toolbarStyle;

        public static BorderlessStyle Get(MacOSWindowTitleHeight titleHeight) => titleHeight switch
        {
            MacOSWindowTitleHeight.Default => regular,
            MacOSWindowTitleHeight.Medium => medium,
            MacOSWindowTitleHeight.Large => large,
            _ => regular,
        };
    }
}
