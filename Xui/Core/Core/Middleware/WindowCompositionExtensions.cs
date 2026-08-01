namespace Xui.Core.Middleware;

/// <summary>
/// Provides traversal helpers for the abstract side of a composed window chain.
/// </summary>
public static class WindowCompositionExtensions
{
    /// <summary>
    /// Returns the first desktop style found from a native window's paired abstract
    /// side towards the application window. Middleware can override the application
    /// style by implementing <see cref="Abstract.IWindow.IDesktopStyle"/> itself.
    /// </summary>
    public static Abstract.IWindow.IDesktopStyle? GetDesktopStyle(this Abstract.IWindow window)
    {
        for (;;)
        {
            if (window is Abstract.IWindow.IDesktopStyle desktopStyle)
                return desktopStyle;

            if (window is IWindow middleware)
            {
                if (middleware.Abstract is not { } @abstract)
                    return null;

                window = @abstract;
                continue;
            }

            return null;
        }
    }

    /// <summary>
    /// Returns the first macOS window style found from a native window's paired
    /// abstract side towards the application window. Middleware can override the
    /// application style by implementing <see cref="Abstract.IMacOSWindowStyle"/> itself.
    /// </summary>
    public static Abstract.IMacOSWindowStyle? GetMacOSWindowStyle(this Abstract.IWindow window)
    {
        for (;;)
        {
            if (window is Abstract.IMacOSWindowStyle macOSWindowStyle)
                return macOSWindowStyle;

            if (window is IWindow middleware)
            {
                if (middleware.Abstract is not { } @abstract)
                    return null;

                window = @abstract;
                continue;
            }

            return null;
        }
    }
}
