namespace Xui.Core.Abstract;

/// <summary>
/// Describes the macOS-specific chrome treatment for a desktop window.
/// Implementing this interface is safe on every target; non-macOS runtimes
/// ignore it.
/// </summary>
public interface IMacOSWindowStyle
{
    /// <summary>
    /// Controls whether AppKit displays the native window title.
    /// </summary>
    public MacOSWindowTitle Title => MacOSWindowTitle.Visible;

    /// <summary>
    /// Selects the native title and toolbar height.
    /// </summary>
    public MacOSWindowTitleHeight TitleHeight => MacOSWindowTitleHeight.Default;

    /// <summary>
    /// Selects the window background treatment.
    /// </summary>
    public MacOSWindowBackground Background => MacOSWindowBackground.Default;

}

/// <summary>
/// The visibility of the native macOS window title.
/// </summary>
public enum MacOSWindowTitle
{
    /// <summary>The native title is visible.</summary>
    Visible,

    /// <summary>The native title is hidden.</summary>
    Hidden,
}

/// <summary>
/// The native macOS title and toolbar height.
/// </summary>
public enum MacOSWindowTitleHeight
{
    /// <summary>The standard AppKit title-bar height.</summary>
    Default,

    /// <summary>The compact unified toolbar height.</summary>
    Medium,

    /// <summary>The unified toolbar height.</summary>
    Large,
}

/// <summary>
/// The native macOS window background treatment.
/// </summary>
public enum MacOSWindowBackground
{
    /// <summary>The standard opaque AppKit window background.</summary>
    Default,

    /// <summary>The standard Acrylic background material.</summary>
    Acrylic,

    /// <summary>
    /// The dynamic macOS glass material. On macOS releases that do not provide
    /// it, this falls back to the Acrylic background material.
    /// </summary>
    Glass,

    /// <summary>
    /// A transparent borderless window background. The native title is hidden
    /// when this background is selected.
    /// </summary>
    Transparent,

    /// <summary>
    /// A transparent, application-drawn window surface intended for custom tools
    /// and widgets. The application provides its own drag and resize hit testing.
    /// </summary>
    Borderless,
}
