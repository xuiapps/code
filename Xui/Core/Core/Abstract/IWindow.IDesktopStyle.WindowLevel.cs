using Xui.Core.Math2D;

namespace Xui.Core.Abstract;

public partial interface IWindow
{
    public partial interface IDesktopStyle
    {
        /// <summary>
        /// Cross-platform window stacking level (Z-order abstraction).
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>macOS</b>: Maps to <c>NSWindow.Level</c> values such as <c>.normal</c>, <c>.floating</c>,
        /// <c>.statusBar</c>, and <c>.modalPanel</c>.
        /// </para>
        /// <para>
        /// <b>Windows</b>: Maps primarily to Win32 Z-order using <c>HWND_TOPMOST</c> /
        /// <c>HWND_NOTOPMOST</c>. Higher levels may be approximated, as Windows exposes fewer
        /// public layering tiers than macOS.
        /// </para>
        /// </remarks>
        public enum DesktopWindowLevel : int
        {
            /// <summary>
            /// Standard application window.
            /// </summary>
            /// <remarks>
            /// macOS: <c>NSWindow.Level.normal</c>.<br/>
            /// Windows: normal (non-topmost) window.
            /// </remarks>
            Normal = 0,

            /// <summary>
            /// Floating utility window above normal windows.
            /// </summary>
            /// <remarks>
            /// macOS: <c>NSWindow.Level.floating</c>.<br/>
            /// Windows: implemented via <c>HWND_TOPMOST</c>.
            /// </remarks>
            Floating = 100,

            /// <summary>
            /// High-floating level for small status or utility UI.
            /// </summary>
            /// <remarks>
            /// macOS: <c>NSWindow.Level.statusBar</c>.<br/>
            /// Windows: approximated as topmost, often with utility window styles.
            /// </remarks>
            StatusBar = 200,

            /// <summary>
            /// Modal dialog level that blocks interaction with other app windows.
            /// </summary>
            /// <remarks>
            /// macOS: <c>NSWindow.Level.modalPanel</c> or sheets.<br/>
            /// Windows: enforced via window ownership and modality, not a unique Z-order tier.
            /// </remarks>
            Modal = 300,
        }
    }
}
