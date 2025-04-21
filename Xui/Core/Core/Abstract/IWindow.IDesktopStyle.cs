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
    public interface IDesktopStyle
    {
        /// <summary>
        /// If <c>true</c>, the window will be created without a system title bar or border.
        /// The entire surface will be treated as a client area, while still maintaining
        /// standard desktop window behaviors (e.g., close/minimize buttons).
        /// </summary>
        public bool Chromeless => false;

        /// <summary>
        /// Optional startup size hint for the window.
        /// If <c>null</c>, the platform will decide the initial size.
        /// </summary>
        public Size? StartupSize => null;
    }
}
