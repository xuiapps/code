namespace Xui.Core.Actual;

/// <summary>
/// Represents a platform-specific window implementation used by the Xui runtime.
/// Each platform (e.g., Windows, macOS, iOS) must provide an implementation of this interface
/// to manage window lifecycle, rendering, and input.
///
/// This interface is typically paired with an abstract window in the Xui framework,
/// and is not used directly by application developers.
/// </summary>
public interface IWindow
{
    /// <summary>
    /// Gets or sets the window title, where supported by the platform (e.g., desktop).
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Displays the window to the user. This may include making it visible, entering the main loop,
    /// or attaching it to the application’s view hierarchy, depending on the platform.
    /// </summary>
    void Show();

    /// <summary>
    /// Requests a redraw of the window surface.
    /// The platform should trigger a paint or render callback as soon as possible.
    /// </summary>
    void Invalidate();

    /// <summary>
    /// Gets or sets whether the window currently requires keyboard input focus.
    /// Platforms may use this to show or hide on-screen keyboards.
    /// </summary>
    bool RequireKeyboard { get; set; }
}
