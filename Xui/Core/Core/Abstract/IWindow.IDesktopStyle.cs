using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    /// <summary>
    /// Provides optional desktop-specific window styling hints for platforms
    /// that support multiple top-level windows (e.g., Windows, macOS, Linux).
    /// </summary>
    /// <remarks>
    /// On mobile platforms, applications typically run in a single full-screen window,
    /// and this interface has no effect. On desktop, implementing this interface allows
    /// apps to influence window chrome, border visibility, and initial sizing.
    /// </remarks>
    public partial interface IDesktopStyle
    {
        /// <summary>
        /// Controls the window backdrop and chrome style.
        /// </summary>
        public WindowBackdrop Backdrop => WindowBackdrop.Default;

        /// <summary>
        /// Optional startup size hint for the window.
        /// If <c>null</c>, the platform will decide the initial size.
        /// </summary>
        public Size? StartupSize => null;

        /// <summary>
        /// Controls the window stacking level (Z-order) relative to other windows.
        /// </summary>
        public DesktopWindowLevel Level => DesktopWindowLevel.Normal;

        /// <summary>
        /// Controls whether the client area includes the title bar region.
        /// </summary>
        /// <remarks>
        /// <b>Default</b>: System-managed non-client area (title bar + borders behave normally).<br/>
        /// <b>Extended</b>: Extends the client area into the title bar region so the app can draw its own
        /// title bar content/background while still allowing native window behaviors via hit testing.<br/><br/>
        /// <b>Windows</b>: Uses <c>DwmExtendFrameIntoClientArea</c> (and relies on <c>WM_NCHITTEST</c> returning
        /// <c>HTCAPTION</c> for draggable regions). This is the recommended way to hide title text/icon reliably,
        /// especially with Mica/Acrylic backdrops.<br/>
        /// <b>macOS</b>: Uses full-size content view / transparent title bar with title visibility hidden.
        /// </remarks>
        public WindowClientArea ClientArea => WindowClientArea.Default;
    }
}
