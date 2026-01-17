namespace Xui.Runtime.MacOS;

/// <summary>
/// Defines the stacking level (Z-order) of an NSWindow.
/// </summary>
/// <remarks>
/// NSInteger - treat as nint
/// </remarks>
public enum NSWindowLevel : int
{
    /// <summary>
    /// Normal application window level.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Floating utility panels (inspectors, palettes).
    /// </summary>
    Floating = 3,

    /// <summary>
    /// Modal panels presented above normal and floating windows.
    /// </summary>
    ModalPanel = 8,

    /// <summary>
    /// Status bar and menu-related windows.
    /// </summary>
    StatusBar = 25,

    /// <summary>
    /// Windows shown during screen saver mode.
    /// Use with extreme caution.
    /// </summary>
    ScreenSaver = 1000,
}
