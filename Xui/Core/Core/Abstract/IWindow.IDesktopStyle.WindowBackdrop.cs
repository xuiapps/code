namespace Xui.Core.Abstract;

public partial interface IWindow
{
    public partial interface IDesktopStyle
    {
        /// <summary>
        /// Controls the window backdrop and chrome style.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>Windows</b>: <c>Default</c> creates a standard <c>WS_TILEDWINDOW</c>.
        /// <c>Chromeless</c> uses <c>WS_POPUP</c> with no system chrome.
        /// <c>Mica</c> and <c>Acrylic</c> use DWM system backdrop APIs (Windows 11+)
        /// with system caption buttons but no icon or title text.
        /// </para>
        /// <para>
        /// <b>macOS</b>: <c>Chromeless</c> maps to <c>FullSizeContentView | Borderless</c>.
        /// <c>Mica</c> and <c>Acrylic</c> fall back to <c>Default</c> for now.
        /// </para>
        /// </remarks>
        public enum WindowBackdrop
        {
            /// <summary>
            /// Standard opaque window with full system chrome (title bar, icon, buttons).
            /// </summary>
            Default = 0,

            /// <summary>
            /// No system chrome. The entire surface is client area and the app draws everything.
            /// </summary>
            Chromeless = 1,

            /// <summary>
            /// Translucent blurred backdrop (Windows 11 Mica). System caption buttons are kept,
            /// but icon and title text are removed. The app can draw into the full window area.
            /// </summary>
            Mica = 2,

            /// <summary>
            /// Acrylic blur-behind backdrop (Windows 11). Similar to Mica but with a more
            /// translucent, in-app blur effect.
            /// </summary>
            Acrylic = 3,
        }
    }
}
