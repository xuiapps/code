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
        /// <b>macOS</b>: <c>Chromeless</c> maps to <c>FullSizeContentView | Borderless</c>
        /// with a transparent title bar and empty toolbar (traffic-light buttons remain, no chrome visible).
        /// <c>Acrylic</c> uses <c>NSVisualEffectView</c> with the same transparent-titlebar + empty-toolbar treatment.
        /// <c>Mica</c> is a Windows-only concept; prefer <c>Acrylic</c> for cross-platform blur-behind.
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
            /// Translucent blurred backdrop using the Windows 11 Mica material. System caption
            /// buttons are kept, but icon and title text are removed.
            /// <b>Windows only</b> — use <see cref="Acrylic"/> for cross-platform blur-behind.
            /// </summary>
            Mica = 2,

            /// <summary>
            /// Blur-behind backdrop. On Windows 11 uses the Acrylic DWM material (more translucent
            /// than Mica). On macOS uses <c>NSVisualEffectView</c> with a transparent title bar and
            /// empty toolbar so traffic-light buttons are visible without any other chrome.
            /// </summary>
            Acrylic = 3,
        }
    }
}
