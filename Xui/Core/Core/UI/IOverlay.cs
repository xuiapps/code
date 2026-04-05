using System;
using Xui.Core.Math2D;

namespace Xui.Core.UI;

/// <summary>
/// A transient in-window overlay acquired via <c>GetService&lt;IOverlay&gt;()</c>.
/// Rendered by <see cref="RootView"/> on top of all other content — no native window is created.
/// Works on every platform (Windows, macOS, iOS, Android, Browser) with no platform-specific code.
/// </summary>
/// <remarks>
/// Use this for cross-platform scenarios: mobile, simulators, web/SPA, or
/// in-app surfaces like top-of-screen search fields that must stay within the window.
/// For native OS-level popups that can extend outside the app window,
/// use <see cref="IPopup"/> instead.
/// <para>
/// Usage pattern:
/// <code>
/// var overlay = this.GetService&lt;IOverlay&gt;();
/// overlay.Show(content, anchorRect, PopupPlacement.Below);
/// </code>
/// The overlay auto-dismisses when the user clicks outside.
/// Disposing the overlay closes it if still visible.
/// </para>
/// </remarks>
public interface IOverlay : IDisposable
{
    /// <summary>
    /// Shows the overlay with the given content, positioned relative to
    /// <paramref name="anchorRect"/> (in the owning window's coordinate space).
    /// </summary>
    /// <param name="content">The view to display inside the overlay.</param>
    /// <param name="anchorRect">
    /// The rect of the triggering element in window coordinates.
    /// Used to position the overlay (for example, below a button).
    /// </param>
    /// <param name="placement">Preferred placement direction.</param>
    /// <param name="size">
    /// Desired overlay size. If <c>null</c>, a default size is used.
    /// </param>
    /// <param name="effect">Visual backdrop effect (reserved for future use).</param>
    void Show(View content, Rect anchorRect, PopupPlacement placement = PopupPlacement.Below, Size? size = null, PopupEffect effect = PopupEffect.None);

    /// <summary>Closes the overlay if visible.</summary>
    void Close();

    /// <summary>Whether the overlay is currently visible.</summary>
    bool IsVisible { get; }

    /// <summary>Raised when the overlay is dismissed (click-outside, explicit close, or parent window close).</summary>
    event Action? Closed;
}
