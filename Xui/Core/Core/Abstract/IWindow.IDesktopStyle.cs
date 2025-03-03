using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    /// <summary>
    /// Mobile platforms usually render in a single full-screen window.
    /// 
    /// Desktop platforms compose multiple application windows side by side and have some extra window options.
    /// </summary>
    public interface IDesktopStyle
    {
        /// <summary>
        /// If true will create a transparent window with no border, no titlebar,
        /// full size client area, AND default desktop window buttons.
        /// </summary>
        public bool Chromeless => false;

        public Size? StartupSize => null;
    }
}
